import {HttpException, HttpStatus} from "@nestjs/common";

export class UserNotExistsException extends HttpException {
    constructor() {
        super("Ez a felhasználó már tagja a közösségünknek!", HttpStatus.BAD_REQUEST);
    }
}