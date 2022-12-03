import { FileType } from "../filesListDto";

export interface CreateRouteMappingInput {
    route: string;
    path: string;
    type: FileType;
    password: string;
}