import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {config} from '../config';
import {Observable} from 'rxjs';
import { map } from 'rxjs/operators';
import {BookCard} from '../models/book-card';
import {BookDetail} from '../models/book-detail';

@Injectable()
export class BookService {


  private url = config.apiUrl + '/api/books';
  public inOrder = {
    nameDesc: 'name_desc',
    nameAsc: 'name',
    markDesc: 'mark_desc',
    markAsc: 'mark'
  };
  constructor(private http: HttpClient) {
  }


  getAllBooks(): Observable<BookCard[]>{
    return this.getBooks(this.url);
  }


  getBooks(url: string): Observable<BookCard[]>{
    return this.http.get<BookDetail[]>(url).pipe(map(
      (books: BookDetail[]) => {
        return books.map(book => {
          // tslint:disable-next-line:no-shadowed-variable
          const b = new BookCard();
          b.id = book.id;
          b.name = book.name;
          b.coverUrl = book.coverUrl;
          b.mark = book.assessment;
          b.authorName = book.authors[0].name;
          return b;
        });
      }
    ));
  }

  getBooksInOrder(order: string): Observable<BookCard[]>{
    const ordurl = `${this.url}/order/${order}`;
    return this.getBooks(ordurl);
  }
  getBooksInOrderCount(order: string, count: number): Observable<BookCard[]>{
    const ordurl = `${this.url}/order/${count}/${order}`;
    return this.getBooks(ordurl);
  }

/*
  getBook(id: number): Observable<Tag> {
    return this.http.get<Tag>(this.url + '/' + id).pipe(map(
      (tags: Tag) =>    tags
    ));
  }

  createBook(tag: Tag) {
    return this.http.post(this.url, tag);
  }
  updateBook(tag: Tag) {

    return this.http.put(this.url, tag);
  }
  deleteBook(id: number) {
    return this.http.delete(this.url + '/' + id);
  }
*/
}
