import { IsNotEmpty, IsOptional, IsString } from 'class-validator';

export class ConnectorGetFilesModel {
  @IsNotEmpty()
  @IsString()
  email: string;
}

export class ConnectorUploadFile {
  @IsNotEmpty()
  @IsString()
  email: string;
  @IsNotEmpty()
  @IsString()
  fileName: string;
}

export class ConnectorRemoveFile {
  @IsNotEmpty()
  @IsString()
  email: string;
  @IsNotEmpty()
  @IsString()
  fileName: string;
}

export class ConnectorDownloadFile {
  @IsNotEmpty()
  @IsString()
  email: string;
  @IsNotEmpty()
  @IsString()
  fileName: string;
}

export class ConnectorGetSystemHealth {
  @IsNotEmpty()
  @IsString()
  email: string;
}
