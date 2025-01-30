import { Component } from '@angular/core';
import { ChatHeaderComponent } from './chat-header/chat-header.component';
import { MatCardModule} from '@angular/material/card';
import { ChatConversationComponent } from './chat-conversation/chat-conversation.component';

@Component({
  selector: 'chat-container',
  templateUrl: './chat-container.component.html',
  styleUrls: ['./chat-container.component.scss'], 
  standalone: true,
  imports: [ChatHeaderComponent, ChatConversationComponent, MatCardModule]
})

export class ChatContainerComponent {

}