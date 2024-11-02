import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
  CategoryService,
  CategoryTranslationService,
  CategoryDto,
  CategoryTranslationDto,
  CreateCategoryTranslationDto,
  UpdateCategoryTranslationDto,
} from '@proxy/categories';
import { ConfirmationService } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-category-item-language',
  templateUrl: './category-item-language.component.html',
  styleUrls: ['./category-item-language.component.scss'],
})
export class CategoryItemLanguageComponent implements OnInit {
  categoryId: string;
  category: CategoryDto;
  isModalOpen = false;
  form: FormGroup;
  selectedLanguage: CategoryTranslationDto = null;
  availableLanguages = [
    { code: 'tr', name: 'Türkçe' },
    { code: 'en', name: 'English' },
    { code: 'ar', name: 'العربية' },
  ];

  constructor(
    private route: ActivatedRoute,
    private categoryService: CategoryService,
    private categoryTranslationService: CategoryTranslationService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.categoryId = params['id'];
      this.loadCategory();
    });
  }

  loadCategory() {
    if (this.categoryId) {
      this.categoryService.get(this.categoryId).subscribe(response => {
        this.category = response;
      });
    }
  }

  createTranslation() {
    this.selectedLanguage = null;
    this.buildForm();
    this.isModalOpen = true;
  }

  editTranslation(translation: CategoryTranslationDto) {
    this.selectedLanguage = translation;
    this.buildForm();
    this.isModalOpen = true;
  }

  buildForm() {
    this.form = this.fb.group({
      languageCode: [this.selectedLanguage?.languageCode || '', Validators.required],
      name: [this.selectedLanguage?.name || '', Validators.required],
      description: [this.selectedLanguage?.description || ''],
      seoTitle: [this.selectedLanguage?.seoTitle || ''],
      seoDescription: [this.selectedLanguage?.seoDescription || ''],
      seoKeywords: [this.selectedLanguage?.seoKeywords || ''],
    });

    if (this.selectedLanguage) {
      this.form.get('languageCode').disable();
    }
  }

  save() {
    if (this.form.invalid || !this.categoryId) {
      return;
    }

    const formValue = this.form.getRawValue();

    if (this.selectedLanguage) {
      // Update
      const updateRequest: UpdateCategoryTranslationDto = {
        categoryId: this.categoryId,
        languageCode: formValue.languageCode,
        name: formValue.name,
        description: formValue.description,
        seoTitle: formValue.seoTitle,
        seoDescription: formValue.seoDescription,
        seoKeywords: formValue.seoKeywords,
      };

      this.categoryTranslationService
        .updateTranslation(this.categoryId, formValue.languageCode, updateRequest)
        .subscribe({
          next: () => {
            this.isModalOpen = false;
            this.loadCategory();
          },
          error: error => {
            console.error('Error updating translation:', error);
          },
        });
    } else {
      // Create
      const createRequest: CreateCategoryTranslationDto = {
        categoryId: this.categoryId, // URL'den gelen categoryId kullanılıyor
        languageCode: formValue.languageCode,
        name: formValue.name,
        description: formValue.description,
        seoTitle: formValue.seoTitle,
        seoDescription: formValue.seoDescription,
        seoKeywords: formValue.seoKeywords,
      };

      this.categoryTranslationService.create(createRequest).subscribe({
        next: () => {
          this.isModalOpen = false;
          this.loadCategory();
        },
        error: error => {
          console.error('Error creating translation:', error);
        },
      });
    }
  }

  deleteTranslation(translation: CategoryTranslationDto) {
    if (!translation || !translation.languageCode || !this.categoryId) return;

    const translationsCount = this.category.translations.length;
    if (translationsCount <= 1) {
      this.confirmation.warn(
        'Uyarı',
        'En az bir dil çevirisi bulunmalıdır. Son çeviriyi silemezsiniz.'
      );
      return;
    }

    this.confirmation
      .warn(
        'Çeviri silinecek',
        `${this.getLanguageName(
          translation.languageCode
        )} dilindeki çeviriyi silmek istediğinizden emin misiniz?`,
        { messageLocalizationParams: [translation.languageCode] }
      )
      .subscribe(status => {
        if (status === 'confirm') {
          this.categoryTranslationService
            .deleteTranslation(this.categoryId, translation.languageCode)
            .subscribe(() => {
              this.loadCategory();
            });
        }
      });
  }

  getLanguageName(languageCode: string): string {
    return this.availableLanguages.find(l => l.code === languageCode)?.name || languageCode;
  }

  getUntranslatedLanguages(): typeof this.availableLanguages {
    const currentLanguageCodes = this.category?.translations.map(t => t.languageCode) || [];
    return this.availableLanguages.filter(lang => !currentLanguageCodes.includes(lang.code));
  }
}
