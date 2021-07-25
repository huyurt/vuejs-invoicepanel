import axios from "axios";

export class InventoryService {
    API_URL = process.env.VUE_APP_API_URL;

    public async getInventory(): Promise<any> {
        const result: any = await axios.get(`${this.API_URL}/inventory/`);
        return result.data;
    }
}