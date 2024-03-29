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

        config.addAuthorizeStep(new AuthorizeStep(this.appState))

        // rules - { auth: true, noTeacher: true, noStudent: true }

        config.map([
            { route: ['', 'home', 'home/index'], name: 'home', moduleId: PLATFORM.moduleName('views/home/index'), nav: false, title: 'Home' },

            { route: ['account/login'], name: 'account-login', moduleId: PLATFORM.moduleName('views/account/login'), nav: false, title: 'Login' },
            { route: ['account/register'], name: 'account-register', moduleId: PLATFORM.moduleName('views/account/register'), nav: false, title: 'Register' },

            // Manage
            { route: ['account/manage/:page?'], name: 'account-manage', moduleId: PLATFORM.moduleName('views/identity/manage'), nav: false, title: 'Manage', settings: { auth: true } },

            // subjects
            { route: ['subjects', 'subjects/index'], name: 'subjects', moduleId: PLATFORM.moduleName('views/subjects/subjects'), nav: false, title: 'Subjects' },
            { route: ['subjects/my'], name: 'student-subjects', moduleId: PLATFORM.moduleName('views/subjects/student-subjects'), nav: false, title: 'Subjects', settings: { auth: true } },
            { route: ['subjects/:id'], name: 'subject-details', moduleId: PLATFORM.moduleName('views/subjects/switcher'), nav: false, title: 'Details' },

            // my semester
            { route: ['semesters', 'semesters/index'], name: 'semesters', moduleId: PLATFORM.moduleName('views/semesters/index'), nav: false, title: 'Semesters', settings: { auth: true, noTeacher: true } },

            // studentsubjects
            { route: ['studentsubjects/:id'], name: 'studentsubjects', moduleId: PLATFORM.moduleName('views/studentsubjects/index'), nav: false, title: 'Students', settings: { auth: true } },
            { route: ['studentsubjects/edit/:id'], name: 'studentsubjects-edit', moduleId: PLATFORM.moduleName('views/studentsubjects/edit'), nav: false, title: 'Manage', settings: { auth: true, noStudent: true } },

            // homeworks
            { route: ['homeworks/create/:subjectId'], name: 'homeworks-create', moduleId: PLATFORM.moduleName('views/homeworks/create-edit'), nav: false, title: 'New Homework', settings: { auth: true, noStudent: true } },
            { route: ['homeworks/edit/:subjectId/:id'], name: 'homeworks-edit', moduleId: PLATFORM.moduleName('views/homeworks/create-edit'), nav: false, title: 'Manage', settings: { auth: true, noStudent: true } },
            { route: ['homeworks/:id'], name: 'homeworks-details', moduleId: PLATFORM.moduleName('views/homeworks/details'), nav: false, title: 'Manage', settings: { auth: true, noStudent: true } },

            // student homeworks
            { route: ['studenthomeworks/create/:homeworkId/:studentSubjectId'], name: 'studenthomeworks-create', moduleId: PLATFORM.moduleName('views/studenthomeworks/create-edit'), nav: false, title: 'Answer', settings: { auth: true, noTeacher: true } },
            { route: ['studenthomeworks/edit/:homeworkId/:id'], name: 'studenthomeworks-edit', moduleId: PLATFORM.moduleName('views/studenthomeworks/create-edit'), nav: false, title: 'Edit', settings: { auth: true, noTeacher: true } },
            { route: ['studenthomeworks/:id'], name: 'studenthomeworks-details', moduleId: PLATFORM.moduleName('views/studenthomeworks/details'), nav: false, title: 'Manage', settings: { auth: true, noTeacher: true } },
            { route: ['studenthomeworks/:homeworkId/:studentSubjectId/teacher'], name: 'teacher-homeworks-submit', moduleId: PLATFORM.moduleName('views/studenthomeworks/teacher-submit'), nav: false, title: 'Manage', settings: { auth: true, noStudent: true } },
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
    constructor(private appState: AppState) {
    }

    run(navigationInstruction: any, next: any) {
        if (navigationInstruction.getAllInstructions().some((i: any) => i.config.settings?.auth ?? false)) {
            if (localStorage.getItem('jwt') === null) {
                return next.cancel(new Redirect('account/login'));
            }
        }
        if (navigationInstruction.getAllInstructions().some((i: any) => i.config.settings?.noTeacher ?? false)) {
            if (this.appState.isTeacher) {
                return next.cancel(new Redirect('home'));
            }
        }
        if (navigationInstruction.getAllInstructions().some((i: any) => i.config.settings?.noStudent ?? false)) {
            if (this.appState.isStudent) {
                return next.cancel(new Redirect('home'));
            }
        }

        return next();
    }
}
