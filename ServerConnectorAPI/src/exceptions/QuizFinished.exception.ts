import {HttpException, HttpStatus} from "@nestjs/common";

export class QuizFinishedException extends HttpException {
    constructor() {
        super("Hiba! Ezt a kvíz már kitöltötted egyszer!", HttpStatus.BAD_REQUEST);
    }
}