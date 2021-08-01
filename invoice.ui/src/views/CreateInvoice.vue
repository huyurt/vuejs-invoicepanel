<template>
  <div>
    <h1 id="createInvoiceTitle">Fatura Oluştur</h1>
    <hr />

    <div class="invoice-step" v-if="invoiceStep === 1">
      <h2>1. Adım: Müşteri Seçimi</h2>
      <div v-if="customers.length" class="invoice-step-detail">
        <label for="customer">Müşteri:</label>
        <select
          v-model="selectedCustomerId"
          class="invoiceCustomer"
          id="customer"
        >
          <option disabled value="">Lütfen bir müşteri seçiniz</option>
          <option v-for="c in customers" :value="c.id" :key="c.id">
            {{ c.firstName + " " + c.lastName }}
          </option>
        </select>
      </div>
    </div>
    <div class="invoice-step" v-else-if="invoiceStep === 2">
      <h2>2. Adım: Fatura Oluştur</h2>
      <div v-if="inventory.length" class="invoice-step-detail">
        <label for="product">Ürün:</label>
        <select v-model="newItem.product" class="invoiceLineItem" id="product">
          <option disabled value="">Lütfen bir ürün seçiniz</option>
          <option v-for="i in inventory" :value="i.product" :key="i.product.id">
            {{ i.product.name }}
          </option>
        </select>
        <label for="quantity">Miktar:</label>
        <input v-model="newItem.quantity" id="quantity" type="number" min="0" />
      </div>
      <div class="invoice-item-actions">
        <my-button
          :disabled="!newItem.product || !newItem.quantity"
          @button:click="addLineItem"
        >
          Ürünü Ekle
        </my-button>
        <my-button :disabled="!lineItems.length" @button:click="finalizeOrder">
          Siparişi Tamamla
        </my-button>

        <div class="invoice-order-list" v-if="lineItems.length">
          <div class="runningTotal">
            <h3>Running Total:</h3>
            {{ runningTotal | price }}
          </div>
          <hr />
          <table class="table">
            <thead>
              <tr>
                <td>Ürün</td>
                <td>Açıklama</td>
                <td>Miktar</td>
                <td>Fiyat</td>
                <td>Ara Toplam</td>
              </tr>
            </thead>
            <tr
              v-for="lineItem in lineItems"
              :key="`index_${lineItem.product.id}`"
            >
              <td>{{ lineItem.product.name }}</td>
              <td>{{ lineItem.product.description }}</td>
              <td>{{ lineItem.quantity }}</td>
              <td>{{ lineItem.product.price }}</td>
              <td>
                {{ (lineItem.product.price * lineItem.quantity) | price }}
              </td>
            </tr>
          </table>
        </div>
      </div>
    </div>
    <div class="invoice-step" v-else-if="invoiceStep === 3">
      <h2>3. Adım: Önizleme ve Onay</h2>
      <my-button @button:click="submitInvoice"> Faturayı Onayla </my-button>
      <hr />
      <div class="invoice-step-detail" id="invoice" ref="invoice">
        <div class="invoice-logo">
          <img id="imgLogo" alt="Logo" src="../assets/images/logo.png" />
          <h3>Fatura: {{ new Date() | humanizeDate }}</h3>
          <h3>
            Müşteri:
            {{
              this.selectedCustomer.firstName +
              " " +
              this.selectedCustomer.lastName
            }}
          </h3>
          <h3>
            Adres: {{ this.selectedCustomer.primaryAddress.addressLine1 }}
          </h3>
          <h3 v-if="this.selectedCustomer.primaryAddress.addressLine2">
            {{ this.selectedCustomer.primaryAddress.addressLine2 }}
          </h3>
          <h3>
            {{ this.selectedCustomer.primaryAddress.city }},
            {{ this.selectedCustomer.primaryAddress.postalCode }}
          </h3>
          <h3>{{ this.selectedCustomer.primaryAddress.country }}</h3>
        </div>
        <table>
          <thead>
            <tr>
              <th>Ürün</th>
              <th>Açıklama</th>
              <th>Miktar</th>
              <th>Fiyat</th>
              <th>Ara Toplam</th>
            </tr>
          </thead>
          <tr
            v-for="lineItem in lineItems"
            :key="`index_${lineItem.product.id}`"
          >
            <td>{{ lineItem.product.name }}</td>
            <td>{{ lineItem.product.description }}</td>
            <td>{{ lineItem.quantity }}</td>
            <td>{{ lineItem.product.price }}</td>
            <td>
              {{ (lineItem.product.price * lineItem.quantity) | price }}
            </td>
          </tr>
          <tr>
            <th colspan="4"></th>
            <th>Toplam Tutar</th>
          </tr>
          <tfoot>
            <tr>
              <td colspan="4" class="due">Sipariş için ödenmesi gereken:</td>
              <td class="price-final">{{ runningTotal | price }}</td>
            </tr>
          </tfoot>
        </table>
      </div>
    </div>

    <div class="invoice-steps-actions">
      <my-button @button:click="prev" :disabled="!canGoPrev">Geri</my-button>
      <my-button @button:click="next" :disabled="!canGoNext">İleri</my-button>
      <my-button @button:click="startOver">Başlangıca Dön</my-button>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IInvoice, ILineItem } from "@/types/IInvoice";
