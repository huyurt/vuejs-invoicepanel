<template>
  <div class="inventory-container">
    <h1 id="inventoryTitle">
      Stok Paneli
    </h1>
    <hr/>

    <div class="inventory-actions">
      <solar-button @click.native="showNewProductModal" id="addNewBtn">
        Yeni Ekle
      </solar-button>
      <solar-button @click.native="showShipmentModal" id="receiveShipmentBtn">
        Kargoya Gönder
      </solar-button>
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
        <td>{{ item.quantityOnHand }}</td>
        <td>{{ item.product.price | price }}</td>
        <td>
          <span v-if="item.product.isTaxable">
            Evet
          </span>
          <span v-else>
            Hayır
          </span>
        </td>
        <td>
          <div>
            X
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
import {Component, Vue} from "vue-property-decorator";
import {IProductInventory} from "@/types/Product";
import SolarButton from "@/components/SolarButton.vue";
import ShipmentModal from "@/components/modals/ShipmentModal.vue";
import NewProductModal from "@/components/modals/NewProductModal.vue";

const inventoryService = new inventoryService();

@Component({
  name: 'Inventory',
  components: {SolarButton, NewProductModal, ShipmentModal},
})

export default class Inventory extends Vue {
  isNewProductVisible: boolean = false;
  isShipmentVisible: boolean = false;
  inventory: IProductInventory[] = [];

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

  saveNewProduct() {

  }

  saveNewShipment() {

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
  color: $solar-green;
}

.yellow {
  font-weight: bold;
  color: $solar-yellow;
}

.red {
  font-weight: bold;
  color: $solar-red;
}

.inventory-actions {
  display: flex;
  margin-bottom: 0.8rem;
}

.product-archive {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $solar-red;
}
</style>