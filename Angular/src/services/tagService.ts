import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Tag} from '../models/tag';
import {config} from '../config';
import {Observable} from 'rxjs';
import { map } from 'rxjs/operators';
import {BookCard} from '../models/book-card';
import {BookService} from './bookService';

@Injectable()
export class TagService {


  private url = config.apiUrl + '/api/tags';

  constructor(private http: HttpClient, private bookService: BookService) {
  }

  getTags(): Observable<Tag[]>{
  return this.http.get<Tag[]>(this.url).pipe(map(
    (tags: Tag[]) =>    tags
  ));
  }

  searchTags(name: string): Observable<Tag[]>{
    const searchUrl = `${this.url}/search/${name}`;
    return this.http.get<Tag[]>(searchUrl).pipe(map(
      (tags: Tag[]) =>    tags
    ));
  }

  getTag(id: number): Observable<Tag> {
    return this.http.get<Tag>(this.url + '/' + id).pipe(map(
      (tags: Tag) =>    tags
    ));
  }

  getBookByTag(id: number): Observable<BookCard[]>{
    const url = `${this.url}/${id}/books`;
    return this.bookService.getBooks(url);
  }

  createProduct(tag: Tag) {
    return this.http.post(this.url, tag);
  }
  updateProduct(tag: Tag) {

    return this.http.put(this.url, tag);
  }
  deleteProduct(id: number) {
    return this.http.delete(this.url + '/' + id);
  }

}
