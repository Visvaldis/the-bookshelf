import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {config} from '../config';
import {Observable, throwError} from 'rxjs';
import {catchError, map} from 'rxjs/operators';
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

  searchBooks(name: string): Observable<BookCard[]>{
    const ordurl = `${this.url}/search/${name}`;
    return this.getBooks(ordurl);
  }
  getBooksInOrderCount(order: string, count: number): Observable<BookCard[]>{
    const ordurl = `${this.url}/order/${count}/${order}`;
    return this.getBooks(ordurl);
  }

  getRandomBooks(count: number): Observable<BookCard[]>{
    const url = `${this.url}/random/${count}`;
    return this.getBooks(url);
  }

  getRandomBook(): Observable<BookDetail> {
    return this.http.get<BookDetail>(`${this.url}/random/1`).pipe(
      map((book: BookDetail) => book[0]),
      catchError((err: HttpErrorResponse) => {
        console.log(err);
        return throwError(err);
      })
    );
  }
  getBook(id: number): Observable<BookDetail> {
    return this.http.get<BookDetail>(this.url + '/' + id).pipe(
      map((book: BookDetail) => book),
      catchError( (err: HttpErrorResponse) => {
          console.log(err);
          return throwError(err);
      })
    );
  }
  download(id) {
    const  url =  'https://thebookshelf.azurewebsites.net/api/books' + '/Download/' + id;
    return this.http.get(url, { observe: 'response', responseType: 'blob'}); }


    sortBooks(books: BookCard[], order: string): BookCard[]{
        switch (order) {
          case this.inOrder.nameAsc:
            books.sort( (a, b) => a.name.localeCompare(b.name));
            break;
          case this.inOrder.nameDesc:
            books.sort( (a, b) => -a.name.localeCompare(b.name));
            break;
          case this.inOrder.markAsc:
            books.sort( (a, b) => a.mark > b.mark ? 1 : -1);
            break;
          case this.inOrder.markDesc:
            books.sort( (a, b) => a.mark < b.mark ? 1 : -1);
            break;
        }
        return books;
    }

    createBook(book: BookDetail) {
      return this.http.post(this.url, book);
    }
    updateBook(book: BookDetail) {

      return this.http.put(this.url, book);
    }
    deleteBook(id: number) {
      return this.http.delete(this.url + '/' + id);
    }

}
