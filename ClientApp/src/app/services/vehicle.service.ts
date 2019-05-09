import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private readonly vehiclesEndpoint = '/api/vehicles';

constructor(private http: Http) { }

  getMakes() {
    return this.http.get('/api/makes').pipe(map(res => res.json()));
  }

  getFeatures() {
    return this.http.get('/api/features').pipe(map(res => res.json()));
  }

  create(vehicle) {
    return this.http.post(this.vehiclesEndpoint, vehicle).pipe(map(res => res.json()));
  }

  update(vehicle) {
    return this.http.put(this.vehiclesEndpoint + '/' + vehicle.id, vehicle).pipe(map(res => res.json()));
  }

  getVehicle(id) {
    return this.http.get(this.vehiclesEndpoint + id).pipe(map(res => res.json()));
  }

  getVehicles(filter) {
    return this.http.get(this.vehiclesEndpoint + '?' + this.toQueryString(filter)).pipe(map(res => res.json()));
  }

  toQueryString(obj) {
    const parts = [];
    // tslint:disable-next-line:forin
    for (const property in obj) {
      const value = obj[property];
      if (value != null && value !== undefined) {
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value))
      }
    }

    return parts.join('&');
  }

  delete(id) {
    return this.http.delete(this.vehiclesEndpoint + id).pipe(map(res => res.json()));
  }

}
