import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {BookService} from '../../../services/bookService';
import {BookCard} from '../../../models/book-card';
import {Tag} from '../../../models/tag';
import {Author} from '../../../models/author';
import {TagService} from '../../../services/tagService';
import {AuthorService} from '../../../services/authorService';
import {Subscription} from 'rxjs';


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
  order: string;

  // tslint:disable-next-line:variable-name
  private _searchReq: string;
  get searchReq(): string {
    return this._searchReq;
  }
  set searchReq(newSearchReq: string) {
      this._searchReq = newSearchReq;
      this.loadSearch(this._searchReq);
  }
  private routeSubscription: Subscription;
  private querySubscription: Subscription;
  constructor(private route: ActivatedRoute, private router: Router,
              public bookService: BookService, private tagService: TagService, private authorService: AuthorService){
    this.routeSubscription = route.params.subscribe(params => this.searchReq = params['searchRequest']);
    this.querySubscription = route.queryParams.subscribe(
      (queryParam: any) => {
        this.order = queryParam['order'];
      }
    );
  }

  ngOnInit(): void {

    if (this.searchReq === undefined)
    {
      this.getAllBooks();
    }
    else{
      this.loadSearch(this.searchReq);
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
      });
  }

  getAllBooks() {
    this.bookService.getBooksInOrder(this.order)
      .subscribe((data: BookCard[]) => {
        this.books = data;
      });
  }

  changeSorting(sort: string){
    this.order = sort;
    this.books = this.bookService.sortBooks(this.books, this.order);
  }
}
