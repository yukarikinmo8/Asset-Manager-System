import axios from "axios";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const registerEmployee = async (employeeData: any) => {
    const response = await axios.post(
        `${API_BASE_URL}/auth/register-employee`,
        employeeData,
        {
            headers: {
                "Content-Type": "application/json",
            },
        }
    );
    return response.data;
};

export const registerManager = async (managerData: any) => {
    const response = await axios.post(
        `${API_BASE_URL}/auth/register-manager`,
        managerData,
        {
            headers: {
                "Content-Type": "application/json",
            },
        }
    );
    return response.data;
};