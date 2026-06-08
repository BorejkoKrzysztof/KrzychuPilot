import { createContext, useContext } from "react";

export interface CurrentPromptsContext {
  prompts: string[];
  addPrompt: (prompt: string) => void;
  sendPrompts: () => void;
}

export const CurrentPromptContext = createContext<
  CurrentPromptsContext | undefined
>(undefined);

export const useCurrentPromptsContext = (): CurrentPromptsContext => {
  const context = useContext(CurrentPromptContext);

  if (context === undefined) {
    throw new Error("useCurrentPromptsContext must be used inside provider.");
  }

  return context;
};
