import { TestBed, async } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { sharedModules, sharedComponents } from './shared';
describe('AppComponent', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [
                ...sharedComponents,
            ],
            imports: [
                ...sharedModules,
            ],
        }).compileComponents();
    }));
    it('should create the app', async(() => {
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app).toBeTruthy();
    }));
});
