<template>
  <div class="hello">
    <h1>SPA - Identity Server Sample</h1>
    <a @click="getValues">Call API using user token</a>

    <ul v-if="values">
      <li v-for="(value, key) in values" :key='key'>{{ value }}</li>
    </ul>

    <a @click="logoutUser">Logout user</a>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { User } from 'oidc-client';

@Component
export default class HelloWorld extends Vue {
  private values: string[] = [];

  private async getValues() {
    try {
      const accessToken = await this.$auth.getAccessToken();

      const response = await fetch('https://localhost:5011/api/values', {
        headers: {
          'authorization': `bearer ${accessToken}`
        }
      });

      this.values = await response.json();
    } catch (error) {
      console.log(error);
    }
  }

  private async logoutUser() {
    await this.$auth.logout();
  }
}
</script>

<style scoped>
ul {
  list-style-type: none;
  padding: 0;
}
li {
  margin: 0 10px;
}
a {
  color: #42b983;
  cursor: pointer;
}
</style>
