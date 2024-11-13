import {Injectable} from '@nestjs/common';
import {
    ConnectorGetFilesModel,
    ConnectorGetSystemHealth,
} from '../../models/connector.model';
import * as fs from 'fs';
import * as os from 'os';
import {exec} from 'child_process';
import * as diskInfo from 'node-disk-info';
import * as osUtils from 'os-utils';
import {PrismaService} from 'src/prisma/prisma.service';

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

export interface Drive {
    device: string;
    mounted: string;
    filesystem: string;
    type: string;
    size: number;
    used: number;
    available: number;
    use: number;
}

@Injectable()
export class ConnectorService {
    constructor(private readonly prismaService: PrismaService) {
    }

    async getFiles(body: ConnectorGetFilesModel) {
        const {email} = body;

        const foundUser = await this.prismaService.user.findFirst({
            where: {
                email: email,
            },
        });
        if (!foundUser) throw new Error('Error! This user is not exists!');

        const fileList = fs.readdirSync(
            os.type().toString().toLowerCase().includes('windows')
                ? foundUser.homedirForWindows
                : foundUser.homedirForLinux, // Default for me, If you just wants to use windows leave it empty or just write a duck :) 'D:\\Organizations\\Molnar-Solutions\\server-handler-gui\\uploadedFiles'
            {
                encoding: 'utf8',
                withFileTypes: true,
                recursive: true,
            },
        );

        let response: FileModel[] = [];
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
        const {email} = body;

        const foundUser = await this.prismaService.user.findFirst({
            where: {
                email: email,
            },
        });
        if (!foundUser) throw new Error('Error! This user is not exists!');

        const osType = os.type();
        const architecture = os.arch();
        const cpuUsage = await this.getCpuUsage() * 100; // convert to percentage

        const availableMemory = os.freemem() / 1024 / 1024 / 1024;
        // KB      MB     GB
        const totalMemory = os.totalmem() / 1024 / 1024 / 1024;
        const availableStorage =
            (await this.getAvailableStorage()) / 1024 / 1024 / 1024;
        const logs = os.type().toString().toLowerCase().includes('windows')
            ? await this.getLogsForWindows()
            : await this.getLogsForLinux();

        let res: SystemHealthResponse = {
            osType,
            architecture,
            cpuUsage,
            availableMemory,
            totalMemory,
            availableStorage: availableStorage,
            logs,
        };

        return res;
    }

    async getAvailableStorage() {
        const data = diskInfo.getDiskInfoSync();
        let totalAvailableMemory = data.reduce(
            (acc, value) => acc + value.available,
            1,
        );
        return totalAvailableMemory;
    }

    async getLogsForWindows(): Promise<string[]> {
        return new Promise((resolve, reject) => {
            exec(
                'PowerShell.exe Get-EventLog -Newest 30 -LogName "Application"',
                (error, stdout, stderr) => {
                    if (error) {
                        reject(
                            new Error(`Error executing PowerShell command: ${error.message}`),
                        );
                    } else {
                        const logs = stdout.split('\n');
                        resolve(logs.slice(0, 300));
                    }
                },
            );
        });
    }

    async getCpuUsage(): Promise<number> {
        return new Promise((resolve, reject) => {
            osUtils.cpuUsage((cpuUsage) => {
                resolve(cpuUsage);
            });
        });
    }

    async getLogsForLinux(): Promise<string[]> {
        const logFilePath =
            /* YOU HAVE TO ENABLE IT WITH SETUIDS OR WITH ACL */
            '/var/log/nginx/error.log'; /* YOU HAVE TO REPLACE IT TO YOUR OWN DESIRES, but if you would like to use it for windows only then leave it as it was */

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
}
