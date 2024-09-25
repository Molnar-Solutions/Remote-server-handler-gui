import {HttpException, HttpStatus} from "@nestjs/common";

export class UserNotAuthenticatedException extends HttpException {
    constructor() {
        super('Hiba! Bejelentkezés szükséges! (Kérés megtagadva)', HttpStatus.FORBIDDEN);
    }
}