import { Component, OnInit } from '@angular/core';
import {AuthorService} from '../../../services/authorService';
import {AuthUser} from '../../auth/models/auth.user';
import {AuthService} from '../../auth/services/auth.service';
import {Author} from '../../../models/author';

@Component({
  selector: 'app-authors',
  templateUrl: './authors.component.html',
  styleUrls: ['./authors.component.css'],
  providers: [AuthorService]
})
export class AuthorsComponent implements OnInit {

  author: Author = new Author();   // изменяемый товар
  authors: Author[];                // массив товаров
  tableMode = true;          // табличный режим
  user: AuthUser;

  constructor(private authorService: AuthorService, private authService: AuthService) { }

  ngOnInit() {
    this.user = this.authService.getUser();
    this.loadAuthors();    // загрузка данных при старте компонента
  }
  // получаем данные через сервис
  loadAuthors() {
    this.authorService.getAuthors()
      .subscribe((data: Author[]) => this.authors = data);
  }
  // сохранение данных
  save() {
    if (this.author.id == null) {
      this.authorService.createAuthor(this.author)
        .subscribe((data: Author) => this.authors.push(data));
    } else {
      this.authorService.updateAuthor(this.author)
        .subscribe(() => this.loadAuthors());
    }
    this.cancel();
  }
  editAuthor(p: Author) {
    this.author = p;
  }
  cancel() {
    this.author = new Author();
    this.tableMode = true;
  }
  delete(p: Author) {
    this.authorService.deleteAuthor(p.id)
      .subscribe(() => this.loadAuthors());
  }
  add() {
    this.cancel();
    this.tableMode = false;
  }

}
