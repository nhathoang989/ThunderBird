import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Headers, RequestOptions } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

import { Register, Team, Cock } from './app.component'
import { TeamService } from './team.service'

@Injectable()
export class AppService {
    url = "/api/cockFighting";
    constructor(private http: Http, private teamService: TeamService) { }


    initRegister(): Register {
        return {
            isEdit: false,
            userName: '',
            phone: '',
            email: '',
            address: '',
            team: this.teamService.initTeam(),
            teams: [this.teamService.initTeam()]
        };
    }

    exportTeamsWithPromise(): Promise<string> {
        let getUrl = this.url + "/export";
        return this.http.get(getUrl).toPromise()
            .then(this.extractData)
            .catch(this.handleErrorPromise);
    }

    removeRegisterWithPromise(phone: string): Promise<Register> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        let delUrl = this.url + '/delete/' + phone;
        return this.http.get(delUrl, options).toPromise()
            .then(this.extractData)
            .catch(this.handleErrorPromise);
    }

    addRegisterWithPromise(register: Register): Promise<Register> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this.http.post(this.url, register, options).toPromise()
            .then(this.extractData)
            .catch(this.handleErrorPromise);
    }

    getRegistersWithPromise(): Promise<Register[]> {
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
