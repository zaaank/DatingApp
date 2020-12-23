import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {
  baseUrl = 'https://localhost:4001/api/';
  validationErrors: string[]= [];

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  get404(){
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe(res=>{
      console.log(res);
    }, err=>{
      console.log(err);
    })
  }
  get400(){
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe(res=>{
      console.log(res);
    }, err=>{
      console.log(err);
    })
  }
  get500(){
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe(res=>{
      console.log(res);
    }, err=>{
      console.log(err);
    })
  }
  get401(){
    this.http.get(this.baseUrl + 'buggy/auth').subscribe(res=>{
      console.log(res);
    }, err=>{
      console.log(err);
    })
  }
  get400ValidationError(){
    this.http.post(this.baseUrl + 'account/register',{}).subscribe(res=>{
      console.log(res);
    }, err=>{
      console.log(err);
      this.validationErrors = err;
    })
  }

}
