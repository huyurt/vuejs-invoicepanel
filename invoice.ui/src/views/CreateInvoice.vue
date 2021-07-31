<template>
  <div>
    <h1 id="createInvoiceTitle">Fatura Oluştur</h1>
    <hr />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component({
  name: "CreateInvoice",
})
export default class CreateInvoice extends Vue {}
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
