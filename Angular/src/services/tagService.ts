import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Tag} from '../models/tag';
import {config} from '../config';
import {Observable} from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class TagService {


  private url = config.apiUrl + '/api/tags';

  constructor(private http: HttpClient) {
  }

  getTags(): Observable<Tag[]>{
  return this.http.get<Tag[]>(this.url).pipe(map(
    (tags: Tag[]) =>    tags
  ));
  }


  getTag(id: number): Observable<Tag> {
    return this.http.get<Tag>(this.url + '/' + id).pipe(map(
      (tags: Tag) =>    tags
    ));
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
