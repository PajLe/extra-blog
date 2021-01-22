import { Directive, ElementRef, EventEmitter, HostListener, Input, OnInit, Output } from "@angular/core";
import { LinkService } from "../../core/services/link.service";
import { Navigation } from "../../shared/models/navigation";

@Directive({
	selector: '[customLink]'
})
export class CustomLink implements OnInit {
	@Input() targetRoute: Navigation | any;
	@Input() targetLink: any;
	@Input() defaultNewTab: boolean = false;

	@Output() changeExpanded = new EventEmitter();

	@HostListener('click', ['$event']) onClick(e: MouseEvent) {
		if (e.shiftKey || e.ctrlKey || this.defaultNewTab) this.link.openPage(this.targetLink ? this.targetLink : this.targetRoute.getFullRoute(), true);
		else if (this.targetRoute) {
			if (this.link.navigate(this.targetRoute)) this.changeExpanded.emit(true);
		} else this.link.openPage(this.targetLink);
	}

	@HostListener('auxclick', ['$event']) onClickAux(e: MouseEvent) {
		if (e.which === 2) this.link.openPage(this.targetLink ? this.targetLink : this.targetRoute.getFullRoute(), true);
	}

	constructor(private link: LinkService, private e: ElementRef) {
	}

	ngOnInit(): void {
		const foundRoutes: number = [this.targetLink, this.targetRoute].filter(e => !!e).length;
		if (foundRoutes !== 1) console.log(`${foundRoutes === 0 ? 'No' : 'Multiple'} routes detected!`, this.e);
		if (this.targetRoute) this.link.allElements.push({route: this.targetRoute, element: this.e});
	}
}
