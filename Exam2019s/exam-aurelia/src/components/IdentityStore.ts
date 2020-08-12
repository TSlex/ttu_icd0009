import { autoinject, PLATFORM } from 'aurelia-framework';
import { AppState } from 'state/state';


@autoinject
export class IdentityStore {

    constructor(protected appState: AppState) { }

    get userName() {
        return this.appState.userName;
    }

    get isTeacher() {
        return this.appState.isTeacher;
    }

    get isStudent() {
        return this.appState.isStudent;
    }

    get isAuthenticated() {
        return this.appState.isAuthenticated;
    }

    get isNotValidated() {
        return !(this.isTeacher || this.isStudent)
    }
}
