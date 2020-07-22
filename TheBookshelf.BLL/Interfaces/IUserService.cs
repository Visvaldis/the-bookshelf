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
		bool LikeBook(int userId, int bookId, out int likes);
		ICollection<BookDTO> GetLikedBooks(int userId);
		Task<ICollection<UserDTO>> GetAll();
		Task<IdentityResult> CreateAsync(UserDTO user, string password);
		UserDTO GetUser(string userName);
		Task SetInitialData(UserDTO adminDto, string password, List<string> roles);
		Task<IdentityResult> DeleteUser(int userId);
		ICollection<string> GetUserRoles(int userId);
		Task<ICollection<RoleDTO>> GetAllRoles();
		Task<IdentityResult> PromoteToRole(int userId, string roleName);
		Task<IdentityResult> RemoveFromRole(int userId, string roleName);
		void AddRole(string roleName);

	}
}
