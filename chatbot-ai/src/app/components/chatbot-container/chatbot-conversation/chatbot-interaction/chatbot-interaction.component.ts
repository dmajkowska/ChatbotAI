import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule} from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { ChatbotService } from '../../../../services/chatbot.service';
import { IChatbotPair } from '../../../../model/chatbot-pair';



export enum ReactionType {
  None = 1,
  Like,
  Dislike
}


@Component({
    selector: 'chatbot-interaction',
    templateUrl: './chatbot-interaction.component.html',
    styleUrls: ['./chatbot-interaction.component.scss'], 
    standalone: true,
    imports: [MatCardModule, MatIconModule, CommonModule, MatMenuModule, MatButtonModule  ]
  })

  export class ChatbotInteraction{
    @Input() public entry!: IChatbotPair;
    @Input() public firstAnswerPart: boolean = false;
    @Input() public reaction: ReactionType = ReactionType.None;

    public grade?: boolean = undefined;

    constructor(private chatbotService: ChatbotService) {}
    
    sendGrade(grade?: boolean) {
      this.grade = grade;
      this.chatbotService.rateAnswer(this.entry.id, grade).subscribe({
        next: () => console.log('Ocena wysłana!'),
        error: (err) => console.error('Błąd:', err)
      });
    }
  }

  