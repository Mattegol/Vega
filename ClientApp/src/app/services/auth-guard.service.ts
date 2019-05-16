import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { CanActivate, Router } from '@angular/router';

@Injectable()
export class AuthGuardService implements CanActivate {

    constructor(public auth: AuthService, public router: Router) { }

    canActivate(): boolean {
        console.log(this.auth.isAuthenticated());
        if (!this.auth.isAuthenticated()) {
          this.router.navigate(['/vehicles']);
          return false;
        }
        return true;
      }
}
