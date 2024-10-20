import {
  BadRequestException,
  Injectable,
  NotFoundException,
} from '@nestjs/common';
import { PrismaService } from '../../prisma/prisma.service';
import { UserLoginDto } from '../../dtos/user.dto';
import { isValidUserPassword } from '../../utils/PasswordManagerService';
import { decodeToken, generateToken } from '../../utils/TokenManagerService';
import * as moment from 'moment';
import handleIncomingError from '../../utils/ErrorManager';
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
    if (!foundUser)
      throw new Error('Nincs ilyen felhasználó vagy nincs aktiválva!');

    // If it is not true then it will throw an error.
    await isValidUserPassword(user.password, foundUser.password);

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
      message: `Új felhasználó lépett be: ${user.email}`,
      type: ESeverity.Information,
    });

    return freshUser;
  }

  async signOut(token: string) {
    const decoded: any = decodeToken(token);
    if (!decoded) throw new BadRequestException('Hiba! A token nem érvényes!');

    const foundUser = await this.prismaService.user.findUnique({
      where: {
        email: decoded.email,
      },
    });

    if (!foundUser)
      throw new BadRequestException('Hiba! A token nem érvényes!');

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
      message: `Egy felhasználó kilépett: ${foundUser.email},
                időpont: ${moment(new Date()).format('YYYY.MM.DD HH:mm')}`,
      type: ESeverity.Information,
    });

    return { status: true };
  }

  async isLoggedIn(token: string) {
    const decoded: any = decodeToken(token);
    if (!decoded) throw new BadRequestException('Hiba! A token nem érvényes!');

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
      throw new BadRequestException('Hiba! A token nem érvényes!');

    if (!foundUser.token) throw new UserNotAuthenticatedException();

    const currentDate = new Date(); // Get the current date and time
    const tokenExpirationDate = new Date(decoded.expiresIn); // Parse the token's expiration time

    // If the token is expired
    const isLogged = moment(currentDate).isBefore(tokenExpirationDate);
    if (!isLogged)
      throw new BadRequestException('Hiba! Nem vagy bejelentkezve!');

    await this.loggingService.log({
      user: 'System',
      message: `${foundUser.email} nevű felhasználó megnézte, hogy mikor lépett be! Időpont: ${moment(new Date()).format('YYYY.MM.DD HH:mm')}`,
      type: ESeverity.Information,
    });

    return foundUser;
  }
}
