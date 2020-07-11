import { Component, OnInit } from '@angular/core';
import {AuthService} from '../../auth/services/auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public searchStr: string;
  constructor(public authService: AuthService, public router: Router) { }
  ngOnInit(): void {
  }

  public search(){
    this.router.navigate(
      ['/search', this.searchStr]);
  }
}
