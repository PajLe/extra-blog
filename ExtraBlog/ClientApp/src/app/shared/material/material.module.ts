import {NgModule} from "@angular/core";
import {MatIconModule} from "@angular/material/icon";
import {MatDialogModule} from "@angular/material/dialog";
import {MatSelectModule} from "@angular/material/select";
import {MatTableModule} from "@angular/material/table";
import {MatButtonModule} from "@angular/material/button";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatInputModule} from "@angular/material/input";
import {MatAutocompleteModule} from "@angular/material/autocomplete";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {MatRadioModule} from "@angular/material/radio";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {MatTooltipModule} from "@angular/material/tooltip";
import {MatStepperModule} from "@angular/material/stepper";
import {MatSlideToggleModule} from "@angular/material/slide-toggle";
import {MatMenuModule} from "@angular/material/menu";
import {MatRippleModule} from "@angular/material/core";
import {MatTreeModule} from "@angular/material/tree";
import {MatSliderModule} from "@angular/material/slider";
import {MatExpansionModule} from "@angular/material/expansion";
import {MatTabsModule} from '@angular/material/tabs';
import {MatCardModule} from '@angular/material/card';

@NgModule({
  imports: [
		MatIconModule,
		MatInputModule,
		MatDialogModule,
		MatSelectModule,
		MatTableModule,
		MatCheckboxModule,
		MatButtonModule,
		MatAutocompleteModule,
		MatDatepickerModule,
		MatSnackBarModule,
		MatRadioModule,
		MatProgressBarModule,
		MatTooltipModule,
		MatStepperModule,
		MatMenuModule,
		MatTreeModule,
		MatSliderModule,
		MatSlideToggleModule,
		MatRippleModule,
		MatTreeModule,
		MatTabsModule,
		MatCardModule
	],
	exports: [
		MatInputModule,
		MatIconModule,
		MatDialogModule,
		MatSelectModule,
		MatTableModule,
		MatButtonModule,
		MatCheckboxModule,
		MatAutocompleteModule,
		MatDatepickerModule,
		MatSnackBarModule,
		MatRadioModule,
		MatProgressBarModule,
		MatTooltipModule,
		MatStepperModule,
		MatSlideToggleModule,
		MatMenuModule,
		MatTreeModule,
		MatSliderModule,
		MatRippleModule,
		MatTreeModule,
		MatExpansionModule,
		MatTabsModule,
		MatCardModule
	],
	providers: []
})
export class MaterialModule { }
