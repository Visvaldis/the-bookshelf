using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheBookshelf.BLL.DTO;
using AutoMapper;
using TheBookshelf.BLL.Services;
using TheBookshelf.Web.Models;

namespace TheBookshelf.Web.Controllers
{
	public class HomeController : Controller
	{
		TagService tagService;
		public HomeController(TagService service)
		{
			tagService = service;
		}
		public ActionResult Index()
		{
			IEnumerable<TagDTO> tagDTOs = tagService.GetAll();
			var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TagDTO, TagViewModel>()).CreateMapper();
			var tags = mapper.Map<IEnumerable<TagDTO>, List<TagViewModel>>(tagDTOs);
			return View(tags);
		}
	}
}
