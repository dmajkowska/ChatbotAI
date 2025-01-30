import { CommonModule } from '@angular/common';
import { Component, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ChatbotService } from '../../../services/chatbot.service';
import { GenerateAnswerResponse } from '../../../model/generate-answer/generate-answer.response';
import { ChatbotInteraction } from './chatbot-interaction/chatbot-interaction.component';
import { ChatbotNewQuestionAreaComponent } from './chatbot-new-question-area/chatbot-new-question-area.component';
import { IChatbotPair } from '../../../model/chatbot-pair';


export type ChatbotState = "Sending" | "Waiting";

@Component({
    selector: 'chatbot-conversation',
    templateUrl: './chatbot-conversation.component.html',
    styleUrls: ['./chatbot-conversation.component.scss'], 
    standalone: true,
    imports: [MatCardModule, CommonModule,ChatbotInteraction, ChatbotNewQuestionAreaComponent ]
  })

  export class ChatbotConversationComponent {
    @ViewChild('scrollContainer', { static: true }) scrollContainer!: ElementRef;
    private isUserScrolling = false;
    private observer!: MutationObserver;
    public  entries: IChatbotPair[] = [];
    public enableSendingQuestion: boolean = true;
    public state: ChatbotState = 'Sending';
    public isAnsweringStopped: boolean = false;

    constructor(private chatbotService: ChatbotService) {}

    ngAfterViewInit() {
      this.setupMutationObserver();
      this.setupScrollListener();
    }

    private setupMutationObserver() {
      if (typeof MutationObserver === 'undefined') return; 
  
      this.observer = new MutationObserver(() => {
        if (!this.isUserScrolling) {
          this.scrollToBottom();
        }
      });
  
      this.observer.observe(this.scrollContainer.nativeElement, { childList: true, subtree: true });
    }
  
    private setupScrollListener() {
      const container = this.scrollContainer.nativeElement;
      container.addEventListener('scroll', () => {
        const isAtBottom =
          container.scrollHeight - container.scrollTop <= container.clientHeight + 10;
        this.isUserScrolling = !isAtBottom; 
      });
    }

  onSendQuestionButtonClicked(question: string) {
    this.sendMessage(question);
  }

  onStopAnsweringButtonClicked() {
    this.isAnsweringStopped = true;
  }


  async pushChatbotPair(response: GenerateAnswerResponse) {
    this.state = 'Waiting';
    this.isAnsweringStopped = false;

    const entry: IChatbotPair = {
      id: response.id,
      question: response.question,
      answer: [],
      rating: undefined
    };

    this.entries.push(entry);

    let charCount = 0;
    const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

    for (let sectionIndex = 0; sectionIndex < response.sectionList.length; sectionIndex++) {
      this.entries[this.entries.length - 1].answer.push('');
      for (const char of response.sectionList[sectionIndex]) {
        if (this.isAnsweringStopped) break;
        this.entries[this.entries.length - 1].answer[sectionIndex] += char;
        charCount++;
        await delay(10);
      }
      if (this.isAnsweringStopped) break;
      charCount += 2; // Dodajemy 2 znaki za "\n\n"
      await delay(500); // Pauza między sekcjami
    }

    if(this.isAnsweringStopped) {
      this.chatbotService.truncateAnswer(entry.id, charCount).subscribe({
        next: () => console.log('Operacja przerwana!'),
        error: (err) => console.error('Błąd:', err)
      });
      this.isAnsweringStopped = false;
    }
    this.state = 'Sending';
}

  sendMessage(question: string) {
    if (!question.trim()) return;
    this.chatbotService.generateAnswer(question).subscribe({
      next: (response) => {
        this.pushChatbotPair(response);
      } ,
      error: (err) => console.error('Błąd:', err),
    });
  }

private scrollToBottom() {
    setTimeout(() => {
      this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
    }, 0);
  }
}