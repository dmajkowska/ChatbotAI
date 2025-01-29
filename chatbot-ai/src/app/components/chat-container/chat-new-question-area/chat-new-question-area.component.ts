import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatIconModule} from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { ChatState } from '../chat-conversation/chat-conversation.component';

@Component({
    selector: 'chat-new-question-area',
    templateUrl: './chat-new-question-area.component.html',
    styleUrls: ['./chat-new-question-area.component.scss'], 
    standalone: true,
    imports: [MatCardModule, MatInputModule, MatFormFieldModule,  MatIconModule, CommonModule, FormsModule]
  })

  export class ChatNewQuestionAreaComponent {
    question: string = '';
    @Output() buttonClicked = new EventEmitter<string>(); 
    @Input() public state!: ChatState;

    sendQuestion() {
      this.buttonClicked.emit(this.question); 
      this.question = "";
    }
  }