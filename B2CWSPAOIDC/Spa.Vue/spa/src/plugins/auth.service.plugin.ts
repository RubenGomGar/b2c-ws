import { UserManagerSettings } from 'oidc-client';
import AuthService from '@/services/auth.service';

export const authServicePlugin = {
  install(vue: any, options: UserManagerSettings) {
    vue.prototype.$auth = new AuthService(options);
  },
};
