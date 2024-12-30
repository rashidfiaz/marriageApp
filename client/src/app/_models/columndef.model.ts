export interface ColumnDef {
    name: string;
    displayName?: string;
    width?: string;
    columnType?: ColumnType
    class?:string;
    defaultText?:string;
    sort?:boolean;
}

export enum ColumnType {
    Text = "text",
    Button = "button",
}