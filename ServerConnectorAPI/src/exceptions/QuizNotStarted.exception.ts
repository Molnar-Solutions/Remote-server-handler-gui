import {HttpException, HttpStatus} from "@nestjs/common";

export class QuizNotStartedException extends HttpException {
    constructor() {
        super("Hiba! Ezt a kvízt nem kezted meg!", HttpStatus.BAD_REQUEST);
    }
}