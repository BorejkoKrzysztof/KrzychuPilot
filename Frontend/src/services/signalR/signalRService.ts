import * as signalR from "@microsoft/signalr";

const HUB_URL = import.meta.env.VITE_HUB_URL || "http://localhost:5000/promptHub";

type EventHandler = (...args: unknown[]) => void;

class SignalRService {
    private connection: signalR.HubConnection | null = null;
    private startPromise: Promise<signalR.HubConnection> | null = null;
    private pendingHandlers = new Map<string, Set<EventHandler>>();

    public async connect(): Promise<signalR.HubConnection> {
        if (this.startPromise) {
            return this.startPromise;
        }

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(HUB_URL, {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets,
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.connection.onreconnecting(() => {
            console.warn("SignalR: reconnecting...");
        });

        this.connection.onreconnected(() => {
            console.info("SignalR: reconnected");
        });

        this.connection.onclose((error) => {
            console.error("SignalR: connection closed", error);
            this.startPromise = null;
            this.connection = null;
        });

        this.startPromise = this.connection.start()
            .then(() => {
                if (!this.connection) {
                    throw new Error("SignalR connection lost during startup");
                }

                for (const [eventName, handlers] of this.pendingHandlers.entries()) {
                    handlers.forEach((handler) => {
                        this.connection?.on(eventName, handler);
                        console.info(`SignalR: handler attached for ${eventName}`);
                    });
                }

                console.info("SignalR: connected to hub", HUB_URL);
                return this.connection;
            })
            .catch((err) => {
                console.error("SignalR: start failed", err);
                this.startPromise = null;
                throw err;
            });

        return this.startPromise;
    }

    public async disconnect(): Promise<void> {
        if (this.connection) {
            try {
                await this.connection.stop();
                console.info("SignalR: disconnected from hub");
            } catch (err) {
                console.error("SignalR: error during disconnect", err);
            } finally {
                this.connection = null;
                this.startPromise = null;
                this.pendingHandlers.clear();
            }
        }
    }

    public on(eventName: string, handler: EventHandler) {
        const handlers = this.pendingHandlers.get(eventName) ?? new Set<EventHandler>();
        handlers.add(handler);
        this.pendingHandlers.set(eventName, handlers);

        if (this.connection && this.connection.state === signalR.HubConnectionState.Connected) {
            this.connection.on(eventName, handler);
            console.info(`SignalR: handler registered (live) for ${eventName}`);
        }
    }

    public off(eventName: string, handler: EventHandler) {
        const handlers = this.pendingHandlers.get(eventName);
        if (handlers) {
            handlers.delete(handler);
            if (handlers.size === 0) {
                this.pendingHandlers.delete(eventName);
            }
        }

        if (this.connection) {
            this.connection.off(eventName, handler);
            console.info(`SignalR: handler removed (live) for ${eventName}`);
        }
    }

    public async invoke(methodName: string, ...args: unknown[]): Promise<void> {
        if (!this.connection) {
            throw new Error("SignalR: not connected");
        }

        try {
            await this.connection.invoke(methodName, ...args);
            console.info(`SignalR: method ${methodName} invoked`);
        } catch (err) {
            console.error(`SignalR: error invoking ${methodName}`, err);
            throw err;
        }
    }

    public getState(): signalR.HubConnectionState | null {
        return this.connection?.state ?? null;
    }

    public isConnected(): boolean {
        return this.connection?.state === signalR.HubConnectionState.Connected;
    }
}

export const signalRService = new SignalRService();
