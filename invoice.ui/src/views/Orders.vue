<template>
  <div>
    <h1 id="ordersTitle">Sipariş Yönetimi</h1>
    <hr />
    <table id="sales-orders" class="table" v-if="orders.length">
      <thead>
        <tr>
          <th>Müşteri No</th>
          <th>Sipariş No</th>
          <th>Sipariş Toplam</th>
          <th>Sipariş Durumu</th>
          <th>Tamamlandı Yap</th>
        </tr>
        <tr v-for="order in orders" :key="order.id">
          <td>
            {{ order.customer.id }}
          </td>
          <td>
            {{ order.id }}
          </td>
          <td>
            {{ getTotal(order) | price }}
          </td>
          <td :class="{ green: order.isPaid }">
            {{ getStatus(order.isPaid) }}
          </td>
          <td>
            <div v-if="!order.isPaid" @click="markComplete(order.id)">
              <i class="lni lni-checkmark-circle order-complete"></i>
            </div>
          </td>
        </tr>
      </thead>
    </table>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { OrderService } from "@/services/order-service";
import { ISalesOrder } from "@/types/SalesOrder";

const orderService = new OrderService();

@Component({
  name: "Orders",
})
export default class Orders extends Vue {
  orders: ISalesOrder[] = [];

  getTotal(order: ISalesOrder) {
    return order.salesOrderItems.reduce(
      (a, b) => a + b["product"]["price"] * b["quantity"],
      0
    );
  }

  getStatus(isPaid: boolean) {
    return isPaid ? "Ödendi" : "Ödenmedi";
  }

  async markComplete(orderId: number) {
    await orderService.markOrderComplete(orderId);
    await this.initialize();
  }

  async initialize() {
    this.orders = await orderService.getOrders();
  }

  async created() {
    await this.initialize();
  }
}
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
