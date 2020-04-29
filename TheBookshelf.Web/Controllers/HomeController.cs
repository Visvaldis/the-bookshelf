using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheBookshelf.BLL.DTO;
using AutoMapper;
using TheBookshelf.BLL.Services;
using TheBookshelf.BLL.Interfaces;

using System.ComponentModel.DataAnnotations;

namespace TheBookshelf.Web.Controllers
{
	public class HomeController : Controller
	{
		
		public ActionResult Index()
		{

			return View();
		}
	}
}
