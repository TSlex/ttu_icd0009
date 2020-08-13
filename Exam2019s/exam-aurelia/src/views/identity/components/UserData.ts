import { IFetchResponse } from '../../../types/Response/IFetchResponseDTO';
import { autoinject } from 'aurelia-framework';
import { ViewBase } from 'components/ViewBase';
import { AccountApi } from 'services/AccountApi';
import { AppState } from 'state/state';
import { IUserDataDTO } from 'types/Identity/IUserDataDTO';
import { AlertType } from 'types/Enums/AlertType';
import { IResponseDTO } from 'types/Response/IResponseDTO';

@autoinject
export class ManageProfileData extends ViewBase {

    constructor(private accountApi: AccountApi, appState: AppState) {
        super(appState);

        this.setLocalLoaded(false);
    }

    private profileDataModel: IUserDataDTO | null = null;

    get UserName() {
        return this.appState.userName
    }

    bind() {
        this.accountApi.getUserData().then(
            (response: IFetchResponse<IUserDataDTO>) => {
                this.profileDataModel = response.data;
                this.setLocalLoaded(true);
            }
        );
    }

    onSubmit() {
        if (this.profileDataModel) {

            this.clearNotifier();

            this.accountApi.putUserData(this.profileDataModel).then(
                (response: IFetchResponse<IResponseDTO>) => {
                    console.log(response)
                    if (response.errors?.length > 0) {
                        this.errors = response.errors;
                    } else {
                        this.alert = { message: response.status, type: AlertType.Success, dismissable: true }
                    }
                }
            );
        }
    }

    unbind() {
        this.setLocalLoaded(false)
    }
}
