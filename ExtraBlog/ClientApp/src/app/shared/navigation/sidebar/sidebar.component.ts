import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { NavigationConfig, NavigationObject } from '../../models/navigation-config';
import { DefaultNavigationComponent } from '../default-navigation.component';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SidebarComponent extends DefaultNavigationComponent implements OnInit {
  mainMenu: boolean = false;
  opened: number = 0;
  
  menuRoutes = [];

  constructor(change: ChangeDetectorRef, public config: NavigationConfig) {
    super(change)
  }

  menuToggle(route: NavigationObject) {
		this.opened = this.opened === route.openId ? -1 : route.openId;
  }

  ngOnInit(): void {
    
  }

}
