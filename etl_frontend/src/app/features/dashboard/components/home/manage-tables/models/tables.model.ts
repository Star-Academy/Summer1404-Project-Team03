export type TableType = {
  schemaId: number;
  tableName: string;
  originalFileName: string;
  columnCount: number;
}

export type TableStore = {
  tables: TableType[];
  isLoading: boolean;
}

export type TableValidTypes = {
  allowedTypes: string[];
  typeAliases: {
    [key: string]: string
  };
}