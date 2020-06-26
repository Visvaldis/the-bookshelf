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
	[RoutePrefix("api/books")]
	public class BooksController : ApiController
	{
		IBookService bookService;
		IUserService userService;
		public BooksController(IBookService books, IUserService users)
		{
			bookService = books;
			userService = users;

		}

		[Route()]
		[HttpGet, ActionName("GetAllBooks")]
		public IHttpActionResult GetAll()
		{
			var books = bookService.GetAll();
			return Ok(books);
		}

		[Route("{id:int}")]
		[HttpGet, ActionName("GetBook")]
		public IHttpActionResult Get(int id)
		{
			if (id <= 0)
				return BadRequest("Id is negative");
			try
			{
				var book = bookService.Get(id);
				return Ok(book);
			}
			catch (ValidationException ex)
			{
				return NotFound();
			}
		}


		[Route("search/{name}")]
		[HttpGet, ActionName("GetBookByName")]
		public IHttpActionResult GetByName(string name)
		{
			if (name is null || name == "")
				return BadRequest("Name is negative");
			try
			{
				var books = bookService.GetBooksByName(name);

				return Ok(books);
			}
			catch (ValidationException ex)
			{
				return NotFound();
			}
		}


		[Authorize]
		[Route()]
		[HttpPost]
		public IHttpActionResult Create([FromBody] BookDTO item)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var name = RequestContext.Principal.Identity.Name;
				var user = userService.GetUser(name);
				item.CreatorId = user.Id;
				item.PublishDate = DateTime.Today;
				item.AddedDate = DateTime.Today;
				int bookId = bookService.Add(item);
				item.Id = bookId;

				return Created(new Uri($"{Request.RequestUri}/{bookId}", UriKind.RelativeOrAbsolute), item);
			}
			catch (ArgumentNullException ex)
			{
				return BadRequest(ex.Message);
			}
		}


		[Route("{id}")]
		[HttpDelete]
		public IHttpActionResult Delete([FromUri] int id)
		{
			if (id < 0)
				return BadRequest("Id is negative");
			if (!bookService.Exist(id))
				return NotFound();

			bookService.Delete(id);
			return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
		}

		[Route("{id}")]
		[HttpPut]
		public IHttpActionResult Update(int id, [FromBody] BookDTO item)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			if (!bookService.Exist(id))
				return NotFound();

			item.Id = id;
			bookService.Update(item);
			return Ok();
		}
	}
}
