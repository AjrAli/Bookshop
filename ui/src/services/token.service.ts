import { Injectable } from "@angular/core";
import { jwtDecode } from "jwt-decode";
import { Role } from "../app/enum/role";
import { IdleTimeoutService } from "./idle-timeout.service";

@Injectable({
    providedIn: 'root',
})
export class TokenService {

    constructor(private idleTimeoutService: IdleTimeoutService) { }

    // Decode the JWT token and return the decoded token.
    decodeToken(token: string): DecodedToken {
        return jwtDecode<DecodedToken>(token);
    }

    // Check if the JWT token is expired.
    isTokenExpired(token: string): boolean {
        const decodedToken = this.decodeToken(token);
        return decodedToken.exp * 1000 < Date.now();
    }

    // Set the JWT token in the local storage and configure idle timeout.
    setToken(token: string) {
        localStorage.setItem('authToken', token);
        const expirationDate = this.getExpirationDateFromToken(token);
        const durationTime = expirationDate - Date.now();

        // Configure idle timeout if the token has a valid duration.
        if (durationTime > 0) {
            this.idleTimeoutService.setIdleTimeoutDuration(expirationDate);
        } else {
            // Remove the token if it's expired or has an invalid duration.
            this.removeTokenStored();
        }
    }

    // Get the JWT token from local storage.
    getToken(): string | null {
        return localStorage.getItem('authToken');
    }

    // Remove the JWT token from local storage and stop the idle timer.
    removeTokenStored() {
        localStorage.removeItem('authToken');
        this.idleTimeoutService.stopIdleTimer();
    }

    // Get the expiration date from the JWT token.
    private getExpirationDateFromToken(token: string): number {
        const tokenPayload = this.decodeToken(token);
        return tokenPayload.exp * 1000; // Convert seconds to milliseconds
    }
}

// Interface representing the decoded JWT token.
export interface DecodedToken {
    role: Role;
    sub: string;
    jti: string;
    unique_name: string;
    exp: number;
    iss: string;
    aud: string;
}
