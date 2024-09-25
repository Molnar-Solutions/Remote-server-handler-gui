import * as jwt from 'jsonwebtoken';
import * as moment from "moment";
import {UserModel} from "../models/user.model";

export const generateToken = (user: UserModel): string => {
    try {
        const secretKey = ""+process.env.SECRET_KEY;
        const issuer = ""+process.env.ISSUER;
        const audience = ""+process.env.AUDIENCE;
        const payload: UserModel = {
            id: user['id'],
            name: user['name'],
            email: user['email'],
        }

        const expiresIn = moment().add(8, 'hours').toDate().toISOString();
        const token = jwt.sign({
            ...payload,
            expiresIn: expiresIn
        }, `${secretKey}`, {audience: audience, issuer: issuer, expiresIn: '10h'});

        return token;
    } catch (error) {
        console.log(error)
    }
}

export const isValidToken = (token: any): Boolean => {
    const secretKey = process.env.SECRET_KEY;
    const issuer = process.env.ISSUER;
    const audience = process.env.AUDIENCE;

    jwt.verify(`${token}`, `${secretKey}`, {
        issuer: `${issuer}`,
        audience: `${audience}`
    }, (err: any, decoded: any) => {
        return !err;
    })

    return false;
}

export const decodeToken = (token: any): object => {
    const secretKey = process.env.SECRET_KEY;
    const issuer = process.env.ISSUER;
    const audience = process.env.AUDIENCE;

    let decodedObject = {};

    jwt.verify(`${token}`, `${secretKey}`, {
        issuer: `${issuer}`,
        audience: `${audience}`
    }, (err: any, decoded: any) => {
        if (err) {
            throw new Error(`${err}`);
        }

        decodedObject = decoded;
    })

    return decodedObject;
}