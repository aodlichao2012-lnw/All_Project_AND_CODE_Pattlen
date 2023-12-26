import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Account/login/login.component';
import { NgModule } from '@angular/core';

export const routes: Routes = [
{ path:"Login" ,component: LoginComponent}
];
@NgModule({
  imports : [RouterModule.forRoot(routes)],
exports:[RouterModule]
})
export class AppRouttingModule{}
