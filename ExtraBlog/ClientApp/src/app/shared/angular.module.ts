import {NgModule} from "@angular/core";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import {RouterModule} from "@angular/router";
import {CommonModule} from "@angular/common";

@NgModule({
	imports: [
		RouterModule,

		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		HttpClientModule
	],
	exports: [
		RouterModule,

		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		HttpClientModule
	]
})
export class AngularModule {
}