using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Infrastructure;
using TheBookshelf.BLL.Interfaces;

namespace TheBookshelf.Web.Controllers
{
	[RoutePrefix("api/tags")]
    public class TagsController : ApiController
    {
		ITagService tagService;
		public TagsController(ITagService service)
		{
			tagService = service;


		}

		[AllowAnonymous]
		[Route()]
		[HttpGet, ActionName("GetAllTags")]
		public IHttpActionResult GetAll()
		{
			try
			{
				var tags = tagService.GetAll();
				return Ok(tags);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}

		[AllowAnonymous]
		[Route("{id}")]
		[HttpGet, ActionName("GetTag")]
		public IHttpActionResult Get(int id)
		{
			if (id <= 0)
				return BadRequest("Id is negative");
			try
			{
				var tag = tagService.Get(id);
				return Ok(tag);
			}
			catch (ValidationException ex)
			{
				return NotFound();
			}
		}

		[AllowAnonymous]
		[Route("{tagId}/books")]
		[HttpGet, ActionName("GetBooksByTag")]
		public IHttpActionResult GetBooksByTag(int tagId)
		{
			if (tagId <= 0)
				return BadRequest("Id is negative");
			try
			{
				var books = tagService.GetBooksByTag(tagId);
				return Ok(books);
			}
			catch (ValidationException ex)
			{
				return NotFound();
			}
			
		}

		[Authorize(Roles = "admin")]
		[Route()]
		[HttpPost]
		public IHttpActionResult Create([FromBody] TagDTO item)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				int tagId = tagService.Add(item);
				item.Id = tagId;
				
				return Created(new Uri($"{Request.RequestUri}/{tagId}", UriKind.RelativeOrAbsolute), item);
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
			catch(ValidationException ex)
			{
				return Ok($"Tag already exist. Id = {ex.Message}");
			}
		}

		[Authorize(Roles = "admin")]
		[Route("{id}")]
		[HttpDelete]
		public IHttpActionResult Delete([FromUri] int id)
		{
			if (id < 0)
				return BadRequest("Id is negative");
			if (!tagService.Exist(id))
				return NotFound();

			tagService.Delete(id);
			return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
		}

		[Authorize(Roles = "admin")]
		[Route("{id}")]
		[HttpPut]
		public IHttpActionResult Update(int id, [FromBody] TagDTO item)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			if (!tagService.Exist(id))
				return NotFound();

			item.Id = id;
			tagService.Update(item);
			return Ok();
		}



	}
}
