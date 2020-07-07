import { Component, OnInit } from '@angular/core';
import {BookCard} from '../../../models/book-card';
import {BookService} from '../../../services/bookService';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  providers: [BookService]
})
export class HomeComponent implements OnInit {

  books: BookCard[];
  constructor(private bookService: BookService) { }

  ngOnInit(): void {
    this.loadBooks();
  }
  loadBooks() {
    this.bookService.getBooks()
      .subscribe((data: BookCard[]) => {
        this.books = data;
        console.log(this.books);
      });
  }
}
