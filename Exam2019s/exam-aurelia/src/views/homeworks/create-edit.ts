import { HomeworksApi } from './../../services/HomeworksApi';
import { autoinject, PLATFORM } from 'aurelia-framework';
import { SubjectsApi } from 'services/SubjectsApi';
import { Router } from 'aurelia-router';
import { HomeworkPostDTO, HomeworkPutDTO } from 'types/Homeworks/HomeworkDTO';

@autoinject
export class Create {
    constructor(private homeworksApi: HomeworksApi, private subjectsApi: SubjectsApi, private router: Router) {
    }

    private subjectTitle!: string;

    private subjectId!: string;
    private id?: string;

    private deadline?: Date;
    private description?: string;
    private title!: string;

    onPost() {
        var postModel: HomeworkPostDTO = {
            deadline: this.deadline,
            subjectId: this.subjectId,
            title: this.title,
            description: this.description
        }

        this.homeworksApi.createHomework(postModel).then(response => {
            // 
        })
    }

    onPut() {
        var putModel: HomeworkPutDTO = {
            deadline: this.deadline,
            subjectId: this.subjectId,
            title: this.title,
            description: this.description,
            id: this.id!
        }

        this.homeworksApi.updateHomework(putModel).then(response => {
            // 
        })
    }

    onCancel() {
        this.router.navigateBack();
    }

    //bind params
    activate(params: any) {
        if (params.subjectId && typeof (params.subjectId) == 'string') {
            this.subjectId = params.subjectId;
        }
        if (params.id && typeof (params.id) == 'string') {
            this.id = params.id;
        }
    }

    async created() {
        const subject = await this.subjectsApi.getDetails(this.subjectId)

        if (subject.errors?.length > 0) return;

        this.subjectTitle = subject.data?.subjectTitle!

        if (this.id) {
            const homework = await this.homeworksApi.getHomeworkModel(this.id)

            if (homework.errors?.length > 0) return;

            this.deadline = homework.data?.deadline
            this.description = homework.data?.description
            this.title = homework.data?.title!
        }
    }
}
