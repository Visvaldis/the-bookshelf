using AutoMapper;
using Microsoft.AspNet.Identity;
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
			Mapper = Mappers.BookshelfMapper;
		}

	

		public async Task<ICollection<UserDTO>> GetAll()
		{
			var users = await userManager.Users.ToListAsync();
			return Mapper.Map<List<User>, List<UserDTO>>(users);
		}


		public void Dispose()
		{
			Database.Dispose();
		}

		public Task<IdentityResult> CreateAsync(UserDTO user, string password)
		{
			var u = Mapper.Map<UserDTO, User>(user);
			return userManager.CreateAsync(u, password);
		}

		public UserDTO GetUser(string userName)
		{
			var user = userManager.FindByName(userName);
			return  Mapper.Map<User, UserDTO>(user);
		}
	}
}
	