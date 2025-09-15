export type RowType = {
  [key: string]: string;
};

export type RowResponse = {
  rows: RowType[];
  nextOffset: number;
}


export type RowStore = {
  rows: RowType[];
  offset: number;
  isLoading: boolean;
};
