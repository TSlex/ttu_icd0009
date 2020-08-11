import { AppState } from './state/state';
import { autoinject, PLATFORM } from 'aurelia-framework';
import { RouterConfiguration, Router, Redirect } from 'aurelia-router';

@autoinject
export class App {
    router?: Router;

    get isLoading() {
        return this.appState.isComponentLoading;
    }

    get userName() {
        return this.appState.userName;
    }

    constructor(private appState: AppState) { }

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

            // posts
            // { route: ['posts', 'posts/index'], name: 'posts', moduleId: PLATFORM.moduleName('views/posts/index'), nav: true, title: 'Posts', settings: { auth: true } },
            // { route: ['posts/create'], name: 'posts-create', moduleId: PLATFORM.moduleName('views/posts/create-edit'), settings: { auth: true } },
            // { route: ['posts/edit/:id'], name: 'posts-edit', moduleId: PLATFORM.moduleName('views/posts/create-edit'), settings: { auth: true } },
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
