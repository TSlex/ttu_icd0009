import { autoinject, bindable } from 'aurelia-framework';
import { ISubjectTeacherDetailsDTO } from 'types/Subjects/ISubjectDTO';
import { HomeworksApi } from 'services/HomeworksApi';

@autoinject
export class TeacherDetails {
    constructor(private homeworksApi: HomeworksApi) { }

    @bindable public model!: ISubjectTeacherDetailsDTO

    onDeleteHomework(homeworkId: string) {
        this.homeworksApi.deleteHomework(homeworkId).then(response => {
            this.model.homeworks.forEach((element, index, arr) => {
                if (element.id == homeworkId) {
                    this.model.homeworks.splice(index, 1)
                }
            })
        })
    }
}
