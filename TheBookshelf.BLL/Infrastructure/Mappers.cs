using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.DTO;
using TheBookshelf.DAL.Entities;

namespace TheBookshelf.BLL.Infrastructure
{
	public class Mappers
	{
		public static IMapper BookshelfMapper
		{
			get
			{
				var mapperCfg = new MapperConfiguration(cfg =>
				{
					cfg.CreateMap<Book, BookDTO>().ReverseMap();

					cfg.CreateMap<User, UserDTO>()
					.ForMember(dest => dest.Roles, conf => conf.Ignore());
					cfg.CreateMap<UserDTO, User>()
					.ForMember(dest => dest.LikedBooks, conf => conf.Ignore())
					.ForMember(dest => dest.EmailConfirmed, conf => conf.Ignore())
					.ForMember(dest => dest.PasswordHash, conf => conf.Ignore())
					.ForMember(dest => dest.SecurityStamp, conf => conf.Ignore())
					.ForMember(dest => dest.PhoneNumber, conf => conf.Ignore())
					.ForMember(dest => dest.PhoneNumberConfirmed, conf => conf.Ignore())
					.ForMember(dest => dest.TwoFactorEnabled, conf => conf.Ignore())
					.ForMember(dest => dest.LockoutEnabled, conf => conf.Ignore())
					.ForMember(dest => dest.LockoutEndDateUtc, conf => conf.Ignore())
					.ForMember(dest => dest.Logins, conf => conf.Ignore())
					.ForMember(dest => dest.AccessFailedCount, conf => conf.Ignore())
					.ForMember(dest => dest.Claims, conf => conf.Ignore());




					cfg.CreateMap<RoleDTO, Role>()
					.ForMember(dest => dest.Users, conf => conf.Ignore());
					cfg.CreateMap<Role, RoleDTO>();

					cfg.CreateMap<Tag, TagDTO>()
					.ForMember(t => t.BookCount, conf => conf.MapFrom(b => b.Books.Count));
					cfg.CreateMap<TagDTO, Tag>()
					.ForMember(t => t.Books, conf => conf.Ignore());

					cfg.CreateMap<Author, AuthorDTO>().ReverseMap();


					cfg.CreateMap<Expression<Func<BookDTO, bool>>,
						Expression<Func<Book, bool>>>();

					cfg.CreateMap<Expression<Func<AuthorDTO, bool>>,
						Expression<Func<Author, bool>>>();
					cfg.CreateMap<Expression<Func<TagDTO, bool>>,
						Expression<Func<Tag, bool>>>();
				});
				return mapperCfg.CreateMapper();
			}
		}
	

	}
}
