import { autoinject } from 'aurelia-framework';
import { AppState } from 'state/state';
import { HttpClient, json } from 'aurelia-fetch-client';
import { BaseApi } from './BaseApi';

import { parseResponse } from 'helpers/ResponseParser';
import { ISemesterDTO } from 'types/Semesters/ISemesterDTO';
import { IFetchResponse } from 'types/Response/IFetchResponseDTO';

@autoinject
export class SemestersApi extends BaseApi {

    constructor(protected appState: AppState, protected httpClient: HttpClient) {
        super(appState, "semesters", httpClient);
    }

    async getSemesters(): Promise<IFetchResponse<ISemesterDTO[]>> {
        const url = `${this.fetchUrl}`;

        return await this._index<ISemesterDTO>(url, { headers: this.headers })
    }
}
