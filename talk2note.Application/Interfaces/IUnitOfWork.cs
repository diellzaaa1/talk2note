namespace talk2note.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        INoteRepository Notes { get; } 
        IUserRepository Users { get; } 
        IFolderRepository Folders { get; } 
        ITagRepository Tags { get; }
        INoteToDoRepository NotesToDo { get; }
        INoteTagRepository NoteTags { get; }
        Task<int> CommitAsync();
       
    }

}
