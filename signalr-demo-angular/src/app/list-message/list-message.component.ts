import { MessageService } from './../message.service';
import { Component, OnInit } from '@angular/core';
import { Message } from '../message.interface';

@Component({
  selector: 'app-list-message',
  templateUrl: './list-message.component.html',
  styleUrls: ['./list-message.component.css']
})
export class ListMessageComponent implements OnInit {

  messages: Message[] = [];

  constructor(private messageService:MessageService) { }

  ngOnInit() {
    this.messageService.getAll().subscribe(
      (result:any)=>{
        this.messages = result
      }
    );
  }

}
