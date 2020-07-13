import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Tag} from '../models/tag';
import {config} from '../config';
import {Observable} from 'rxjs';
import { map } from 'rxjs/operators';
import {Author} from '../models/author';

@Injectable()
export class AuthorService {


  private url = config.apiUrl + '/api/authors';

  constructor(private http: HttpClient) {
  }

  getTags(): Observable<Author[]>{
    return this.http.get<Author[]>(this.url).pipe(map(
      (authors: Author[]) =>    authors
    ));
  }

  searchAuthors(name: string): Observable<Author[]>{
    const searchUrl = `${this.url}/search/${name}`;
    return this.http.get<Author[]>(searchUrl).pipe(map(
      (authors: Author[]) =>    authors
    ));
  }

  getTag(id: number): Observable<Author> {
    return this.http.get<Author>(this.url + '/' + id).pipe(map(
      (author: Author) =>    author
    ));
  }

  createProduct(author: Author) {
    return this.http.post(this.url, author);
  }
  updateProduct(author: Author) {

    return this.http.put(this.url, author);
  }
  deleteProduct(id: number) {
    return this.http.delete(this.url + '/' + id);
  }

}
