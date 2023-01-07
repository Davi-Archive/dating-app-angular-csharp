import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;

  url: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }


  getUsers() {
    this.http.get(this.url + "users").subscribe({
      next: (res) => {
        this.users = res
        console.log(res);
      },
      error: err => console.log(err),
      complete: () => console.log('Request has completed')
    })
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event
  }

}
