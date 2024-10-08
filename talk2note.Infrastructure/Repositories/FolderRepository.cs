﻿using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;
using talk2note.Infrastructure.Data;
using talk2note.Infrastructure.Persistence;

namespace talk2note.Infrastructure.Repositories
{
    public class FolderRepository : GenericRepository<Folder>, IFolderRepository
    {
        private readonly AppDbContext _context;

        public FolderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
