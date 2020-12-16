import {Component, OnInit} from '@angular/core';
import {HubConnection, HubConnectionBuilder, LogLevel} from '@aspnet/signalr';
import {Message} from './message.interface';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';

  private _hubConnection: HubConnection;
  msgs: Message[] = [];

  constructor() {
  }

  ngOnInit(): void {
    this._hubConnection = new HubConnectionBuilder()

      .withUrl('https://e-mobile-shop.azurewebsites.net/signalr')
      .configureLogging(LogLevel.Information)
      .build();
    this._hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));

    this._hubConnection.on('SendNofti', (data: Message) => {
      this.msgs.push(data);

    });
  }
}
