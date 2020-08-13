import { AppState } from './state/state';
import { autoinject, PLATFORM } from 'aurelia-framework';
import { RouterConfiguration, Router, Redirect } from 'aurelia-router';
import { IdentityStore } from 'components/IdentityStore';

@autoinject
export class App extends IdentityStore {
    router?: Router;

    get isLoading() {
        return this.appState.isComponentLoading;
    }

    constructor(appState: AppState) {
        super(appState);
    }

    configureRouter(config: RouterConfiguration, router: Router): void {
        this.router = router;

        config.title = "Default";

        config.addAuthorizeStep(new AuthorizeStep())

        config.map([
            { route: ['', 'home', 'home/index'], name: 'home', moduleId: PLATFORM.moduleName('views/home/index'), nav: false, title: 'Home', settings: { auth: true } },

            { route: ['account/login'], name: 'account-login', moduleId: PLATFORM.moduleName('views/account/login'), nav: false, title: 'Login' },
            { route: ['account/register'], name: 'account-register', moduleId: PLATFORM.moduleName('views/account/register'), nav: false, title: 'Register' },

            // Manage
            { route: ['account/manage/:page?'], name: 'account-manage', moduleId: PLATFORM.moduleName('views/identity/manage'), nav: false, title: 'Manage', settings: { auth: true } },

            // subjects
            { route: ['subjects', 'subjects/index'], name: 'subjects', moduleId: PLATFORM.moduleName('views/subjects/subjects'), nav: false, title: 'Subjects' },
            { route: ['subjects/my'], name: 'student-subjects', moduleId: PLATFORM.moduleName('views/subjects/student-subjects'), nav: false, title: 'Subjects' },
            { route: ['subjects/:id'], name: 'subject-details', moduleId: PLATFORM.moduleName('views/subjects/switcher'), nav: false, title: 'Details' },

            // my semester
            { route: ['semesters', 'semesters/index'], name: 'semesters', moduleId: PLATFORM.moduleName('views/semesters/index'), nav: false, title: 'Semesters' },

            // studentsubjects
            { route: ['studentsubjects/:id'], name: 'studentsubjects', moduleId: PLATFORM.moduleName('views/studentsubjects/index'), nav: false, title: 'Students' },
            { route: ['studentsubjects/edit/:id'], name: 'studentsubjects-edit', moduleId: PLATFORM.moduleName('views/studentsubjects/edit'), nav: false, title: 'Manage' },

            // homeworks
            { route: ['homeworks/create/:subjectId'], name: 'homeworks-create', moduleId: PLATFORM.moduleName('views/homeworks/create-edit'), nav: false, title: 'New Homework' },
            { route: ['homeworks/edit/:subjectId/:id'], name: 'homeworks-edit', moduleId: PLATFORM.moduleName('views/homeworks/create-edit'), nav: false, title: 'Manage' },
            { route: ['homeworks/:id'], name: 'homeworks-details', moduleId: PLATFORM.moduleName('views/homeworks/details'), nav: false, title: 'Manage' },

        ]);

        config.addAuthorizeStep

        config.mapUnknownRoutes('views/home/index');
    }

    logout() {
        this.appState.jwt = null;
        this.router!.navigateToRoute('account-login');
    }
}

class AuthorizeStep {
    run(navigationInstruction: any, next: any) {
        if (navigationInstruction.getAllInstructions().some((i: any) => i.config.settings?.auth ?? false)) {
            if (localStorage.getItem('jwt') === null) {
                return next.cancel(new Redirect('account/login'));
            }
        }

        return next();
    }
}
