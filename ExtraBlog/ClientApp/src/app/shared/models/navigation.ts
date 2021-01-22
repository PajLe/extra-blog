export class Navigation {
	static Users = new Navigation('');
	static Logout = new Navigation('login');

	protected readonly route: string[];

	constructor(route: string) {
		if (route.startsWith('/')) route = route.substring(1);
		this.route = route.split('/');
	}

	getFullRoute(params: boolean = true): string {
		return params ? `/${this.route.join('/')}` : `/${this.route.filter(r => Number.isNaN(Number(r))).join('/')}`;
	}

	getLastRoute(): string {
		return this.route[this.route.length - 1];
	}

	addParameters(data: number | string): Navigation {
		if (data || data === 0) {
			let navigationTarget = this.getFullRoute();
			let queryParams = '';
			if(this.getFullRoute().indexOf('?') !== -1) {
				queryParams = this.getFullRoute().substr(this.getFullRoute().indexOf('?'), this.getFullRoute().length);
				navigationTarget = this.getFullRoute().substr(0, this.getFullRoute().indexOf('?'));
			}
			let target = new Navigation(navigationTarget);
			target.route.push(`${data}`);
			if(queryParams)
				target = target.addQueryParameters(queryParams)
			return target;
		} else return this;
	}

	addQueryParameter(key: string, value: any): Navigation {
		if(key && value) {
			const route = this.getFullRoute();
			const separator = route.indexOf('?') !== -1 ? '&' : '?';
			return new Navigation(`${route}${separator}${key}=${value}`);
		} else return this;
	}

	addQueryParameters(queryParams: string) {
		if(queryParams) {
			const route = this.getFullRoute();
			return new Navigation(`${route}?${queryParams}`)
        }
        return this;
	}
}