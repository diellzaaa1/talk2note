using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talk2note.Application.DTO.Note;
using talk2note.Application.Interfaces;
using talk2note.Domain.Entities;

namespace talk2note.Application.Services
{
    public class NoteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NoteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    
    }

}
