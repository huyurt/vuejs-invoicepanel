<template>
  <my-modal>
    <template v-slot:header> Yeni Müşteri Ekle</template>
    <template v-slot:body>
      <ul class="newCustomer">
        <li>
          <label for="firstName"> Adı </label>
          <input type="text" id="firstName" v-model="customer.firstName" />
        </li>
        <li>
          <label for="lastName"> Soyadı </label>
          <input type="text" id="lastName" v-model="customer.lastName" />
        </li>
        <li>
          <label for="address1"> Adres Satır 1 </label>
          <input
            type="text"
            id="address1"
            v-model="customer.primaryAddress.addressLine1"
          />
        </li>
        <li>
          <label for="address2"> Adres Satır 2 </label>
          <input
            type="text"
            id="address2"
            v-model="customer.primaryAddress.addressLine2"
          />
        </li>
        <li>
          <label for="city"> Şehir </label>
          <input type="text" id="city" v-model="customer.primaryAddress.city" />
        </li>
        <li>
          <label for="postalCode"> Posta Kodu </label>
          <input
            type="text"
            id="postalCode"
            v-model="customer.primaryAddress.postalCode"
          />
        </li>
        <li>
          <label for="state"> Ülke </label>
          <input
            type="text"
            id="state"
            v-model="customer.primaryAddress.country"
          />
        </li>
      </ul>
    </template>
    <template v-slot:footer>
      <my-button type="button" @button:click="save" aria-label="Kaydet">
        Kaydet
      </my-button>
      <my-button type="button" @button:click="close" aria-label="Kapat">
        Kapat
      </my-button>
    </template>
  </my-modal>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import MyModal from "@/components/modals/MyModal.vue";
import MyButton from "@/components/MyButton.vue";
import { ICustomer } from "@/types/Customer";

@Component({
  name: "NewCustomerModal",
  components: { MyButton, MyModal },
})
export default class NewProductModal extends Vue {
  customer: ICustomer = {
    primaryAddress: {},
    createdOn: new Date(),
    updatedOn: new Date(),
    firstName: "",
    lastName: "",
  };

  save() {
    this.$emit("save:customer", this.customer);
  }

  close() {
    this.$emit("close");
  }
}
</script>

<style scoped lang="scss">
.newCustomer {
  display: flex;
  flex-wrap: wrap;
  list-style: none;
  padding: 0;
  margin: 0;

  input {
    width: 80%;
    height: 1.8rem;
    margin: 0.8rem 2rem 0.8rem 0.8rem;
    font-size: 1.1rem;
    line-height: 1.3rem;
    padding: 0.2rem;
    color: #555;
  }

  label {
    font-weight: bold;
    margin: 0.8rem;
    display: block;
  }
}
</style>
