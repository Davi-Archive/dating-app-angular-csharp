import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/models/member';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.scss']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: Member | undefined;

  constructor() { }

  ngOnInit(): void {
  }

}
