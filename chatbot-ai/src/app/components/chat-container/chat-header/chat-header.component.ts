import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

@Component({
    selector: 'chat-header',
    templateUrl: './chat-header.component.html',
    styleUrls: ['./chat-header.component.scss'], 
    standalone: true,
    imports: [MatCardModule]
  })

  export class ChatHeaderComponent {

  }