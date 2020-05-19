using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBookshelf.BLL.DTO;
using TheBookshelf.DAL.Entities;

namespace TheBookshelf.BLL.Infrastructure
{
	public class Mappers
	{
		public static IMapper BookMapper
		{
			get
			{
				return new MapperConfiguration(cfg => {
					cfg.CreateMap<Book, BookDTO>();
					cfg.CreateMap<BookDTO, Book>();
					cfg.CreateMap<User, UserDTO>();
					cfg.CreateMap<UserDTO, User>();
					cfg.CreateMap<Tag, TagDTO>();
					cfg.CreateMap<TagDTO, Tag>();
					//.ForMember(b => b.Creator, conf => conf.MapFrom(b => b.Creator))
					//.ForMember(b => b.Authors, conf => conf.MapFrom(b => b.Authors))
					//.ForMember(b => b.FanUser, conf => conf.MapFrom(b => b.FanUser))
					//.ForMember(b => b.Tags, conf => conf.MapFrom(b => b.Tags));
				}).CreateMapper();
			}
		}


		public static IMapper TagMapper
		{
			get
			{
				return new MapperConfiguration(cfg => {
					cfg.CreateMap<Tag, TagDTO>()
					.ForMember(t => t.BookCount, conf => conf.MapFrom(b => b.Books.Count));
					cfg.CreateMap<TagDTO, Tag>();
				}).CreateMapper();
			}
		}

		public static IMapper UserMapper
		{
			get
			{
				return new MapperConfiguration(cfg => {
					cfg.CreateMap<User, UserDTO>();
					cfg.CreateMap<UserDTO, User>();
				}).CreateMapper();
			}
		}
		public static IMapper AuthorMapper
		{
			get
			{
				return new MapperConfiguration(cfg => {
					cfg.CreateMap<Author, AuthorDTO>();
					cfg.CreateMap<AuthorDTO, Author>();
				}).CreateMapper();
			}
		}
	

	}
}
