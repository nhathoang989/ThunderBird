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
var app_service_1 = require("./app.service");
var team_service_1 = require("./team.service");
var match_service_1 = require("./match.service");
var Register = /** @class */ (function () {
    function Register() {
    }
    return Register;
}());
exports.Register = Register;
var Team = /** @class */ (function () {
    function Team() {
    }
    return Team;
}());
exports.Team = Team;
var Cock = /** @class */ (function () {
    function Cock() {
    }
    return Cock;
}());
exports.Cock = Cock;
var Match = /** @class */ (function () {
    function Match() {
    }
    return Match;
}());
exports.Match = Match;
var AppComponent = /** @class */ (function () {
    function AppComponent(appService, teamService, matchService) {
        this.appService = appService;
        this.teamService = teamService;
        this.matchService = matchService;
        this.diff = 25;
        this.isRegen = false;
        this.isAuth = false;
        this.userName = '';
        this.password = '';
        this.isLoading = false;
        this.defaultTeam = this.teamService.initTeam();
        this.register = new Register();
    }
    AppComponent.prototype.ngOnInit = function () {
        this.matches = [];
        this.register = this.appService.initRegister();
        this.register.team = this.register.teams[0];
        if (this.isAuth) {
            this.fetchRegisterData();
            this.genMatches();
        }
    };
    ;
    AppComponent.prototype.onSelect = function (register) {
        this.register = register;
        this.register.isEdit = true;
        this.register.team = this.register.teams[0];
    };
    AppComponent.prototype.addTeam = function () {
        var newTeam = this.teamService.initTeam();
        this.register.team = newTeam;
        this.register.teams.push(newTeam);
    };
    AppComponent.prototype.submitRegister = function () {
        var _this = this;
        this.appService.addRegisterWithPromise(this.register)
            .then(function (r) { _this.reset(); }, function (error) { return _this.errorMessage = error; });
    };
    AppComponent.prototype.removeRegister = function (phone) {
        var _this = this;
        if (confirm("Are you sure")) {
            this.appService.removeRegisterWithPromise(phone)
                .then(function (r) { _this.reset(); }, function (error) { return _this.errorMessage = error; });
        }
    };
    AppComponent.prototype.removeTeam = function (id) {
        var _this = this;
        if (confirm("Are you sure")) {
            this.teamService.removeTeamWithPromise(id)
                .then(function (r) { _this.reset(); }, function (error) { return _this.errorMessage = error; });
        }
    };
    AppComponent.prototype.fetchRegisterData = function () {
        var _this = this;
        this.isLoading = true;
        this.appService.getRegistersWithPromise()
            .then(function (registers) { _this.registers = registers; _this.isLoading = false; }, function (error) { _this.errorMessage = error; _this.isLoading = false; });
    };
    AppComponent.prototype.genMatches = function () {
        var _this = this;
        this.isLoading = true;
        this.matchService.getMatchesWithPromise(this.isRegen, this.diff)
            .then(function (matches) { _this.matches = matches; _this.isLoading = false; }, function (error) { _this.errorMessage = error; _this.isLoading = false; });
        this.excel = "";
    };
    AppComponent.prototype.exportMatches = function () {
        var _this = this;
        this.isLoading = true;
        this.matchService.exportMatchesWithPromise()
            .then(function (excel) { _this.excel = excel; _this.isLoading = false; }, function (error) { _this.errorMessage = error; _this.isLoading = false; });
    };
    AppComponent.prototype.exportTeams = function () {
        var _this = this;
        this.isLoading = true;
        this.appService.exportTeamsWithPromise()
            .then(function (excel) { _this.teamsExcel = excel; _this.isLoading = false; }, function (error) { _this.errorMessage = error; _this.isLoading = false; });
    };
    AppComponent.prototype.reset = function () {
        this.register = this.appService.initRegister();
        this.excel = "";
        this.teamsExcel = "";
        this.matches = [];
        this.fetchRegisterData();
        this.isLoading = false;
        this.isRegen = false;
    };
    AppComponent.prototype.login = function () {
        if (this.userName.toLowerCase() == 'admin' && this.password == '1234qwe@') {
            this.isAuth = true;
            this.reset();
            this.genMatches();
        }
        else {
            this.errorMessage = 'Login Failed.';
        }
    };
    AppComponent.prototype.Log = function () {
        console.log(this.register);
    };
    AppComponent = __decorate([
        core_1.Component({
            selector: 'my-app',
            templateUrl: 'app.component.html',
            styleUrls: ['app.component.css'],
            moduleId: module.id
        }),
        __metadata("design:paramtypes", [app_service_1.AppService,
            team_service_1.TeamService,
            match_service_1.MatchService])
    ], AppComponent);
    return AppComponent;
}());
exports.AppComponent = AppComponent;
//# sourceMappingURL=app.component.js.map