import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Headers, RequestOptions } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

import { Team } from './app.component'
import { Cock } from './app.component'

@Injectable()
export class TeamService {
    url = "/api/Team";
    constructor(private http: Http) { }
    
    initTeam(): Team{
        return {
            id: 0,
            userPhone: '',
            name: '',
            cocks: [
                this.initCock(),
                this.initCock(),
                this.initCock(),
                this.initCock(),
            ]
        };
    }

    initCock(): Cock {
        return { id: 0, code: '', weight: 0 };
    }

    removeTeamWithPromise(id: number): Promise<Team> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        let delUrl = this.url + '/delete/' + id;
        return this.http.get(delUrl, options).toPromise()
            .then(this.extractData)
            .catch(this.handleErrorPromise);
    }

    submitTeamWithPromise(Team: Team): Promise<Team> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this.http.post(this.url, Team, options).toPromise()
            .then(this.extractData)
            .catch(this.handleErrorPromise);
    }	
    getTeamsWithPromise(): Promise<Team[]> {
        return this.http.get(this.url).toPromise()
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
