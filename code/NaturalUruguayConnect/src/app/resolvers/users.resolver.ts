import { Injectable } from '@angular/core';

import { Resolve, ActivatedRouteSnapshot } from '@angular/router';

import {RouterService} from '../services/router/router.service';
import {UserService} from '../services/users/user.service';
import {User} from '../models/user';

@Injectable({ providedIn: 'root' })
export class UsersResolver implements Resolve<{data: User[]}> {
  constructor(private userService: UserService, private routerService: RouterService) {}

  resolve(route: ActivatedRouteSnapshot) {

    const users = this.userService.getUsers();
    return users;
  }
}
