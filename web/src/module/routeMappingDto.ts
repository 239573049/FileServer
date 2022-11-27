import { FileType } from "./filesListDto";

export interface RouteMappingDto {
    id: string;
    route: string;
    path: string;
    type: FileType;
    visitor: boolean;
    createUserInfoId: string;
}