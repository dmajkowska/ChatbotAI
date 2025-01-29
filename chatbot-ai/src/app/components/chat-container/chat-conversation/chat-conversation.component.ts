import { CommonModule } from '@angular/common';
import { Component, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ChatQuestionSection } from './chat-question-section/chat-question-section.component';
import { ChatAnswerSection, ReactionType } from './chat-answer-section/chat-answer-section.component';
import { ChatNewQuestionAreaComponent } from '../chat-new-question-area/chat-new-question-area.component';
import { HttpClientModule } from '@angular/common/http';
import { ChatService } from '../../../services/chat.service';
import { GenerateAnswerResponse } from '../../../model/generate-answer/generate-answer.response';

interface IQuestion {
  content: string;
}

interface IAnswer {
  id: number;
  content: string[];
}

export type ChatState = "Sending" | "Waiting";

@Component({
    selector: 'chat-conversation',
    templateUrl: './chat-conversation.component.html',
    styleUrls: ['./chat-conversation.component.scss'], 
    standalone: true,
    imports: [MatCardModule, CommonModule,ChatQuestionSection, ChatAnswerSection, ChatNewQuestionAreaComponent]
  })

  export class ChatConversationComponent {
    @ViewChild('scrollContainer') private scrollContainer!: ElementRef;
    @ViewChildren('lastMessage') private lastMessage!: QueryList<ElementRef>;

    public  entries: (IQuestion | IAnswer)[] = [];
    public enableSendingQuestion: boolean = true;
    public state: ChatState = 'Sending';

    constructor(private chatService: ChatService) {}

  ngAfterViewChecked() {
    this.scrollToBottom();
  }
  
  isQuestion(entry: IAnswer | IQuestion): boolean  {
     return !Array.isArray(entry.content);
  }

  castToQuestion(entry: IAnswer | IQuestion): IQuestion {
    return entry as IQuestion;
  }

  castToAnswer(entry: IAnswer | IQuestion): IAnswer {
    return entry as IAnswer;
  }

  castToArray(content: string | string[]): string[] {
    return Array.isArray(content) ? content : [];
  }

 
      
  castToString(content: string | string[]): string {
    return !Array.isArray(content) ? content : '';
  }

  onSendQuestionButtonClicked(question: string) {
    this.sendMessage(question);
  }


  pushQuestion(question: string) {
    this.entries.push({
      content: question
    })
  }


  pushAnswerLetterByLetter(answer: GenerateAnswerResponse) {
    this.entries.push({
      id: answer.id,
      content: []
    });

    let totalDelay = 0; 
    const deleteBetweenLetters = 10;
    const deleteBetweenSections = 500;

    answer.sectionList.forEach((section, sectionIndex) => {
      this.castToArray(this.entries[this.entries.length - 1].content).push('');
      
      let sectionDelay = totalDelay; 

      section.split('').forEach((char, charIndex) => {
        setTimeout(() => {
          this.castToArray(this.entries[this.entries.length - 1].content)[sectionIndex] += char;
        }, sectionDelay + charIndex * deleteBetweenLetters);
      });

      totalDelay += section.length * deleteBetweenLetters + deleteBetweenSections; // Dodatkowe 500ms pauzy między sekcjami
    });
  }

  sendMessage(question: string) {
    if (!question.trim()) return;
    this.state = 'Waiting';
    this.chatService.generateAnswer(question).subscribe({
      next: (data) => {
        this.pushQuestion(question);
        this.pushAnswerLetterByLetter(data);
        this.state = 'Sending';
      } ,
      error: (err) => console.error('Błąd:', err),
      complete: () => {
        
      }
    });
  }

  private scrollToBottom() {
    this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
  }
}