using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;
using Nest;

namespace talk2note.Infrastructure.Services
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly IElasticClient _elasticClient;

        public ElasticsearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task IndexDocumentAsync(Note note)
        {
            var indexResponse = await _elasticClient.IndexAsync(note, i => i
                .Id(note.NoteId) 
            );

            if (!indexResponse.IsValid)
            {
                throw new Exception($"Failed to index document {note.NoteId}: {indexResponse.OriginalException.Message}");
            }
        }

        public async Task DeleteDocumentAsync(int noteId)
        {
            var deleteResponse = await _elasticClient.DeleteAsync<Note>(noteId);
            if (!deleteResponse.IsValid)
            {
                throw new Exception($"Failed to delete document: {deleteResponse.ServerError}");
            }
        }

        public async Task<Note> GetDocumentAsync(int noteId)
        {
            var getResponse = await _elasticClient.GetAsync<Note>(noteId);
            if (!getResponse.IsValid)
            {
                throw new Exception($"Failed to get document: {getResponse.ServerError}");
            }
            return getResponse.Source;
        }

        public async Task<ISearchResponse<Note>> SearchAsync(string query)
        {
            var searchResponse = await _elasticClient.SearchAsync<Note>(s => s
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Query(query)
                        .Fields(f => f
                            .Field(n => n.Title)
                            .Field(n => n.Content)
                        )
                    )
                )
            );
            return searchResponse;
        }

    }
}
