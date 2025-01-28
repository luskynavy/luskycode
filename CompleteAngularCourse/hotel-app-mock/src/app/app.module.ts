import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeModule } from './home/home.module';
import { ReservationModule } from './reservation/reservation.module';
//import { HttpClientModule } from '@angular/common/http'; //angular 16 for httpClient
import { provideHttpClient } from '@angular/common/http'; //angular 18 for httpClient

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HomeModule,
    ReservationModule,
    //HttpClientModule //angular 16 for httpClient
  
  ],
  //providers: [], //angular 16 for httpClient
  providers: [provideHttpClient()], //angular 18 for httpClient
  bootstrap: [AppComponent]
})
export class AppModule { }
