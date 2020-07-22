import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {switchMap} from 'rxjs/operators';
import {BookDetail} from '../../../models/book-detail';
import {BookService} from '../../../services/bookService';
import {AuthUser} from '../../auth/models/auth.user';
import {AuthService} from '../../auth/services/auth.service';
import { saveAs } from 'file-saver';
import {Subscribable} from 'rxjs';
import {UserService} from '../../../services/userService';
import {NgxSpinnerService} from 'ngx-spinner';
import {HttpErrorResponse} from '@angular/common/http';


@Component({
  selector: 'app-book-detail',
  templateUrl: './book-detail.component.html',
  styleUrls: ['./book-detail.component.css'],
  providers: [BookService, UserService]
})
export class BookDetailComponent implements OnInit {
  book: BookDetail;
  user: AuthUser;
  isLiked = false;
  loading = true;

  bookSubscription: Subscribable<BookDetail>;
  constructor(private route: ActivatedRoute, private router: Router,
              private bookService: BookService,  private authService: AuthService,
              private userService: UserService, private spinner: NgxSpinnerService){  }
  ngOnInit()
  {
    this.spinner.show();
    this.user = this.authService.getUser();
    this.route.paramMap.pipe(
      switchMap(params => params.getAll('id'))).subscribe(data =>
      {
        // tslint:disable-next-line:triple-equals
        if (data == 'r') {
          this.bookSubscription = this.bookService.getRandomBook();
        }
        else {
          this.bookSubscription =  this.bookService.getBook(+data);
      }});
    this.bookSubscription.subscribe((value: BookDetail) => {
        this.book = value;
        this.isLiked = this.book.fanUsers.some(user => user.userName === this.user.userName);
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


  }


  downloadBook(id: number){
    this.spinner.show();
    this.bookService.download(id).subscribe(
      response => {
        const name = response.headers.get('File-Name');
        const downloadName = this.book.name + name.substr(name.lastIndexOf('.'));
        this.spinner.hide();
        saveAs(response.body, downloadName);
      }, (error: HttpErrorResponse) => {
        if (error.status === 404)
        {
           alert('Sorry, file is not found');
        }
        else {
          console.log(error.message);
        }
      }
    );
  }

  like(){
   this.userService.likeBook(this.book.id).subscribe(
     response => {
       console.log(response.body);
       this.isLiked = response.body['isLiked'];
       this.book.assessment = response.body['likes'];
     }, error => {
       if (error.status === 401)
       {
         alert('You need to be logged in if you want to mark a book');
         this.router.navigate(['login']);
       }
       else {
         this.spinner.hide();
         alert(error.message);
       }
     });

  }

}
/*
const d = this.http.get(url)  .subscribe((resp: Blob) => {
      map()
      console.log(resp);
      //  const file = new File(resp, 'filename.zip', {type: 'application/octet-stream'});
      FileSaver.saveAs(resp);
    });


    */
