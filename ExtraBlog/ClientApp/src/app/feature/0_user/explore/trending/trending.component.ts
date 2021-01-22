import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { DataService } from 'src/app/core/services/data.service';
import { DataRest } from 'src/app/shared/models/data-response';

@Component({
  selector: 'app-trending',
  templateUrl: './trending.component.html',
  styleUrls: ['./trending.component.css']
})
export class TrendingComponent implements OnInit {

  trending$: Observable<DataRest>

  constructor(private data: DataService) { }

  ngOnInit(): void {
    this.trending$ = this.data.getTrending();
  }

}
