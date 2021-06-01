import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-message',
  templateUrl: './member-message.component.html',
  styleUrls: ['./member-message.component.css']
})
export class MemberMessageComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm;
  @Input() messages: Message[];
  @Input() username: string;
  messageContent: string;
  loading = false;
    
  constructor(public messageService: MessageService) { }

  ngOnInit(): void {}

  sendMessage() {
    this.loading = true;

    this.messageService.sendMessage(this.username, this.messageContent).then(() => {
      
      this.messageForm.reset();
    }).finally(() => this.loading = false);
  }

}
