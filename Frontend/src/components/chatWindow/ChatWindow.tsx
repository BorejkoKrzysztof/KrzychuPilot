import React, { useEffect } from "react";
import { useQueryClient } from "@tanstack/react-query";
import styles from "./ChatWindow.module.scss";
import ChatMessage from "../chatMessage/ChatMessage";
import { useGetAllPrompts } from "../../hooks/useGetAllPrompts/useGetAllPrompts";
import { signalRService } from "../../services/signalR/signalRService";
import type { PromptTaskDto } from "../../api/promptService/PromptTaskDto";

const ChatWindow = () => {
  const queryClient = useQueryClient();
  const { data: promptTasks } = useGetAllPrompts();

  useEffect(() => {
    const handleStatusUpdate = (...args: unknown[]) => {
      const update = args[0] as {
        taskId: string;
        status: string;
        result?: string | null;
      } | undefined;

      if (!update?.taskId) {
        return;
      }

      queryClient.setQueryData<PromptTaskDto[]>(["promptsList"], (old) => {
        if (!old) {
          return old;
        }

        return old.map((task) =>
          task.id === update.taskId
            ? {
              ...task,
              status: update.status,
              result: update.result ?? task.result,
            }
            : task,
        );
      });
    };

    signalRService.on("ReceiveStatusUpdate", handleStatusUpdate);

    return () => {
      signalRService.off("ReceiveStatusUpdate", handleStatusUpdate);
    };
  }, [queryClient]);

  return (
    <section className={styles.chatWindow}>
      <div className={styles.chatContentArea}>
        {promptTasks?.map((promptTask, id) => (
          <React.Fragment key={id}>
            <ChatMessage isMessageReceived={false} prompTask={promptTask} />
            <ChatMessage isMessageReceived={true} prompTask={promptTask} />
          </React.Fragment>
        ))}
      </div>
    </section>
  );
};

export default ChatWindow;
