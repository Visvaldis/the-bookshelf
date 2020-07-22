import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from './components/home/home.component';
import {TagsComponent} from './components/tags/tags.component';
import {LoginComponent} from './auth/components/login/login.component';
import {RegisterComponent} from './auth/components/register/register.component';

import {AuthGuard} from './auth/guards/auth.guard';
import {ProfileComponent} from './components/profile/profile.component';
import {ProfileGuard} from './auth/guards/profile.guard';
import {SearchComponent} from './components/search/search.component';
import {BookDetailComponent} from './components/book-detail/book-detail.component';
import {Error404Component} from './components/error404/error404.component';
import {TagDetailComponent} from './components/tag-detail/tag-detail.component';
import {AuthorsDetailsComponent} from './components/authors-details/authors-details.component';
import {AuthorsComponent} from './components/authors/authors.component';
import {AdminTagsComponent} from './admin/components/admin-tags/admin-tags.component';
import {AdminBooksComponent} from './admin/components/admin-books/admin-books.component';
import {AdminAuthorsComponent} from './admin/components/admin-authors/admin-authors.component';
import {AdminUsersComponent} from './admin/components/admin-users/admin-users.component';
import {AdminComponent} from './admin/admin.component';
import {AdminGuard} from './admin/guards/admin.guard';


const adminRoutes: Routes = [
  { path: 'tags', component: AdminTagsComponent},
  { path: 'books', component: AdminBooksComponent},
  { path: 'authors', component: AdminAuthorsComponent},
  { path: 'users', component: AdminUsersComponent},
];

// определение маршрутов
const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'login', component: LoginComponent, canActivate : [AuthGuard]},
  { path: 'register', component: RegisterComponent, canActivate : [AuthGuard]},
  { path: 'profile', component: ProfileComponent, canActivate : [ProfileGuard]},
  { path: 'tags', component: TagsComponent},
  { path: 'tag/:id', component: TagDetailComponent},
  { path: 'authors', component: AuthorsComponent},
  { path: 'author/:id', component: AuthorsDetailsComponent},
  { path: 'search', component: SearchComponent},
  { path: 'book/:id', component: BookDetailComponent},
  { path: 'search/:searchRequest', component: SearchComponent},
  { path: 'error404', component: Error404Component},
  { path: 'admin', component: AdminComponent, children: adminRoutes, canActivate : [AdminGuard]},
  { path: '*', redirectTo: ''}
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
