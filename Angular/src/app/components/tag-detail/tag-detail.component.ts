import { Component, OnInit } from '@angular/core';
import {TagService} from '../../../services/tagService';
import {BookCard} from '../../../models/book-card';
import {Tag} from '../../../models/tag';
import {ActivatedRoute, Router} from '@angular/router';
import {switchMap} from 'rxjs/operators';
import {NgxSpinnerService} from 'ngx-spinner';

@Component({
  selector: 'app-tag-detail',
  templateUrl: './tag-detail.component.html',
  styleUrls: ['./tag-detail.component.css'],
  providers: [TagService]
})
export class TagDetailComponent implements OnInit {

  books: BookCard[] = [];
  tag: Tag = new Tag();
  loading = true;
  constructor(private route: ActivatedRoute, private router: Router,
              private tagService: TagService, private spinner: NgxSpinnerService) { }


  ngOnInit(): void {
    this.spinner.show();
    this.route.paramMap.pipe(
      switchMap(params => params.getAll('id'))
    )
      .subscribe(data => {
        this.tagService.getTag(+data)
          .subscribe((val: Tag) => {
            this.tag = val;
            this.loadBooks(+data);
            this.spinner.hide();
            this.loading = false;
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


  private loadBooks(id: number) {
    this.tagService.getBookByTag(id)
      .subscribe((value: BookCard[]) => {
          this.books = value;
        },
        error => {
          alert(error.message);
        });
  }
}
