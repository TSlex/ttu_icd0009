import { autoinject, PLATFORM } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { StudentHomeworkPostDTO, StudentHomeworkPutDTO } from 'types/StudentHomeworks/StudentHomeworkDTO';
import { StudentHomeworksApi } from 'services/StudentHomeworksApi';
import { HomeworkGetDTO } from 'types/Homeworks/HomeworkDTO';
import { HomeworksApi } from 'services/HomeworksApi';

@autoinject
export class CreateEdit {
    constructor(private studentHomeworksApi: StudentHomeworksApi, private homeworksApi: HomeworksApi, private router: Router) {
    }

    private id?: string;
    private studentSubjectId?: string;

    private homeworkId!: string;
    private homework!: HomeworkGetDTO;

    private studentAnswer?: string;

    private deadline?: Date;
    private title!: string;
    private description!: string;

    onPost() {
        var postModel: StudentHomeworkPostDTO = {
            homeWorkId: this.homeworkId,
            studentAnswer: this.studentAnswer,
            studentSubjectId: this.studentSubjectId!,
        }

        this.studentHomeworksApi.createHomeworkAnswer(postModel).then(response => {
            // 
            this.onCancel();
        })
    }

    onPut() {
        var putModel: StudentHomeworkPutDTO = {
            id: this.id!,
            studentAnswer: this.studentAnswer,
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
        if (params.studentSubjectId && typeof (params.studentSubjectId) == 'string') {
            this.studentSubjectId = params.studentSubjectId;
        }
    }

    async created() {
        const homeworkResponse = await this.homeworksApi.getHomeworkModel(this.homeworkId)

        console.log(homeworkResponse)

        if (homeworkResponse.errors?.length > 0) this.onCancel();

        this.homework = homeworkResponse.data!

        this.deadline = this.homework.deadline;;
        this.description = this.homework.description;
        this.title = this.homework.title!;

        if (this.id) {
            const modelResponse = await this.studentHomeworksApi.getHomeworkAnswerDetails(this.id)

            if (modelResponse.errors?.length > 0) this.onCancel();

            this.studentAnswer = modelResponse.data!.studentAnswer

        } else {

            this.studentAnswer = ""
        }
    }
}