import { ICustomer } from "@/types/Customer";
import { IProductInventory } from "@/types/Product";
import { CustomerService } from "@/services/customer-service";
import { InventoryService } from "@/services/inventory-service";
import { InvoiceService } from "@/services/invoice-service";
import MyButton from "@/components/MyButton.vue";
import jsPDF from "jspdf";
import html2canvas from "html2canvas";

const customerService = new CustomerService();
const inventoryService = new InventoryService();
const invoiceService = new InvoiceService();

@Component({
  name: "CreateInvoice",
  components: { MyButton },
})
export default class CreateInvoice extends Vue {
  invoiceStep = 1;
  invoice: IInvoice = {
    createdOn: new Date(),
    updatedOn: new Date(),
    customerId: 0,
    lineItems: [],
  };
  customers: ICustomer[] = [];
  selectedCustomerId = 0;
  inventory: IProductInventory[] = [];
  lineItems: ILineItem[] = [];
  newItem: ILineItem = {
    product: undefined,
    quantity: 0,
  };

  get canGoNext() {
    if (this.invoiceStep === 1) {
      return this.selectedCustomerId !== 0;
    }

    if (this.invoiceStep === 2) {
      return this.lineItems.length;
    }

    if (this.invoiceStep === 3) {
      return false;
    }

    return false;
  }

  get canGoPrev() {
    return this.invoiceStep !== 1;
  }

  get runningTotal() {
    return this.lineItems.reduce(
      (a, b) => a + b["product"]["price"] * b["quantity"],
      0
    );
  }

  get selectedCustomer() {
    return this.customers.find((c) => c.id === this.selectedCustomerId);
  }

  async submitInvoice() {
    this.invoice = {
      customerId: this.selectedCustomerId,
      lineItems: this.lineItems,
    };

    await invoiceService.makeNewInvoice(this.invoice);
    this.downloadPdf();
    await this.$router.push("/orders");
  }

  downloadPdf() {
    let pdf = new jsPDF("p", "pt", "a4", true);
    let invoice = document.getElementById("invoice");
    let width = this.$refs.invoice.clientWidth;
    let height = this.$refs.invoice.clientHeight;

    html2canvas(invoice).then((canvas) => {
      let image = canvas.toDataURL("image/png");
      pdf.addImage(image, "PNG", 0, 0, width * 0.55, height * 0.55);
      pdf.save("invoice");
    });
  }

  addLineItem() {
    let newItem: ILineItem = {
      product: this.newItem.product,
      quantity: Number(this.newItem.quantity.toString()),
    };

    let existingItems = this.lineItems.map((item) => item.product.id);

    if (existingItems.includes(newItem.product.id)) {
      let lineItem = this.lineItems.find(
        (item) => item.product.id === newItem.product.id
      );

      let currentQuantity = Number(lineItem.quantity);
      let updatedQuantity = (currentQuantity += newItem.quantity);
      lineItem.quantity = updatedQuantity;
    } else {
      this.lineItems.push(newItem);
    }

    this.newItem = {
      product: undefined,
      quantity: 0,
    };
  }

  startOver(): void {
    this.invoice = { customerId: 0, lineItems: [] };
    this.invoiceStep = 1;
  }

  finalizeOrder() {
    this.invoiceStep = 3;
  }

  prev(): void {
    if (this.invoiceStep === 1) {
      return;
    }
    this.invoiceStep -= 1;
  }

  next(): void {
    if (this.invoiceStep === 3) {
      return;
    }
    this.invoiceStep += 1;
  }

  async initialize(): Promise<void> {
    this.customers = await customerService.getCustomers();
    this.inventory = await inventoryService.getInventory();
  }

  async created() {
    await this.initialize();
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.invoice-steps-actions {
  display: flex;
  width: 100%;
}

.invoice-step {
}

.invoice-step-detail {
  margin: 1.2rem;
}

.invoice-order-list {
  margin-top: 1.2rem;
  padding: 0.8rem;

  .line-item {
    display: flex;
    border-bottom: 1px dashed #ccc;
    padding: 0.8rem;
  }

  .item-col {
    flex-grow: 1;
  }
}

.invoice-item-actions {
  display: flex;
}

.price-pre-tax {
  font-weight: bold;
}

.price-final {
  font-weight: bold;
  color: $my-green;
}

.due {
  font-weight: bold;
}

.invoice-header {
  width: 100%;
  margin-bottom: 1.2rem;
}

.invoice-logo {
  padding-top: 1.4rem;
  text-align: center;

  img {
    width: 280px;
  }
}
</style>
