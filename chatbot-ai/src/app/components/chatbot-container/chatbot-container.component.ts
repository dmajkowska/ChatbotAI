import { Component } from '@angular/core';
import { ChatbotHeaderComponent } from './chatbot-header/chatbot-header.component';
import { MatCardModule} from '@angular/material/card';
import { ChatbotConversationComponent } from './chatbot-conversation/chatbot-conversation.component';

@Component({
  selector: 'chatbot-container',
  templateUrl: './chatbot-container.component.html',
  styleUrls: ['./chatbot-container.component.scss'], 
  standalone: true,
  imports: [ChatbotHeaderComponent, ChatbotConversationComponent, MatCardModule]
})

export class ChatbotContainerComponent {

}