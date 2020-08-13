
import { autoinject, bindable } from 'aurelia-framework';
import { ISubjectStudentDetailsDTO } from 'types/Subjects/ISubjectDTO';

@autoinject
export class StudentDetails {
    constructor() { }

    @bindable public model!: ISubjectStudentDetailsDTO
}
