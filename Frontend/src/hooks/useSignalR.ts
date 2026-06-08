import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";

export const useSignalR = (hubUrl: string) => {
    const [connection, setConnection] = useState<signalR.HubConnection | null>(
        null,
    );
    const [isConnected, setIsConnected] = useState<boolean>(false);

    useEffect(() => {
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl(hubUrl, {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets,
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        setConnection(newConnection);

        return () => {
            newConnection.stop();
        };
    }, [hubUrl]);

    useEffect(() => {
        if (!connection) return;

        connection
            .start()
            .then(() => setIsConnected(() => true))
            .catch((err) => console.error("Signal Connection Error: ", err));

        connection.onreconnecting(() => setIsConnected(() => false));
        connection.onreconnected(() => setIsConnected(() => true));
    }, [connection]);

    return { connection, isConnected };
};
