import {
  Body,
  Controller,
  Get,
  Param,
  Post,
  Req,
  Res,
  UploadedFile,
  UseGuards,
  Response,
  UseInterceptors,
  Query,
} from '@nestjs/common';
import { ConnectorService } from './connector.service';
import {
  ConnectorGetFilesModel,
  ConnectorRemoveFile,
  ConnectorUploadFile,
} from '../../models/connector.model';
import { FileInterceptor } from '@nestjs/platform-express';
import { diskStorage } from 'multer';
import * as fs from 'fs';

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

  @Post('remove-file')
  async RemoveFile(@Body() body: ConnectorRemoveFile) {
    let response = {
      StatusCode: 200,
      Message: '',
      Data: null,
      Date: new Date(),
    };

    try {
      await fs.promises.unlink(
        `D:\\Organizations\\Molnar-Solutions\\server-handler-gui\\uploadedFiles\\${body.fileName}`,
      );

      return response;
    } catch (error) {
      response.StatusCode = 400;
      response.Message = error.message;

      return response;
    }
  }

  @Get('download-file')
  async DownloadFile(@Query('fileName') fileName: string) {
    let response = {
      StatusCode: 200,
      Message: '',
      Data: null,
      Date: new Date(),
    };

    try {
      const filePath = `D:\\Organizations\\Molnar-Solutions\\server-handler-gui\\uploadedFiles\\${fileName}`;
      const fileData = fs.readFileSync(filePath).toJSON();

      response.Data = fileData;

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
    let response = {
      StatusCode: 200,
      Message: '',
      Data: null,
      Date: new Date(),
    };

    try {
      return response;
    } catch (error) {
      response.StatusCode = 400;
      response.Message = error.message;

      return response;
    }
  }
}
