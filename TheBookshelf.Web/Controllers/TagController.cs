using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Interfaces;
using TheBookshelf.Web.Models;

namespace TheBookshelf.Web.Controllers
{
    public class TagController : Controller
    {
		ITagService<TagDTO> tagService;
		IMapper mapper;

		public TagController(ITagService<TagDTO> service)
		{
			tagService = service;
		    mapper = new MapperConfiguration(cfg => cfg.CreateMap<TagDTO, TagViewModel>()).CreateMapper();
		}
		// GET: Tag
		public ActionResult Index()
        {
			
			IEnumerable<TagDTO> tagDTOs = tagService.GetAll();	
			var tags = mapper.Map<IEnumerable<TagDTO>, List<TagViewModel>>(tagDTOs);
			return View(tags);
		}

        // GET: Tag/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tag/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tag/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
				// TODO: Add insert logic here
				var name = collection["Name"];
				tagService.Add(new TagDTO { Name = name });
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
				return Content($"<h2>ERROR: {ex.Message}</h2>");
			}
        }

        // GET: Tag/Edit/5
        public ActionResult Edit(int id)
        {
		//	var tag = mapper.Map<TagDTO, TagViewModel>();
			return View(tagService.Get(id));
        }

        // POST: Tag/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
			try
			{
				// TODO: Add insert logic here
				var name = collection["Name"];
				tagService.Update(new TagDTO {Id = id, Name = name });
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return Content($"<h2>ERROR: {ex.Message}</h2>");
			}
		}

        // GET: Tag/Delete/5
        public ActionResult Delete(int id)
        {
			var tag = mapper.Map<TagDTO, TagViewModel>(tagService.Get(id));
			return View(tag);
		}

        // POST: Tag/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
			try
			{
				// TODO: Add insert logic here
				var name = collection["Name"];
				tagService.Delete(id);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return Content($"<h2>ERROR: {ex.Message}</h2>");
			}
		}
    }
}
