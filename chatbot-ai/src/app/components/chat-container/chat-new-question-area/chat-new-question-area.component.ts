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
    @Output() sendButtonClicked = new EventEmitter<string>(); 
    @Output() stopButtonClicked = new EventEmitter<boolean>(); 
    @Input() public state!: ChatState;

    sendQuestion() {
      this.sendButtonClicked.emit(this.question); 
      this.question = "";
    }

    stopAnswering() {
      this.stopButtonClicked.emit(true);
    }

  }