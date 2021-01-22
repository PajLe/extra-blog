import { Navigation } from "./navigation";

export class NavigationConfig {
	title!: string;

	menuRoutes: NavigationObject[] = [];
}

export class NavigationObject {
    target: Navigation | string;
    
	static nextOpenId: number = 1;
	openId: number;
	external: boolean = false;
	new: boolean = false;
	badge: number = 0;

	constructor(target: Navigation, public icon: string, public title: string, public subtitle: string = '', public beta: boolean = false, public categories: NavigationObject[] = []) {
		this.target = target;
		this.openId = NavigationObject.nextOpenId++;
	}
}