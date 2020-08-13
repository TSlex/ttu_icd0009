import { autoinject, PLATFORM } from 'aurelia-framework';
import { SubjectsApi } from 'services/SubjectsApi';
import { ISubjectDetailsDTO, ISubjectStudentDetailsDTO, ISubjectTeacherDetailsDTO } from 'types/Subjects/ISubjectDTO';
import { IFetchResponse } from 'types/Response/IFetchResponseDTO';

@autoinject
export class DetailsSwitcher {
    constructor(private subjectsApi: SubjectsApi) { }

    private model!: ISubjectDetailsDTO | ISubjectStudentDetailsDTO | ISubjectTeacherDetailsDTO

    async activate(params: any) {
        if (params.id && typeof (params.id) == 'string') {
            this.subjectsApi.getDetails(params.id)
                .then((response: IFetchResponse<ISubjectDetailsDTO | ISubjectStudentDetailsDTO | ISubjectTeacherDetailsDTO>) => {

                    if (response?.errors?.length === 0) {
                        this.model = response.data!
                    }
                })
        }
    }
}
