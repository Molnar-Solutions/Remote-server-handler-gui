import { Module } from '@nestjs/common';
import { UserController } from './user.controller';
import { UserService } from './user.service';
import {AuthGuard} from "../../guards/auth/auth.guard";
import {LoggingModule} from "../../services/logging/logging.module";

@Module({
  imports: [LoggingModule],
  controllers: [UserController],
  providers: [UserService, AuthGuard]
})
export class UserModule {}
