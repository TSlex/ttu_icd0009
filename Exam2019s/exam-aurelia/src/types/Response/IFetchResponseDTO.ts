export interface IFetchResponse<TData> {
  status: string;
  errors: string[];
  data: TData | null;
}
