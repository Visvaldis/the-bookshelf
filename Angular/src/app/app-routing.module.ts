import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from './components/home/home.component';
import {TagsComponent} from './components/tags/tags.component';
import {LoginComponent} from './auth/components/login/login.component';
import {RegisterComponent} from './auth/components/register/register.component';

import {AuthGuard} from './auth/guards/auth.guard';
import {ProfileComponent} from './auth/components/profile/profile.component';
import {ProfileGuard} from './auth/guards/profile.guard';
import {SearchComponent} from './components/search/search.component';
import {BookDetailComponent} from './components/book-detail/book-detail.component';
import {Error404Component} from './components/error404/error404.component';
import {TagDetailComponent} from './components/tag-detail/tag-detail.component';
import {AuthorsDetailsComponent} from './authors-details/authors-details.component';
import {AuthorsComponent} from './authors/authors.component';

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
