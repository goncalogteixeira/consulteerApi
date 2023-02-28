using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Consulteer.Application.Interfaces
{
    public interface ITokenServices
    {
        string GenerateToken(string userName, string role, IList<Claim> claims);

    }
}
