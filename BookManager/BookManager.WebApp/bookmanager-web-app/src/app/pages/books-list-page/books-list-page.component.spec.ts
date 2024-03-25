import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BooksListPageComponent } from './books-list-page.component';

describe('BooksListPageComponent', () => {
  let component: BooksListPageComponent;
  let fixture: ComponentFixture<BooksListPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BooksListPageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BooksListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
