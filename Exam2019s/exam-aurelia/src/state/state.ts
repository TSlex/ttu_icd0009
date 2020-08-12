import JwtDecode from "jwt-decode";

export class AppState {
    constructor() {
    }

    public readonly baseUrl = 'https://localhost:5001/api/v1';

    // component loading
    private _isComponentLoading: boolean = false

    get isComponentLoading() {
        return this._isComponentLoading;
    }

    set isComponentLoading(value: boolean) {
        this._isComponentLoading = value;
    }

    // identity
    get jwt(): string | null {
        return localStorage.getItem('jwt');
    }

    set jwt(value: string | null) {
        if (value) {
            localStorage.setItem('jwt', value);
        } else {
            localStorage.removeItem('jwt');
        }
    }

    get isAuthenticated() {
        return this.jwt != null;
    }

    get userId() {
        if (this.isAuthenticated) {
            const decoded = JwtDecode(this.jwt!) as Record<string, string>;
            return decoded[
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
            ];
        }
        return "null";
    }

    get userName() {
        if (this.isAuthenticated) {
            const decoded = JwtDecode(this.jwt!) as Record<string, string>;
            return decoded[
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
            ];
        }
        return "null";
    }

    get isTeacher() {
        var result: boolean = false
        this.userRoles.forEach((element: string) => {
            if (element.toLowerCase().indexOf("teacher") !== -1) {
                result = true;
            }
        });

        return result;
    }

    get isStudent() {
        var result: boolean = false
        this.userRoles.forEach((element: string) => {
            if (element.toLowerCase().indexOf("student") !== -1) {
                result = true;
            }
        });

        return result;
    }

    get isAdmin() {
        var result: boolean = false
        this.userRoles.forEach((element: string) => {
            if (element.toLowerCase().indexOf("admin") !== -1) {
                result = true;
            }
        });

        return result;
    }

    get userRoles() {
        if (this.isAuthenticated) {
            const decoded = JwtDecode(this.jwt!) as Record<string, string>;
            return decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"].toString().split(',') as string[]
        }
        return [];
    }

}
