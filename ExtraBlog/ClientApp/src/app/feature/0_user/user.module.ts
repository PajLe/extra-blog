import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersRoutingModule } from './user.routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { CoreModule } from 'src/app/core/core.module';
import { ExploreComponent } from './explore/explore.component';
import { NetworkComponent } from './network/network.component';
import { UserComponent } from './user.component';
import { DialogOverviewExampleDialog, DocumentPreview, FeedComponent } from './explore/feed/feed.component';
import { TrendingComponent } from './explore/trending/trending.component';
import { AngularModule } from 'src/app/shared/angular.module';


@NgModule({
  declarations: [
    ExploreComponent,
    NetworkComponent,
    UserComponent,
    FeedComponent,
    TrendingComponent,
    DialogOverviewExampleDialog,
    DocumentPreview
  ],
  imports: [
    CommonModule,
    UsersRoutingModule,
    SharedModule,
    CoreModule
  ]
})
export class UserModule { }
