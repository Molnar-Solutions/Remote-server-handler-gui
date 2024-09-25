import {HttpException, HttpStatus} from "@nestjs/common";

export class QuizOverdueException extends HttpException {
    constructor() {
        super('Hiba! A kitöltési időlimit lejárt!', HttpStatus.FORBIDDEN);
    }
}