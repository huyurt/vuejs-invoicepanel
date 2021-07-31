import axios from "axios";
import { IShipment } from "@/types/Shipment";

export class InventoryService {
  API_URL = process.env.VUE_APP_API_URL;

  public async getInventory(): Promise<any> {
    const result = await axios.get(`${this.API_URL}/inventory/`);
    return result.data;
  }

  public async updateInventoryQuantity(shipment: IShipment): Promise<any> {
    const result = await axios.patch(`${this.API_URL}/inventory/`, shipment);
    return result.data;
  }
}
