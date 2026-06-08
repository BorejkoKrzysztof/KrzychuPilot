import { useState, type ReactNode } from "react";
import { CurrentPromptContext } from "./CurrentPromptsContext";
import { useCreatePrompts } from "../hooks/useCreatePrompts/useCreatePrompts";

interface CurrentPromptsProviderProps {
  children: ReactNode;
}

export const CurrentPromptsProvider: React.FC<CurrentPromptsProviderProps> = ({
  children,
}) => {
  const [prompts, setPrompts] = useState<string[]>([]);
  const createPromptMutation = useCreatePrompts();

  const addPrompt = (newPrompt: string) => {
    setPrompts((prev) => [...prev, newPrompt]);
  };

  const sendPrompts = () => {
    createPromptMutation.mutate(
      { prompts },
      {
        onSuccess: () => {
          setPrompts(() => []);
        },
      },
    );
  };

  return (
    <CurrentPromptContext.Provider value={{ prompts, addPrompt, sendPrompts }}>
      {children}
    </CurrentPromptContext.Provider>
  );
};
