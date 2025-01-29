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
    public isAnsweringStopped: boolean = false;

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

  onStopAnsweringButtonClicked() {
    this.isAnsweringStopped = true;
  }

  pushQuestion(question: string) {
    this.entries.push({
      content: question
    })
  }


  async pushAnswerLetterByLetter(answer: GenerateAnswerResponse) {
    this.state = 'Waiting';
    this.isAnsweringStopped = false;
    this.entries.push({
        id: answer.id,
        content: []
    });

    let charCount = 0;
    const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

    for (let sectionIndex = 0; sectionIndex < answer.sectionList.length; sectionIndex++) {
        

        this.castToArray(this.entries[this.entries.length - 1].content).push('');
        for (const char of answer.sectionList[sectionIndex]) {
            if (this.isAnsweringStopped) break;

            this.castToArray(this.entries[this.entries.length - 1].content)[sectionIndex] += char;
            charCount++;
            await delay(10);
        }
        if (this.isAnsweringStopped) break;
        charCount += 2; // Dodajemy 2 znaki za "\n\n"
        await delay(500); // Pauza między sekcjami
    }

    if(this.isAnsweringStopped) {
      this.chatService.truncateAnswer(answer.id, charCount).subscribe({
        next: () => console.log('Operacja przerwana!'),
        error: (err) => console.error('Błąd:', err)
      });
      this.isAnsweringStopped = false;
    }


    this.state = 'Sending';
    


}

  sendMessage(question: string) {
    if (!question.trim()) return;
    this.chatService.generateAnswer(question).subscribe({
      next: (data) => {
        this.pushQuestion(question);
        this.pushAnswerLetterByLetter(data);
      } ,
      error: (err) => console.error('Błąd:', err),
    });
  }

  private scrollToBottom() {
    this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
  }
}