import { useToast } from "primevue/usetoast";

export function useAppToast() {
    const toast = useToast();

    const success = (message: string, summary = "Success", life = 3000) => {
        toast.add({
            severity: "success",
            summary,
            detail: message,
            life,
            closable: false,
        });
    };

    const error = (message: string, summary = "Error", life = 3000) => {
        toast.add({
            severity: "error",
            summary,
            detail: message,
            life,
            closable: false,
        });
    };

    const info = (message: string, summary = "Info", life = 3000) => {
        toast.add({
            severity: "info",
            summary,
            detail: message,
            life,
            closable: false,
        });
    };

    const warn = (message: string, summary = "Warning", life = 3000) => {
        toast.add({
            severity: "warn",
            summary,
            detail: message,
            life,
            closable: false,
        });
    };

    return { success, error, info, warn };
}