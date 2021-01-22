import { ElementRef, Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Navigation } from "../../shared/models/navigation";

@Injectable({
	providedIn: 'root'
})
export class LinkService {
	public allElements: { route: Navigation, element: ElementRef }[] = [];

	private menuActive: string = 'menu-item-active';

	constructor(private route: Router) {
	}

	getCurrent = () => this.route.url;

	getQueryParam = (param: string) => (new URLSearchParams(window.location.search)).get(param);

	reload() {
		window.location.reload();
	}

	setDefault() {
		this.setActive(new Navigation(this.getCurrent()));
	}

	setActive(target: Navigation) {
		this.allElements.forEach(e => e.element.nativeElement.classList.remove(this.menuActive));
		const targetFullRoute = target.getFullRoute(false);
		let t = this.allElements.find(e => e.route.getFullRoute() === targetFullRoute);
		if(!t)
			t = this.allElements.find(e => e.route.getFullRoute() === targetFullRoute.substr(0, targetFullRoute.indexOf('?')))

		if (t) t.element.nativeElement.classList.add(this.menuActive);
	}

	public navigate(target: Navigation, extras: any = {}): boolean {
		const diff = target.getFullRoute() !== this.getCurrent();
		if(target.getFullRoute() === Navigation.Logout.getFullRoute())
			extras.skipLocationChange = true;
		this.route.navigateByUrl(target.getFullRoute(), extras).then((res: boolean) => {
			if (res) this.setActive(target);
			window.scrollTo(0, 0)
		});
		return diff;
	}

	public openPage(target: string, useNewTab: boolean = false, params: any = {}) {
		let t: string = target;
		Object.keys(params).forEach(key => t = t.replace(new RegExp(key, 'g'), params[key]));
		window.open(t, useNewTab ? '_blank' : '_self');
	}
}