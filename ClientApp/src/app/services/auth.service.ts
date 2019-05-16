// src/app/auth/auth.service.ts

// import Auth0Lock from 'auth0-lock';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as auth0 from 'auth0-js';

(window as any).global = window;
@Injectable()
export class AuthService {
  userProfile: any;

  private _scopes: string;
  requestedScopes = 'openid profile read:admin write:admin';

  private _idToken: string;
  private _accessToken: string;
  private _expiresAt: number;

  auth0 = new auth0.WebAuth({
    clientID: 't2TdmklnoCmWGkIf1M1JKjDyov9XJkAr',
    domain: 'mattegol.eu.auth0.com',
    responseType: 'token id_token',
    redirectUri: 'https://localhost:5001/callback',
    scope: this.requestedScopes
  });

  constructor(public router: Router) {
    console.log(this.requestedScopes);
  }

  get accessToken(): string {
    return this._accessToken;
  }

  get idToken(): string {
    return this._idToken;
  }

  public login(): void {
    this.auth0.authorize();
  }

  public handleAuthentication(): void {
    this.auth0.parseHash((err, authResult) => {
      if (authResult && authResult.accessToken && authResult.idToken) {
        this.localLogin(authResult);
        this.router.navigate(['/vehicles']);
      } else if (err) {
        this.router.navigate(['/vehicles']);
        console.log(`Error: ${err.error}. Check the console for further details.`);
      }
    });
  }

  private localLogin(authResult): void {
    // const scopes = authResult.scope || this.requestedScopes || '';
    const scopes = this.requestedScopes || '';

    // Set the time that the access token will expire at
    const expiresAt = (authResult.expiresIn * 1000) + Date.now();
    this._accessToken = authResult.accessToken;
    this._idToken = authResult.idToken;
    this._expiresAt = expiresAt;

    this._scopes = JSON.stringify(scopes);
  }

  public userHasScopes(scopes: Array<string>): boolean {
    const grantedScopes = JSON.parse(this._scopes).split(' ');
    console.log('Granted Scopes:', grantedScopes);
    return scopes.every(scope => grantedScopes.includes(scope));
  }

  public getProfile(cb): void {
    if (!this._accessToken) {
      throw new Error('Access Token must exist to fetch profile');
    }

    const self = this;
    this.auth0.client.userInfo(this._accessToken, (err, profile) => {
      if (profile) {
        self.userProfile = profile;
      }
      cb(err, profile);
    });
  }

  public renewTokens(): void {
    this.auth0.checkSession({}, (err, authResult) => {
       if (authResult && authResult.accessToken && authResult.idToken) {
         this.localLogin(authResult);
       } else if (err) {
         alert(`Could not get a new token (${err.error}: ${err.error_description}).`);
         this.logout();
       }
    });
  }

  public logout(): void {
    // Remove tokens and expiry time
    this._accessToken = '';
    this._idToken = '';
    this._expiresAt = 0;
    this.userProfile = null;

    this.auth0.logout({
      returnTo: window.location.origin
    });
  }

  public isAuthenticated(): boolean {
    // Check whether the current time is past the
    // access token's expiry time
    return this._accessToken && Date.now() < this._expiresAt;
  }
}
