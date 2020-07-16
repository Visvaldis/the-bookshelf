import { Component, OnInit } from '@angular/core';
import {AuthorService} from '../../../services/authorService';
import {BookCard} from '../../../models/book-card';
import {ActivatedRoute, Router} from '@angular/router';
import {switchMap} from 'rxjs/operators';
import {Author} from '../../../models/author';
import {NgxSpinnerService} from 'ngx-spinner';

@Component({
  selector: 'app-authors-details',
  templateUrl: './authors-details.component.html',
  styleUrls: ['./authors-details.component.css'],
  providers: [AuthorService]
})
export class AuthorsDetailsComponent implements OnInit {
  books: BookCard[] = [];
  author: Author = new Author();
  loading = true;

  constructor(private route: ActivatedRoute, private router: Router,
              private authorService: AuthorService, private spinner: NgxSpinnerService) { }


  ngOnInit(): void {
    this.spinner.show();
    this.route.paramMap.pipe(
      switchMap(params => params.getAll('id'))
    )
      .subscribe(data => {
        this.authorService.getAuthor(+data)
          .subscribe((val: Author) => {
            this.author = val;
            this.loadBooks(+data);
            this.spinner.hide();
            this.loading = false;
            },
            error => {
              if (error.status === 404)
              {
                this.router.navigate(['error404']);
              }
              else {
                this.spinner.hide();
                alert(error.message);
              }
            });
      });
  }


  private loadBooks(id: number) {
    this.authorService.getBookByAuthor(id)
      .subscribe((value: BookCard[]) => {
          this.books = value;
        },
        error => {
          alert(error.message);
        });
  }
}
