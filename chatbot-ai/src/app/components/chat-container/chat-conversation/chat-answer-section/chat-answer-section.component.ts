import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import {MatIconModule} from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { ChatService } from '../../../../services/chat.service';
import { HttpClientModule } from '@angular/common/http';


export enum ReactionType {
  None = 1,
  Like,
  Dislike
}


@Component({
    selector: 'chat-answer-section',
    templateUrl: './chat-answer-section.component.html',
    styleUrls: ['./chat-answer-section.component.scss'], 
    standalone: true,
    imports: [MatCardModule, MatIconModule, CommonModule, MatMenuModule, MatButtonModule  ]
  })

  export class ChatAnswerSection{
    @Input() public content: string[] = [];
    @Input() public firstAnswerPart: boolean = false;
    @Input() public reaction: ReactionType = ReactionType.None;
    @Input() public id: number = 0;

    public grade?: boolean = undefined;

    constructor(private chatService: ChatService) {}
    
    sendGrade(grade?: boolean) {
      this.grade = grade;
      this.chatService.rateAnswer(this.id, grade).subscribe({
        next: () => console.log('Ocena wysłana!'),
        error: (err) => console.error('Błąd:', err)
      });
    }
  }

  