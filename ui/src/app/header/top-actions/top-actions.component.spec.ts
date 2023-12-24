import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopActionsComponent } from './top-actions.component';

describe('TopActionsComponent', () => {
  let component: TopActionsComponent;
  let fixture: ComponentFixture<TopActionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopActionsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TopActionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
