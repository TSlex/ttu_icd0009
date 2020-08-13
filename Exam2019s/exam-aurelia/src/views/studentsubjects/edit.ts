import { autoinject, PLATFORM } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { IdentityStore } from 'components/IdentityStore';
import { AppState } from 'state/state';
import { StudentSubjectsApi } from 'services/StudentSubjectsApi';
import { StudentSubjectDTO, StudentSubjectPutDTO } from 'types/StudentSubjects/StudentSubjectDTO';

@autoinject
export class Edit extends IdentityStore {
    constructor(appState: AppState, private studentSubjectsApi: StudentSubjectsApi, private router: Router) {
        super(appState);
    }

    private model!: StudentSubjectDTO;

    onSubmit() {
        var putModel: StudentSubjectPutDTO = {
            id: this.model.id,
            isAccepted: this.model.isAccepted,
            grade: this.model.grade
        }

        this.studentSubjectsApi.updateStudent(putModel).then(response => {
            this.onCancel();
        })
    }

    async activate(params: any) {
        if (params.id && typeof (params.id) == 'string') {
            const response = await this.studentSubjectsApi.getStudent(params.id)

            if (response.errors?.length > 0) this.onCancel();

            this.model = response.data!;
        }
    }

    onCancel() {
        this.router.navigateToRoute('subjects')
    }
}
