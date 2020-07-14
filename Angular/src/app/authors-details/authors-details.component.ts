import { Component, OnInit } from '@angular/core';
import {AuthorService} from '../../services/authorService';
import {BookCard} from '../../models/book-card';
import {ActivatedRoute, Router} from '@angular/router';
import {switchMap} from 'rxjs/operators';
import {Author} from '../../models/author';

@Component({
  selector: 'app-authors-details',
  templateUrl: './authors-details.component.html',
  styleUrls: ['./authors-details.component.css'],
  providers: [AuthorService]
})
export class AuthorsDetailsComponent implements OnInit {
  books: BookCard[] = [];
  author: Author;
  constructor(private route: ActivatedRoute, private router: Router,
              private authorService: AuthorService) { }


  ngOnInit(): void {
    this.route.paramMap.pipe(
      switchMap(params => params.getAll('id'))
    )
      .subscribe(data => {
        console.log(data);
        this.authorService.getAuthor(+data)
          .subscribe((val: Author) => {
            this.author = val;
            this.loadBooks(+data);
            },
            error => {
              if (error.status === 404)
              {
                this.router.navigate(['error404']);
              }
              else {
                alert(error.message);
              }
            });
      });
  }


  private loadBooks(id: number) {
    this.authorService.getBookByAuthor(id)
      .subscribe((value: BookCard[]) => {
          this.books = value;
          console.log(this.books);
          console.log(value);
        },
        error => {
          alert(error.message);
        });
  }
}
