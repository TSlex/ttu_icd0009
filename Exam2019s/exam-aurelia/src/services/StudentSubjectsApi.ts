import { autoinject } from 'aurelia-framework';
import { AppState } from 'state/state';
import { HttpClient, json } from 'aurelia-fetch-client';
import { BaseApi } from './BaseApi';

import { parseResponse } from 'helpers/ResponseParser';
import { IFetchResponse } from 'types/Response/IFetchResponseDTO';
import { ISubjectDTO } from 'types/Subjects/ISubjectDTO';
import { StudentSubjectDTO, StudentSubjectPutDTO } from 'types/StudentSubjects/StudentSubjectDTO';

@autoinject
export class StudentSubjectsApi extends BaseApi {

    constructor(protected appState: AppState, protected httpClient: HttpClient) {
        super(appState, "studentsubjects", httpClient);
    }

    async getStudents(subjectId: string): Promise<IFetchResponse<StudentSubjectDTO[]>> {
        const url = `${this.fetchUrl}/${subjectId}`;

        return await this._index<StudentSubjectDTO>(url, { headers: this.headers })
    }

    async getStudent(id: string): Promise<IFetchResponse<StudentSubjectDTO>> {
        const url = `${this.fetchUrl}/editmodel/${id}`;

        return await this._details<StudentSubjectDTO>(url, { headers: this.headers })
    }

    async updateStudent(model: StudentSubjectPutDTO): Promise<IFetchResponse<StudentSubjectDTO>> {
        const url = `${this.fetchUrl}`;

        return await this._edit(url, model, { headers: this.headers })
    }

    async registerToCourse(subjectId: string): Promise<IFetchResponse<any>> {
        const url = `${this.fetchUrl}/subject/register?subjectid=${subjectId}`;

        try {
            const response = await this.httpClient.post(url, JSON.stringify({}), { headers: this.headers })

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return {
                        status: response.status.toString(),
                        errors: [],
                        data: (await response.json()) as any[]
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

    async cancelRegistration(subjectId: string): Promise<IFetchResponse<any>> {
        const url = `${this.fetchUrl}/subject/unregister?subjectid=${subjectId}`;

        try {
            const response = await this.httpClient.post(url, JSON.stringify({}), { headers: this.headers })

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return {
                        status: response.status.toString(),
                        errors: [],
                        data: (await response.json()) as any[]
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

    async acceptStudent(studentSubjectId: string, subjectId: string): Promise<IFetchResponse<any>> {
        const url = `${this.fetchUrl}/student/new`;

        try {
            const response = await this.httpClient.post(url, JSON.stringify({ id: studentSubjectId, subjectId }), { headers: this.headers })

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return {
                        status: response.status.toString(),
                        errors: [],
                        data: (await response.json()) as any[]
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

    async removeStudent(studentSubjectId: string, subjectId: string): Promise<IFetchResponse<any>> {
        const url = `${this.fetchUrl}/student/remove`;

        try {
            const response = await this.httpClient.post(url, JSON.stringify({ id: studentSubjectId, subjectId }), { headers: this.headers })

            switch (response.status) {
                case 200:
                case 201:
                case 204:
                    return {
                        status: response.status.toString(),
                        errors: [],
                        data: (await response.json()) as any[]
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
