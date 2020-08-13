export interface StudentHomeworkDTO {
    id: string;
    title: string;
    description: string;
    deadline: Date;
    subjectTitle: string;
    subjectCode: string;
    subjectId: string;
    homeWorkId: string;
    isAccepted: boolean;
    isChecked: boolean;
    studentAnswer: string;
    answerDateTime: Date;
    grade: number;
}

export interface StudentHomeworkPostDTO {
    studentAnswer?: string;
    studentSubjectId: string;
    homeWorkId: string;
}

export interface StudentHomeworkPutDTO {
    id: string;
    studentAnswer?: string;
}

export interface StudentHomeworkTeacherSubmitDTO {
    id: string;
    isAccepted: boolean;
    isChecked: boolean;
    grade: number;
}
