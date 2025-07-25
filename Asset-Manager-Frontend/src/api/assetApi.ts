import axios from "axios";
import CookieService from "../service/cookieService";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
const cookieService = new CookieService();

export const getAllAssets = async (status?: string) => {
    const token = await cookieService.getCookie("access_token");
    const url = `${API_BASE_URL}/assets/get-assets${status ? `?status=${status}` : ""}`;
    const response = await axios.get(url, {
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`,
        },
    });
    return response.data;
};