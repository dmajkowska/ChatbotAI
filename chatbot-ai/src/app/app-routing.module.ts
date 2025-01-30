import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatbotContainerComponent } from './components/chatbot-container/chatbot-container.component';

const routes: Routes = [];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    ChatbotContainerComponent,
  ],
  exports: [
    RouterModule,
    ChatbotContainerComponent,
  ],
  declarations: []
})
export class AppRoutingModule { }
