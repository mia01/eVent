import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { TasksComponent } from './tasks/tasks.component';
import { SideNavComponent } from './side-nav/side-nav.component';
import { CalenderComponent } from './calender/calender.component';
import { AuthGuard } from './services/auth/authGuard.service';


const appRoutes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    {
        path: 'app',
        component: SideNavComponent,
        children: [
            { path: 'tasks', pathMatch: 'full', component: TasksComponent, canActivate: [AuthGuard] },
            { path: 'calender', pathMatch: 'full', component: CalenderComponent, canActivate: [AuthGuard] },
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
