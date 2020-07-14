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
  searchReq: string;
  order: string;

  private routeSubscription: Subscription;
  private querySubscription: Subscription;
  constructor(private route: ActivatedRoute, private router: Router,
              private bookService: BookService, private tagService: TagService, private authorService: AuthorService){
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
        console.log(this.books);
      });
  }

  getAllBooks() {
    this.bookService.getBooksInOrder(this.order)
      .subscribe((data: BookCard[]) => {
        this.books = data;
        console.log(this.books);
      });
  }

}
