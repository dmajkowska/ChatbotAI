import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenerateAnswerResponse } from '../model/generate-answer/generate-answer.response';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { HttpTransportType, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class ChatbotService {
  private hostUrl = 'https://localhost:7004'
  private apiUrl = `${this.hostUrl}/api/chatbot`; 
  private hubUrl = `${this.hostUrl}/chatBotHub`;  

  private hubConnection: signalR.HubConnection;

  private receivedMessages = new BehaviorSubject<{ id: number; piece: string }>({id: 0, piece: ''});
  receivedMessages$ = this.receivedMessages.asObservable();

  private isAnswerComplete = new BehaviorSubject<boolean>(true); 
  private isAnswerTruncated = new BehaviorSubject<boolean>(false); 
  isAnswerComplete$ = this.isAnswerComplete.asObservable(); 
  isAnswerTruncated$ = this.isAnswerTruncated.asObservable(); 
  
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
        this.isAnswerTruncated.next(false)
      });

      this.hubConnection.on('ReceiveAnswerTruncated', (id: number) => {
        this.isAnswerComplete.next(true); 
        this.isAnswerTruncated.next(true)
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

  generateAnswer(question: string): Observable<GenerateAnswerResponse> {
    return this.http.post<GenerateAnswerResponse>(`${this.apiUrl}`, { question });
  }

  rateAnswer(id: number, rating?: boolean): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/rate`, { id, rating });
  }

  truncateAnswer(id: number, displayedCharactersCount: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/truncate`, { id, displayedCharactersCount });
  }

}