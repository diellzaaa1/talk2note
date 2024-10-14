using Nest;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;

namespace talk2note.Infrastructure.External.Elastic
{
    public class NoteSearch : INoteSearch
    {
        private readonly IElasticClient _elasticClient;

        public NoteSearch(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<List<Note>> SearchUserNotesAsync(int userId, string searchTerm, string[] additionalFields = null)
        {
            additionalFields ??= Array.Empty<string>(); 

            var searchResponse = await _elasticClient.SearchAsync<Note>(s => s
                .Index("notes")
                .Query(q => q
                    .Bool(b => b
                        .Filter(f => f
                            .Term(t => t.UserId, userId)
                        )
                        .Must(m => m
                            .MultiMatch(mm => mm
                                .Query(searchTerm)
                                .Fields(f => f
                                    .Field(n => n.Title)
                                    .Field(n => n.Content)
                                    .Fields(additionalFields)
                                )
                                .Fuzziness(Fuzziness.Auto)
                                .Boost(1.5)
                            )
                        )
                    )
                )
                .Sort(s => s
                    .Descending(d => d.CreatedAt)
                )
            );

            return searchResponse.Documents.ToList();
        }



    }
}
