using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Identity;
using TheBookshelf.BLL.Infrastructure;
using TheBookshelf.BLL.Interfaces;
using TheBookshelf.DAL.Entities;
using TheBookshelf.DAL.Interfaces;

namespace TheBookshelf.BLL.Services
{
	public class UserService : IUserService
	{
		IUnitOfWork Database { get; set; }
		IMapper Mapper;
		ApplicationUserManager userManager;
		ApplicationRoleManager roleManager;

		public UserService(IUnitOfWork uow)
		{
			Database = uow;
			userManager = new ApplicationUserManager(Database.UserStore);
			roleManager = new ApplicationRoleManager(Database.RoleStore);
			Mapper = Mappers.BookshelfMapper;
		}

	

		public async Task<ICollection<UserDTO>> GetAll()
		{
			var users = await userManager.Users.Include(x => x.Roles).ToListAsync();
			List<UserDTO> userDTOs = Mapper.Map<List<User>, List<UserDTO>>(users);
			foreach (var item in userDTOs)
			{
				var rolesName = userManager.GetRoles(item.Id);
				List<RoleDTO> roles = new List<RoleDTO>();
				foreach (var roleIter in rolesName)
				{
					var role = await roleManager.FindByNameAsync(roleIter);
					var roleDto = Mapper.Map<Role, RoleDTO>(role);
					roles.Add(roleDto);
				}
				item.Roles = roles;
			}
			return userDTOs;
		}
		public async Task<ICollection<RoleDTO>> GetAllRoles()
		{
			var roles = await roleManager.Roles.ToListAsync();
			return Mapper.Map<List<Role>, List<RoleDTO>>(roles);
		}

		public async Task<IdentityResult> PromoteToRole(int userId, string roleName)
		{
			return await userManager.AddToRoleAsync(userId, roleName);
		}

		public async Task<IdentityResult> RemoveFromRole(int userId, string roleName)
		{
			return await userManager.RemoveFromRoleAsync(userId, roleName);
		}

		public async void AddRole(string roleName)
		{
			var role = await roleManager.FindByNameAsync(roleName);
			if (role == null)
			{
				role = new Role { Name = roleName };
				await roleManager.CreateAsync(role);
			}
		}

		public void Dispose()
		{
			Database.Dispose();
		}

		public async Task<IdentityResult> CreateAsync(UserDTO user, string password)
		{
			var u = Mapper.Map<UserDTO, User>(user);
			var res = await userManager.CreateAsync(u, password);
			await userManager.AddToRoleAsync(
				userManager.Find(user.UserName, password).Id,
								 roleManager.Roles.Where(r => r.Name == "user").FirstOrDefault().Name);
			return res;
		}

		public UserDTO GetUser(string userName)
		{
			var user = userManager.FindByName(userName);
			return  Mapper.Map<User, UserDTO>(user);
		}

		public ICollection<string> GetUserRoles(int userId)
		{
			var roles = userManager.GetRoles(userId);
			return roles;
		}

		public bool LikeBook(int userId, int bookId, out int likes)
		{
			var user = userManager.FindById(userId);
			if (user == null)
				throw new ValidationException("User not found");
			var book = Database.Books.Get(bookId);
			if (book == null)
				throw new ValidationException("Book not found");

			if(user.LikedBooks.Contains(book) == true)
			{
				book.Assessment--;
				user.LikedBooks.Remove(book);
				Database.Save();
				likes = book.Assessment;
				return false;
			}
			else
			{
				book.Assessment++;
				user.LikedBooks.Add(book);
				Database.Save();
				likes = book.Assessment;
				return true;
			}
			
		}

		public ICollection<BookDTO> GetLikedBooks(int userId)
		{
			
			var user = userManager.FindById(userId);
			if (user == null)
				throw new ValidationException("User not found");
			var books = user.LikedBooks;
			return Mapper.Map<ICollection<BookDTO>>(books);
		}


		public async Task SetInitialData(UserDTO adminDto, string password, List<string> roles)
		{
			foreach (string roleName in roles)
			{
				var role = await roleManager.FindByNameAsync(roleName);
				if (role == null)
				{
					role = new Role { Name = roleName };
					await roleManager.CreateAsync(role);
				}
			}
			var u = Mapper.Map<UserDTO, User>(adminDto);
			var res = await CreateAsync(adminDto, password);
			await userManager.AddToRoleAsync(userManager.Find(adminDto.UserName,password).Id, roles[1]);
		}

		public Task<IdentityResult> DeleteUser(int userId)
		{
			var user = userManager.FindById(userId);
			return userManager.DeleteAsync(user);
		}
	}
}
	