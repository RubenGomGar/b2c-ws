import { UserManager, User, UserManagerSettings } from 'oidc-client';

export default class AuthService {
  private userManager: UserManager;

  constructor(userManagerSettings: UserManagerSettings) {
    this.userManager = new UserManager(userManagerSettings);
  }

  public async getUser(): Promise<User | null> {
    return await this.userManager.getUser();
  }

  public async login(): Promise<void> {
    return await this.userManager.signinRedirect();
  }

  public async logout(): Promise<void> {
    return await this.userManager.signoutRedirect();
  }

  public async getAccessToken(): Promise<string> {
    const data = await this.userManager.getUser();
    return data ? data.access_token : '';
  }

  public async isLoggedIn(): Promise<boolean> {
    const user = await this.userManager.getUser();
    if (!user || user.expired === undefined) {
      return false;
    }

    return !user.expired;
  }
}
