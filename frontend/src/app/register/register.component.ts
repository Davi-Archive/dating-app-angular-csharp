import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  @Input() usersFromHomeComponent: any;
  model: any = {}

  constructor() { }

  ngOnInit(): void {
  }

  register() {
    console.log("method not implemented");

  }
  cancel() {
    console.log("mthod not impl");

  }

}
