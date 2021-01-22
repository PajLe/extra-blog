import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { DataService } from "../services/data.service";
import { LinkService } from "../services/link.service";

@Injectable({
	providedIn: 'root'
})
export class AuthService implements CanActivate {
	constructor(private data: DataService, private link: LinkService) {
	}

	canActivate = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> =>
		new Observable(o => o.next(!!localStorage.getItem("token")))
}