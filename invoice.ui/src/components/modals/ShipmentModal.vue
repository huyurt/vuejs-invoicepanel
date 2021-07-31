<template>
  <my-modal>
    <template v-slot:header> Kargo Gönder </template>
    <template v-slot:body>
      <label for="product">Ürün:</label>
      <select v-model="selectedProduct" class="shipmentItems" id="product">
        <option disabled value="">Lütfen bir ürün seçiniz</option>
        <option v-for="item in inventory" :value="item" :key="item.product.id">
          {{ item.product.name }}
        </option>
      </select>
      <label for="qtyReceived">Gönderilen Miktar:</label>
      <input type="number" id="qtyReceived" v-model="qtyReceived" />
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
import { Component, Prop, Vue } from "vue-property-decorator";
import MyButton from "@/components/MyButton.vue";
import MyModal from "@/components/modals/MyModal.vue";
import { IProduct, IProductInventory } from "@/types/Product";
import { IShipment } from "@/types/Shipment";

@Component({
  name: "ShipmentModal",
  components: { MyButton, MyModal },
})
export default class ShipmentModal extends Vue {
  @Prop({ required: true, type: Array as () => IProductInventory[] })
  inventory!: IProductInventory[];

  selectedProduct: IProduct = {
    createdOn: new Date(),
    updatedOn: new Date(),
    id: 0,
    description: "",
    isTaxable: false,
    name: "",
    price: 0,
    isArchived: false,
  };

  qtyReceived = 0;

  close() {
    this.$emit("close");
  }

  save() {
    let shipment: IShipment = {
      productId: this.selectedProduct.id,
      adjustment: this.qtyReceived,
    };

    this.$emit("save:shipment", shipment);
  }
}
</script>

<style scoped></style>
