import Vue from 'vue';
import App from './App.vue';
import { WebStorageStateStore } from 'oidc-client';
import { authServicePlugin } from './plugins/auth.service.plugin';
Vue.config.productionTip = false;
// Configuration of the oidc-client
const userManagerSettings = {
    userStore: new WebStorageStateStore({ store: window.localStorage }),
    authority: 'https://trainingb2cvr.b2clogin.com/trainingb2cvr.onmicrosoft.com/B2C_1_SignUpSignIn/v2.0',
    client_id: 'a318deee-65c4-429d-9d71-6fcf306eeb42',
    redirect_uri: 'https://localhost:5022/callback.html',
    response_type: 'code',
    scope: 'https://trainingb2cvr.onmicrosoft.com/a318deee-65c4-429d-9d71-6fcf306eeb42/spa',
    filterProtocolClaims: true,
};
Vue.use(authServicePlugin, userManagerSettings);
new Vue({
    render: h => h(App),
}).$mount('#app');
//# sourceMappingURL=main.js.map