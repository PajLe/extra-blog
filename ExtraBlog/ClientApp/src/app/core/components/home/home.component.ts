import { Component, OnInit } from '@angular/core';
import { NavigationNeo } from 'src/app/shared/constants/navigation.neo4j';
import { AlertifyService } from '../../services/alertify.service';
import { DataService } from '../../services/data.service';
import { LinkService } from '../../services/link.service';
import jwt_decode from "jwt-decode";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  user: any = {};

  constructor(private data: DataService, private link: LinkService, private alertifyService: AlertifyService) { }

  sendRequest(login) {
    if (login)
      this.data.login(this.user).subscribe(dat => {
        if (dat) {
          localStorage.setItem('token', dat.token);
          localStorage.setItem('decoded', JSON.stringify(jwt_decode(dat.token)));
          this.link.navigate(NavigationNeo.Explore);
        } else this.alertifyService.error('Unauthorized!')
      }, err => this.alertifyService.error('Unauthorized!'))
    else
      this.data.registerUser(this.user).subscribe(dat => {
        if (dat) {
          this.alertifyService.success('Successfully Registered')
        } else this.alertifyService.error('Unauthorized!')
      }, err => this.alertifyService.error('Unauthorized!'))
  }

  ngOnInit(): void {
    if (localStorage.getItem('token'))
      localStorage.removeItem('token');
  }

}
