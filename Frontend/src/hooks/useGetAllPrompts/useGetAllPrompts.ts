import { useQuery } from "@tanstack/react-query";
import { promptService } from "../../api/promptService/PromptService";

export const useGetAllPrompts = () => {
  return useQuery({
    queryKey: ["promptsList"],
    queryFn: promptService.getAllPrompts,
    staleTime: 1000 * 60 * 5,
  });
};
