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
  ConnectorGetSystemHealth,
  ConnectorRemoveFile,
  ConnectorUploadFile,
} from '../../models/connector.model';
import { FileInterceptor } from '@nestjs/platform-express';
import { diskStorage } from 'multer';
import * as fs from 'fs';
import * as os from 'os';
import { PrismaService } from 'src/prisma/prisma.service';

export const multerOptions = {
  storage: diskStorage({
    destination: (req, file, cb) => {
      const destination =
        'D:\\Organizations\\Molnar-Solutions\\server-handler-gui\\uploadedFiles'; /* BASE OPTION, I REWRITE IT IN EVERY SINGLE ROUTE */
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
  constructor(
    private readonly connectorService: ConnectorService,
    private readonly prismaService: PrismaService,
  ) {}

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
      const { email } = body;

      const foundUser = await this.prismaService.user.findFirst({
        where: {
          email: email,
        },
      });
      if (!foundUser) throw new Error('Error! This user is not exists!');

      let separator = os.type().toString().toLowerCase().includes('windows')
        ? '\\'
        : '/';

      await fs.promises.unlink(
        `${os.type().toString().toLowerCase().includes('windows') ? foundUser.homedirForWindows : foundUser.homedirForLinux}${separator}${body.fileName}`, // Use this code or paste yours, in my case it was: `D:\\Organizations\\Molnar-Solutions\\server-handler-gui\\uploadedFiles\\${body.fileName}`,
      );

      return response;
    } catch (error) {
      response.StatusCode = 400;
      response.Message = error.message;

      return response;
    }
  }

  @Get('download-file')
  async DownloadFile(
    @Query('fileName') fileName: string,
    @Query('email') email: string,
  ) {
    let response = {
      StatusCode: 200,
      Message: '',
      Data: null,
      Date: new Date(),
    };

    try {
      const foundUser = await this.prismaService.user.findFirst({
        where: {
          email: email,
        },
      });
      if (!foundUser) throw new Error('Error! This user is not exists!');

      let separator = os.type().toString().toLowerCase().includes('windows')
        ? '\\'
        : '/';
      const filePath = `${os.type().toString().toLowerCase().includes('windows') ? foundUser.homedirForWindows : foundUser.homedirForLinux}${separator}${fileName}`;
      const fileData = fs.readFileSync(filePath).toJSON();

      response.Data = fileData;

      return response;
    } catch (error) {
      response.StatusCode = 400;
      response.Message = error.message;

      return response;
    }
  }

  @Post('system-health')
  async SystemHealth(@Body() body: ConnectorGetSystemHealth) {
    let response = {
      StatusCode: 200,
      Message: '',
      Data: null,
      Date: new Date(),
    };

    try {
      response.Data = await this.connectorService.getSystemHealth(body);

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
      const { email } = body;

      const foundUser = await this.prismaService.user.findFirst({
        where: {
          email: email,
        },
      });
      if (!foundUser) throw new Error('Error! This user is not exists!');

      const folderPath = `${os.type().toString().toLowerCase().includes('windows') ? foundUser.homedirForWindows : foundUser.homedirForLinux}`;

      multerOptions.storage = diskStorage({
        destination: (req, file, cb) => {
          const destination = folderPath;
          cb(null, destination);
        },
        filename: (req, file, cb) => {
          const uniqueSuffix =
            Date.now() + '-' + Math.round(Math.random() * 1e9);
          const filename = `${file.fieldname}-${uniqueSuffix}.${file.originalname.split('.').pop()}`;
          cb(null, filename);
        },
      });

      return response;
    } catch (error) {
      response.StatusCode = 400;
      response.Message = error.message;

      return response;
    }
  }
}
