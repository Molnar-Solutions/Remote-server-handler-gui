import {HttpException, HttpStatus} from "@nestjs/common";

export class QuizExpiredException extends HttpException {
    constructor() {
        super('Hiba! Letelt a kvíz kitöltésére szolgáló idő!', HttpStatus.FORBIDDEN);
    }
}