import { autoinject, PLATFORM } from 'aurelia-framework';

@autoinject
export class StudentSubjects {

    constructor() { }

    private searchBar!: string;

    onSearch() {
        // search
        this.searchBar = ""
    }
}
