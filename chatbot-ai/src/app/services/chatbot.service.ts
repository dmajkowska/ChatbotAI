import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InteractWithChatbotResponse } from '../model/interact-with-chatbot/interact-with-chatbot.response';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { HttpTransportType, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class ChatbotService {
  private hostUrl = 'http://localhost:7004'
  private apiUrl = `${this.hostUrl}/api/chatbot`; 
  private hubUrl = `${this.hostUrl}/chatBotHub`;  

  private hubConnection: signalR.HubConnection;

  private receivedMessages = new BehaviorSubject<{ id: number; piece: string }>({id: 0, piece: ''});
  receivedMessages$ = this.receivedMessages.asObservable();

  private isAnswerComplete = new BehaviorSubject<boolean>(true); 
  private isAnswerInterrupted = new BehaviorSubject<boolean>(false); 
  isAnswerComplete$ = this.isAnswerComplete.asObservable(); 
  isAnswerInterrupted$ = this.isAnswerInterrupted.asObservable(); 
  
  constructor(private http: HttpClient) {
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.hubUrl, {
      skipNegotiation: true, 
      transport: HttpTransportType.WebSockets
    })
    .withAutomaticReconnect()
    .build();

    this.hubConnection.onclose(error => {
      console.error("Połączenie SignalR zerwane:", error);
      setTimeout(() => this.startConnection(), 5000); 
    });

    this.startConnection();
  }

  private startConnection() {
    
    this.hubConnection.start()
      .then(() => console.log('SignalR Connected'))
      .catch(err => console.error('Error connecting to SignalR:', err));

      this.hubConnection.on('ReceiveAnswerPiece', (id: number, piece: string) => {
        this.receivedMessages.next({ id, piece }); 
      });

      this.hubConnection.on('ReceiveAnswerComplete', (id: number) => {
        this.isAnswerComplete.next(true); 
        this.isAnswerInterrupted.next(false)
      });

      this.hubConnection.on('ReceiveAnswerInterrupted', (id: number) => {
        this.isAnswerComplete.next(true); 
        this.isAnswerInterrupted.next(true)
      });
  }

  stopAnswer(id: number) {
    this.hubConnection.invoke("StopAnswer", id)
      .then(() => console.log(`Wysłano żądanie zatrzymania odpowiedzi ID ${id}`))
      .catch(err => console.error("Błąd podczas zatrzymywania odpowiedzi:", err));
  }

  resetAnswerComplete() {
    this.isAnswerComplete.next(false); 
  }

  interactWithChatbot(question: string): Observable<InteractWithChatbotResponse> {
    return this.http.post<InteractWithChatbotResponse>(`${this.apiUrl}`, { question });
  }

  rateAnswer(id: number, rating?: boolean): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/rate`, { id, rating });
  }
}