import { autoinject, bindable } from 'aurelia-framework';
import { ISubjectTeacherDetailsDTO } from 'types/Subjects/ISubjectDTO';

@autoinject
export class TeacherDetails {
    constructor() { }

    @bindable public model!: ISubjectTeacherDetailsDTO
}
