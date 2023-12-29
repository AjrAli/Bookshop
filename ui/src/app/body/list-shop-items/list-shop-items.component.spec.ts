import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListShopItemsComponent } from './list-shop-items.component';

describe('ListShopItemsComponent', () => {
  let component: ListShopItemsComponent;
  let fixture: ComponentFixture<ListShopItemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListShopItemsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ListShopItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
