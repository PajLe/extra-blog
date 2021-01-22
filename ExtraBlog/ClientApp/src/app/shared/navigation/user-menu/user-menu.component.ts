import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { DefaultNavigationComponent } from '../default-navigation.component';

class Project {
  constructor(public image: string, public newTab: boolean = false) { }
}

@Component({
  selector: 'app-user-menu',
  templateUrl: './user-menu.component.html',
  styleUrls: ['./user-menu.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserMenuComponent extends DefaultNavigationComponent implements OnInit {
  @Output() expandMenu = new EventEmitter();

	window = window;

  projects: Project[] = [new Project('fa fa-facebook-square')];

  constructor(change: ChangeDetectorRef) {
    super(change);
  }

  ngOnInit(): void {
  }

}
