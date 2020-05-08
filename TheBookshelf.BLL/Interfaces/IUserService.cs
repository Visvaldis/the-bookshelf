using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Infrastructure;
using TheBookshelf.DAL.Entities;

namespace TheBookshelf.BLL.Interfaces
{
	public interface IUserService : IDisposable
	{
		Task<ICollection<UserDTO>> GetAll();
		Task<IdentityResult> CreateAsync(UserDTO user, string password);
	}
}
