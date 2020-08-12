import { autoinject } from 'aurelia-framework';
import { AppState } from 'state/state';
import { HttpClient, json } from 'aurelia-fetch-client';
import { BaseApi } from './BaseApi';

import { parseResponse } from 'helpers/ResponseParser';

@autoinject
export class SemestersApi extends BaseApi {

    constructor(protected appState: AppState, protected httpClient: HttpClient) {
        super(appState, "semesters", httpClient);
    }
}
