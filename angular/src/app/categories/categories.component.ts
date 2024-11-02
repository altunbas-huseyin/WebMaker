import { Component, OnInit } from '@angular/core';
import { CategoryService, CategoryDto, CreateUpdateCategoryDto } from '@proxy/categories';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ListService } from '@abp/ng.core';
import { ConfirmationService } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss'],
  providers: [ListService],
})
export class CategoriesComponent implements OnInit {
  categories: CategoryDto[] = [];
  parentCategories: CategoryDto[] = [];
  selectedCategory = {} as CategoryDto;
  form: FormGroup;
  isModalOpen = false;

  constructor(
    private categoryService: CategoryService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getAll().subscribe(response => {
      this.categories = response.items;
      this.parentCategories = this.getCategoryHierarchy(this.categories);
    });
  }

  getCategoryHierarchy(categories: CategoryDto[], parentId: string = null): CategoryDto[] {
    return categories
      .filter(c => c.parentCategoryId === parentId)
      .map(category => ({
        ...category,
        children: this.getCategoryHierarchy(categories, category.id),
      })) as CategoryDto[];
  }

  getCategoryLevel(categoryId: string): number {
    let level = 0;
    let currentCategory = this.categories.find(c => c.id === categoryId);

    while (currentCategory?.parentCategoryId) {
      level++;
      currentCategory = this.categories.find(c => c.id === currentCategory.parentCategoryId);
    }

    return level;
  }

  hasSubCategories(categoryId: string): boolean {
    return this.categories.some(c => c.parentCategoryId === categoryId);
  }

  createCategory(parentCategoryId?: string) {
    this.selectedCategory = {} as CategoryDto;
    this.buildForm(parentCategoryId);
    this.isModalOpen = true;
  }

  editCategory(id: string) {
    this.categoryService.get(id).subscribe(category => {
      this.selectedCategory = category;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm(parentCategoryId?: string) {
    const defaultTranslation = this.selectedCategory.translations?.[0];

    this.form = this.fb.group({
      name: [defaultTranslation?.name || '', Validators.required],
      description: [defaultTranslation?.description || ''],
      parentCategoryId: [parentCategoryId || this.selectedCategory.parentCategoryId],
      seoTitle: [defaultTranslation?.seoTitle || ''],
      seoDescription: [defaultTranslation?.seoDescription || ''],
      seoKeywords: [defaultTranslation?.seoKeywords || ''],
      seoSlug: [this.selectedCategory.seoSlug || ''],
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const request = {
      ...this.form.value,
      languageCode: 'tr', // Default language for new categories
    } as CreateUpdateCategoryDto;

    if (this.selectedCategory.id) {
      this.categoryService.update(this.selectedCategory.id, request).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.loadCategories();
      });
    } else {
      this.categoryService.create(request).subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.loadCategories();
      });
    }
  }

  delete(id: string) {
    const category = this.categories.find(c => c.id === id);
    if (!category) return;

    const hasChildren = this.hasSubCategories(id);
    const message = hasChildren
      ? 'Bu kategoriyi silmek için önce alt kategorilerini silmelisiniz!'
      : 'Bu kategoriyi silmek istediğinizden emin misiniz?';

    if (hasChildren) {
      this.confirmation.warn('Uyarı', message);
      return;
    }

    this.confirmation.warn('Kategori silinecek', message).subscribe(status => {
      if (status === 'confirm') {
        this.categoryService.delete(id).subscribe(() => {
          this.loadCategories();
        });
      }
    });
  }

  getCategoryName(category: CategoryDto): string {
    return category.translations?.[0]?.name || '';
  }

  getParentCategoryName(parentCategoryId: string): string {
    const parent = this.categories.find(c => c.id === parentCategoryId);
    return parent?.translations?.[0]?.name || '';
  }
}
