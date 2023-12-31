import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyShoppingcartComponent } from './my-shoppingcart.component';

describe('MyShoppingcartComponent', () => {
  let component: MyShoppingcartComponent;
  let fixture: ComponentFixture<MyShoppingcartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyShoppingcartComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MyShoppingcartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
