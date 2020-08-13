export interface StudentHomeworkDTO {
    subjectId: string;
    homeWorkId: string;
    studentName: string;
    studentCode: string;
    isAccepted: boolean;
    isChecked: boolean;
    grade: number;
}

export interface HomeworkDTO {
    id: string;
    title: string;
    description: string;
    subjectTitle: string;
    subjectCode: string;
    subjectId: string;
    deadline: Date;
    studentHomeWorks: StudentHomeworkDTO[];
}

export interface HomeworkGetDTO {
    id: string;
    title: string;
    description: string;
    subjectTitle: string;
    subjectId: string;
    deadline: Date;
}

export interface HomeworkPostDTO {
    title: string;
    subjectId: string;
    description?: string;
    deadline?: Date;
}

export interface HomeworkPutDTO {
    title: string;
    subjectId: string;
    description?: string;
    deadline?: Date;
    id: string;
}
