import { Routes } from '@angular/router';
import {LayoutComponent} from './components/layout/layout.component';
import {IntroComponent} from './pages/intro/intro.component';
import {InstructionComponent} from './pages/instruction/instruction.component';
import {InfoComponent} from './pages/info/info.component';
import {ServicePortfolioBuilderComponent} from './pages/service-portfolio-builder/service-portfolio-builder.component';
import {LoginComponent} from './pages/login/login.component';
import {NoAuthGuard} from './guard/no-auth.guard';
import {AuthGuard} from './guard/auth.guard';
import {RegisterComponent} from './pages/register/register.component';
import {UserTasksComponent} from './pages/user-tasks/user-tasks.component';

export const routes: Routes =
  [
    {path: '', component: LayoutComponent, children: [
        {path : '', component : IntroComponent,canActivate:[AuthGuard]},
        {path : 'instruction', component : InstructionComponent,canActivate:[AuthGuard]},
        {path : 'info', component : InfoComponent},
        {path : 'builder', component : ServicePortfolioBuilderComponent,canActivate:[AuthGuard]},
        {path : 'tasks', component : UserTasksComponent,canActivate:[AuthGuard]},
        {path: 'login',component: LoginComponent,canActivate:[NoAuthGuard]},
        {path: 'signup',component: RegisterComponent,canActivate:[NoAuthGuard]}
      ]
    },
  ];

