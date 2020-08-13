import { IdentityStore } from './../../components/IdentityStore';
import { AppState } from './../../state/state';
import { autoinject, PLATFORM } from 'aurelia-framework';
import { StudentSubjectDTO } from 'types/StudentSubjects/StudentSubjectDTO';
import { StudentSubjectsApi } from 'services/StudentSubjectsApi';
import { IFetchResponse } from 'types/Response/IFetchResponseDTO';

@autoinject
export class Index extends IdentityStore {
    constructor(appState: AppState, private studentSubjectsApi: StudentSubjectsApi) {
        super(appState);
    }

    private model!: StudentSubjectDTO[]

    private get Students() {
        return this.model
    }

    private get AcceptedStudents() {
        return this.model.filter(student => student.isAccepted)
    }

    private get NotAcceptedStudents() {
        return this.model.filter(student => !student.isAccepted)
    }

    async activate(params: any) {
        if (params.id && typeof (params.id) == 'string') {
            this.studentSubjectsApi.getStudents(params.id)
                .then((response: IFetchResponse<StudentSubjectDTO[]>) => {
                    if (response?.errors?.length === 0) {
                        this.model = response.data!
                    }
                })
        }
    }
}
