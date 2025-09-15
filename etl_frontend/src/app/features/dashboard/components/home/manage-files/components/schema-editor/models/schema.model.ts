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
    isFetching: boolean;
    isSaving: boolean;
    dbTypes: string[];
    error: null | string;
    isSaveSuccess: null | boolean;
}