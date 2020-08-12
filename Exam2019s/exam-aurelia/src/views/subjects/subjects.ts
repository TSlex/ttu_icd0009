import { autoinject, PLATFORM } from 'aurelia-framework';

@autoinject
export class Subjects {

    constructor() { }

    private searchBar!: string;

    onSearch() {
        // search
        this.searchBar = ""
    }
}
