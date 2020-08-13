export interface ISemesterSubjectDTO {
    id: string;
    grade: number;
    subjectTitle: string;
    subjectCode: string;
    teacherName: string;
    isAccepted: boolean;
}

export interface ISemesterDTO {
    id: string;
    title: string;
    subjects: ISemesterSubjectDTO[];
}
