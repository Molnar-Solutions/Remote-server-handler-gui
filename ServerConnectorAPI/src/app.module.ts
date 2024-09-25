import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { PrismaModule } from "./prisma/prisma.module";
import { UserModule } from "./routes/user/user.module";
import {ConfigModule} from '@nestjs/config';
import { LoggingModule } from "./services/logging/logging.module";
import { ConnectorModule } from "./routes/connector/connector.module";

@Module({
  imports: [
    PrismaModule,
    UserModule,
    LoggingModule,
    ConnectorModule,
    ConfigModule.forRoot({
      isGlobal: true,
      envFilePath: '.env',
    }),
  ],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
