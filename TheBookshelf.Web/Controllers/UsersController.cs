using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Infrastructure;
using TheBookshelf.BLL.Interfaces;
using TheBookshelf.Web.Models;

namespace TheBookshelf.Web.Controllers
{

	[RoutePrefix("api/Account")]
	public class UsersController : ApiController
    {
		IUserService userService;
		IBookService bookService;
		public UsersController(IUserService users, IBookService books)
		{
			userService = users;
			bookService = books;
		}

		// POST api/Account/Register
		[AllowAnonymous]
		[Route("Register")]
		public async Task<IHttpActionResult> Register(RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var user = new UserDTO() { UserName  = model.Email, Email = model.Email};

			IdentityResult result = await userService.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				return GetErrorResult(result);
			}

			return Ok();
		}

		[Authorize(Roles = "admin")]
		[Route("")]
		[HttpGet]
		public async Task<IHttpActionResult> GetAll()
		{
			var us = await userService.GetAll();
			return Ok(us);
		}

		[Authorize(Roles = "admin, user")]
		[Route("GetRoles")]
		[HttpGet]
		public IHttpActionResult GetRoles()
		{
			var userId = RequestContext.Principal.Identity.GetUserId<int>();
			var roles = userService.GetUserRoles(userId);
			return Ok(roles);
		}

		[Authorize(Roles = "admin, user")]
		[Route("like/{bookId}")]
		[HttpPost]
		public IHttpActionResult LikeOrDislikeBook(int bookId)
		{
			var userId = RequestContext.Principal.Identity.GetUserId<int>();
			int likes;
			try
			{
				bool isLiked = userService.LikeBook(userId, bookId, out likes);
				var book = bookService.Get(bookId);

				var content = new { IsLiked = isLiked, Likes = likes, Book = book };
				return Ok(content);
			}
			catch (ValidationException ex)
			{
				return BadRequest(ex.Message);
			}
			
		}
		[Authorize(Roles = "admin, user")]
		[Route("likedbooks")]
		[HttpGet]
		public IHttpActionResult GetLikedBooks()
		{
			var userId = RequestContext.Principal.Identity.GetUserId<int>();
			try
			{
				var books = userService.GetLikedBooks(userId);
				return Ok(books);
			}
			catch (Exception)
			{
				return NotFound();
			}
		}
		[Authorize(Roles = "admin, user")]
		[Route("{userId}/likedbooks")]
		[HttpGet]
		public IHttpActionResult GetLikedBooks(int userId)
		{
			try
			{
				var books = userService.GetLikedBooks(userId);
				return Ok(books);
			}
			catch (Exception)
			{
				return NotFound();
			}
		}

		[NonAction]
		[AllowAnonymous]
		[Route("initdb")]
		[HttpGet]
		public async Task SetInitialDataAsync()
		{
			await userService.SetInitialData(new UserDTO
			{
				Email = "admin@admin.com",
				UserName = "admin@admin.com",
			}, "admin!", new List<string> { "user", "admin" });
		}

		[Authorize(Roles = "admin")]
		[HttpDelete]
		[Route("delete/{id}")]
		public async Task<IHttpActionResult> DeleteUser(int id)
		{
			if (id < 0)
				return BadRequest("Id is negative");
			IdentityResult result = await userService.DeleteUser(id);
			if (result.Succeeded)
				return Ok();
			else
				return BadRequest(result.Errors.First());
		}

		private IAuthenticationManager Authentication
		{
			get { return Request.GetOwinContext().Authentication; }
		}
		

		private IHttpActionResult GetErrorResult(IdentityResult result)
		{
			if (result == null)
			{
				return InternalServerError();
			}

			if (!result.Succeeded)
			{
				if (result.Errors != null)
				{
					foreach (string error in result.Errors)
					{
						ModelState.AddModelError("", error);
					}
				}

				if (ModelState.IsValid)
				{
					// Ошибки ModelState для отправки отсутствуют, поэтому просто возвращается пустой BadRequest.
					return BadRequest();
				}

				return BadRequest(ModelState);
			}

			return null;
		}
	}
}
