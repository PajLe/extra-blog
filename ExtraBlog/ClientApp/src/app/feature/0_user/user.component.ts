import { Component, OnInit } from '@angular/core';
import { LinkService } from 'src/app/core/services/link.service';
import { ViewType } from 'src/app/shared/constants/view-types';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  ViewType = ViewType;
  view: ViewType;
  
  constructor(private link: LinkService) {
    
  }

  ngOnInit() {
		window.addEventListener('resize', e => this.refreshView());
    this.refreshView();
	}

	refreshView() {
		this.view = window.innerWidth < 576 ? ViewType.MOBILE : window.innerWidth < 1200 ? ViewType.TABLET : ViewType.DESKTOP;
	}
}
