import { CurrentPromptsProvider } from "../../context/CurrentPromptsProvider";
import CurrentPromptsActions from "../currentPromptsAction/CurrentPromptsActions";
import CurrentPromptsForm from "../currentPromptsForm/CurrentPromptsForm";
import CurrentPromptsWindow from "../currentPromptsWindow/CurrentPromptsWindow";
import styles from "./CurrentPrompts.module.scss";

// type Props = {};

const CurrentPrompts = () =>
  // props: Props
  {
    return (
      <section className={styles.currentPromptsWrapper}>
        <CurrentPromptsProvider>
          <CurrentPromptsWindow />
          <CurrentPromptsForm />
          <CurrentPromptsActions />
        </CurrentPromptsProvider>
      </section>
    );
  };

export default CurrentPrompts;
