import { IResponseDTO } from 'types/Response/IResponseDTO';
import { Router } from 'aurelia-router';
import { autoinject } from "aurelia-framework"
import { AppState } from "state/state";
import { AccountApi } from 'services/AccountApi';
import { IRegisterDTO } from 'types/Identity/IRegisterDTO';
import { IFetchResponse } from 'types/Response/IFetchResponseDTO';

@autoinject
export class AccountRegister {
    private passwordConfirmation = "";

    private model: IRegisterDTO = {
        username: "",
        firstName: "",
        lastName: "",
        email: "",
        password: ""
    };

    private errors: string[] = [];

    constructor(private accountApi: AccountApi, private appState: AppState, private router: Router) {
    }

    onSubmit(event: Event) {
        event.preventDefault();

        if (this.model.password !== this.passwordConfirmation) {
            this.errors = ["Passwords must match!"]
            return
        }

        if (
            this.model.firstName.length > 0 &&
            this.model.lastName.length > 0 &&
            this.model.username.length > 0 &&
            this.model.email.length > 0 &&
            this.model.password.length > 0
        ) {
            const element = (document.querySelector('button[type="submit"]') as HTMLButtonElement)

            element.disabled = true;

            this.errors = [];

            this.accountApi.register(this.model).then(
                (response: IFetchResponse<IResponseDTO>) => {
                    element.disabled = false;
                    if (response?.errors) {
                        this.errors = response.errors;
                    } else {
                        this.router!.navigateToRoute('account-login');
                    }
                }
            )
        }
    }
}
