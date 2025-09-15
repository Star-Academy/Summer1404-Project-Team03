export interface CsvParserConfig {
    delimiter?: string;
    header?: boolean;
}

export interface ParsedCsvData {
    headers: string[];
    data: Record<string, any>[];
    errors: string[];
}