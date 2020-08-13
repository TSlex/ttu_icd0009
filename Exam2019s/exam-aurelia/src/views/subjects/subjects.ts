import { autoinject, PLATFORM } from 'aurelia-framework';
import { SubjectsApi } from 'services/SubjectsApi';
import { IFetchResponse } from 'types/Response/IFetchResponseDTO';
import { ISubjectDTO } from 'types/Subjects/ISubjectDTO';

@autoinject
export class Subjects {
    constructor(private subjectsApi: SubjectsApi,) { }

    private searchBar!: string;

    private subjects!: ISubjectDTO[]

    onSearch() {
        this.subjectsApi.searchSubjects(this.searchBar)
            .then((response: IFetchResponse<ISubjectDTO[]>) => {

                if (response?.errors?.length === 0) {
                    this.subjects = response.data!
                }

                this.searchBar = ""
            })
    }

    created() {
        this.subjectsApi.getSubjects()
            .then((response: IFetchResponse<ISubjectDTO[]>) => {

                if (response?.errors?.length === 0) {
                    this.subjects = response.data!
                }
            })
    }
}
