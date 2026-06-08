import { useCurrentPromptsContext } from "../../context/CurrentPromptsContext";
import styles from "./currentPromptsWindow.module.scss";

const CurrentPromptsWindow = () => {
  const { prompts } = useCurrentPromptsContext();

  return (
    <div className={styles.currentPromptsWindow}>
      <div className={styles.currentPromptsWindowContent}>
        {prompts.map((prompt, id) => {
          return <p key={id}>{prompt}</p>;
        })}
      </div>
    </div>
  );
};

export default CurrentPromptsWindow;
