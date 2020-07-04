import { Injectable } from '@angular/core';

@Injectable()

export class ApiConfig {

  private apiUrl: string;

  constructor() {

    this.apiUrl = 'https://thebookshelf.azurewebsites.net';

  }

  getUrl() {

    return this.apiUrl;

  }

}
