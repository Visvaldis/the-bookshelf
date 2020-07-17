using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Interfaces;
using TheBookshelf.Web.Util;

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

		/// <summary>
		/// Get all authors
		/// </summary>
		/// <returns>200 - List of authors</returns>
		[ResponseCodes(HttpStatusCode.OK)]
		[ResponseType(typeof(List<AuthorDTO>))]
		[AllowAnonymous]
		[Route()]
		[HttpGet, ActionName("GetAllAuthors")]
		public IHttpActionResult GetAll()
		{
			var authors = authorService.GetAll();
			return Ok(authors);
		}
		/// <summary>
		/// Get author from id
		/// </summary>
		/// <param name="id">Unique author identifier </param>
		/// <returns>200 - Author
		/// 400 - if id is negative
		/// 404 - if author is not found</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
		[ResponseType(typeof(AuthorDTO))]
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
		/// <summary>
		/// Get all books from author
		/// </summary>
		/// <param name="authorId">Author id, whose books we want get </param>
		/// <returns>200 - List of BookDTO with same author
		/// 400 - if id is negative
		/// 404 - if author is not found</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
		[ResponseType(typeof(List<BookDTO>))]
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

		/// <summary>
		/// Create author. Authorization is required (admin only).
		/// </summary>
		/// <param name="item">Author you want to add</param>
		/// <returns>201 - Created author
		/// 400 - if model is not valid or some internal mistakes</returns>
		[ResponseCodes(HttpStatusCode.Created, HttpStatusCode.BadRequest)]
		[ResponseType(typeof(AuthorDTO))]
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

		/// <summary>
		/// Remove author from database.  Authorization is required (admin only).
		/// </summary>
		/// <param name="id">Unique author identifier</param>
		/// <returns>204 - if successfully deleted
		/// 400 - if id is negative
		/// 404 - if author is not found</returns>
		[ResponseCodes(HttpStatusCode.NoContent, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
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

		/// <summary>
		/// Update author with new model. Authorization is required (admin only).
		/// </summary>
		/// <param name="id">Id of author, that will be updated</param>
		/// <param name="item">New model for author</param>
		/// <returns>200 - if author successfully updated
		/// 400 - if model is not valid
		/// 404 - if author is not found</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
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

		/// <summary>
		/// Find all authors, whose name contains search string  
		/// </summary>
		/// <param name="name">Search string</param>
		/// <returns>200 - All authors, that fits search
		/// 400 - if name is empty or some internal mistake</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
		[ResponseType(typeof(List<AuthorDTO>))]
		[AllowAnonymous]
		[Route("search/{name}")]
		[HttpGet, ActionName("GetAuthorsByName")]
		public IHttpActionResult GetByName(string name)
		{
			if (name is null || name == "")
				return BadRequest("Name is null");
			try
			{
				var authors = authorService.GetByName(name);

				return Ok(authors);
			}
			catch (ValidationException ex)
			{
				return BadRequest(ex.Message);
			}
		}


	}
}
