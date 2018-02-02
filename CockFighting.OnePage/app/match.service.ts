import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Headers, RequestOptions } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

import { Match } from './app.component'

@Injectable()
export class MatchService {
    url = "/api/Match/";
    constructor(private http: Http) { }

    getMatchesWithPromise(isRegen: boolean, diff: number): Promise<Match[]> {
        let getUrl = this.url + isRegen + "/" + diff;
        return this.http.get(getUrl).toPromise()
            .then(this.extractData)
            .catch(this.handleErrorPromise);
    }

    exportMatchesWithPromise(): Promise<string> {
        let getUrl = this.url;
        return this.http.get(getUrl).toPromise()
            .then(this.extractData)
            .catch(this.handleErrorPromise);
    }
   
    private extractData(res: Response) {
        let body = res.json();

        return body.data || {};
    }

    private handleErrorObservable(error: Response | any) {
        console.error(error.message || error);
        return Observable.throw(error.message || error);
    }

    private handleErrorPromise(error: Response | any) {
        console.error(error.message || error);
        return Promise.reject(error.message || error);
    }	
}
