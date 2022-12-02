import { PagedRequestInput } from "./pagedRequestInput";

export interface GetStatisticsInput extends PagedRequestInput {
    keywords: string;
}