import { useMutation, useQueryClient } from "@tanstack/react-query";
import { promptService } from "../../api/promptService/PromptService";

export const useCreatePrompts = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: promptService.createPrompt,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["promptsList"] });
    },
    onError: (error) => {
      console.error(`Error while sending request ${error.message}`);
    },
  });
};
