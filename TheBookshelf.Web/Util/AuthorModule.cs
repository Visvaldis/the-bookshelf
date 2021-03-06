﻿using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheBookshelf.BLL.Interfaces;
using TheBookshelf.BLL.Services;

namespace TheBookshelf.Web.Util
{
	public class AuthorModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IAuthorService>().To<AuthorService>();
		}
	}
}