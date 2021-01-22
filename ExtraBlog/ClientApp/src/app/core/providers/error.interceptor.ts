import { Injectable } from "@angular/core";
import { HttpInterceptor, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor() {
    }

    intercept(
        req: import("@angular/common/http").HttpRequest<any>, 
        next: import("@angular/common/http").HttpHandler): import("rxjs").Observable<import("@angular/common/http").HttpEvent<any>> {
            return next.handle(req).pipe(
                catchError(error => throwError(error))
            )
    }

}

export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
};