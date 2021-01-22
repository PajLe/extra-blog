import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../shared/material/material.module';
import { HomeComponent } from './components/home/home.component';
import { InfiniteScrollComponent } from './components/infinite-scroll/infinite-scroll.component';
import { AngularModule } from '../shared/angular.module';

@NgModule({
  declarations: [ 
    HomeComponent, InfiniteScrollComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    AngularModule
  ],
  exports: [ InfiniteScrollComponent ]
})
export class CoreModule { }
