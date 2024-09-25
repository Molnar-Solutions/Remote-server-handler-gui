import {IsBoolean, IsNotEmpty, IsNumber, IsOptional, IsString} from 'class-validator';

interface IOlympian {
  preview: string;
  name: string;
  sport: string;
  born: string;
  diedAt: string | null;
  forwardLink: string;
  isCoach: boolean;
  extract: string;
  history: string;
}

export class CreateOlympianDto implements IOlympian {
  @IsNotEmpty()
  @IsString()
  preview: string;
  @IsNotEmpty()
  @IsString()
  name: string;
  @IsNotEmpty()
  @IsString()
  sport: string;
  @IsNotEmpty()
  @IsString()
  born: string;
  @IsOptional()
  isDied: boolean;
  @IsOptional()
  diedAt: string;
  @IsOptional()
  @IsString()
  forwardLink: string;
  @IsOptional()
  @IsBoolean()
  isCoach: boolean;
  @IsOptional()
  @IsString()
  extract: string;
  @IsOptional()
  @IsString()
  history: string;
}

export class UpdateOlympianDto implements IOlympian {
  @IsNotEmpty()
  @IsNumber()
  id: number;

  @IsOptional()
  @IsString()
  preview: string;
  @IsOptional()
  @IsString()
  name: string;
  @IsOptional()
  @IsString()
  sport: string;
  @IsOptional()
  @IsString()
  born: string;
  @IsOptional()
  @IsString()
  diedAt: string;
  @IsOptional()
  @IsString()
  forwardLink: string;
  @IsOptional()
  @IsBoolean()
  isCoach: boolean;
  @IsOptional()
  @IsBoolean()
  isDied: boolean;
  @IsOptional()
  @IsString()
  extract: string;
  @IsOptional()
  @IsString()
  history: string;
}