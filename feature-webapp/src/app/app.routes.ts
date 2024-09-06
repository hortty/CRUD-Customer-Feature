import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { EditClientComponent } from './edit-client/edit-client.component';

export const routes: Routes = [
    { path: '', component: MainComponent },
    { path: 'clients', component: MainComponent },
    { path: 'edit-client/:id', component: EditClientComponent },
    { path: 'add-client', component: EditClientComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }