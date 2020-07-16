import { Component, OnInit } from '@angular/core';
import {BookCard} from '../../../models/book-card';
import {UserService} from '../../../services/userService';
import {AuthUser} from '../../auth/models/auth.user';
import {AuthService} from '../../auth/services/auth.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  providers: [UserService]
})
export class ProfileComponent implements OnInit {

  books: BookCard[] = [];
  user: AuthUser;
  constructor(private userService: UserService, private authServive: AuthService) { }

  ngOnInit(): void {
    this.loadBooks();
    this.user = this.authServive.getUser();
  }

  private loadBooks() {
    this.userService.getLikedBooks()
      .subscribe((value: BookCard[]) => {
          this.books = value;
          console.log(this.books);
        },
        error => {
          alert(error.message);
        });
  }

}
