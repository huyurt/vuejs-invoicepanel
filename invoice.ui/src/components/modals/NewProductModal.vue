<template>
  <solar-modal>
    <template v-slot:header>
      Yeni Ürün Ekle
    </template>
    <template v-slot:body>
      <ul class="newProduct">
        <li>
          <label for="isTaxable">Vergi istisnası var mı?</label>
          <input
              type="checkbox"
              id="isTaxable"
              v-model="newProduct.isTaxable"
          />
        </li>
        <li>
          <label for="productName">Ürün Adı</label>
          <input type="text" id="productName" v-model="newProduct.name"/>
        </li>

        <li>
          <label for="productDesc">Ürün Açıklaması</label>
          <input
              type="text"
              id="productDesc"
              v-model="newProduct.description"
          />
        </li>

        <li>
          <label for="productPrice">Birim Fiyatı (₺)</label>
          <input type="number" id="productPrice" v-model="newProduct.price"/>
        </li>
      </ul>
    </template>
    <template v-slot:footer>
      <solar-button
          type="button"
          @click.native="save"
          aria-label="save new item"
      >
        Kaydet
      </solar-button>
      <solar-button
          type="button"
          @click.native="close"
          aria-label="close modal"
      >
        Kapat
      </solar-button>
    </template>
  </solar-modal>
</template>

<script lang="ts">
import {Component, Vue} from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";
import SolarModal from "@/components/modals/SolarModal.vue";
import {IProduct} from "@/types/Product";

@Component({
  name: "NewProductModal",
  components: {SolarButton, SolarModal}
})
export default class NewProductModal extends Vue {
  newProduct: IProduct = {
    id: 0,
    createdOn: new Date(),
    updatedOn: new Date(),
    name: "",
    description: "",
    price: 0,
    isTaxable: false,
    isArchived: false
  };

  close() {
    this.$emit("close");
  }

  save() {
    this.$emit("save:product", this.newProduct);
  }
}
</script>

<style scoped lang="scss">
.newProduct {
  list-style: none;
  padding: 0;
  margin: 0;

  input {
    width: 100%;
    height: 1.8rem;
    margin-bottom: 1.2rem;
    font-size: 1.1rem;
    line-height: 1.3rem;
    padding: 0.2rem;
    color: #555;
  }

  label {
    font-weight: bold;
    display: block;
    margin-bottom: 0.3rem;
  }
}
</style>