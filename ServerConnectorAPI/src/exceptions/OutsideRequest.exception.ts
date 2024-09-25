import {HttpException, HttpStatus} from "@nestjs/common";

export class OutsideRequestException extends HttpException {
    constructor() {
        super('Hiba! Csak belső hálózatból érhető el!', HttpStatus.FORBIDDEN);
    }
}