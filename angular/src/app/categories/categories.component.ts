// categories.component.ts
import { Component, OnInit } from '@angular/core';
import { CategoryService, CategoryDto, CreateUpdateCategoryDto } from '@proxy/categories';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss'],
  providers: [ListService],
})
export class CategoriesComponent implements OnInit {
  categories: CategoryDto[] = [];
  form: FormGroup;
  selectedCategory = {} as CategoryDto;
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
      // Sadece parentCategoryId'si olmayan (null veya undefined) kategorileri filtrele
      this.categories = response.items.filter(category => !category.parentCategoryId);
    });
  }

  createCategory() {
    this.selectedCategory = {} as CategoryDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editCategory(id: string) {
    const category = this.categories.find(c => c.id === id);
    if (category) {
      this.selectedCategory = category;
      this.buildForm();
      this.isModalOpen = true;
    }
  }

  buildForm() {
    // İlk translation'ı al (varsayılan dil için)
    const defaultTranslation = this.selectedCategory.translations?.[0];

    this.form = this.fb.group({
      name: [defaultTranslation?.name || '', Validators.required],
      description: [defaultTranslation?.description || ''],
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
      parentCategoryId: null, // Ana kategori olarak kaydet
    } as CreateUpdateCategoryDto;

    if (this.selectedCategory.id) {
      this.categoryService
        .updateTranslation(
          this.selectedCategory.id,
          this.selectedCategory.translations[0].languageCode,
          {
            name: this.form.value.name,
            description: this.form.value.description,
            seoTitle: this.form.value.seoTitle,
            seoDescription: this.form.value.seoDescription,
            seoKeywords: this.form.value.seoKeywords,
          }
        )
        .subscribe(() => {
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
    this.confirmation
      .warn('Kategori silinecek', 'Bu kategoriyi silmek istediğinizden emin misiniz?')
      .subscribe(status => {
        if (status === 'confirm') {
          alert('delete eklenecek.');
          //this.categoryService.delete(id).subscribe(() => {
          //  this.loadCategories();
          //});
        }
      });
  }
}
