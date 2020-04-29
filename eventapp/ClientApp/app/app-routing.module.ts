import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { TasksComponent } from './tasks/tasks.component';
import { SideNavComponent } from './side-nav/side-nav.component';
import { CalenderComponent } from './calender/calender.component';
import { AuthGuard } from './services/auth/authGuard.service';
import { ApplicationPaths } from './models/auth/auth.constants';
import { LogoutComponent } from './logout/logout.component';
import { FriendsComponent } from './friends/friends.component';
import { EventsComponent } from './events/events.component';


const appRoutes: Routes = [
    { path: '', redirectTo: 'app/calender', pathMatch: 'full', canActivate: [AuthGuard]},
    { path: ApplicationPaths.Login, component: LoginComponent },
    { path: ApplicationPaths.LoginFailed, component: LoginComponent },
    { path: ApplicationPaths.LoginCallback, component: LoginComponent },
    { path: ApplicationPaths.Register, component: LoginComponent },
    { path: ApplicationPaths.Profile, component: LoginComponent },
    { path: ApplicationPaths.LogOut, component: LogoutComponent },
    { path: ApplicationPaths.LoggedOut, component: LogoutComponent },
    { path: ApplicationPaths.LogOutCallback, component: LogoutComponent },
    {
        path: 'app',
        component: SideNavComponent,
        canActivate: [AuthGuard],
        children: [
            { path: 'events', pathMatch: 'full', component: EventsComponent, canActivate: [AuthGuard] },
            { path: 'tasks', pathMatch: 'full', component: TasksComponent, canActivate: [AuthGuard] },
            { path: 'calender', pathMatch: 'full', component: CalenderComponent, canActivate: [AuthGuard] },
            { path: 'friends', pathMatch: 'full', component: FriendsComponent, canActivate: [AuthGuard] },
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
