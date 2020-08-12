import { AlertType } from "types/Enums/AlertType";

export interface IAlertData {
    message: string;
    dismissable?: boolean;
    type: AlertType
}
