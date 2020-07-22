import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminBooksComponent } from './components/admin-books/admin-books.component';
import { AdminTagsComponent } from './components/admin-tags/admin-tags.component';
import { AdminAuthorsComponent } from './components/admin-authors/admin-authors.component';
import { AdminUsersComponent } from './components/admin-users/admin-users.component';
import {AppRoutingModule} from '../app-routing.module';
import {RouterModule} from '@angular/router';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { AdminComponent } from './admin.component';
import {NgSelectModule} from '@ng-select/ng-select';



@NgModule({
  declarations: [AdminBooksComponent, AdminTagsComponent, AdminAuthorsComponent, AdminUsersComponent, AdminComponent],
  imports: [
    CommonModule,
    AppRoutingModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    NgSelectModule
  ]
})
export class AdminModule { }
