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
		void LikeBook(int userId, int bookId);
		void DislikeBook(int userId, int bookId);
		ICollection<BookDTO> GetLikedBooks(int userId);
		ICollection<BookDTO> GetAddedBooks(int userId);
		Task<ICollection<UserDTO>> GetAll();
		Task<IdentityResult> CreateAsync(UserDTO user, string password);
		UserDTO GetUser(string userName);
	}
}
