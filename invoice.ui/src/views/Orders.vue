<template>
  <div>
    <h1 id="ordersTitle">Sipariş Yönetimi</h1>
    <hr />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component({
  name: "Orders",
})
export default class Orders extends Vue {}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.green {
  font-weight: bold;
  color: $my-green;
}

.order-complete {
  cursor: pointer;
  text-align: center;
}
</style>
