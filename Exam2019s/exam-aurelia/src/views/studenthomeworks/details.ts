import { autoinject, PLATFORM } from 'aurelia-framework';
import { Router } from 'aurelia-router';

@autoinject
export class Details {
    constructor(private router: Router) {
    }

    onCancel() {
        this.router.navigateBack();
    }
}
