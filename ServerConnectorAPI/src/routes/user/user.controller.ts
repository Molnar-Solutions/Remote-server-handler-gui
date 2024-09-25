import {
    BadRequestException,
    Body,
    Controller,
    Post,
    UseGuards,
    ValidationPipe
} from '@nestjs/common';
import {UserLoginDto} from "../../dtos/user.dto";
import {UserService} from "./user.service";
import {PublicGuard} from "../../guards/public/public.guard";
import handleIncomingError from "../../utils/ErrorManager";
import {AuthGuard} from "../../guards/auth/auth.guard";

@Controller('user')
export class UserController {
    constructor(private readonly userService: UserService) {}

    @UseGuards(PublicGuard)
    @Post("sign-in")
    signIn(@Body(ValidationPipe) user: UserLoginDto) {
        try {
            return this.userService.signIn(user);
        } catch (error) {
            handleIncomingError(error)
        }
    }

    @UseGuards(AuthGuard)
    @Post("sign-out")
    signOut(@Body("token") token: string) {
        try {
            return this.userService.signOut(token);
        } catch (error) {
            handleIncomingError(error)
        }
    }

    @UseGuards(AuthGuard)
    @Post("loggedin")
    isLoggedIn(@Body("token") token: string) {
        try {
            if (!token) throw new BadRequestException("Hiba! Token megadása szükséges!");
            return this.userService.isLoggedIn(token);
        } catch (error) {
            handleIncomingError(error)
        }
    }
}
