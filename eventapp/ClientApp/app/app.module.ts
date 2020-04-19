import { NgModule, ErrorHandler } from '@angular/core';
import { sharedModules, sharedComponents } from './shared';
import { AppComponent } from './app.component';
import { CustomErrorHandler } from './handlers/custom-error.handler';
import { AddTaskComponent } from './tasks/add/add.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthorizeInterceptor } from './services/auth/authorize.interceptor';

@NgModule({
    declarations: [
        ...sharedComponents,
    ],
    imports: [
        ...sharedModules,
    ],
    entryComponents: [AddTaskComponent],
    providers: [
        { provide: ErrorHandler, useClass: CustomErrorHandler },
        { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
