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
using System.Web;
using TheBookshelf.Web.Util;
using System.Net.Http.Headers;
using CopyStatus = Microsoft.Azure.Storage.Blob.CopyStatus;

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
		public async Task<IHttpActionResult> Delete([FromUri] int id)
		{
			if (id < 0)
				return BadRequest("Id is negative");
			if (!bookService.Exist(id))
				return NotFound();
			var book = bookService.Get(id);
			var containerClient = GetBlobContainer();
			var name = containerClient.GetBlobs()
			.Where(b => b.Name.Contains($"{book.Id}-{book.Name}"))
			.Select(b => b.Name).FirstOrDefault();
			if (name != null)
			{
				// Get a reference to a blob
				BlobClient blobClient = containerClient.GetBlobClient(name);
				await blobClient.DeleteAsync();
			}
			bookService.Delete(id);
			return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
		}

		[Authorize(Roles = "admin")]
		[Route("{id}")]
		[HttpPut]
		public async Task<IHttpActionResult> Update(int id, [FromBody] BookDTO item)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			if (!bookService.Exist(id))
				return NotFound();
			
			var book = bookService.Get(id);
			item.Id = id;
			if (item.Name != book.Name) {
				var containerClient = GetBlobContainer();
				var name = containerClient.GetBlobs()
				.Where(b => b.Name.Contains($"{book.Id}-{book.Name}"))
				.Select(b => b.Name).FirstOrDefault();
				if (name != null)
				{
					var blobName = $"{item.Id}-{item.Name}{name.Substring(name.LastIndexOf("."))}";
					await RenameAsync(containerClient, name, blobName);
				}
			}
				
			bookService.Update(item);
			return Ok();
		}
		public static async Task RenameAsync(BlobContainerClient container, string oldName, string newName)
		{
			BlobClient source = container.GetBlobClient(oldName);
			BlobClient target = container.GetBlobClient(newName);

			await target.StartCopyFromUriAsync(source.Uri);

			await source.DeleteAsync();
		}


		[Authorize(Roles = "admin")]
		[Route("Upload/{id}")]
		[HttpPost]
		public async Task<IHttpActionResult> Upload([FromUri] int id)
		{

			if (System.Web.HttpContext.Current.Request.Files.AllKeys.Length > 0)
			{
				// Get the files using the name attribute as a key. 
				var httpPostedFile = System.Web.HttpContext.Current.Request.Files["file"];
				if (httpPostedFile != null)
				{
					var book = bookService.Get(id);
					var fname = httpPostedFile.FileName;
					var blobName = $"{book.Id}-{book.Name}{fname.Substring(fname.LastIndexOf("."))}";

					var container = GetBlobContainer();
					var blockBlob = container.GetBlobClient(blobName);
					try
					{
						await blockBlob.UploadAsync(httpPostedFile.InputStream);
						return Ok();
					}
					catch (Exception ex)
					{
						return BadRequest(ex.Message);
					}
					
				}
				return BadRequest("File is null");
			}
			return BadRequest("File is not found");
		}





		[Authorize(Roles = "admin, user")]
		[Route("Download/{id}")]
		[HttpGet]
		public async Task<IHttpActionResult> Download([FromUri] int id)
		{
			var containerClient = GetBlobContainer();

			var book = bookService.Get(id);
			var name = containerClient.GetBlobs()
				.Where(b => b.Name.Contains($"{book.Id}-{book.Name}"))
				.Select(b => b.Name).FirstOrDefault();
			if (name != null)
			{
				// Get a reference to a blob
				BlobClient blobClient = containerClient.GetBlobClient(name);

				BlobDownloadInfo download = await blobClient.DownloadAsync();
				
				var result = new HttpResponseMessage(HttpStatusCode.OK)
				{
					Content = new StreamContent(download.Content)
				};
				result.Content.Headers.ContentDisposition =
			  new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
			  {
				  FileName = name
			  };
				result.Content.Headers.ContentType =
					new MediaTypeHeaderValue("application/octet-stream");
				IHttpActionResult response;
				response = ResponseMessage(result);
				return response;
			}
			return BadRequest();
		}

		[NonAction]
		private BlobContainerClient GetBlobContainer()
		{
			string connectionString = WebConfigurationManager.AppSettings["BlobStorage"];

			// Create a BlobServiceClient object which will be used to create a container client
			BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

			//Create a unique name for the container
			string containerName = "thebookshelf-blob";

			// Create the container and return a container client object
			BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
			return containerClient;
		}


		[AllowAnonymous]
		[Route("order/{sortOrder}")]
		[HttpGet]
		public IHttpActionResult GetBooksOrdering([FromUri] string sortOrder)
		{
			var books = bookService.GetAll();
			IOrderedEnumerable<BookDTO> booksOrd;
			switch (sortOrder)
			{
				case "name":
					booksOrd = books.OrderBy(s => s.Name);
					break;
				case "name_desc":
					booksOrd = books.OrderByDescending(s => s.Name);
					break;
				case "mark":
					booksOrd = books.OrderBy(s => s.Assessment);
					break;
				case "mark_desc":
					booksOrd = books.OrderByDescending(s => s.Assessment);
					break;
				default:
					booksOrd = books.OrderBy(s => s.Name);
					break;
			}
			return Ok(booksOrd.ToList());
		}

		[AllowAnonymous]
		[Route("order/{count}/{sortOrder}")]
		[HttpGet]
		public IHttpActionResult GetBooksOrdering(int count, [FromUri] string sortOrder)
		{
			var books = bookService.GetAll();
			IOrderedEnumerable<BookDTO> booksOrd;
			switch (sortOrder)
			{
				case "name":
					booksOrd = books.OrderBy(s => s.Name);
					break;
				case "name_desc":
					booksOrd = books.OrderByDescending(s => s.Name);
					break;
				case "mark":
					booksOrd = books.OrderBy(s => s.Assessment);
					break;
				case "mark_desc":
					booksOrd = books.OrderByDescending(s => s.Assessment);
					break;
				default:
					booksOrd = books.OrderBy(s => s.Name);
					break;
			}
			return Ok(booksOrd.Take(count).ToList());
		}
	}
}
