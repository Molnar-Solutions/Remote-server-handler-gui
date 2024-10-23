import { HttpException, HttpStatus } from '@nestjs/common';

export class OutsideRequestException extends HttpException {
  constructor() {
    super(
      'Woooo wooo! This service is only available inside the project network!',
      HttpStatus.FORBIDDEN,
    );
  }
}
