import { Router } from 'aurelia-router';
import { autoinject, PLATFORM } from 'aurelia-framework';

@autoinject
export class TeacherSubmit {
    constructor(private router: Router) {
    }

    onCancel() {
        this.router.navigateBack();
    }
}
