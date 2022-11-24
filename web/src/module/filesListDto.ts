export interface FilesListDto {
  type: FileType;
  name: string | null;
  length: number;
  icon: string | null;
  updateTime: string | null;
  fileType: string | null;
  createdTime: string | null;
  fullName: string | null;
}

export enum FileType {
  Directory,
  File,
}
