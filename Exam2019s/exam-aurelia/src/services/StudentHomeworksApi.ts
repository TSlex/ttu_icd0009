import { IFetchResponse } from 'types/Response/IFetchResponseDTO';
import { autoinject } from 'aurelia-framework';
import { AppState } from 'state/state';
import { HttpClient, json } from 'aurelia-fetch-client';
import { BaseApi } from './BaseApi';

import { parseResponse } from 'helpers/ResponseParser';
import { StudentHomeworkDTO } from 'types/Homeworks/HomeworkDTO';
import { StudentHomeworkTeacherSubmitDTO, StudentHomeworkPutDTO, StudentHomeworkPostDTO } from 'types/StudentHomeworks/StudentHomeworkDTO';

@autoinject
export class StudentHomeworksApi extends BaseApi {

    constructor(protected appState: AppState, protected httpClient: HttpClient) {
        super(appState, "studenthomeworks", httpClient);
    }

    async getHomeworkAnswerDetails(id: String): Promise<IFetchResponse<StudentHomeworkDTO>> {
        const url = `${this.fetchUrl}/${id}`;

        return await this._details<StudentHomeworkDTO>(url, { headers: this.headers })
    }
    async createHomeworkAnswer(model: StudentHomeworkPostDTO): Promise<IFetchResponse<any>> {
        const url = `${this.fetchUrl}`;

        return await this._create(url, model, { headers: this.headers })
    }
    async updateHomeworkAnswer(model: StudentHomeworkPutDTO): Promise<IFetchResponse<any>> {
        const url = `${this.fetchUrl}`;

        return await this._edit(url, model, { headers: this.headers })
    }
    async getSubmitModel(homeworkId: String, studentSubjectId: String): Promise<IFetchResponse<StudentHomeworkDTO>> {
        const url = `${this.fetchUrl}/${homeworkId}/${studentSubjectId}/teacher`;

        return await this._details<StudentHomeworkDTO>(url, { headers: this.headers })
    }
    async teacherSubmit(model: StudentHomeworkTeacherSubmitDTO): Promise<IFetchResponse<any>> {
        const url = `${this.fetchUrl}/teacher`;

        return await this._edit(url, model, { headers: this.headers })
    }
}
