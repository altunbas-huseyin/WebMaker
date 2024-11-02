import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryItemLanguageComponent } from './category-item-language.component';

const routes: Routes = [
  {
    path: ':id',
    component: CategoryItemLanguageComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CategoryItemLanguageRoutingModule {}
