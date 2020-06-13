import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MessageService } from 'src/services/message.service';
import { Message } from 'src/app/entities/message';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit {

  @Input() message: any;
  @Output() action = new EventEmitter<Message>();

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
  }

  deleteMessage() {
    this.messageService.deleteMessage(this.message);
    console.log(this.message);
  }

  readMessage() {
    this.action.emit(this.message);
    this.message.read = true;
  }

}
