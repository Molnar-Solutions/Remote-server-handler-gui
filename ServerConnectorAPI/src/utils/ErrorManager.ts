import {
    BadRequestException, ConflictException,
    HttpException,
    HttpStatus,
    NotAcceptableException,
    NotFoundException, UnauthorizedException
} from "@nestjs/common";

export default function handleIncomingError(error: any, message: string = ""): void {
    if (error instanceof NotFoundException) {
        throw new HttpException(error.message, HttpStatus.NOT_FOUND);
    }
    if (error instanceof NotAcceptableException) {
        throw new HttpException(message, HttpStatus.NOT_ACCEPTABLE);
    }
    if (error instanceof ConflictException) {
        throw new HttpException(error.message, HttpStatus.CONFLICT);
    }
    if (error instanceof UnauthorizedException) {
        throw new HttpException(error.message, HttpStatus.UNAUTHORIZED);
    }

    if (error.code) {
        switch (error.code) {
            case 'P2002':
                throw new HttpException(message, HttpStatus.CONFLICT);
            case 'P2007':
                throw new HttpException(error.message, HttpStatus.BAD_REQUEST);
            case 'P2007':
                throw new HttpException(error.message, HttpStatus.BAD_REQUEST);
            case 'P2025':
                throw new HttpException(message, HttpStatus.NOT_FOUND);
            default:
                throw new HttpException(error.message, HttpStatus.INTERNAL_SERVER_ERROR);

        }
    } else {
        if (error.message) {
            throw new HttpException(error.message, HttpStatus.BAD_REQUEST);
        }
        throw new HttpException("Szerver oldali hiba! Értesítse a rendszergazdát!", HttpStatus.INTERNAL_SERVER_ERROR);
    }
}
