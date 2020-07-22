import { Component, OnInit } from '@angular/core';
import {UserService} from '../../../../services/userService';
import {AuthUser} from '../../../auth/models/auth.user';

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent implements OnInit {

  user: AuthUser = new AuthUser();   // изменяемый товар
  users: AuthUser[];                // массив товаров
  tableMode = true;          // табличный режим

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.loadUsers();    // загрузка данных при старте компонента
  }
  // получаем данные через сервис
  loadUsers() {
    this.userService.getUsers()
      .subscribe((data: AuthUser[]) => {
        this.users = data;
        console.log(data);
        console.log(this.users);
      });
  }
  delete(p: AuthUser) {
    this.userService.deleteUser(p.id)
      .subscribe(() => this.loadUsers());
  }

}
