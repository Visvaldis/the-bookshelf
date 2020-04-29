using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Interfaces;

namespace TheBookshelf.Web.Controllers
{
    public class TagsController : ApiController
    {
		ITagService tagService;
		public TagsController(ITagService service)
		{
			tagService = service;
		}

		[Authorize]
		[HttpGet]
		public IHttpActionResult GetAll()
		{
			var tags = tagService.GetAll();
			return Ok(tags);
		}

		[HttpPost]
		public IHttpActionResult Create([FromBody] TagDTO item)
		{
			tagService.Add(item);
			return Ok();
		}
    }
}
