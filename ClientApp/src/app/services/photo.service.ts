import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { map } from 'rxjs/operators';

@Injectable()
export class PhotoService {

    constructor(private http: Http) { }

    upload(vehicleId, photo) {
        const formData = new FormData();
        formData.append('file', photo);
        return this.http.post(`/api/vehicles/${vehicleId}/photos`, formData).pipe(map(res => res.json()));
    }

    getPhotos(vehicleId) {
        return this.http.get(`/api/vehicles/${vehicleId}/photos`).pipe(map(res => res.json()));
    }
}
