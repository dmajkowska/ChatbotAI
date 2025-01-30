import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GenerateAnswerResponse } from '../model/generate-answer/generate-answer.response';

@Injectable({
  providedIn: 'root'
})
export class ChatbotService {
  private apiUrl = 'https://localhost:7004/api/chatbot/'; 

  constructor(private http: HttpClient) {}

  generateAnswer(question: string): Observable<GenerateAnswerResponse> {
    return this.http.post<GenerateAnswerResponse>(`${this.apiUrl}`, { question });
  }

  rateAnswer(id: number, rating?: boolean): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}rate`, { id, rating });
  }

  truncateAnswer(id: number, displayedCharactersCount: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}truncate`, { id, displayedCharactersCount });
  }
}