import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {switchMap} from 'rxjs/operators';
import {BookDetail} from '../../../models/book-detail';
import {BookService} from '../../../services/bookService';
import {AuthUser} from '../../auth/models/auth.user';
import {AuthService} from '../../auth/services/auth.service';
import { saveAs } from 'file-saver';


@Component({
  selector: 'app-book-detail',
  templateUrl: './book-detail.component.html',
  styleUrls: ['./book-detail.component.css'],
  providers: [BookService]
})
export class BookDetailComponent implements OnInit {
  book: BookDetail;
  user: AuthUser;



  constructor(private route: ActivatedRoute, private router: Router,
              private bookService: BookService,  private authService: AuthService){}
  ngOnInit() {
    this.user = this.authService.getUser();
    this.route.paramMap.pipe(
      switchMap(params => params.getAll('id'))
    )
      .subscribe(data =>
      {
        console.log(data);
        // tslint:disable-next-line:triple-equals
        if (data == 'r'){
          return this.bookService.getRandomBook().subscribe((value: BookDetail) => {
              this.book = value;
              console.log(this.book);
            },
            error => {
              if (error.status === 404)
              {
                this.router.navigate(['error404']);
              }
              else {
                alert(error.message);
              }
            });
        }
        return this.bookService.getBook(+data).subscribe((value: BookDetail) => {
          this.book = value;
          console.log(this.book);
      },
          error => {
          if (error.status === 404)
          {
            this.router.navigate(['error404']);
          }
          else {
            alert(error.message);
          }
          });
      });
  }


  downloadBook(id: number){
    this.bookService.download(id).subscribe(
      response => {
        const name = response.headers.get('File-Name');
        const downloadName = this.book.name + name.substr(name.lastIndexOf('.'));
        saveAs(response.body, downloadName);
      }
    );
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
