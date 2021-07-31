<template>
  <div class="side-menu-container">
    <router-link to="/">
      <img id="imgLogo" alt="Logo" src="../assets/images/logo.png" />
    </router-link>
    <h1>Fatura Portalı</h1>
    <my-button
      id="menuInventory"
      :is-full-width="true"
      @button:click="goToRoute('/inventory')"
    >
      Stok
    </my-button>
    <my-button
      id="menuCustomers"
      :is-full-width="true"
      @button:click="goToRoute('/customers')"
    >
      Müşteriler
    </my-button>
    <my-button
      id="menuInvoice"
      :is-full-width="true"
      @button:click="goToRoute('/invoice/new')"
    >
      Yeni Fatura
    </my-button>
    <my-button
      id="menuOrders"
      :is-full-width="true"
      @button:click="goToRoute('/orders')"
    >
      Siparişler
    </my-button>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import MyButton from "@/components/MyButton.vue";

@Component({
  name: "SideMenu",
  components: {
    MyButton,
  },
})
export default class SideMenu extends Vue {
  goToRoute(route: string) {
    this.$router.push(route);
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.side-menu-container {
  background-color: #fcfcfc;
  height: 100vh;
  width: $menu-width;
  display: flex;
  flex-direction: column;
  padding: 0.8rem;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.12), 0 1px 1px rgba(0, 0, 0, 0.24);
  box-sizing: border-box;
}

#imgLogo {
  width: 100%;
}

h1 {
  font-size: 1.2rem;
  margin: 1rem 0;
}
</style>
