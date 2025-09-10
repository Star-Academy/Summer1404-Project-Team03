export type ColumnType =  {
  id: number;
  ordinalPosition: number;
  name: string;
  type: string;
  originalName: string;
}

export type ColumnStore = {
  columns: ColumnType[];
  isLoading: boolean;
};
