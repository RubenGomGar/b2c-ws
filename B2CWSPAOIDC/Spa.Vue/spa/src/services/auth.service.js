import { UserManager } from 'oidc-client';
export default class AuthService {
    userManager;
    constructor(userManagerSettings) {
        this.userManager = new UserManager(userManagerSettings);
    }
    async getUser() {
        return await this.userManager.getUser();
    }
    async login() {
        return await this.userManager.signinRedirect();
    }
    async logout() {
        return await this.userManager.signoutRedirect();
    }
    async getAccessToken() {
        const data = await this.userManager.getUser();
        return data ? data.access_token : '';
    }
    async isLoggedIn() {
        const user = await this.userManager.getUser();
        if (!user || user.expired === undefined) {
            return false;
        }
        return !user.expired;
    }
}
//# sourceMappingURL=auth.service.js.map