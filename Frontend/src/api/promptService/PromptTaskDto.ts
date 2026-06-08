export type PromptTaskDto = {
  id: string;
  content: string;
  status: string;
  result: string | undefined;
  createdAt: Date;
};
