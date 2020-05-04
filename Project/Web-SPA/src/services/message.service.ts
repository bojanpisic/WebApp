import { Injectable } from '@angular/core';
import { Message } from '../app/entities/message';
import { UserService } from './user.service';
import { RegisteredUser } from 'src/app/entities/registeredUser';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  receiver: RegisteredUser;

  constructor(private userService: UserService) { }

  sendMessage(message: Message) {
    this.receiver = this.userService.getUser(message.receives.id);
    this.receiver.messages.push(message);
  }

  deleteMessage(message: Message) {
    this.receiver = this.userService.getUser(message.receives.id);
    const index = this.receiver.messages.indexOf(message);
    this.receiver.messages.splice(index, 1);
    this.userService.updateUser(this.receiver);
    console.log(this.receiver.messages.length);
  }
}
