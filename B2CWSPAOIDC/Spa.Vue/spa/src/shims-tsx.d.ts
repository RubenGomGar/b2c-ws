import Vue, { VNode } from 'vue'
import AuthService from './services/auth.service';

declare global {
  namespace JSX {
    // tslint:disable no-empty-interface
    interface Element extends VNode {}
    // tslint:disable no-empty-interface
    interface ElementClass extends Vue {}
    interface IntrinsicElements {
      [elem: string]: any
    }
  }
}

declare module 'vue/types/vue' {
    export interface Vue {
        $auth: AuthService;
    }
}
