import { Message } from './message.interface';
import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';

const httpOptions ={
  headers:new HttpHeaders({'Content-Type':'Application/json'})
}
const apiUrl = 'https://localhost:44313/api/Message/all';

class Mes implements Message{
  id: string;
  from: string;
  timestamp: string;
  message: string;
  connectionId:string
}

@Injectable()
export class MessageService {

  constructor(private httpClient:HttpClient) { }
  mes:Mes;
  groupName:string;
  getAll()
  {
    return this.httpClient.get<Message[]>(apiUrl).pipe();
  }

  send()
  {
    this.mes = new Mes()
    this.mes.message="AAA";

    return this.httpClient.post<Mes>('https://localhost:44313/api/Message/send',this.mes).pipe();
  }
  addToGroup()
  {
    this.groupName = "fbb12f15-e823-45d8-931b-29ba01926ffa";
    return this.httpClient.post('https://localhost:44313/api/Message/group/add',this.groupName).pipe();
  }
}
