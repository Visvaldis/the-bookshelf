import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import {Routes, RouterModule} from '@angular/router';

import { AppComponent } from './app.component';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { TagsComponent } from './tags/tags.component';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {ApiConfig} from '../apiConfig';

// определение маршрутов
const appRoutes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'tags', component: TagsComponent},
  { path: '**', redirectTo: ''}
];

@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    HeaderComponent,
    HomeComponent,
    TagsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule.forRoot(appRoutes),
    FormsModule,
    HttpClientModule
  ],
  providers: [ApiConfig],
  bootstrap: [AppComponent]
})
export class AppModule { }
