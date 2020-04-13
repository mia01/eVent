import { NgModule, ErrorHandler } from '@angular/core';
import { sharedModules, sharedComponents } from './shared';
import { AppComponent } from './app.component';
import { CustomErrorHandler } from './handlers/custom-error.handler';
import { AddTaskComponent } from './tasks/add/add.component';

@NgModule({
    declarations: [
        ...sharedComponents,
    ],
    imports: [
        ...sharedModules,
    ],
    entryComponents: [AddTaskComponent],
    providers: [
        { provide: ErrorHandler, useClass: CustomErrorHandler }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
