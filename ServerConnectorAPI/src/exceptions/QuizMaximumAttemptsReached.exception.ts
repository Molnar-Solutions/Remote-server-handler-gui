import {HttpException, HttpStatus} from "@nestjs/common";

export class QuizMaximumAttemptsReached extends HttpException {
    constructor() {
        super('Hiba! Elérted a maximális kvíz kitöltési limitet!', HttpStatus.FORBIDDEN);
    }
}