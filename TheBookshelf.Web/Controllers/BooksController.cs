using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TheBookshelf.BLL.DTO;
using TheBookshelf.BLL.Infrastructure;
using TheBookshelf.BLL.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;
using System.IO;
using System.Web.Configuration;

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

		[AllowAnonymous]
		[Route()]
		[HttpGet, ActionName("GetAllBooks")]
		public IHttpActionResult GetAll()
		{
			var books = bookService.GetAll();
			return Ok(books);
		}

		[AllowAnonymous]
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

		[AllowAnonymous]
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


		[Authorize(Roles = "admin")]
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

		[Authorize(Roles = "admin")]
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

		[Authorize(Roles = "admin")]
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

		[Authorize(Roles = "admin")]
		[Route("Upload/{id}")]
		[HttpPost]
		public async Task Upload([FromUri] int id, [FromBody] string pathToFile)
		{
			string connectionString = WebConfigurationManager.AppSettings["BlobStorage"];
			// Retrieve storage account from connection string.
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
			// Create the blob client.
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
			// Retrieve reference to a previously created container.
			CloudBlobContainer container = blobClient.GetContainerReference("thebookshelf-blob");

			var book = bookService.Get(id);
			// Retrieve reference to a blob named "myblob".
			CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{book.Id}-{book.Name}");

			// Create or overwrite the "myblob" blob with contents from a local file.
			blockBlob.UploadFromFile(pathToFile);			
		}

		[Authorize(Roles = "admin, user")]
		[Route("Download/{id}")]
		[HttpPost]
		public async Task Download([FromUri] int id, [FromBody] string downloadPath)
		{
			string connectionString = WebConfigurationManager.AppSettings["BlobStorage"];
			// Retrieve storage account from connection string.
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

			// Create the blob client.
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			// Retrieve reference to a previously created container.
			CloudBlobContainer container = blobClient.GetContainerReference("thebookshelf-blob");

			var book = bookService.Get(id);
			// Retrieve reference to a blob named "myblob".
			CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{book.Id}-{book.Name}");

			blockBlob.DownloadToFile(downloadPath, FileMode.Create);
		}
	}
}
