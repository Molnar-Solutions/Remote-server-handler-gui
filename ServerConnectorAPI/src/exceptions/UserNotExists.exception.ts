import { HttpException, HttpStatus } from '@nestjs/common';

export class UserNotExistsException extends HttpException {
  constructor() {
    super('This user is not exists!', HttpStatus.BAD_REQUEST);
  }
}
