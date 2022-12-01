export interface UploadModule {
    fileName: string;
    uploadingProgress: number;
    size: number,
    complete: boolean;
    state: string;
    message: string;
}
