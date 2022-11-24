import { PagedRequestInput } from './pagedRequestInput';

export interface GetListInput extends PagedRequestInput {
  name: string;
  path: string;
}
