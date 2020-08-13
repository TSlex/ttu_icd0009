import { autoinject, PLATFORM } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { StudentHomeworkPostDTO, StudentHomeworkPutDTO } from 'types/StudentHomeworks/StudentHomeworkDTO';
import { StudentHomeworksApi } from 'services/StudentHomeworksApi';
import { HomeworkGetDTO } from 'types/Homeworks/HomeworkDTO';

@autoinject
export class CreateEdit {
    constructor(private studentHomeworksApi: StudentHomeworksApi, private router: Router) {
    }

    private id?: string;
    private homeworkId!: string;

    private homework!: HomeworkGetDTO;

    private deadline?: Date;
    private studentAnswer?: string;
    private title!: string;

    onPost() {
        var postModel: StudentHomeworkPostDTO = {
            homeWorkId: this.homeworkId,
            studentAnswer,
            studentSubjectId,
        }

        this.studentHomeworksApi.createHomeworkAnswer(postModel).then(response => {
            // 
            this.onCancel();
        })
    }

    onPut() {
        var putModel: StudentHomeworkPutDTO = {
            id: this.id!,
            homeWorkId: this.homeworkId,
            studentAnswer,
            studentSubjectId,
        }

        this.studentHomeworksApi.updateHomeworkAnswer(putModel).then(response => {
            // 
            this.onCancel();
        })
    }

    onCancel() {
        this.router.navigateBack();
    }

    //bind params
    activate(params: any) {
        if (params.homeworkId && typeof (params.homeworkId) == 'string') {
            this.homeworkId = params.homeworkId;
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
