import { Injectable } from '@nestjs/common';
import {
  ConnectorGetFilesModel,
  ConnectorGetSystemHealth,
  ConnectorUploadFile,
} from '../../models/connector.model';
import * as fs from 'fs';
import * as os from 'os';
import { exec } from 'child_process';

export interface FileModel {
  privilege: string;
  owner: string;
  sizeInBytes: number;
  date: string;
  fileName: string;
}

export interface SystemHealthResponse {
  osType: string;
  architecture: string;
  cpuUsage: number;
  availableMemory: number;
  totalMemory: number;
  availableStorage: number;
  logs: string[];
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

  async getSystemHealth(body: ConnectorGetSystemHealth) {
    const osType = os.type();
    const architecture = os.arch();
    const cpuUsage = 12; // await this.getCpuUsage();
    const availableMemory = os.freemem();
    const totalMemory = os.totalmem();
    const availableStorage = 320; // await this.getAvailableStorage();
    const logs = []; // await this.getLogs();

    /* I stopped here */
    /* I have to do the commented function on linux and windows as well */

    let res: SystemHealthResponse = {
      osType,
      architecture,
      cpuUsage,
      availableMemory,
      totalMemory,
      availableStorage,
      logs,
    };

    return res;
  }

  async getCpuUsage(): Promise<number> {
    return new Promise((resolve, reject) => {
      exec('top -n 1', (error, stdout, stderr) => {
        if (error) {
          reject(new Error(`Error executing 'top': ${error.message}`));
        } else {
          const lines = stdout.split('\n');
          const cpuLine = lines.find((line) => line.startsWith('Cpu(s)'));
          if (cpuLine) {
            const cpuUsageStr = cpuLine.split(' ')[1];
            const cpuUsage = parseFloat(cpuUsageStr.replace(/%/g, ''));
            resolve(cpuUsage);
          } else {
            reject(new Error('Could not find CPU usage information'));
          }
        }
      });
    });
  }

  async getAvailableStorage(): Promise<number> {
    return new Promise((resolve, reject) => {
      exec('df -h', (error, stdout, stderr) => {
        if (error) {
          reject(new Error(`Error executing 'df -h': ${error.message}`));
        } else {
          const lines = stdout.split('\n');
          const headerLine = lines.find((line) =>
            line.startsWith('Filesystem'),
          );
          const availableStorageLine = lines.find((line) =>
            line.includes('/dev/sda1'),
          ); // Adjust for your storage device
          if (headerLine && availableStorageLine) {
            const availableStorageStr = availableStorageLine.split(' ')[3];
            const availableStorage = parseFloat(
              availableStorageStr.replace('G', ''),
            );
            resolve(availableStorage * 1024 * 1024 * 1024); // Convert to bytes
          } else {
            reject(new Error('Could not find available storage information'));
          }
        }
      });
    });
  }

  async getLogs(): Promise<string[]> {
    // Replace with your actual log file path
    const logFilePath = 'path/to/your/logs/application.log';

    return new Promise((resolve, reject) => {
      fs.readFile(logFilePath, 'utf8', (err, data) => {
        if (err) {
          reject(new Error(`Error reading log file: ${err.message}`));
        } else {
          const logs = data.split('\n');
          resolve(logs);
        }
      });
    });
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
