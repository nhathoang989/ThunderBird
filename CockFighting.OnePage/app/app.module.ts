import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; // <-- NgModel lives here
import { AppComponent } from './app.component';
import { HttpModule } from '@angular/http';
import { AppService } from './app.service';
import { TeamService } from './team.service';
import { MatchService } from './match.service';
@NgModule({
    imports: [
        BrowserModule
        , FormsModule // <-- import the FormsModule before binding with [(ngModel)]
        , HttpModule
    ],
    declarations: [AppComponent],
    providers: [AppService, TeamService, MatchService],
    bootstrap: [AppComponent]
})
export class AppModule { }
