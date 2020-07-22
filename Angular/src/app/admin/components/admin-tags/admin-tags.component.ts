import { Component, OnInit } from '@angular/core';
import {Tag} from '../../../../models/tag';
import {TagService} from '../../../../services/tagService';

@Component({
  selector: 'app-admin-tags',
  templateUrl: './admin-tags.component.html',
  styleUrls: ['./admin-tags.component.css']
})
export class AdminTagsComponent implements OnInit {

  tag: Tag = new Tag();   // изменяемый товар
  tags: Tag[];                // массив товаров
  tableMode = true;          // табличный режим

  constructor(private tagService: TagService) { }

  ngOnInit() {
    this.loadProducts();    // загрузка данных при старте компонента
  }
  // получаем данные через сервис
  loadProducts() {
    this.tagService.getTags()
      .subscribe((data: Tag[]) => this.tags = data);
  }
  // сохранение данных
  save() {
    if (this.tag.id == null) {
      this.tagService.createProduct(this.tag)
        .subscribe((data: Tag) => this.tags.push(data));
    } else {
      this.tagService.updateProduct(this.tag)
        .subscribe(data => this.loadProducts());
    }
    this.cancel();
  }
  editProduct(p: Tag) {
    this.tag = p;
  }
  cancel() {
    this.tag = new Tag();
    this.tableMode = true;
  }
  delete(p: Tag) {
    this.tagService.deleteProduct(p.id)
      .subscribe(data => this.loadProducts());
  }
  add() {
    this.cancel();
    this.tableMode = false;
  }

}
