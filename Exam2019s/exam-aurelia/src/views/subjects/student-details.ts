
import { autoinject, bindable } from 'aurelia-framework';
import { ISubjectStudentDetailsDTO } from 'types/Subjects/ISubjectDTO';
import { StudentSubjectsApi } from 'services/StudentSubjectsApi';

@autoinject
export class StudentDetails {
    constructor(private studentSubjectsApi: StudentSubjectsApi) { }

    @bindable public model!: ISubjectStudentDetailsDTO

    onRegister() {
        if (!this.model.isAccepted)
            this.studentSubjectsApi.registerToCourse(this.model.id).then(response => {
                this.model.isEnrolled = true;
            })
    }

    onCancelRegistration() {
        if (!this.model.isAccepted)
            this.studentSubjectsApi.cancelRegistration(this.model.id).then(response => {
                this.model.isEnrolled = false;
            })
    }
}
