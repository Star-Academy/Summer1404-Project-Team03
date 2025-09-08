export type TableType ={
  schemaId: number;
  tableName: string;
  originalFileName: string;
  columnCount: number;
}

export type TableStore = {
  tables: TableType[];
  isLoading: boolean;
}
