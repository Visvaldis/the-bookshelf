using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.Infrastructure;
using TheBookshelf.DAL.Entities;
using TheBookshelf.DAL.Interfaces;

namespace TheBookshelf.BLL.Identity
{
	public class ApplicationUserManager : UserManager<User, int>
	{
		public ApplicationUserManager(IUserStore<User, int> store) : base(store)
		{
		}


		public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
		{
			IKernel kernel = new StandardKernel(new ServiceModule("BookshelfContext"));
			IUnitOfWork uow = kernel.Get<IUnitOfWork>();
			var store = uow.UserStore;
			var manager = new ApplicationUserManager(store);

			// Настройка логики проверки имен пользователей
			manager.UserValidator = new UserValidator<User, int>(manager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};
			// Настройка логики проверки паролей
			manager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = true,
				RequireDigit = true,
				RequireLowercase = true,
				RequireUppercase = true,
			};
			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null)
			{
				manager.UserTokenProvider = new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
			}
			return manager;
		}
	}
}