import { Injectable } from '@angular/core';

import { Resolve, ActivatedRouteSnapshot } from '@angular/router';

import {RouterService} from '../services/router/router.service';
import {UserService} from '../services/users/user.service';
import {User} from '../models/user';

@Injectable({ providedIn: 'root' })
export class UserByIdResolver implements Resolve<User> {
  constructor(private userService: UserService, private routerService: RouterService) {}

  resolve(route: ActivatedRouteSnapshot) {
    const idParam = this.routerService.idParam;
    const id = Number(route.paramMap.get(idParam));
    const user = this.userService.getUser(id);
    return user;
  }
}
