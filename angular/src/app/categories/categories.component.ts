// categories.component.ts
import { Component, OnInit } from '@angular/core';
import { CategoryService, CategoryDto, CreateUpdateCategoryDto } from '@proxy/categories';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ListService, PagedResultDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import { ConfirmationService } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss'],
  providers: [ListService],
})
export class CategoriesComponent implements OnInit {
  category = { items: [], totalCount: 0 } as PagedResultDto<CategoryDto>;
  form: FormGroup;
  selectedCategory = {} as CategoryDto;
  isModalOpen = false;

  constructor(
    public readonly list: ListService,
    private categoryService: CategoryService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit() {
    const categoryStreamCreator = (query: PagedAndSortedResultRequestDto) =>
      this.categoryService.getList(query);

    this.list.hookToQuery(categoryStreamCreator).subscribe(response => {
      this.category = response;
    });
  }

  createCategory() {
    this.selectedCategory = {} as CategoryDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editCategory(id: string) {
    this.categoryService.get(id).subscribe(category => {
      this.selectedCategory = category;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedCategory.name || '', Validators.required],
      description: [this.selectedCategory.description || ''],
      parentCategoryId: [this.selectedCategory.parentCategoryId || ''],
      seoTitle: [this.selectedCategory.seoTitle || ''],
      seoDescription: [this.selectedCategory.seoDescription || ''],
      seoKeywords: [this.selectedCategory.seoKeywords || ''],
      seoSlug: [this.selectedCategory.seoSlug || ''],
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedCategory.id
      ? this.categoryService.update(this.selectedCategory.id, this.form.value)
      : this.categoryService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }

  delete(id: string) {
    this.confirmation
      .warn('Kategori silinecek', 'Bu kategoriyi silmek istediğinizden emin misiniz?')
      .subscribe(status => {
        if (status === 'confirm') {
          this.categoryService.delete(id).subscribe(() => {
            this.list.get();
          });
        }
      });
  }
}
