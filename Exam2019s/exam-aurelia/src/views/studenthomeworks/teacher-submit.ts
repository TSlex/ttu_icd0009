import { Router } from 'aurelia-router';
import { autoinject, PLATFORM } from 'aurelia-framework';
import { HomeworkGetDTO } from 'types/Homeworks/HomeworkDTO';
import { HomeworksApi } from 'services/HomeworksApi';
import { StudentHomeworksApi } from 'services/StudentHomeworksApi';
import { StudentHomeworkDTO, StudentHomeworkTeacherSubmitDTO } from 'types/StudentHomeworks/StudentHomeworkDTO';

@autoinject
export class TeacherSubmit {
    constructor(private studentHomeworksApi: StudentHomeworksApi, private homeworksApi: HomeworksApi, private router: Router) {
    }

    private homework!: HomeworkGetDTO;
    private model!: StudentHomeworkDTO;

    private homeworkId!: string;
    private studentSubjectId!: string;

    onSubmit() {
        if (!(this.model.grade >= -1 && this.model.grade < 6)) return
        var submitModel: StudentHomeworkTeacherSubmitDTO = {
            grade: this.model.grade,
            id: this.model.id,
            isAccepted: this.model.isAccepted,
            isChecked: this.model.isChecked
        };

        this.studentHomeworksApi.teacherSubmit(submitModel).then(response => {
            this.onCancel()
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
        if (params.studentSubjectId && typeof (params.studentSubjectId) == 'string') {
            this.studentSubjectId = params.studentSubjectId;
        }
    }

    async created() {
        const homeworkResponse = await this.homeworksApi.getHomeworkModel(this.homeworkId)

        if (homeworkResponse.errors?.length > 0) this.onCancel();

        this.homework = homeworkResponse.data!


        const modelResponse = await this.studentHomeworksApi.getSubmitModel(this.homeworkId, this.studentSubjectId)

        if (modelResponse.errors?.length > 0) this.onCancel();

        this.model = modelResponse.data!
    }
}
