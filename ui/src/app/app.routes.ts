import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginProposaleComponent } from './components/login-proposale/login-proposale.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';

export const routes: Routes = [
    { path: '', component: HomeComponent},
    { path: 'login', component: LoginProposaleComponent},
    { path: 'sign-up', component: SignUpComponent}
];
