import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { ShoppingGuard } from './guards/shopping.guard';

export const routes: Routes = [
    { path: '', loadComponent: () => import('./components/home/home.component').then(mod => mod.HomeComponent) },
    { path: 'login', loadComponent: () => import('./components/login-proposale/login-proposale.component').then(mod => mod.LoginProposaleComponent) },
    {
        path: 'steps',
        loadComponent: () => import('./components/steps/steps.component').then(mod => mod.StepsComponent),
        children: [
            { path: '', redirectTo: 'my-shoppingcart', pathMatch: 'full' },
            {
                path: 'my-shoppingcart',
                loadComponent: () => import('./components/steps/my-shoppingcart/my-shoppingcart.component').then(mod => mod.MyShoppingCartComponent)
            },
            { path: 'authentication', loadComponent: () => import('./components/steps/authentication/authentication.component').then(mod => mod.AuthenticationComponent) },
            { path: 'payment', loadComponent: () => import('./components/steps/payment/payment.component').then(mod => mod.PaymentComponent), canActivate: [ShoppingGuard]},
            { path: 'confirmation', loadComponent: () => import('./components/steps/confirmation/confirmation.component').then(mod => mod.ConfirmationComponent), canActivate: [ShoppingGuard]}
        ],
    },
    { path: '**', redirectTo: '' }
];
