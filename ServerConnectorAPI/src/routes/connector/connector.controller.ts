import { Body, Controller, Get, Req, Res } from "@nestjs/common";
import { ConnectorService } from "./connector.service";
import handleIncomingError from "../../utils/ErrorManager";
import { ConnectorGetFilesModel } from "../../models/connector.model";
import { Response } from "express";
import { RequestInformation } from "../../models/req-error.model";

@Controller("connector")
export class ConnectorController {
  constructor(private readonly connectorService: ConnectorService) {
  }

  @Get("/list-files")
  async getFiles(@Res({ passthrough: true }) res: Response, @Body() body: ConnectorGetFilesModel) {
    const responseInformation: RequestInformation = {
      message: "",
      statusCode: 200,
      isFailed: false
    };

    try {
      res["responseInformation"] = responseInformation;

      console.log("request");

      return await this.connectorService.getFiles(body);

    } catch (error) {
      responseInformation.isFailed = true;
      responseInformation.message = error.message;
      responseInformation.statusCode = 400;

      res["responseInformation"] = responseInformation;
    }
  }
}
