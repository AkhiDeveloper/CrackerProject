import { Routes } from '@angular/router';
import { BooksListPageComponent } from './pages/books-list-page/books-list-page.component';

export const routes: Routes = [
    {
        path: 'books',
        component: BooksListPageComponent,
        pathMatch: 'full'
    },
    {
        path: '',
        redirectTo: 'books',
        pathMatch: 'full'
    },
    {
        path: '**',
        redirectTo: '',
    }
];
