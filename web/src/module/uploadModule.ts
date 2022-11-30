export interface UploadModule {
    fileName: string;
    uploadingProgress: number;
    complete: boolean;
    state: UploadState;
    message: string;
}

export enum UploadState {
    BeingProcessed,
    Complete,
    BeDefeated
}