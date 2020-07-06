using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Interfaces;

namespace TheBookshelf.Web.Controllers
{

	[RoutePrefix("api/authors")]
	public class AuthorsController : ApiController
	{
		IAuthorService authorService;
		public AuthorsController(IAuthorService _authorService)
		{
			authorService = _authorService;
		}

		[AllowAnonymous]
		[Route()]
		[HttpGet, ActionName("GetAllAuthors")]
		public IHttpActionResult GetAll()
		{
			var authors = authorService.GetAll();
			return Ok(authors);
		}

		[AllowAnonymous]
		[Route("{id:int}")]
		[HttpGet, ActionName("GetAuthor")]
		public IHttpActionResult Get(int id)
		{
			if (id <= 0)
				return BadRequest("Id is negative");
			try
			{
				var author = authorService.Get(id);
				return Ok(author);
			}
			catch (ValidationException ex)
			{
				return NotFound();
			}
		}


		[AllowAnonymous]
		[Route("{authorId}/books")]
		[HttpGet, ActionName("GetBooksByAuthor")]
		public IHttpActionResult GetBooksByTag(int authorId)
		{
			if (authorId <= 0)
				return BadRequest("Id is negative");
			try
			{
				var books = authorService.GetBooksByAuthor(authorId);
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
		public IHttpActionResult Create([FromBody] AuthorDTO item)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				int authorId = authorService.Add(item);
				item.Id = authorId;
				return Created(new Uri($"{Request.RequestUri}/{authorId}", UriKind.RelativeOrAbsolute), item);
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
		}


		[Authorize(Roles = "admin")]
		[Route("{id}")]
		[HttpDelete]
		public IHttpActionResult Delete([FromUri] int id)
		{
			if (id < 0)
				return BadRequest("Id is negative");
			if (!authorService.Exist(id))
				return NotFound();

			authorService.Delete(id);
			return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
		}

		[Authorize(Roles = "admin")]
		[Route("{id}")]
		[HttpPut]
		public IHttpActionResult Update(int id, [FromBody] AuthorDTO item)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			if (!authorService.Exist(id))
				return NotFound();

			item.Id = id;
			authorService.Update(item);
			return Ok();
		}



	}
}
