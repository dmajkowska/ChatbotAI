import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatContainerComponent } from './components/chat-container/chat-container.component';
import { ChatHeaderComponent } from './components/chat-container/chat-header/chat-header.component';

const routes: Routes = [];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    ChatContainerComponent,
  ],
  exports: [
    RouterModule,
    ChatContainerComponent,
  ],
  declarations: []
})
export class AppRoutingModule { }
