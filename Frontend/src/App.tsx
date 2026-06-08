import { useEffect } from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import "./App.module.scss";
import ChatWindow from "./components/chatWindow/ChatWindow";
import CurrentPrompts from "./components/currentPrompts/CurrentPrompts";
import { signalRService } from "./services/signalR/signalRService";

const queryClient = new QueryClient();

function App() {
  useEffect(() => {
    signalRService.connect().catch((err) => {
      console.error("Failed to connect to SignalR hub:", err);
    });

    return () => {
      signalRService.disconnect();
    };
  }, []);

  return (
    <QueryClientProvider client={queryClient}>
      <CurrentPrompts />
      <ChatWindow />
    </QueryClientProvider>
  );
}

export default App;
