import { Injectable } from "@nestjs/common";
import { ConnectorGetFilesModel } from "../../models/connector.model";
import * as fs from "fs";

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
    const {
      userName,
      excludedDirectories
    } = body;

    const fileList = fs.readdirSync("D:\\Organizations\\Molnar-Solutions\\server-handler-gui", {
      encoding: "utf8",
      withFileTypes: true,
      recursive: true
    });

    let response: FileModel[] = [];
    let excludedFiles: string[] = excludedDirectories ? excludedDirectories.split(";") : [];

    for (let file of fileList) {
      let stat = fs.statSync(file.path);

      const permissions = stat.mode.toString(8);

      response.push({
        fileName: file.name,
        date: "" + stat.birthtime.toISOString(),
        group: "",
        hardLinkCount: 0,
        owner: "",
        privilege: ""+permissions,
        sizeInBytes: Number("" + stat.size)
      });
    }

    return response;
  }
}
