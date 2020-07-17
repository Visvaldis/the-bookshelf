using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Infrastructure;
using TheBookshelf.BLL.Interfaces;
using TheBookshelf.Web.Util;

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

		/// <summary>
		/// Get all tags
		/// </summary>
		/// <returns>200 - List of tags</returns>
		[ResponseCodes(HttpStatusCode.OK)]
		[ResponseType(typeof(List<TagDTO>))]
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

		/// <summary>
		/// Get tag from id
		/// </summary>
		/// <param name="id">Unique tag identifier </param>
		/// <returns>200 - Tag
		/// 400 - if id is negative
		/// 404 - if tag is not found</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
		[ResponseType(typeof(TagDTO))]
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

		/// <summary>
		/// Get all books from tag
		/// </summary>
		/// <param name="authorId">Id of the tag whose books we want to retrieve </param>
		/// <returns>200 - List of BookDTO with same tag
		/// 400 - if id is negative
		/// 404 - if author is not found</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
		[ResponseType(typeof(List<BookDTO>))]
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

		/// <summary>
		/// Create tag. Authorization is required (admin only).
		/// </summary>
		/// <param name="item">Tag you want to add</param>
		/// <returns>201 - Created tag
		/// 400 - if model is not valid or some internal mistakes</returns>
		[ResponseCodes(HttpStatusCode.Created, HttpStatusCode.BadRequest)]
		[ResponseType(typeof(TagDTO))]
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

		/// <summary>
		/// Remove tag from database.  Authorization is required (admin only).
		/// </summary>
		/// <param name="id">Unique tag identifier</param>
		/// <returns>204 - if successfully deleted
		/// 400 - if id is negative
		/// 404 - if tag is not found</returns>
		[ResponseCodes(HttpStatusCode.NoContent, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
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

		/// <summary>
		/// Update tag with new model. Authorization is required (admin only).
		/// </summary>
		/// <param name="id">Id of tag, that will be updated</param>
		/// <param name="item">New model for tag</param>
		/// <returns>200 - if tag successfully updated
		/// 400 - if model is not valid
		/// 404 - if author is not found</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
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

		/// <summary>
		/// Find all tags, whose name contains search string  
		/// </summary>
		/// <param name="name">Search string</param>
		/// <returns>200 - All tags, that fits search
		/// 400 - if name is empty or some internal mistake</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
		[ResponseType(typeof(List<TagDTO>))]
		[AllowAnonymous]
		[Route("search/{name}")]
		[HttpGet, ActionName("GetTagsByName")]
		public IHttpActionResult GetByName(string name)
		{
			if (name is null || name == "")
				return BadRequest("Name is null");
			try
			{
				var tags = tagService.GetByName(name);

				return Ok(tags);
			}
			catch (ValidationException ex)
			{
				return NotFound();
			}
		}



	}
}
