import { Component, OnInit } from '@angular/core';
import {AuthService} from '../../auth/services/auth.service';
import {Router} from '@angular/router';
import {BookService} from '../../../services/bookService';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  providers: [BookService]
})
export class HeaderComponent implements OnInit {

  public searchStr: string;

  constructor(public authService: AuthService, public router: Router,  public bookService: BookService) { }
  ngOnInit(): void {
  }

  public search(){
    this.router.navigate(
      ['/search', this.searchStr]);
  }
}
