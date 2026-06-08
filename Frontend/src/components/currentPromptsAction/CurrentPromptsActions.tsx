import { useCurrentPromptsContext } from "../../context/CurrentPromptsContext";
import styles from "./CurrentPromptsActions.module.scss";

const CurrentPromptsActions = () => {
  const { prompts, sendPrompts } = useCurrentPromptsContext();

  return (
    <button
      className={styles.sendPromptsButton}
      onClick={sendPrompts}
      disabled={prompts.length === 0}
    >
      Send Prompts
    </button>
  );
};

export default CurrentPromptsActions;
