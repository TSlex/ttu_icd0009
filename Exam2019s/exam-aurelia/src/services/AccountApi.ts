import { autoinject } from 'aurelia-framework';
import { AppState } from 'state/state';
import { HttpClient, json } from 'aurelia-fetch-client';
import { BaseApi } from './BaseApi';

import { parseResponse } from 'helpers/ResponseParser';

import { IFetchResponse } from 'types/Response/IFetchResponseDTO';
import { IJwtResponseDTO } from 'types/Response/IJwtResponseDTO';
import { IRegisterDTO } from 'types/Identity/IRegisterDTO';
import { ILoginDTO } from 'types/Identity/ILoginDTO';
import { IEmailDTO } from 'types/Identity/IEmailDTO';
import { IUserDataDTO } from 'types/Identity/IUserDataDTO';
import { IPasswordDTO } from 'types/Identity/IPasswordDTO';
import { IDeleteDTO } from 'types/Identity/IDeleteDTO';
import { IResponseDTO } from 'types/Response/IResponseDTO';


@autoinject
export class AccountApi extends BaseApi {

    constructor(protected appState: AppState, protected httpClient: HttpClient) {
        super(appState, "identity", httpClient);
    }

    async login(loginModel: ILoginDTO): Promise<IJwtResponseDTO> {
        const url = `${this.fetchUrl}/login`;

        try {
            const response = await this.httpClient.post(url, JSON.stringify(loginModel))

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return (await response.json()) as IJwtResponseDTO
                default:
                    return parseResponse(response) as any
            }
        } catch (reason) {
            return {
                errors: ["Authorisation fails"]
            } as IJwtResponseDTO
        }
    }

    async register(registerModel: IRegisterDTO): Promise<IFetchResponse<IResponseDTO>> {
        const url = `${this.fetchUrl}/register`;

        try {
            const response = await this.httpClient.post(url, JSON.stringify(registerModel))

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return parseResponse(response)
                default:
                    return parseResponse(response)
            }
        } catch (reason) {
            return {
                errors: ["Authorisation fails"]
            } as IFetchResponse<any>
        }
    }

    async getEmail(): Promise<IFetchResponse<string>> {
        const url = `${this.fetchUrl}/getemail`

        try {
            const response = await this.httpClient.get(url, { headers: this.headers });

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return {
                        status: response.status.toString(),
                        errors: [],
                        data: (await response.json()) as string
                    }
                default:
                    return {
                        status: response.status.toString(),
                        errors: [(await response.json()).errors],
                        data: null
                    }
            }
        } catch (reason) {
            return {
                status: "-1",
                errors: [reason],
                data: null
            }
        }
    }

    async putEmail(emailModel: IEmailDTO): Promise<IFetchResponse<IResponseDTO>> {
        const url = `${this.fetchUrl}/updateappuseremail`

        try {
            const response = await this.httpClient.put(url, JSON.stringify(emailModel), { headers: this.headers });

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return parseResponse(response)
                default:
                    return parseResponse(response)
            }
        } catch (reason) {
            return {
                status: "-1",
                errors: [reason],
                data: null
            }
        }
    }

    async getUserData(): Promise<IFetchResponse<IUserDataDTO>> {
        const url = `${this.fetchUrl}/getappuserdata`

        try {
            const response = await this.httpClient.get(url, { headers: this.headers });

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return {
                        status: response.status.toString(),
                        errors: [],
                        data: (await response.json()) as IUserDataDTO
                    }
                default:
                    return {
                        status: response.status.toString(),
                        errors: [(await response.json()).errors],
                        data: null
                    }
            }
        } catch (reason) {
            return {
                status: "-1",
                errors: [reason],
                data: null
            }
        }
    }

    async putUserData(dataModel: IUserDataDTO): Promise<IFetchResponse<IResponseDTO>> {
        const url = `${this.fetchUrl}/updateappuserdata`

        try {
            const response = await this.httpClient.put(url, JSON.stringify(dataModel), { headers: this.headers });

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return parseResponse(response)
                default:
                    return parseResponse(response)
            }
        } catch (reason) {
            return {
                status: "-1",
                errors: [reason],
                data: null
            }
        }
    }

    async putPassword(passwordModel: IPasswordDTO): Promise<IFetchResponse<IResponseDTO>> {
        const url = `${this.fetchUrl}/updateappuserpassword`

        try {
            const response = await this.httpClient.put(url, JSON.stringify(passwordModel), { headers: this.headers });

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return parseResponse(response)
                default:
                    return parseResponse(response)
            }
        } catch (reason) {
            return {
                status: "-1",
                errors: [reason],
                data: null
            }
        }
    }

    async deleteUser(model: IDeleteDTO): Promise<IFetchResponse<IResponseDTO>> {
        const url = `${this.fetchUrl}/deleteappuser/${this.appState.userId}`;
        try {
            const response = await this.httpClient.post(url, JSON.stringify(model), { headers: this.headers });

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return parseResponse(response)
                default:
                    return parseResponse(response)
            }
        } catch (reason) {
            return {
                status: "-1",
                errors: [reason],
                data: null
            }
        }
    }
}
