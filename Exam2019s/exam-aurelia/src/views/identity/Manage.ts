import { ViewBase } from 'components/ViewBase';
import { ManageNav } from './ManageNav';

export class Manage extends ViewBase {

    private currentPage: ManageNav = ManageNav.UserData;
    private navLoaded: boolean = true;

    get ManageNavs() {
        return ManageNav;
    }

    resolveStartup(page: string): ManageNav {
        let result: ManageNav = ManageNav.UserData

        for (let key in ManageNav) {
            let value: string = ManageNav[key as keyof typeof ManageNav];
            if (value === page) {
                result = value as ManageNav;
            }
        }

        return result;
    }

    getNavStyle(page: ManageNav) {
        if (page === this.currentPage) {
            return "nav-link active"
        } else {
            return "nav-link"
        }
    }

    activate(params: any) {
        this.setLoaded(false)

        // bind properties
        if (params.page && typeof (params.page) == 'string') {
            this.setPage(this.resolveStartup(params.page))
        } else {
            this.setPage(this.resolveStartup(ManageNav.UserData))
        }

        this.setLoaded(true)
    }

    async setPage(page: ManageNav) {
        this.navLoaded = false;
        this.currentPage = page;
        setTimeout(() => { this.navLoaded = true; }, .001)
    }
}
