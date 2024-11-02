import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryItemLanguageComponent } from './category-item-language.component';

describe('CategoryItemLanguageComponent', () => {
  let component: CategoryItemLanguageComponent;
  let fixture: ComponentFixture<CategoryItemLanguageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CategoryItemLanguageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryItemLanguageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
