import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'client';
  users: any;
  url="https://localhost:4001/api";

  constructor(private http: HttpClient){}

  ngOnInit() {
   this.getUsers();
  }

  getUsers(){
    this.http.get(this.url+'/users').subscribe(res=>{
      this.users = res;
    },err=>{
      console.log(err);
    });
  }
}
