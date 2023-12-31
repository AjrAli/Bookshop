import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '', loadComponent: () => import('./components/home/home.component').then(mod => mod.HomeComponent) },
    { path: 'login', loadComponent: () => import('./components/login-proposale/login-proposale.component').then(mod => mod.LoginProposaleComponent) },
    { path: 'sign-up', loadComponent: () => import('./components/sign-up/sign-up.component').then(mod => mod.SignUpComponent) },
    {
        path: 'steps',
        loadComponent: () => import('./components/steps/steps.component').then(mod => mod.StepsComponent),
        children: [
            { path: '', redirectTo: 'my-shoppingcart', pathMatch: 'full' },
            {
                path: 'my-shoppingcart',
                loadComponent: () => import('./components/steps/my-shoppingcart/my-shoppingcart.component').then(mod => mod.MyShoppingCartComponent)
            }
        ],
    }
];
