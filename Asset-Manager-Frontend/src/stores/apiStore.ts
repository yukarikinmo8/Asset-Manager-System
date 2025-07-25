import { defineStore } from "pinia";
import apiClient from "../service/apiClient";
import CookieService from "../service/cookieService";
import { jwtDecode } from "jwt-decode";

export const useAuthStore = defineStore("auth", {
    state: () => ({
        user: null as any,
        isAuthenticated: false,
    }),
    actions: {
        async login(formdata: { email: string; password: string }) {
            const response = await apiClient.post("/auth/login", formdata);

            const { token } = response.data;

            const cookieService = new CookieService();
            await cookieService.addCookie("access_token", token);

            return token;
        },

        async fetchUser(): Promise<string | null> {
            try {
                const cookieService = new CookieService();
                const token = await cookieService.getCookie("access_token");
                if (!token) {
                    this.user = null;
                    this.isAuthenticated = false;
                    return null;
                }

                const decoded: any = jwtDecode(token);

                this.user = {
                    role: decoded[
                        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                    ],
                    id: decoded["sub"],
                };

                this.isAuthenticated = true;
                return token;
            } catch (err) {
                console.error("fetchUser error:", err);
                this.user = null;
                this.isAuthenticated = false;
                return null;
            }
        },

        async logout() {
            const cookieService = new CookieService();
            await cookieService.destroyCookie("access_token");
            this.user = null;
            this.isAuthenticated = false;
        },

    },
});
