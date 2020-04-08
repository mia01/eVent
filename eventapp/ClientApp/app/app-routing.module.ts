import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { TasksComponent } from './tasks/tasks.component';
import { SideNavComponent } from './side-nav/side-nav.component';
import { CalenderComponent } from './calender/calender.component';


const appRoutes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    {
        path: 'app',
        component: SideNavComponent,
        children: [
            { path: 'tasks', pathMatch: 'full', component: TasksComponent },
            { path: 'calender', pathMatch: 'full', component: CalenderComponent },
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forRoot(
            appRoutes,
            // { enableTracing: true } // <-- debugging purposes only
        )
    ],
    exports: [
        RouterModule
    ]
})
export class AppRoutingModule { }
