<template>
  <div>
    <h1 id="customersTitle">Müşteri Yönetimi</h1>
    <hr />
    <div class="customer-actions">
      <my-button @button:click="showNewCustomerModal"> Müşteri Ekle</my-button>
    </div>
    <table id="customers" class="table">
      <tr>
        <th>Müşteri</th>
        <th>Adres</th>
        <th>Ülke</th>
        <th>Oluşturulma Tarihi</th>
        <th>Sil</th>
      </tr>
      <tr v-for="customer in customers">
        <td>{{ customer.firstName + " " + customer.lastName }}</td>
        <td>
          {{
            customer.primaryAddress.addressLine1 +
            " " +
            customer.primaryAddress.addressLine2
          }}
        </td>
        <td>{{ customer.primaryAddress.country }}</td>
        <td>{{ customer.createdOn | humanizeDate }}</td>
        <td>
          <div @click="deleteCustomer(customer.id)">
            <i class="lni lni-cross-circle customer-delete"></i>
          </div>
        </td>
      </tr>
    </table>
    <new-customer-modal
      v-if="isCustomerModalVisible"
      @save:customer="saveNewCustomer"
      @close="close"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import MyButton from "@/components/MyButton.vue";
import { ICustomer } from "@/types/Customer";
import { CustomerService } from "@/services/customer-service";
import NewCustomerModal from "@/components/modals/NewCustomerModal.vue";

const customerService = new CustomerService();

@Component({
  name: "Customers",
  components: { NewCustomerModal, MyButton },
})
export default class Customers extends Vue {
  customers: ICustomer[] = [];
  isCustomerModalVisible = false;

  showNewCustomerModal() {
    this.isCustomerModalVisible = true;
  }

  async deleteCustomer(customerId: number) {
    await customerService.deleteCustomer(customerId);
    await this.initialize();
  }

  async initialize() {
    this.customers = await customerService.getCustomers();
  }

  async created() {
    await this.initialize();
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.customer-actions {
  display: flex;
  margin-bottom: 0.8rem;

  div {
    margin-right: 0.8rem;
  }
}

.customer-delete {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $my-red;
}
</style>
