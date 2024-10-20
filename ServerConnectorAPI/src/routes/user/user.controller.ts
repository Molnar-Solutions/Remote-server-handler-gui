import {
  BadRequestException,
  Body,
  Controller,
  Post,
  UseGuards,
  ValidationPipe,
} from '@nestjs/common';
import { UserLoginDto } from '../../dtos/user.dto';
import { UserService } from './user.service';
import { AuthGuard } from '../../guards/auth/auth.guard';

@Controller('user')
export class UserController {
  constructor(private readonly userService: UserService) {}

  @Post('sign-in')
  async signIn(@Body(ValidationPipe) user: UserLoginDto) {
    let response = {
      StatusCode: 200,
      Message: '',
      Data: null,
      Date: new Date(),
    };

    try {
      response.Data = {
        ...(await this.userService.signIn(user)),
      };

      return response;
    } catch (error) {
      response.StatusCode = 400;
      response.Message = error.message;
    }
  }

  @UseGuards(AuthGuard)
  @Post('sign-out')
  async signOut(@Body('token') token: string) {
    let response = {
      StatusCode: 200,
      Message: '',
      Data: null,
      Date: new Date(),
    };

    try {
      let res = await this.userService.signOut(token);

      response.Data = {
        ...res,
      };

      return response;
    } catch (error) {
      response.StatusCode = 400;
      response.Message = error.message;
    }
  }

  @UseGuards(AuthGuard)
  @Post('loggedin')
  async isLoggedIn(@Body('token') token: string) {
    let response = {
      StatusCode: 200,
      Message: '',
      Data: null,
      Date: new Date(),
    };

    try {
      if (!token)
        throw new BadRequestException('Hiba! Token megadása szükséges!');

      let res = await this.userService.isLoggedIn(token);

      response.Data = {
        ...res,
      };

      return response;
    } catch (error) {
      response.StatusCode = 400;
      response.Message = error.message;
    }
  }
}
