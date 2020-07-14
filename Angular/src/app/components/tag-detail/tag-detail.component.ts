import { Component, OnInit } from '@angular/core';
import {TagService} from '../../../services/tagService';
import {BookCard} from '../../../models/book-card';
import {Tag} from '../../../models/tag';
import {ActivatedRoute, Router} from '@angular/router';
import {switchMap} from 'rxjs/operators';

@Component({
  selector: 'app-tag-detail',
  templateUrl: './tag-detail.component.html',
  styleUrls: ['./tag-detail.component.css'],
  providers: [TagService]
})
export class TagDetailComponent implements OnInit {

  books: BookCard[] = [];
  tag: Tag;
  constructor(private route: ActivatedRoute, private router: Router,
              private tagService: TagService) { }


  ngOnInit(): void {
    this.route.paramMap.pipe(
      switchMap(params => params.getAll('id'))
    )
      .subscribe(data => {
        console.log(data);
        this.tagService.getTag(+data)
          .subscribe((val: Tag) => {
            this.tag = val;
            this.loadBooks(+data);
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
          console.log(this.books);
          console.log(value);
        },
        error => {
          alert(error.message);
        });
  }
}
