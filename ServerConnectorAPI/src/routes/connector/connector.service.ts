import { Injectable } from '@nestjs/common';
import {
  ConnectorGetFilesModel,
  ConnectorUploadFile,
} from '../../models/connector.model';
import * as fs from 'fs';

export interface FileModel {
  privilege: string;
  owner: string;
  sizeInBytes: number;
  date: string;
  fileName: string;
}

@Injectable()
export class ConnectorService {
  async getFiles(body: ConnectorGetFilesModel) {
    const { userName, excludedDirectories } = body;

    const fileList = fs.readdirSync(
      'D:\\Organizations\\Molnar-Solutions\\server-handler-gui\\uploadedFiles',
      {
        encoding: 'utf8',
        withFileTypes: true,
        recursive: false,
      },
    );

    let response: FileModel[] = [];
    let excludedFiles: string[] = excludedDirectories
      ? excludedDirectories.split(';')
      : [];

    for (let file of fileList) {
      let stat = fs.statSync(file.path);

      const permissions = stat.mode.toString(8);

      response.push({
        fileName: file.name,
        date: '' + stat.birthtime.toISOString(),
        owner: '' + stat.uid,
        privilege: '' + permissions,
        sizeInBytes: Number('' + stat.size),
      });
    }

    return response;
  }

  async uploadFile(file: Express.Multer.File, body: ConnectorUploadFile) {
    return 200;
  }

  private async moveFile(sourcePath: string, destinationPath: string) {
    return new Promise<void>((resolve, reject) => {
      fs.rename(sourcePath, destinationPath, (err) => {
        if (err) {
          reject(err);
        } else {
          resolve();
        }
      });
    });
  }
}
