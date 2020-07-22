 using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Infrastructure;
using TheBookshelf.BLL.Interfaces;
using TheBookshelf.Web.Models;
using TheBookshelf.Web.Util;

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

		/// <summary>
		///  Register user in system  
		/// </summary>
		/// <param name="model">Register model</param>
		/// <returns>200 - if user successfully registered
		/// 400 - if mode is not valid
		/// </returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest)]
		[AllowAnonymous]
		[Route("Register")]
		public async Task<IHttpActionResult> Register(RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				List<string> errors = new List<string>();
				var a = ModelState.Values.Select(x => x.Errors).ToList();
				a.ForEach(x => x.ForEach(y => errors.Add( y.ErrorMessage)));
				string s = "";
				errors.ForEach(e => s += e + "\n");
				return BadRequest(s);
			}
			var user = new UserDTO() { UserName  = model.Email, Email = model.Email};

			IdentityResult result = await userService.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				return GetErrorResult(result);
			}

			return Ok();
		}
		/// <summary>
		/// Get all users.  Authorization is required (admin only).
		/// </summary>
		/// <returns>200 - Collection of UserDTO</returns>
		[ResponseCodes(HttpStatusCode.OK)]
		[ResponseType(typeof(List<UserDTO>))]
		[Authorize(Roles = "admin")]
		[Route("")]
		[HttpGet]
		public async Task<IHttpActionResult> GetAll()
		{
			var us = await userService.GetAll();
			return Ok(us);
		}

		/// <summary>
		/// Get roles of current user.
		///  Authorization is required (admin and user).
		/// </summary>
		/// <returns>200 - Collection of RoleDTO</returns>
		[ResponseCodes(HttpStatusCode.OK)]
		[ResponseType(typeof(List<RoleDTO>))]
		[Authorize(Roles = "admin, user")]
		[Route("UserRoles")]
		[HttpGet]
		public IHttpActionResult GetUserRoles()
		{
			var userId = RequestContext.Principal.Identity.GetUserId<int>();
			var roles = userService.GetUserRoles(userId);
			return Ok(roles);
		}


		/// <summary>
		/// Get all roles
		/// </summary>
		/// <returns>200 - Collection of RoleDTO</returns>
		[ResponseCodes(HttpStatusCode.OK)]
		[ResponseType(typeof(List<RoleDTO>))]
		[Route("Roles")]
		[HttpGet]
		public IHttpActionResult GetAllRoles()
		{
			var roles = userService.GetAllRoles();
			return Ok(roles);
		}


		/// <summary>
		/// Add new role
		///  Authorization is required (admin only).
		/// </summary>
		/// <param name="roleName">Role name</param>
		/// <returns>200</returns>
		[ResponseCodes(HttpStatusCode.OK)]
		[Authorize(Roles = "admin")]
		[Route("Roles")]
		[HttpPost]
		public IHttpActionResult AddNewRole([FromBody] string roleName)
		{
			userService.AddRole(roleName);
			return Ok();
		}


		/// <summary>
		/// Add user to role
		/// </summary>
		/// <param name="userId">User identifier</param>
		/// <param name="roleName">Role name</param>
		/// <returns>200 - Ok</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest)]
		[Authorize(Roles = "admin")]
		[Route("Roles/{userId}")]
		[HttpPost]
		public async Task<IHttpActionResult> PromoteToRole(int userId, [FromBody] string roleName)
		{
			var res = await userService.PromoteToRole(userId, roleName);
			if (res.Succeeded)
				return Ok();
			else
				return BadRequest(res.Errors.ToString());
		}


		/// <summary>
		/// Remove user from role
		/// </summary>
		/// <param name="userId">User identifier</param>
		/// <param name="roleName">Role name</param>
		/// <returns>200 - Ok</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest)]
		[Authorize(Roles = "admin")]
		[Route("Roles/{userId}")]
		[HttpDelete]
		public async Task<IHttpActionResult> RemoveFromRole(int userId, [FromBody] string roleName)
		{
			var res = await userService.RemoveFromRole(userId, roleName);
			if (res.Succeeded)
				return Ok();
			else
				return BadRequest(res.Errors.ToString());
		}

		/// <summary>
		/// Like some book or dislike (if current user has already liked it)
		///  Authorization is required (admin and user).
		/// </summary>
		/// <param name="bookId">Book id</param>
		/// <returns>200 - {
		/// 'isLiked': bool (true if user likes this book, false if user not likes it)
		/// 'likes' : number (count of likes)
		/// }
		/// 400 - if user or book not found or some error
		/// </returns>
		[ResponseCodes(HttpStatusCode.OK)]
		[Authorize(Roles = "admin, user")]
		[Route("like/{bookId}")]
		[HttpGet]
		public IHttpActionResult LikeOrDislikeBook(int bookId)
		{
			var userId = RequestContext.Principal.Identity.GetUserId<int>();
			int likes;
			try
			{
				bool isLiked = userService.LikeBook(userId, bookId, out likes);
				//var book = bookService.Get(bookId);

				var content = (IsLiked: isLiked, Likes: likes);
				return Ok(content);
			}
			catch (ValidationException ex)
			{
				return BadRequest(ex.Message);
			}
			
		}

		/// <summary>
		/// Get users's liked books 
		///  Authorization is required (admin and user).
		/// </summary>
		/// <returns>
		/// 200 - Collection of BookDTO
		/// 404 - if user not found</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.NotFound)]
		[ResponseType(typeof(List<BookDTO>))]
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

		/// <summary>
		/// Delete user
		/// </summary>
		/// <param name="id">User identifier</param>
		/// <returns>200 - ok
		/// 400 - if some errors or id is negative</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.NotFound)]
		[Authorize(Roles = "admin")]
		[HttpDelete]
		[Route("{id}")]
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
