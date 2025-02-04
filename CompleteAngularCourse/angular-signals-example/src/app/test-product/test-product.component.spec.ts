import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestProductComponent } from './test-product.component';

describe('TestProductComponent', () => {
  let component: TestProductComponent;
  let fixture: ComponentFixture<TestProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TestProductComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
