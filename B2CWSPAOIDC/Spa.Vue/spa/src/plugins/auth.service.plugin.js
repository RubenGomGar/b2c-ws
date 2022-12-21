import AuthService from '@/services/auth.service';
export const authServicePlugin = {
    install(vue, options) {
        vue.prototype.$auth = new AuthService(options);
    },
};
//# sourceMappingURL=auth.service.plugin.js.map