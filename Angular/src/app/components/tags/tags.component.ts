import { Component, OnInit } from '@angular/core';
import {Tag} from '../../../models/tag';
import {TagService} from '../../../services/tagService';
import {AuthService} from '../../auth/services/auth.service';
import {AuthUser} from '../../auth/models/auth.user';

@Component({
  selector: 'app-tags',
  templateUrl: './tags.component.html',
  styleUrls: ['./tags.component.css'],
  providers: [TagService]
})
export class TagsComponent implements OnInit {
  tags: Tag[];
  user: AuthUser;

  constructor(private tagService: TagService, private authService: AuthService) { }

  ngOnInit() {
    this.user = this.authService.getUser();
    this.loadProducts();    // загрузка данных при старте компонента
  }
  // получаем данные через сервис
  loadProducts() {
    this.tagService.getTags()
      .subscribe((data: Tag[]) => this.tags = data);
  }
}
