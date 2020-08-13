import { autoinject } from 'aurelia-framework';
import { AppState } from 'state/state';
import { HttpClient, json } from 'aurelia-fetch-client';
import { BaseApi } from './BaseApi';

import { parseResponse } from 'helpers/ResponseParser';
import { IFetchResponse } from 'types/Response/IFetchResponseDTO';
import { ISubjectDTO, ISubjectDetailsDTO, ISubjectStudentDetailsDTO, ISubjectTeacherDetailsDTO } from 'types/Subjects/ISubjectDTO';

@autoinject
export class SubjectsApi extends BaseApi {

    constructor(protected appState: AppState, protected httpClient: HttpClient) {
        super(appState, "subjects", httpClient);
    }

    async getSubjects(): Promise<IFetchResponse<ISubjectDTO[]>> {
        const url = `${this.fetchUrl}`;

        return await this._index<ISubjectDTO>(url, { headers: this.headers })
    }

    async getMySubjects(): Promise<IFetchResponse<ISubjectDTO[]>> {
        const url = `${this.fetchUrl}/my`;

        return await this._index<ISubjectDTO>(url, { headers: this.headers })
    }

    async searchSubjects(keywords: string): Promise<IFetchResponse<ISubjectDTO[]>> {
        const url = `${this.fetchUrl}/search?keywords=${keywords}`;

        try {
            const response = await this.httpClient.post(url, JSON.stringify({}), { headers: this.headers })

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return {
                        status: response.status.toString(),
                        errors: [],
                        data: (await response.json()) as ISubjectDTO[]
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

    async searchMySubjects(keywords: string): Promise<IFetchResponse<ISubjectDTO[]>> {
        const url = `${this.fetchUrl}/search/my?keywords=${keywords}`;

        try {
            const response = await this.httpClient.post(url, JSON.stringify({ keywords: keywords }), { headers: this.headers })

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return {
                        status: response.status.toString(),
                        errors: [],
                        data: (await response.json()) as ISubjectDTO[]
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

    async getDetails(subjectId: string): Promise<IFetchResponse<ISubjectDetailsDTO | ISubjectStudentDetailsDTO | ISubjectTeacherDetailsDTO>> {
        const url = `${this.fetchUrl}/${subjectId}`;

        try {
            const response = await this.httpClient.get(url, { headers: this.headers })

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return {
                        status: response.status.toString(),
                        errors: [],
                        data: (await response.json()) as ISubjectDetailsDTO | ISubjectStudentDetailsDTO | ISubjectTeacherDetailsDTO
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
}
