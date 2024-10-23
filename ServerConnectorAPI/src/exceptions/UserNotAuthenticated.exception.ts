import { HttpException, HttpStatus } from '@nestjs/common';

export class UserNotAuthenticatedException extends HttpException {
  constructor() {
    super('Whoops! This user is not authenticated yet!', HttpStatus.FORBIDDEN);
  }
}
