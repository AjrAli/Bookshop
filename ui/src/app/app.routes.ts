import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginProposaleComponent } from './components/login-proposale/login-proposale.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { StepsComponent } from './components/steps/steps.component';
import { MyShoppingCartComponent } from './components/steps/my-shoppingcart/my-shoppingcart.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'login', component: LoginProposaleComponent },
    { path: 'sign-up', component: SignUpComponent },
    { path: 'my-shoppingcart', component: MyShoppingCartComponent },
    {
        path: 'steps',
        component: StepsComponent,
        children: [
            { path: '', redirectTo: 'my-shoppingcart', pathMatch: 'full' },
            { path: 'my-shoppingcart', component: MyShoppingCartComponent }
        ],
    }
];
