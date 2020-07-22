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
using System.Web.Routing;
using System.Web.Http.Description;

namespace TheBookshelf.Web.Controllers
{

	[RoutePrefix("api/books")]
	public class BooksController : ApiController
	{
		IBookService bookService;
		public BooksController(IBookService books)
		{
			bookService = books;

		}

		/// <summary>
		/// Get all books
		/// </summary>
		/// <returns>200 - Collection of books</returns>
		[ResponseCodes(HttpStatusCode.OK)]
		[ResponseType(typeof(List<BookDTO>))]
		[AllowAnonymous]
		[Route()]
		[HttpGet, ActionName("GetAllBooks")]
		public IHttpActionResult GetAll()
		{
			var books = bookService.GetAll();
			return Ok(books);
		}

		/// <summary>
		/// Get book from id
		/// </summary>
		/// <param name="id">Unique book identifier </param>
		/// <returns>200 - Book
		/// 400 - if id is negative
		/// 404 - if book is not found</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
		[ResponseType(typeof(BookDTO))]
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

		/// <summary>
		/// Create book. Authorization is required (admin only).
		/// </summary>
		/// <param name="item">Book you want to add</param>
		/// <returns>201 - Created book
		/// 400 - if model is not valid or some internal mistakes</returns>
		[ResponseCodes(HttpStatusCode.Created, HttpStatusCode.BadRequest)]
		[ResponseType(typeof(BookDTO))]
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

		/// <summary>
		/// Remove book from database. Book's file also will be deleted  Authorization is required (admin only).
		/// </summary>
		/// <param name="id">Unique book identifier</param>
		/// <returns>204 - if successfully deleted
		/// 400 - if id is negative
		/// 404 - if book is not found</returns>
		[ResponseCodes(HttpStatusCode.NoContent, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
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

		/// <summary>
		/// Update book with new model. Book's file will be also updated. Authorization is required (admin only).
		/// </summary>
		/// <param name="id">Id of book, that will be updated</param>
		/// <param name="item">New model for book</param>
		/// <returns>200 - if book successfully updated
		/// 400 - if model is not valid
		/// 404 - if book is not found</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
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
		private static async Task RenameAsync(BlobContainerClient container, string oldName, string newName)
		{
			BlobClient source = container.GetBlobClient(oldName);
			BlobClient target = container.GetBlobClient(newName);

			await target.StartCopyFromUriAsync(source.Uri);

			await source.DeleteAsync();
		}


		/// <summary>
		/// Upload file, that refers to choosen book. 
		/// You need upload file from front with name 'file'. 
		/// In case of Postman: Body -> form-data -> Key='file', type=File, Value=yourFile
		/// Authorization is required (admin only).
		/// </summary>
		/// <param name="id"> Book id</param>
		/// <returns>200 - if upload is susseccfull
		/// 400 - if file is not found / null/ or some mistake</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
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
					BookDTO book;
					if (id <= 0)
						return BadRequest("Id is negative");
					try
					{
						 book = bookService.Get(id);
					}
					catch (ValidationException ex)
					{
						return NotFound();
					}	
					var fname = httpPostedFile.FileName;
					var blobName = $"{book.Id}-{book.Name}{fname.Substring(fname.LastIndexOf("."))}";

					var container = GetBlobContainer();

					var name = container.GetBlobs()
					.Where(b => b.Name.Contains($"{book.Id}-{book.Name}"))
					.Select(b => b.Name).FirstOrDefault();
					if (name != null)
					{
						// Get a reference to a blob
						BlobClient blobClient = container.GetBlobClient(name);
						await blobClient.DeleteAsync();
					}

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



		/// <summary>
		/// Endpoint for downloading book's file from book id.
		/// Name of fill contains in header "File-Name":"filename"
		///  Authorization is required (admin and user).
		/// </summary>
		/// <param name="id">Book id</param>
		/// <returns>200 - stream data, with will be processed by your browser
		/// 400 - if some mistake</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest)]
		[Authorize(Roles = "admin, user")]
		[Route("Download/{id}")]
		[HttpGet]
		public async Task<IHttpActionResult> Download([FromUri] int id)
		{

			var containerClient = GetBlobContainer();
			BookDTO book;
			if (id <= 0)
				return BadRequest("Id is negative");
			try
			{
				book = bookService.Get(id);
			}
			catch (ValidationException ex)
			{
				return NotFound();
			}
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
					Content = new StreamContent(download.Content),

				};

				result.Content.Headers.Add("File-Name", name);
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
			return NotFound();
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


		/// <summary>
		/// Find all books, whose name contains search string  
		/// </summary>
		/// <param name="name">Search string</param>
		/// <returns>200 - All books, that fits search
		/// 400 - if name is empty or some internal mistake</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound)]
		[ResponseType(typeof(List<BookDTO>))]
		[AllowAnonymous]
		[Route("search/{name}")]
		[HttpGet, ActionName("GetBookByName")]
		public IHttpActionResult GetByName(string name)
		{
			if (name is null || name == "")
				return BadRequest("Name is null");
			try
			{
				var books = bookService.GetByName(name);

				return Ok(books);
			}
			catch (ValidationException ex)
			{
				return NotFound();
			}
		}

		/// <summary>
		///  Get random books
		/// </summary>
		/// <param name="count">Count of books</param>
		/// <returns>200 - Random collection of BookDTO
		/// 400 - if count is negative</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest)]
		[ResponseType(typeof(List<BookDTO>))]
		[AllowAnonymous]
		[Route("random/{count}")]
		[HttpGet]
		public IHttpActionResult GetRandomBooks(int count)
		{
			if (count <= 0)
				return BadRequest("Count can`t be neganive");
			var books = bookService.GetRandomBooks(count);
			return Ok(books);
		}

		/// <summary>
		/// Get sorted books
		/// </summary>
		/// <param name="sortOrder">'name' - order by name ascending
		/// 'name_desc' - order by name descending
		/// 'mark' - order by assessment ascending
		/// 'mark_desc' - order by assessment descending</param>
		/// <returns>>200 - Ordered collection of BookDTO</returns>
		[ResponseCodes(HttpStatusCode.OK)]
		[ResponseType(typeof(List<BookDTO>))]
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

		/// <summary>
		///  Get first N sorted books
		/// </summary>
		/// <param name="count"></param>
		/// <param name="sortOrder">'name' - order by name ascending
		/// 'name_desc' - order by name descending
		/// 'mark' - order by assessment ascending
		/// 'mark_desc' - order by assessment descending</param>
		/// <returns>>200 - Ordered collection of BookDTO
		/// 400 - if count is negative</returns>
		[ResponseCodes(HttpStatusCode.OK, HttpStatusCode.BadRequest)]
		[ResponseType(typeof(List<BookDTO>))]
		[AllowAnonymous]
		[Route("order/{count}/{sortOrder}")]
		[HttpGet]
		public IHttpActionResult GetBooksOrdering(int count, [FromUri] string sortOrder)
		{
			if (count <= 0)
				return BadRequest("Count can`t be neganive");
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
