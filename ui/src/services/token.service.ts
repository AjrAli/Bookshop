import { Injectable } from "@angular/core";
import { jwtDecode } from "jwt-decode";
import { Role } from "../app/enum/role";

@Injectable({
    providedIn: 'root',
})
export class TokenService {
    decodeToken(token: string): DecodedToken {
        return jwtDecode<DecodedToken>(token);
    }

    isTokenExpired(token: string): boolean {
        const decodedToken = this.decodeToken(token);
        return decodedToken.exp * 1000 < Date.now();
    }
    setToken(token: string) {
        localStorage.setItem('authToken', token);
    }

    getToken(): string | null {
        return localStorage.getItem('authToken');
    }
    removeTokenStored() {
        localStorage.removeItem('authToken');
    }
}

export interface DecodedToken {
    role: Role;
    sub: string;
    jti: string;
    unique_name: string;
    exp: number;
    iss: string;
    aud: string;
}