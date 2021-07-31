<template>
  <div class="inventory-container">
    <h1 id="inventoryTitle">Stok Paneli</h1>
    <hr />

    <div class="inventory-actions">
      <my-button @button:click="showNewProductModal" id="addNewBtn">
        Ürün Ekle
      </my-button>
      <my-button @button:click="showShipmentModal" id="receiveShipmentBtn">
        Stok Ekle
      </my-button>
    </div>

    <table id="inventoryTable" class="table">
      <tr>
        <th>Ürün</th>
        <th>Mevcut Miktar</th>
        <th>Birim Fiyatı</th>
        <th>Vergi İstisnası</th>
        <th>Sil</th>
      </tr>
      <tr v-for="item in inventory" :key="item.id">
        <td>{{ item.product.name }}</td>
        <td
          v-bind:class="`${applyColor(
            item.quantityOnHand,
            item.idealQuantity
          )}`"
        >
          {{ item.quantityOnHand }}
        </td>
        <td>{{ item.product.price | price }}</td>
        <td>
          <span v-if="item.product.isTaxable"> Evet </span>
          <span v-else> Hayır </span>
        </td>
        <td>
          <div @click="archiveProduct(item.product.id)">
            <i class="lni lni-cross-circle product-archive"></i>
          </div>
        </td>
      </tr>
    </table>
    <new-product-modal
      v-if="isNewProductVisible"
      @save:product="saveNewProduct"
      @close="closeModals"
    />
    <shipment-modal
      v-if="isShipmentVisible"
      :inventory="inventory"
      @save:shipment="saveNewShipment"
      @close="closeModals"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IProduct, IProductInventory } from "@/types/Product";
import MyButton from "@/components/MyButton.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import NewProductModal from "@/components/modals/NewProductModal.vue";
import { InventoryService } from "@/services/inventory-service";
import { IShipment } from "@/types/Shipment";
import { ProductService } from "@/services/product-service";

const inventoryService = new InventoryService();
const productService = new ProductService();

@Component({
  name: "Inventory",
  components: { MyButton, NewProductModal, ShipmentModal },
})
export default class Inventory extends Vue {
  isNewProductVisible = false;
  isShipmentVisible = false;
  inventory: IProductInventory[] = [];

  async archiveProduct(productId: number) {
    await productService.archive(productId);
    await this.initialize();
  }

  async saveNewProduct(newProduct: IProduct) {
    await productService.save(newProduct);
    this.isNewProductVisible = true;
    await this.initialize();
  }

  applyColor(current: number, target: number): string {
    if (current <= 0) {
      return "red";
    }

    if (Math.abs(target - current) > 8) {
      return "yellow";
    }

    return "green";
  }

  closeModals() {
    this.isNewProductVisible = false;
    this.isShipmentVisible = false;
  }

  showNewProductModal() {
    this.isNewProductVisible = true;
  }

  showShipmentModal() {
    this.isShipmentVisible = true;
  }

  async saveNewShipment(shipment: IShipment) {
    await inventoryService.updateInventoryQuantity(shipment);
    this.isShipmentVisible = true;
    await this.initialize();
  }

  async initialize() {
    this.inventory = await inventoryService.getInventory();
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

.yellow {
  font-weight: bold;
  color: $my-yellow;
}

.red {
  font-weight: bold;
  color: $my-red;
}

.inventory-actions {
  display: flex;
  margin-bottom: 0.8rem;
}

.product-archive {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $my-red;
}
</style>
