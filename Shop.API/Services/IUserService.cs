using Shop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.API.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
    }
}
