import { autoinject, PLATFORM } from 'aurelia-framework';
import { AppState } from 'state/state';
import { IAlertData } from 'types/IAlertData';

@autoinject
export class ViewBase {

    private _isLoaded: boolean = true;
    private _isLoading: boolean = true;

    protected alert: IAlertData | null = null;
    protected errors: string[] = [];

    clearNotifier() {
        this.errors = [];
        this.alert = null;

        document.querySelectorAll('alert').forEach((element: Element) => { element.remove() })
    }

    checkLoaded(whenLoad: boolean) {
        this.setLoaded(false)
        if (whenLoad) {
            this.isLoading = false;
            this.isLoaded = true;
        } else {
            this.isLoading = true;
            this.isLoaded = false;
        }
    }

    setLoaded(bool: boolean) {
        this.isLoaded = bool;
        this.isLoading = !bool;
    }

    setLocalLoaded(bool: boolean) {
        this.isLoaded = bool;
        this.isLocalLoading = !bool;
    }

    get isLoading() {
        return this.appState.isComponentLoading;
    }

    set isLoading(value: boolean) {
        this.appState.isComponentLoading = value;
        this._isLoading = value;
    }

    get isLocalLoading() {
        return this._isLoading;
    }

    set isLocalLoading(value: boolean) {
        this._isLoading = value;
    }

    get isLoaded() {
        return this._isLoaded;
    }

    set isLoaded(value: boolean) {
        this._isLoaded = value;
    }

    get bluredClass() {
        return this.isLoading ? 'blur-transition blur' : null;
    }

    constructor(protected appState: AppState) { }
}
