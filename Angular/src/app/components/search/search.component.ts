import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {BookService} from '../../../services/bookService';
import {switchMap} from 'rxjs/operators';
import {BookCard} from '../../../models/book-card';
import {Tag} from '../../../models/tag';
import {Author} from '../../../models/author';
import {TagService} from '../../../services/tagService';
import {BookDetail} from '../../../models/book-detail';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  providers: [BookService, TagService]
})
export class SearchComponent implements OnInit {

  books: BookCard[];
  tags: Tag[];
  authors: Author[];
  searchReq: string;
  constructor(private route: ActivatedRoute, private router: Router,
              private bookService: BookService, private tagService: TagService, ) { }

  ngOnInit(): void {
    this.route.paramMap.pipe(
      switchMap(params => params.getAll('searchRequest'))
    ).subscribe(data =>
      {
        console.log(data);
        this.searchReq = data;
        this.loadBooks(this.searchReq);
      });
    if (this.searchReq === undefined)
    {
      console.log('all');
      this.getAllBooks();
    }
  }

  loadBooks(name: string) {
    this.bookService.searchBooks(name)
      .subscribe((data: BookCard[]) => {
        this.books = data;
        console.log(this.books);
      });
  }
  getAllBooks() {
    this.bookService.getAllBooks()
      .subscribe((data: BookCard[]) => {
        this.books = data;
        console.log(this.books);
      });
  }

}
