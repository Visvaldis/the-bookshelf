import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './components/app.component';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { HomeComponent } from './components/home/home.component';
import { TagsComponent } from './components/tags/tags.component';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {AuthModule} from './auth/auth.module';
import { BookCardComponent } from './components/book-card/book-card.component';
import { SearchComponent } from './components/search/search.component';
import { BookDetailComponent } from './components/book-detail/book-detail.component';
import { Error404Component } from './components/error404/error404.component';
import { TagDetailComponent } from './components/tag-detail/tag-detail.component';
import { AuthorsComponent } from './components/authors/authors.component';
import { AuthorsDetailsComponent } from './components/authors-details/authors-details.component';
import {BookService} from '../services/bookService';
import {TagService} from '../services/tagService';
import {AuthorService} from '../services/authorService';
import { ProfileComponent } from './components/profile/profile.component';
import { NgxSpinnerModule } from 'ngx-spinner';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {AdminModule} from './admin/admin.module';
import {UserService} from '../services/userService';


@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    HeaderComponent,
    HomeComponent,
    TagsComponent,
    BookCardComponent,
    SearchComponent,
    BookDetailComponent,
    Error404Component,
    TagDetailComponent,
    AuthorsComponent,
    AuthorsDetailsComponent,
    ProfileComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    AuthModule,
    AdminModule,
    NgxSpinnerModule,
    BrowserAnimationsModule,
  ],
  providers: [BookService, TagService, AuthorService, UserService],
  exports: [
    BookCardComponent
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  bootstrap: [AppComponent]
})
export class AppModule { }
