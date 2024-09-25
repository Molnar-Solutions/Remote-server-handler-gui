import {IsNotEmpty, IsNumber} from "class-validator";

interface IPrivacyUpdate {
    id: number;
    privacy: string;
}

export class PrivacyUpdateDto implements IPrivacyUpdate {
    @IsNotEmpty()
    @IsNumber()
    id: number;
    @IsNotEmpty()
    privacy: string;
}