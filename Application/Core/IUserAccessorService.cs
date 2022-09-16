using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public interface IUserAccessorService
    {
        string GetUsername();
        Task<IList<string>> GetUserRole();
    }
}
