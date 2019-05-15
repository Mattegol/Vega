import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  profile: any;

  constructor(public auth: AuthService) {
    auth.handleAuthentication();
  }

  // tslint:disable-next-line:use-life-cycle-interface
  ngOnInit() {
    if (this.auth.isAuthenticated()) {
      this.auth.renewTokens();
    }
  }
}
