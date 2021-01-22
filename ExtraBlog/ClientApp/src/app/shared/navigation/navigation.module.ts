import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserMenuComponent } from './user-menu/user-menu.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { NavigationConfig } from '../models/navigation-config';
import { CustomLink } from '../../core/directives/custom-link.directive';
import { MaterialModule } from '../material/material.module';

@NgModule({
  declarations: [
    UserMenuComponent, 
    SidebarComponent,
    CustomLink
  ],
  imports: [
    CommonModule,
    MaterialModule
  ],
  exports: [
    UserMenuComponent,
    SidebarComponent,
    CustomLink
  ]
})
export class NavigationModule {
  constructor(public config: NavigationConfig) {
	}

	static setup(config: NavigationConfig): ModuleWithProviders<NavigationModule> {
		return {
			ngModule: NavigationModule,
			providers: [{
				provide: NavigationConfig,
				useValue: config
			}]
		};
	}
 }
