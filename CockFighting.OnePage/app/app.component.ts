import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AppService } from './app.service';
import { TeamService } from './team.service'
import { MatchService } from './match.service'

export class Register {
    phone: string;
    userName: string;
    email: string;
    address: string;
    teams: Team[];
    team: Team;
    isEdit: boolean;
    constructor() {
    }
}

export class Team {
    id: number;
    name: string;
    userPhone: string;
    cocks: Cock[];

    constructor() {
    }
}

export class Cock {
    id: number;
    code: string;
    weight: number;

    constructor() {
    }
}

export class Match {
    id: number;
    cock1: Cock;
    cock2: Cock;
    constructor() {
    }
}

@Component({
    selector: 'my-app',
    templateUrl: 'app.component.html',
    styleUrls: ['app.component.css'],
    moduleId: module.id
})

export class AppComponent implements OnInit {
    registers: Register[];
    diff = 25;
    isRegen = false;
    isAuth = false;
    userName = '';
    password = '';
    isLoading = false;
    matches: Match[];
    excel: string;
    teamsExcel: string;
    defaultTeam = this.teamService.initTeam();
    register = new Register();
    errorMessage: string;
    constructor(private appService: AppService,
        private teamService: TeamService,
        private matchService: MatchService
    ) { }

    ngOnInit(): void {
        this.matches = [];
        this.register = this.appService.initRegister();
        this.register.team = this.register.teams[0];
        if (this.isAuth) {
            this.fetchRegisterData();
            this.genMatches();
        }
        
    };

    onSelect(register: Register): void {

        this.register = register;
        this.register.isEdit = true;
        this.register.team = this.register.teams[0];
    }

    addTeam(): void {
        let newTeam = this.teamService.initTeam()
        this.register.team = newTeam;
        this.register.teams.push(newTeam);

    }

    submitRegister(): void {
        this.appService.addRegisterWithPromise(this.register)
            .then(r => { this.reset(); },
            error => this.errorMessage = <any>error);
    }
    removeRegister(phone: string): void {
        if (confirm("Are you sure")) {
            this.appService.removeRegisterWithPromise(phone)
                .then(r => { this.reset() },
                error => this.errorMessage = <any>error);
        }
    }
    removeTeam(id: number): void {
        if (confirm("Are you sure")) {
            this.teamService.removeTeamWithPromise(id)
                .then(r => { this.reset() },
                error => this.errorMessage = <any>error);

        }
    }

    fetchRegisterData(): void {
        this.isLoading = true;
        this.appService.getRegistersWithPromise()
            .then(registers => { this.registers = registers; this.isLoading = false; },
            error => { this.errorMessage = <any>error; this.isLoading = false; });
    }

    genMatches(): void {
        this.isLoading = true;
        this.matchService.getMatchesWithPromise(this.isRegen, this.diff)
            .then(matches => { this.matches = matches; this.isLoading = false; },
            error => { this.errorMessage = <any>error; this.isLoading = false; });
        this.excel = "";
    }

    exportMatches(): void {
        this.isLoading = true;
        this.matchService.exportMatchesWithPromise()
            .then(excel => { this.excel = excel; this.isLoading = false; },
            error => { this.errorMessage = <any>error; this.isLoading = false; });
    }
    exportTeams(): void {
        this.isLoading = true;
        this.appService.exportTeamsWithPromise()
            .then(excel => { this.teamsExcel = excel; this.isLoading = false; },
            error => { this.errorMessage = <any>error; this.isLoading = false; });
    }


    reset(): void {        
        this.register = this.appService.initRegister();
        this.excel = "";
        this.teamsExcel = "";        
        this.matches = [];
        this.fetchRegisterData();
        this.isLoading = false;
        this.isRegen = false;
    }

    login(): void {
        if (this.userName.toLowerCase() == 'admin' && this.password == '1234qwe@') {
            this.isAuth = true;
            this.reset();
            this.genMatches();
        }
        else {
            this.errorMessage = 'Login Failed.'
        }
    }

    Log(): void {
        console.log(this.register);
    }
}
