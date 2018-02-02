"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var http_2 = require("@angular/http");
var Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/map");
require("rxjs/add/operator/toPromise");
var team_service_1 = require("./team.service");
var AppService = /** @class */ (function () {
    function AppService(http, teamService) {
        this.http = http;
        this.teamService = teamService;
        this.url = "/api/cockFighting";
    }
    AppService.prototype.initRegister = function () {
        return {
            isEdit: false,
            userName: '',
            phone: '',
            email: '',
            address: '',
            team: this.teamService.initTeam(),
            teams: [this.teamService.initTeam()]
        };
    };
    AppService.prototype.exportTeamsWithPromise = function () {
        var getUrl = this.url + "/export";
        return this.http.get(getUrl).toPromise()
            .then(this.extractData)
            .catch(this.handleErrorPromise);
    };
    AppService.prototype.removeRegisterWithPromise = function (phone) {
        var headers = new http_2.Headers({ 'Content-Type': 'application/json' });
        var options = new http_2.RequestOptions({ headers: headers });
        var delUrl = this.url + '/delete/' + phone;
        return this.http.get(delUrl, options).toPromise()
            .then(this.extractData)
            .catch(this.handleErrorPromise);
    };
    AppService.prototype.addRegisterWithPromise = function (register) {
        var headers = new http_2.Headers({ 'Content-Type': 'application/json' });
        var options = new http_2.RequestOptions({ headers: headers });
        return this.http.post(this.url, register, options).toPromise()
            .then(this.extractData)
            .catch(this.handleErrorPromise);
    };
    AppService.prototype.getRegistersWithPromise = function () {
        return this.http.get(this.url).toPromise()
            .then(this.extractData)
            .catch(this.handleErrorPromise);
    };
    AppService.prototype.extractData = function (res) {
        var body = res.json();
        return body.data || {};
    };
    AppService.prototype.handleErrorObservable = function (error) {
        console.error(error.message || error);
        return Observable_1.Observable.throw(error.message || error);
    };
    AppService.prototype.handleErrorPromise = function (error) {
        console.error(error.message || error);
        return Promise.reject(error.message || error);
    };
    AppService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.Http, team_service_1.TeamService])
    ], AppService);
    return AppService;
}());
exports.AppService = AppService;
//# sourceMappingURL=app.service.js.map