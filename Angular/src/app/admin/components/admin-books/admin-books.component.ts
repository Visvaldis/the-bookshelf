import { Component, OnInit } from '@angular/core';
import {BookService} from '../../../../services/bookService';
import {BookDetail} from '../../../../models/book-detail';
import {AuthUser} from '../../../auth/models/auth.user';
import {AuthService} from '../../../auth/services/auth.service';
import {AuthorService} from '../../../../services/authorService';
import {Author} from '../../../../models/author';
import {TagService} from '../../../../services/tagService';
import {Tag} from '../../../../models/tag';

@Component({
  selector: 'app-admin-books',
  templateUrl: './admin-books.component.html',
  styleUrls: ['./admin-books.component.css']
})
export class AdminBooksComponent implements OnInit {
  book: BookDetail = new BookDetail();   // изменяемый товар
  books: BookDetail[];                // массив товаров
  authors: Author[];
  tags: Tag[];
  tableMode = true;          // табличный режим
  user: AuthUser;
  fileToUpload: File = null;
  fileName = 'Choose file';

  constructor(private bookService: BookService, private authorService: AuthorService, private tagService: TagService,
              private authService: AuthService) { }

  ngOnInit() {
    this.user = this.authService.getUser();
    this.loadBooks();    // загрузка данных при старте компонента
    this.getAuthors();
    this.getTags();
  }
  // получаем данные через сервис
  loadBooks() {
    this.bookService.getAllDetail()
      .subscribe((data: BookDetail[]) => this.books = data);
  }
  getAuthors() {
    this.authorService.getAuthors()
      .subscribe((data: Author[]) => this.authors = data);
  }
  getTags() {
    this.tagService.getTags()
      .subscribe((data: Tag[]) => this.tags = data);
  }

  addTag = (tag) => ({name: tag});
  save() {
    if (this.book.id == null) {
      this.bookService.createBook(this.book)
        .subscribe((data: BookDetail) => {
          this.books.push(data);
          if (this.fileToUpload !== null){
            this.uploadFileToBook(data.id);
          }
        });
    } else {
      this.bookService.updateBook(this.book)
        .subscribe(() => this.loadBooks());
    }
    this.cancel();
  }
  editBook(p: BookDetail) {
    this.book = p;
  }
  cancel() {
    this.book = new BookDetail();
    this.fileToUpload = null;
    this.fileName = 'Choose file';
    this.tableMode = true;
  }
  delete(p: BookDetail) {
    this.bookService.deleteBook(p.id)
      .subscribe(() => this.loadBooks());
  }
  add() {
    this.cancel();
    this.tableMode = false;
  }
  handleFileInput(event) {
    const files = event.srcElement.files;
    this.fileToUpload = files.item(0);
    this.fileName = this.fileToUpload.name;
  }
  uploadFileToBook(id: number) {
    this.bookService.upload(id, this.fileToUpload).subscribe(data => {
      // do something, if upload success
    }, error => {
      console.log(error);
    });
    this.cancel();
  }
}
