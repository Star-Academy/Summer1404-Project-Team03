export interface Schema {
    stagedFileId: number,
    columns: ShemaColumn[];
}

export interface ShemaColumn {
    ordinalPosition: number;
    columnName: string;
    originalColumnName: string;
    columnType: string;
}

export interface SchemaEditorState {
    schema: Schema | null;
    isLoading: boolean;
    dbTypes: string[];
    error: null | string;
}