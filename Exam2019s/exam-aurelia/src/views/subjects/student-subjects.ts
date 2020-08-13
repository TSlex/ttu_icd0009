import { autoinject, PLATFORM } from 'aurelia-framework';
import { SubjectsApi } from 'services/SubjectsApi';
import { ISubjectDTO } from 'types/Subjects/ISubjectDTO';
import { IFetchResponse } from 'types/Response/IFetchResponseDTO';

@autoinject
export class StudentSubjects {

    constructor(private subjectsApi: SubjectsApi,) { }

    private searchBar!: string;

    private subjects!: ISubjectDTO[]

    onSearch() {
        this.subjectsApi.searchMySubjects(this.searchBar)
            .then((response: IFetchResponse<ISubjectDTO[]>) => {

                if (response?.errors?.length === 0) {
                    this.subjects = response.data!
                }

                this.searchBar = ""
            })
    }

    created() {
        this.subjectsApi.getMySubjects()
            .then((response: IFetchResponse<ISubjectDTO[]>) => {

                if (response?.errors?.length === 0) {
                    this.subjects = response.data!
                }
            })
    }
}
