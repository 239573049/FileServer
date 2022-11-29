export interface PieInput {
    type: PieType;
}

export enum PieType {
    Today,
    Yesterday,
    Month,
    Total
}