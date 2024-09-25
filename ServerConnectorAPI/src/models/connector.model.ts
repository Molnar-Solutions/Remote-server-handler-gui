import { IsNotEmpty, IsOptional, IsString } from "class-validator";

export class ConnectorGetFilesModel {
  @IsNotEmpty()
  @IsString()
  userName: string;
  @IsOptional()
  excludedDirectories?: string;
}