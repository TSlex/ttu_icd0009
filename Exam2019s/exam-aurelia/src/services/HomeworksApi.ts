import { autoinject } from 'aurelia-framework';
import { AppState } from 'state/state';
import { HttpClient, json } from 'aurelia-fetch-client';
import { BaseApi } from './BaseApi';

import { parseResponse } from 'helpers/ResponseParser';
import { HomeworkDTO, HomeworkPostDTO, HomeworkGetDTO, HomeworkPutDTO } from 'types/Homeworks/HomeworkDTO';
import { IFetchResponse } from 'types/Response/IFetchResponseDTO';

@autoinject
export class HomeworksApi extends BaseApi {

    constructor(protected appState: AppState, protected httpClient: HttpClient) {
        super(appState, "homeworks", httpClient);
    }

    async getHomeworkDetails(id: string): Promise<IFetchResponse<HomeworkDTO>> {
        const url = `${this.fetchUrl}/${id}`;

        return await this._details<HomeworkDTO>(url, { headers: this.headers })
    }
    async createHomework(model: HomeworkPostDTO): Promise<IFetchResponse<any>> {
        const url = `${this.fetchUrl}`;

        return await this._create(url, model, { headers: this.headers })
    }
    async getHomeworkModel(id: string): Promise<IFetchResponse<HomeworkGetDTO>> {
        const url = `${this.fetchUrl}/editmodel/${id}`;

        return await this._details<HomeworkGetDTO>(url, { headers: this.headers })
    }
    async updateHomework(model: HomeworkPutDTO): Promise<IFetchResponse<any>> {
        const url = `${this.fetchUrl}`;

        return await this._edit(url, model, { headers: this.headers })
    }

    async deleteHomework(id: string): Promise<IFetchResponse<any>> {
        const url = `${this.fetchUrl}/${id}`;

        return await this._delete(url, { headers: this.headers })
    }
}
