import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title: string = 'Dating App';
  users: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get("https://localhost:8080/api/users").subscribe({
      next: (res) => this.users = res,
      error: err => console.log(err),
      complete: () => console.log('Request has completed')
    })
  }
}
