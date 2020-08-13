export interface StudentSubjectDTO {
    id: string;
    isAccepted: boolean;
    studentCode: string;
    studentName: string;
    subjectId: string;
    grade: number;
}

export interface StudentSubjectPutDTO {
    id: string;
    isAccepted: boolean;
    grade: number;
}
