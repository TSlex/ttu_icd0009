import { autoinject, PLATFORM } from 'aurelia-framework';
import { SemestersApi } from 'services/SemestersApi';
import { IFetchResponse } from 'types/Response/IFetchResponseDTO';
import { ISemesterDTO } from 'types/Semesters/ISemesterDTO';

@autoinject
export class Index {
    constructor(private semestersApi: SemestersApi) { }

    private semesters!: ISemesterDTO[]

    created() {
        this.semestersApi.getSemesters()
            .then((response: IFetchResponse<ISemesterDTO[]>) => {

                if (response?.errors?.length === 0) {
                    this.semesters = response.data!
                }
            })
    }
}
