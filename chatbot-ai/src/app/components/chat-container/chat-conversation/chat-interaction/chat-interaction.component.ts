import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import {MatIconModule} from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { ChatService } from '../../../../services/chat.service';
import { IChatPair } from '../../../../model/chat-pair';



export enum ReactionType {
  None = 1,
  Like,
  Dislike
}


@Component({
    selector: 'chat-interaction',
    templateUrl: './chat-interaction.component.html',
    styleUrls: ['./chat-interaction.component.scss'], 
    standalone: true,
    imports: [MatCardModule, MatIconModule, CommonModule, MatMenuModule, MatButtonModule  ]
  })

  export class ChatInteraction{
    @Input() public entry!: IChatPair;
    @Input() public firstAnswerPart: boolean = false;
    @Input() public reaction: ReactionType = ReactionType.None;

    public grade?: boolean = undefined;

    constructor(private chatService: ChatService) {}
    
    sendGrade(grade?: boolean) {
      this.grade = grade;
      this.chatService.rateAnswer(this.entry.id, grade).subscribe({
        next: () => console.log('Ocena wysłana!'),
        error: (err) => console.error('Błąd:', err)
      });
    }
  }

  