import { autoinject, PLATFORM } from 'aurelia-framework';
import { HomeworksApi } from 'services/HomeworksApi';
import { HomeworkDTO } from 'types/Homeworks/HomeworkDTO';
import { Router } from 'aurelia-router';

@autoinject
export class Details {
    constructor(private homeworksApi: HomeworksApi, private router: Router) { }

    private id!: string;

    private model!: HomeworkDTO

    onEdit() {
        this.router.navigateToRoute("homeworks-edit", { subjectId: this.model.subjectId, id: this.model.id })
    }

    onReturn() {
        this.router.navigateBack()
    }

    activate(params: any) {
        if (params.id && typeof (params.id) == 'string') {
            this.id = params.id;
        }
    }

    async created() {
        if (this.id) {
            const response = await this.homeworksApi.getHomeworkDetails(this.id)

            if (response.errors?.length > 0) return;

            this.model = response.data!
        }
    }
}
