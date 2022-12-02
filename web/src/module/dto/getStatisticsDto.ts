export interface GetStatisticsDto {
    id: string;
    code: number;
    succeed: boolean;
    responseTime: number;
    path: string;
    createdTime: string;
    userId: string | null;
    query: string;
}