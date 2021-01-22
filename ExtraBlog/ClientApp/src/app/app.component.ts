import { Component } from '@angular/core';
import { LinkService } from './core/services/link.service';
import { NavigationNeo } from './shared/constants/navigation.neo4j';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(private link: LinkService) {

  }

  ngOnInit() {
    if (!localStorage.getItem('token'))
      this.link.navigate(NavigationNeo.Home)
    else
      this.link.navigate(NavigationNeo.Explore);
  }

}
