import {
  Body,
  Controller,
  Get,
  Post,
  Req,
  Res,
  UploadedFile,
  UseGuards,
  UseInterceptors,
} from '@nestjs/common';
import { ConnectorService } from './connector.service';
import {
  ConnectorGetFilesModel,
  ConnectorUploadFile,
} from '../../models/connector.model';
import { Response } from 'express';
import { AuthGuard } from 'src/guards/auth/auth.guard';
import { FileInterceptor } from '@nestjs/platform-express';
import { diskStorage } from 'multer';
import { extname } from 'path';

export const storage = diskStorage({
  destination:
    'D:\\Organizations\\Molnar-Solutions\\server-handler-gui\\uploadedFiles',
  filename: (req, file, callback) => {
    callback(null, generateFilename(file));
  },
});

export const multerOptions = {
  storage: diskStorage({
    destination: (req, file, cb) => {
      const destination =
        'D:\\Organizations\\Molnar-Solutions\\server-handler-gui\\uploadedFiles';
      cb(null, destination);
    },
    filename: (req, file, cb) => {
      const uniqueSuffix = Date.now() + '-' + Math.round(Math.random() * 1e9);
      const filename = `${file.fieldname}-${uniqueSuffix}.${file.originalname.split('.').pop()}`;
      cb(null, filename);
    },
  }),
};

function generateFilename(file) {
  return `${Date.now()}.${extname(file.originalname)}`;
}

@Controller('connector')
export class ConnectorController {
  constructor(private readonly connectorService: ConnectorService) {}

  @Post('/list-files')
  async getFiles(
    @Res({ passthrough: true }) res: Response,
    @Body() body: ConnectorGetFilesModel,
  ) {
    let response = {
      StatusCode: 200,
      Message: '',
      Data: null,
      Date: new Date(),
    };

    try {
      response.Data = await this.connectorService.getFiles(body);

      return response;
    } catch (error) {
      response.StatusCode = 400;
      response.Message = error.message;

      return response;
    }
  }

  @Post('upload-file')
  @UseInterceptors(FileInterceptor('fileContent', multerOptions))
  async UploadFile(
    @UploadedFile() fileContent: Express.Multer.File,
    @Body() body: ConnectorUploadFile,
  ) {
    console.log(fileContent);

    let response = {
      StatusCode: 200,
      Message: '',
      Data: null,
      Date: new Date(),
    };

    try {
      // await this.connectorService.uploadFile(fileContent, body);
      return response;
    } catch (error) {
      response.StatusCode = 400;
      response.Message = error.message;

      return response;
    }
  }
}
