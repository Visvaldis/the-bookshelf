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


		[HttpGet]
		public IHttpActionResult GetAll()
		{
			var tags = tagService.GetAll();
			return Ok(tags);
		}

		[Authorize]
		[HttpGet]
		public IHttpActionResult Get(int id)
		{
			var tag = tagService.Get(id);
			return Ok(tag);
		}

		[Route("api/Tags/")]
		[HttpPost]
		public IHttpActionResult Create([FromBody] TagDTO item)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			try
			{
				tagService.Add(item);
				return Ok();
			//	return Created()
				//return Created(new Uri(Url.Link(ViewRouteName, new { taskId = taskId, id = view.Id })), view);
			
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}

		}
    }
}
