import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material/material.module';
import { NavigationModule } from './navigation/navigation.module';
import { NavigationObject } from './models/navigation-config';
import { AngularModule } from './angular.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NavigationNeo } from './constants/navigation.neo4j';

@NgModule({
  declarations: [ ],
  imports: [
    CommonModule,
    NavigationModule.setup({
			title: 'Extra Blog',
			menuRoutes: [
        new NavigationObject(NavigationNeo.Explore, 'fa fa-search', 'Explore'),
				new NavigationObject(NavigationNeo.Home, 'fa fa-external-link', 'Logout') //NavigationNeo.Logout
			]
    }),
    AngularModule,
    ReactiveFormsModule
  ],
  exports: [
    CommonModule,
    MaterialModule,
    NavigationModule,
    AngularModule,
    ReactiveFormsModule
  ]
})
export class SharedModule { }
