import { useState } from "react";
import { useCurrentPromptsContext } from "../../context/CurrentPromptsContext";
import styles from "./CurrentPromptsForm.module.scss";

const CurrentPromptsForm = () => {
  const [inputValue, setInputValue] = useState<string>("");
  const { addPrompt } = useCurrentPromptsContext();

  const handleSubmit = (e: React.SyntheticEvent) => {
    e.preventDefault();
    if (!inputValue?.trim()) return;

    addPrompt(inputValue);
    setInputValue(() => "");
  };

  return (
    <form className={styles.promptsForm} onSubmit={handleSubmit}>
      <textarea
        className={styles.promptInput}
        value={inputValue}
        onChange={(e) => setInputValue(() => e.target.value)}
        placeholder="Add new prompt"
      />
      <button className={styles.addPromptButton} type="submit">
        Add Prompt
      </button>
    </form>
  );
};

export default CurrentPromptsForm;
