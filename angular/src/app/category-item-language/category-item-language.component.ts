import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CategoryService, CategoryDto } from '@proxy/categories';

@Component({
  selector: 'app-category-item-language',
  templateUrl: './category-item-language.component.html',
  styleUrls: ['./category-item-language.component.scss'],
})
export class CategoryItemLanguageComponent implements OnInit {
  categoryId: string;
  category: CategoryDto;

  constructor(private route: ActivatedRoute, private categoryService: CategoryService) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.categoryId = params['id'];
      this.loadCategory();
    });
  }

  loadCategory() {
    // Kategori detayını yükle
    this.categoryService.getAll().subscribe(response => {
      this.category = response.items.find(c => c.id === this.categoryId);
    });
  }
}
