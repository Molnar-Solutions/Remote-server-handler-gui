import { Injectable } from '@nestjs/common';
import {
  ConnectorGetFilesModel,
  ConnectorUploadFile,
} from '../../models/connector.model';
import * as fs from 'fs';

export interface FileModel {
  privilege: string;
  hardLinkCount: number;
  owner: string;
  group: string;
  sizeInBytes: number;
  date: string;
  fileName: string;
}

@Injectable()
export class ConnectorService {
  async getFiles(body: ConnectorGetFilesModel) {
    const { userName, excludedDirectories } = body;

    const fileList = fs.readdirSync(
      'D:\\Organizations\\Molnar-Solutions\\server-handler-gui',
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
        group: '',
        hardLinkCount: 0,
        owner: '' + stat.uid,
        privilege: '' + permissions,
        sizeInBytes: Number('' + stat.size),
      });
    }

    return response;
  }

  async uploadFile(file: Express.Multer.File, body: ConnectorUploadFile) {
    const destinationPath =
      'D:\\Organizations\\Molnar-Solutions\\server-handler-gui\\uploadedFiles';
    const filePath = `${destinationPath}/${file.originalname}`;

    await this.moveFile(file.path, filePath);

    /*
    const command = 'ls -l'; // Replace with your desired Linux command
    const childProcess = spawn(command, { shell: true });

    childProcess.stdout.on('data', (data) => {
      console.log(`stdout: ${data}`);
    });

    childProcess.stderr.on('data', (data) => {
      console.error(`stderr: ${data}`);   

    });
    */

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
