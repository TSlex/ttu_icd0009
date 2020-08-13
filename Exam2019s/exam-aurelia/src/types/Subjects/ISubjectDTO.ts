export interface ISubjectDTO {
    id: string;
    subjectTitle: string;
    subjectCode: string;
    semesterTitle: string;
    teacherName: string;
}

export interface ISubjectStudentDetailsHomeworkDTO {
    id: string;
    studentHomeworkId: string;
    title: string;
    deadline?: Date;
    isAccepted: boolean;
    isChecked: boolean;
    isStarted: boolean;
    grade: number;
}

export interface ISubjectStudentDetailsDTO {
    studentsCount: number;
    grade: number;
    homeWorksGrade: number;
    isAccepted: boolean;
    isEnrolled: boolean;
    homeworks: ISubjectStudentDetailsHomeworkDTO[];
    id: string;
    subjectTitle: string;
    subjectCode: string;
    studentSubjectId: string;
    semesterTitle: string;
    teacherName: string;
}

export interface ISubjectDetailsDTO {
    studentsCount: number;
    id: string;
    subjectTitle: string;
    subjectCode: string;
    semesterTitle: string;
    teacherName: string;
}

export interface ISubjectTeacherDetailsHomeworkDTO {
    id: string;
    title: string;
    deadline?: Date;
    averageGrade: number;
}

export interface ISubjectTeacherDetailsDTO {
    acceptedStudentsCount: number;
    notAcceptedStudentsCount: number;
    notGradedCount: number;
    failedCount: number;
    passedCount: number;
    score1Count: number;
    score2Count: number;
    score3Count: number;
    score4Count: number;
    score5Count: number;
    homeworks: ISubjectTeacherDetailsHomeworkDTO[];
    id: string;
    subjectTitle: string;
    subjectCode: string;
    semesterTitle: string;
    teacherName: string;
}
