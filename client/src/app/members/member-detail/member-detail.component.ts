import { ThisReceiver } from '@angular/compiler';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static: true}) memberTabsVariable: TabsetComponent;
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  activeTab: TabDirective;
  messages: Message[] = [];
  user: User;

  constructor(
          public presenceService: PresenceService,
          private memberService: MembersService, 
          private route: ActivatedRoute, 
          private messageService: MessageService,
          private accountService: AccountService,
          private router: Router) { 

      this.accountService.currentUser$.pipe(take(1)).subscribe(response => this.user = response);

      this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    }
  
  ngOnInit(): void {

    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false
    }];

    // Load member
    this.route.data.subscribe(response => {
      this.member = response.member;
    });

    this.galleryImages = this.getImages();

    // After page loading, jump to the tab which is specified in query string. Ex: Messages tab
    this.route.queryParams.subscribe(p => {
      p.tab ? this.selectTab(p.tab) : this.selectTab(0);
    });
  }

  getImages(): NgxGalleryImage[] {
    const imageUrls = [];
    for (const photo of this.member.photos) {
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      });
    }

    return imageUrls;
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.messages.length === 0) {
      
      this.messageService.createHubConnection(this.user, this.member.userName);
    } else {
      this.messageService.stopHubConnection();
    }
  }

  selectTab(tabId: number) {
    this.memberTabsVariable.tabs[tabId].active = true;
  }

  ngOnDestroy(): void {

    this.messageService.stopHubConnection();
  }

}
