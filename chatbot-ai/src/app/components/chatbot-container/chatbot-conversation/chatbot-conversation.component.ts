import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ChatbotService } from '../../../services/chatbot.service';
import { InteractWithChatbotResponse } from '../../../model/interact-with-chatbot/interact-with-chatbot.response';
import { ChatbotInteraction } from './chatbot-interaction/chatbot-interaction.component';
import { ChatbotNewQuestionAreaComponent } from './chatbot-new-question-area/chatbot-new-question-area.component';
import { IChatbotPair } from '../../../model/chatbot-pair';
import { filter, Observable } from 'rxjs';


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
  isAnswerComplete = false;
  isAnswerTruncated = false;

  constructor(private chatbotService: ChatbotService) {}

  ngOnInit() {
    this.chatbotService.isAnswerComplete$.subscribe(isComplete => {
      this.isAnswerComplete = isComplete;
    });

    this.chatbotService.isAnswerTruncated$.subscribe(isTruncated => {
      this.isAnswerTruncated = isTruncated;

      if(isTruncated) {
        console.log("Przerwano");
      }
      
    });
  }

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
    const currentAnswerId = this.entries[this.entries.length - 1].id;
    if (!currentAnswerId) return;

    this.chatbotService.stopAnswer(currentAnswerId);
  }


  async pushChatbotPair(response: InteractWithChatbotResponse) {
    this.isAnsweringStopped = false;

    const entry: IChatbotPair = {
      id: response.id,
      question: response.question,
      answer: [''],
      rating: undefined
    };

    this.entries.push(entry);
  }

  async patchAnswer(newValue: {id: number, piece: string}) {
    if(newValue.piece == '\n\n') {
      this.entries[this.entries.length - 1].answer.push("");
    } 
    else {
      const lastEntry = this.entries[this.entries.length - 1];
      lastEntry.answer[lastEntry.answer.length - 1] += newValue.piece;
    } 

    this.scrollToBottom();
  }

  getFirstData(question: string): Observable<InteractWithChatbotResponse> {
    return this.chatbotService.interactWithChatbot(question);
  }


  getSecondData(id: number): Observable<{ id: number; piece: string }> {
    return this.chatbotService.receivedMessages$.pipe(
      filter(msg => msg !== null && msg.id === id)
    );
  }


  sendMessage(question: string) {
    if (!question.trim()) return;

    this.chatbotService.resetAnswerComplete();

    this.getFirstData(question).subscribe(firstResult => {
      this.pushChatbotPair(firstResult);

      this.getSecondData(firstResult.id).subscribe(secondResult => {
        if(secondResult.id === firstResult.id) {
          this.patchAnswer(secondResult);
        }
      });
    });
  }

  private scrollToBottom() {
    setTimeout(() => {
      this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
    }, 100);
  }
}