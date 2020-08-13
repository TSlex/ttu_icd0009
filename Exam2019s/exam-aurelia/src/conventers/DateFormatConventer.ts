import * as moment from 'moment';

export class DateFormatValueConverter {
    toView(value: string | null) {
        if (!value) return value;
        return moment(value).format('MM/DD/YYYY HH:mm');
    }
}

export class TimeFormatValueConverter {
    toView(value: string | null) {
        if (!value) return value;
        return moment(value).format('HH:mm');
    }
}

export class GradeFormatValueConverter {
    toView(value: number) {
        return value > 0 ? value : "Not graded"
    }
}
