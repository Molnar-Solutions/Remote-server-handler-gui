import { Injectable } from '@nestjs/common';
import { PrismaService } from '../../prisma/prisma.service';
import { ESeverity } from '@prisma/client';

@Injectable()
export class LoggingService {
  constructor(private readonly prismaService: PrismaService) {}

  async log({
    type,
    message,
    user,
  }: {
    type: ESeverity;
    message: string;
    user?: string;
  }) {
    try {
      await this.prismaService.log.create({
        data: {
          user: user ?? 'System',
          message: message,
          date: new Date(),
          severity: type,
        },
      });
    } catch (error) {
      console.error(error);
    }
  }
}
