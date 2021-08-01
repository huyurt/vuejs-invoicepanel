<template>
  <div class="btn-link">
    <button
      @click="onClick"
      :disabled="disabled"
      :class="['my-button', { 'full-width': isFullWidth }]"
      type="button"
    >
      <slot></slot>
    </button>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";

@Component({
  name: "MyButton",
  components: {},
})
export default class MyButton extends Vue {
  @Prop({ required: false, type: Boolean, default: false })
  isFullWidth?: boolean;
  @Prop({ required: false, type: Boolean, default: false }) disabled?: boolean;

  onClick() {
    this.$emit("button:click");
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.my-button {
  background: lighten($my-blue, 10%);
  color: white;
  padding: 0.8rem;
  transition: background-color 0.5s;
  margin: 0.3rem 0.2rem;
  display: inline-block;
  cursor: pointer;
  font-size: 1rem;
  min-width: 100px;
  border: none;
  border-bottom: 2px solid darken($my-blue, 20%);
  border-radius: 3px;

  &:hover {
    background: lighten($my-blue, 20%);
    transition: background-color 0.5s;
  }

  &:disabled {
    background: lighten($my-blue, 15%);
    border-bottom: 2px solid lighten($my-blue, 20%);
  }

  &:active {
    background: $my-yellow;
    border-bottom: 2px solid lighten($my-yellow, 20%);
  }
}

.full-width {
  display: block;
  width: 100%;
}
</style>
