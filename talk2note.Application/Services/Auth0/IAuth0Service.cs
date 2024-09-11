using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talk2note.Application.Services.Auth0
{
    public interface IAuth0Service
    {
        Task<string> GenerateTokenAsync();
    }

}
