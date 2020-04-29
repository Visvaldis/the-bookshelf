﻿using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Interfaces;
using TheBookshelf.Web.Models;

namespace TheBookshelf.Web.Controllers
{
	[Authorize]
	[RoutePrefix("api/Account")]
	public class UsersController : ApiController
    {
		IUserService userService;
		public UsersController(IUserService service)
		{
			userService = service;
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
			var user = new UserDTO() { UserName  = model.Email, Email = model.Email };

			IdentityResult result = await userService.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				return GetErrorResult(result);
			}

			return Ok();
		}

		[AllowAnonymous]
		[HttpGet]
		public IHttpActionResult GetAll()
		{
			var us = userService.GetAll();
			return Ok(us);
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