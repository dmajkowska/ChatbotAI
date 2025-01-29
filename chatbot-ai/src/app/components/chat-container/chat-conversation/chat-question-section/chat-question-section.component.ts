import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';



@Component({
    selector: 'chat-question-section',
    templateUrl: './chat-question-section.component.html',
    styleUrls: ['./chat-question-section.component.scss'], 
    standalone: true,
    imports: [MatCardModule, CommonModule]
  })

  export class ChatQuestionSection{
    @Input() public content: string = "";
   
  }