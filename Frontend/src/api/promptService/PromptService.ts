import { apiClients } from "../client/ApiClients";
import type { PromptTaskDto } from "./PromptTaskDto";

export const promptService = {
  getAllPrompts: async (): Promise<PromptTaskDto[]> => {
    const { data } = await apiClients.get<{
      isSuccess: boolean;
      data: PromptTaskDto[];
      errorMessage?: string | null;
    }>("/Prompts");

    return data.data ?? [];
  },

  createPrompt: async (dto: { prompts: string[] }): Promise<string[]> => {
    const { data } = await apiClients.post<{
      isSuccess: boolean;
      data: string[];
      errorMessage?: string | null;
    }>("/Prompts", dto);

    return data.data ?? [];
  }
}
