import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { ShoppingGuard } from './guards/shopping.guard';

export const routes: Routes = [
    { path: '', loadComponent: () => import('./components/home/home.component').then(mod => mod.HomeComponent) },
    { path: 'search', loadComponent: () => import('./components/search/search.component').then(mod => mod.SearchComponent) },
    { path: 'customer/view-profile', loadComponent: () => import('./components/customer/view-profile/view-profile.component').then(mod => mod.ViewProfileComponent), canActivate: [AuthGuard] },
    { path: 'customer/edit-profile', loadComponent: () => import('./components/customer/edit-profile/edit-profile.component').then(mod => mod.EditProfileComponent), canActivate: [AuthGuard] },
    { path: 'customer/edit-password', loadComponent: () => import('./components/customer/edit-password/edit-password.component').then(mod => mod.EditPasswordComponent), canActivate: [AuthGuard] },
    { path: 'books', loadComponent: () => import('./components/books/books.component').then(mod => mod.BooksComponent) },
    { path: 'orders', loadComponent: () => import('./components/orders/orders.component').then(mod => mod.OrdersComponent), canActivate: [AuthGuard] },
    { path: 'books/author/:id', loadComponent: () => import('./components/books/books.component').then(mod => mod.BooksComponent), data: { type: 'author' } },
    { path: 'books/category/:id', loadComponent: () => import('./components/books/books.component').then(mod => mod.BooksComponent), data: { type: 'category' } },
    { path: 'book/:id', loadComponent: () => import('./components/book-details/book-details.component').then(mod => mod.BookDetailsComponent) },
    { path: 'order/:id', loadComponent: () => import('./components/order-details/order-details.component').then(mod => mod.OrderDetailsComponent), canActivate: [AuthGuard] },
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
            { path: 'payment', loadComponent: () => import('./components/steps/payment/payment.component').then(mod => mod.PaymentComponent), canActivate: [ShoppingGuard] },
            { path: 'confirmation', loadComponent: () => import('./components/steps/confirmation/confirmation.component').then(mod => mod.ConfirmationComponent), canActivate: [ShoppingGuard] }
        ],
    },
    { path: '**', redirectTo: '' }
];
