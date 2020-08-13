import { autoinject, PLATFORM } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { StudentHomeworksApi } from 'services/StudentHomeworksApi';
import { StudentHomeworkDTO } from 'types/StudentHomeworks/StudentHomeworkDTO';

@autoinject
export class Details {
    constructor(private studentHomeworksApi: StudentHomeworksApi, private router: Router) {
    }

    onCancel() {
        this.router.navigateBack();
    }

    private id!: string;

    private model!: StudentHomeworkDTO;

    //bind params
    activate(params: any) {
        if (params.id && typeof (params.id) == 'string') {
            this.id = params.id;
        }
    }

    async created() {
        const response = await this.studentHomeworksApi.getHomeworkAnswerDetails(this.id)

        if (response.errors?.length > 0) this.onCancel();

        this.model = response.data!
    }
}
