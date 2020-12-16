import { AddMessageComponent } from './add-message/add-message.component';
import { HttpClient, HttpClientModule, HttpHandler } from '@angular/common/http';
import { MessageService } from './message.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { ListMessageComponent } from './list-message/list-message.component';


@NgModule({
  declarations: [
    AppComponent,
    ListMessageComponent,
    AddMessageComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [MessageService, HttpClient],
  bootstrap: [AppComponent]
})
export class AppModule { }
