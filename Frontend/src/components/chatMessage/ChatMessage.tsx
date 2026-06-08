import type { PromptTaskDto } from "../../api/promptService/PromptTaskDto";
import styles from "./ChatMessage.module.scss";

type ChatMessageProps = {
  isMessageReceived: boolean;
  prompTask: PromptTaskDto;
};

const ChatMessage = (props: ChatMessageProps) => {
  const { isMessageReceived, prompTask } = props;

  return (
    <article
      className={`${styles.messageBlock} ${isMessageReceived ? styles.receivedBlock : styles.sentBlock}`}
    >
      <div
        className={`${styles.message} ${isMessageReceived ? styles.received : styles.sent}`}
      >
        <p>
          {!isMessageReceived ? prompTask.content : prompTask.result ?? "..."}
        </p>
      </div>
      {isMessageReceived && (
        <div className={styles.statusWrapper}>
          <div className={`${styles.statusDot} ${styles[prompTask.status.toLowerCase() as keyof typeof styles]}`} />
          <p>Status: {prompTask.status}</p>
        </div>
      )}
    </article>
  );
};

export default ChatMessage;
