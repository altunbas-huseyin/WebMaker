import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoryItemLanguageRoutingModule } from './category-item-language-routing.module';
import { CategoryItemLanguageComponent } from './category-item-language.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [CategoryItemLanguageComponent],
  imports: [CommonModule, CategoryItemLanguageRoutingModule, SharedModule],
})
export class CategoryItemLanguageModule {}
