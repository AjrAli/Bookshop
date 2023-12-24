import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlyerPanelComponent } from './flyer-panel.component';

describe('FlyerPanelComponent', () => {
  let component: FlyerPanelComponent;
  let fixture: ComponentFixture<FlyerPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FlyerPanelComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FlyerPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
