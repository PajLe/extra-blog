/*******response********/
export class DefaultResponse {
	success: boolean;

	status?: number;
	error?: string;
	errorTitle?: string;

	[propName: string]: any | any[];
}

export class DataRest extends DefaultResponse {
	[propName: string]: any | any[];
}

/*******link modification*******/
export class DataLink {
	base: string = '';
	hasParam: boolean = false;

	constructor(path: string) {
		this.aPath(path);
	}

	aPath(data: string | number): DataLink {
		if (data || data === 0) this.base += `/${data}`;
		return this;
	}

	aPar(name: string, value: any): DataLink {
		if (value && value !== 'undefined') {
			this.base += `${this.hasParam ? '&' : '?'}${name}=${encodeURIComponent(value)}`;
			this.hasParam = true;
		}
		return this;
	}

	res(): string {
		return this.aPar('ts', Date.now()).base;
	}
}

export class TokenData extends DefaultResponse {
	token: string;
	user: any;
}

/*******pagination&searchOptions*******/
export class DataOptions {
	[propName: string]: any;

	limit: number = 5;
	search: string = undefined;
	page: number = 1;

	constructor(options?: any) {
		if (options) this.update(options);
	}

	public getSize(): number { return this.limit * 5 + 1 };

	public getOffset(page: number): number { return this.limit * (page - 1); }

	public update(options: any): DataOptions {
		if (options) Object.keys(options)
			.forEach(key => this[key] = key === 'search' ? options[key]?.trim() : options[key]);
		return this;
	}
}