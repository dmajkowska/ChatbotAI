import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

@Component({
    selector: 'chatbot-header',
    templateUrl: './chatbot-header.component.html',
    styleUrls: ['./chatbot-header.component.scss'], 
    standalone: true,
    imports: [MatCardModule]
  })

export class ChatbotHeaderComponent {}