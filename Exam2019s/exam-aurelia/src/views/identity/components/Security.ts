import { Router } from 'aurelia-router';
import { IFetchResponse } from 'types/Response/IFetchResponseDTO';
import { autoinject } from 'aurelia-framework';
import { ViewBase } from 'components/ViewBase';
import { AccountApi } from 'services/AccountApi';
import { AppState } from 'state/state';
import { IUserDataDTO } from 'types/Identity/IUserDataDTO';
import { IResponseDTO } from 'types/Response/IResponseDTO';
import { IDeleteDTO } from 'types/Identity/IDeleteDTO';

@autoinject
export class ManageSecurity extends ViewBase {

    private password: string = "";

    constructor(private accountApi: AccountApi, appState: AppState, private router: Router) {
        super(appState);
    }

    downloadData() {
        this.accountApi.getUserData().then(
            (response: IFetchResponse<IUserDataDTO>) => {
                let dataStr =
                    "data:text/json;charset=utf-8," +
                    encodeURIComponent(JSON.stringify(response.data, null, ' '));
                let downloadAnchorNode = document.createElement("a");
                downloadAnchorNode.setAttribute("href", dataStr);
                downloadAnchorNode.setAttribute("download", "personal.json");
                document.body.appendChild(downloadAnchorNode); // required for firefox
                downloadAnchorNode.click();
                downloadAnchorNode.remove();
            }
        );
    }

    deleteProfile() {
        if (this.password.length > 0) {
            this.accountApi.deleteUser({ password: this.password } as IDeleteDTO).then(
                (response: IFetchResponse<IResponseDTO>) => {
                    if (response.errors?.length > 0) {
                        this.errors = response.errors;
                    } else {
                        this.appState.jwt = null
                        this.router.navigateToRoute("account-login");
                    }
                }
            )
        }
    }
}
