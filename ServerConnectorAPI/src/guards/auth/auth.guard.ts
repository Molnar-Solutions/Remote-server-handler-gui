import {CanActivate, ExecutionContext, Injectable} from '@nestjs/common';
import {decodeToken} from "../../utils/TokenManagerService";

@Injectable()
export class AuthGuard implements CanActivate {
    async canActivate(context: ExecutionContext): Promise<boolean> {
        const request = context.switchToHttp().getRequest();

        if (!request?.headers || !request.headers.authorization) {
            return false;
        }

        const token = request.headers.authorization.split(' ')[1] || request.body.token;

        if (!token) {
            return false;
        }

        try {
            const decoded: any = decodeToken(token);
            if (!decoded) return false;

            return true;
        } catch (error) {
            return false;
        }
    }
}
