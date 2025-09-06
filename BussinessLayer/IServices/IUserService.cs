using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DTO;

namespace BussinessLayer.IServices
{
    public interface IUserService
    {
        Task<string> Login(LoginDTO dto);

    }
}
