import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
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
import { AuthorsComponent } from './authors/authors.component';
import { AuthorsDetailsComponent } from './authors-details/authors-details.component';
import {BookService} from '../services/bookService';
import {TagService} from '../services/tagService';
import {AuthorService} from '../services/authorService';


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
    AuthorsDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    AuthModule
  ],
  providers: [BookService, TagService, AuthorService],
  bootstrap: [AppComponent]
})
export class AppModule { }
