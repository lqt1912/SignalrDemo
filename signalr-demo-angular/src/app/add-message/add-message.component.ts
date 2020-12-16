import { Message } from './../message.interface';

import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MessageService } from './../message.service';

@Component({
  selector: 'app-add-message',
  templateUrl: './add-message.component.html',
  styleUrls: ['./add-message.component.css']
})



export class AddMessageComponent implements OnInit {

   apiUrl = 'https://localhost:44313/api/Message/send';



  constructor(private httpClient:HttpClient, private messageService:MessageService) { }

  ngOnInit() {

  }

  sendMessage()
  {
   
    this.messageService.send().subscribe((reponse)=>{
      console.log(reponse);
     }); ;
    console.log("CLicked");
  }

  addToGroup()
  {
    this.messageService.addToGroup().subscribe((respose)=>{
      console.log(respose);
    })
  }

}
