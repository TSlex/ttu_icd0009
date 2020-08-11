import { autoinject } from 'aurelia-framework';
import { AppState } from 'state/state';

@autoinject
export class HomeIndex {
    constructor(private appState: AppState) {
        this.appState.isComponentLoading = false;
    }
}
