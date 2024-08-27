using Azure;
using System;
using System.Collections.Generic;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;
using talk2note.Infrastructure.Data;

namespace talk2note.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        // Repositories
        public IGenericRepository<Note> Notes { get; private set; }
        public IGenericRepository<User> Users { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Notes = new GenericRepository<Note>(_context);
            Users = new GenericRepository<User>(_context);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
