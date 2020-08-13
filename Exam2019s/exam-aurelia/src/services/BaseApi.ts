import { autoinject } from 'aurelia-framework';
import { HttpClient, json } from 'aurelia-fetch-client';
import { AppState } from 'state/state';
import { IFetchResponse } from 'types/Response/IFetchResponseDTO';
import { parseResponse } from 'helpers/ResponseParser';

@autoinject
export class BaseApi {

    protected fetchUrl: string;

    protected headers: HeadersInit = {
        Authorization: 'Bearer ',
        "Content-Type": "application/json",
        'accept-language': 'en-GB'
    }

    constructor(protected appState: AppState, protected url: string, protected httpClient: HttpClient) {
        (this.headers as any)['Authorization'] += this.appState.jwt;
        this.fetchUrl = this.appState.baseUrl + "/" + url;
    }

    protected async _index<TData>(input: string | Request, init?: RequestInit): Promise<IFetchResponse<TData[]>> {
        try {
            const response = await this.httpClient.get(input, init)

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return {
                        status: response.status.toString(),
                        errors: [],
                        data: (await response.json()) as TData[]
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

    protected async _details<TData>(input: string | Request, init?: RequestInit): Promise<IFetchResponse<TData>> {
        try {
            const response = await this.httpClient.get(input, init)

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return {
                        status: response.status.toString(),
                        errors: [],
                        data: (await response.json()) as TData
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

    protected async _create(input: string | Request, body?: any, init?: RequestInit): Promise<IFetchResponse<any>> {
        try {
            const response = await this.httpClient.post(input, JSON.stringify(body), init)

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

    protected async _createAndReturn<TData>(input: string | Request, body?: any, init?: RequestInit): Promise<IFetchResponse<TData>> {
        try {
            const response = await this.httpClient.post(input, JSON.stringify(body), init)

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return {
                        status: response.status.toString(),
                        errors: [],
                        data: (await response.json()) as TData
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

    protected async _edit(input: string | Request, body?: any, init?: RequestInit): Promise<IFetchResponse<any>> {
        try {
            const response = await this.httpClient.put(input, JSON.stringify(body), init)

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

    protected async _delete(input: string | Request, init?: RequestInit): Promise<IFetchResponse<any>> {
        try {
            const response = await this.httpClient.delete(input, {}, init)

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
