import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {BookService} from '../../../services/bookService';
import {switchMap} from 'rxjs/operators';
import {BookCard} from '../../../models/book-card';
import {Tag} from '../../../models/tag';
import {Author} from '../../../models/author';
import {TagService} from '../../../services/tagService';
import {BookDetail} from '../../../models/book-detail';
import {AuthorService} from '../../../services/authorService';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  providers: [BookService, TagService, AuthorService]
})
export class SearchComponent implements OnInit {

  books: BookCard[] = [];
  tags: Tag[] = [];
  authors: Author[] = [];
  searchReq: string;
  constructor(private route: ActivatedRoute, private router: Router,
              private bookService: BookService, private tagService: TagService, private authorService: AuthorService) { }

  ngOnInit(): void {
    this.route.paramMap.pipe(
      switchMap(params => params.getAll('searchRequest'))
    ).subscribe(data =>
      {
        console.log(data);
        this.searchReq = data;

        this.loadSearch(this.searchReq);
      });
    if (this.searchReq === undefined)
    {
      console.log('all');
      this.getAllBooks();
    }
  }


  loadSearch(name: string){
    this.loadBooks(name);
    this.loadAuthors(name);
    this.loadTags(name);
  }

  loadAuthors(name: string) {
    this.authorService.searchAuthors(name)
      .subscribe((data: Author[]) => {
        this.authors = data;
      });
  }
  loadTags(name: string) {
    this.tagService.searchTags(name)
      .subscribe((data: Tag[]) => {
        this.tags = data;
      });
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
