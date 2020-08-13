import { autoinject, bindable } from 'aurelia-framework';
import { ISubjectDetailsDTO } from 'types/Subjects/ISubjectDTO';

@autoinject
export class SubjectDetails {
    constructor() { }

    @bindable public model!: ISubjectDetailsDTO
}
