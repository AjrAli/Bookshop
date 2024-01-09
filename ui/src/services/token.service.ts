import { Injectable } from "@angular/core";
import { jwtDecode } from "jwt-decode";
import { Role } from "../app/enum/role";
import { IdleTimeoutService } from "./idle-timeout.service";

@Injectable({
    providedIn: 'root',
})
export class TokenService {

    constructor(private idleTimeoutService: IdleTimeoutService) { }

    decodeToken(token: string): DecodedToken {
        return jwtDecode<DecodedToken>(token);
    }

    isTokenExpired(token: string): boolean {
        const decodedToken = this.decodeToken(token);
        return decodedToken.exp * 1000 < Date.now();
    }
    setToken(token: string) {
        localStorage.setItem('authToken', token);
        const expirationDate = this.getExpirationDateFromToken(token);
        const durationTime = expirationDate - Date.now();
        if (durationTime > 0) {
            this.idleTimeoutService.setIdleTimeoutDuration(expirationDate);
        } else {
            this.removeTokenStored();
        }
    }

    getToken(): string | null {
        return localStorage.getItem('authToken');
    }
    removeTokenStored() {
        localStorage.removeItem('authToken');
        this.idleTimeoutService.stopIdleTimer();
    }

    private getExpirationDateFromToken(token: string): number {
        const tokenPayload = this.decodeToken(token);
        return tokenPayload.exp * 1000; // Convert seconds to milliseconds
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