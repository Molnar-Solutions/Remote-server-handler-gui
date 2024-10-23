import { BadRequestException, Injectable } from '@nestjs/common';
import { PrismaService } from '../../prisma/prisma.service';
import { UserLoginDto } from '../../dtos/user.dto';
import { isValidUserPassword } from '../../utils/PasswordManagerService';
import { decodeToken, generateToken } from '../../utils/TokenManagerService';
import * as moment from 'moment';
import { LoggingService } from '../../services/logging/logging.service';
import { ESeverity } from '@prisma/client';
import { UserNotAuthenticatedException } from '../../exceptions/UserNotAuthenticated.exception';

@Injectable()
export class UserService {
  constructor(
    private readonly prismaService: PrismaService,
    private readonly loggingService: LoggingService,
  ) {}

  async signIn(user: UserLoginDto) {
    const foundUser = await this.prismaService.user.findFirst({
      where: {
        email: user.email,
      },
      select: {
        id: true,
        email: true,
        name: true,
        password: true,
      },
    });
    if (!foundUser) throw new Error('This user is not exists!');

    // If it is not true then it will throw an error.
    await isValidUserPassword(user.password, foundUser.password);

    /* Basic token generation, if you prefer then use the embedded nestjs solution for this one :) */
    const token = generateToken(foundUser);

    const freshUser = await this.prismaService.user.update({
      where: {
        email: user.email,
      },
      data: {
        token: token,
      },
      select: {
        id: true,
        email: true,
        name: true,
        token: true,
      },
    });

    await this.loggingService.log({
      user: 'System',
      message: `${user.email} user logged in :)`,
      type: ESeverity.Information,
    });

    return freshUser;
  }

  async signOut(token: string) {
    const decoded: any = decodeToken(token);
    if (!decoded)
      throw new BadRequestException('Whoops! The token is expired or invalid!');

    const foundUser = await this.prismaService.user.findUnique({
      where: {
        email: decoded.email,
      },
    });

    if (!foundUser)
      throw new BadRequestException('Whoops! The token is expired or invalid!');

    await this.prismaService.user.update({
      where: {
        email: decoded.email,
      },
      data: {
        token: '',
      },
    });

    await this.loggingService.log({
      user: 'System',
      message: ` ${foundUser.email} logged out,
                time: ${moment(new Date()).format('YYYY.MM.DD HH:mm')}`,
      type: ESeverity.Information,
    });

    return { status: true };
  }

  async isLoggedIn(token: string) {
    const decoded: any = decodeToken(token);
    if (!decoded)
      throw new BadRequestException('Whoops! The token is expired or invalid!');

    const foundUser = await this.prismaService.user.findFirst({
      where: {
        email: decoded.email,
      },
      select: {
        id: true,
        email: true,
        name: true,
        token: true,
      },
    });
    if (!foundUser)
      throw new BadRequestException('Whoops! The token is expired or invalid!');

    if (!foundUser.token) throw new UserNotAuthenticatedException();

    const currentDate = new Date(); // Get the current date and time
    const tokenExpirationDate = new Date(decoded.expiresIn); // Parse the token's expiration time

    // If the token is expired
    const isLogged = moment(currentDate).isBefore(tokenExpirationDate);
    if (!isLogged)
      throw new BadRequestException(
        'Whoops! You are not logged in, maybe your token is expired :(',
      );

    await this.loggingService.log({
      user: 'System',
      message: `${foundUser.email} logged in, time: ${moment(new Date()).format('YYYY.MM.DD HH:mm')}`,
      type: ESeverity.Information,
    });

    return foundUser;
  }
}
