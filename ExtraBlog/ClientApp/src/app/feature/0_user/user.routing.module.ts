import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ExploreComponent } from './explore/explore.component';
import { NetworkComponent } from './network/network.component';
import { UserComponent } from './user.component';

export const routes: Routes = [
    { path: '', component: UserComponent,
      children: [
        { path: 'explore', component: ExploreComponent },
        { path: 'network', component: NetworkComponent }
      ]
    },
    
];

@NgModule({
    declarations: [],
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UsersRoutingModule { }