import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginProposaleComponent } from './login-proposale.component';

describe('LoginProposaleComponent', () => {
  let component: LoginProposaleComponent;
  let fixture: ComponentFixture<LoginProposaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginProposaleComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LoginProposaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
