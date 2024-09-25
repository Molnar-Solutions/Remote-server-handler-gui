import { CanActivate, ExecutionContext, Injectable } from '@nestjs/common';
import { Observable } from 'rxjs';

@Injectable()
export class PublicGuard implements CanActivate {
  canActivate(
    context: ExecutionContext,
  ): boolean | Promise<boolean> | Observable<boolean> {
    const request = context.switchToHttp().getRequest();

    console.log(request.headers)

    if (!request?.headers || !request.headers['x-public-key']) {
      return false;
    }

    const publicKey = request.headers['x-public-key'];
    if(!publicKey) return false;

    const originalKey = `${process.env.PUBLIC_REQUEST_KEY}`;

    return (publicKey === originalKey);
  }
}
