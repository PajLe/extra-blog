import { ChangeDetectorRef, Directive, Input, OnInit } from '@angular/core';
import { ViewType } from '../constants/view-types';

@Directive()
export class DefaultNavigationComponent implements OnInit {
    @Input() view: ViewType;
    ViewType = ViewType;
    expanded: boolean = false;

    constructor(protected change: ChangeDetectorRef) { }

    ngOnInit(): void {
    }

    open() {
        this.expanded = true;
        this.change.detectChanges();
    }

    close() {
        this.expanded = false;
        this.change.detectChanges();
    }

}
