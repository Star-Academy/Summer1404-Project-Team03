import { Injectable } from '@angular/core';
import { CsvParserConfig, ParsedCsvData } from '../../models/csv-parser.model';
import { Observable, Observer } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CsvParserService {

  constructor() { }

  public parse(file: File, config: CsvParserConfig = {}): Observable<ParsedCsvData> {
    const finalConfig: Required<CsvParserConfig> = {
      delimiter: config.delimiter ?? ',',
      header: config.header ?? true,
    };

    return new Observable((observer: Observer<ParsedCsvData>) => {
      if (!file) {
        observer.error('A valid File object must be provided.');
        return;
      }

      const reader = new FileReader();

      reader.onload = () => {
        try {
          const text = reader.result as string;
          const result = this.processCsvText(text, finalConfig);
          observer.next(result);
          observer.complete();
        } catch (error) {
          observer.error(error);
        }
      };

      reader.onerror = () => {
        observer.error(`Error reading the file: ${file.name}`);
      };

      reader.readAsText(file);
    });
  }

  private processCsvText(text: string, config: Required<CsvParserConfig>): ParsedCsvData {
    const lines = text.split(/\r\n|\n/).filter(line => line.trim() !== '');
    if (lines.length === 0) {
      return { headers: [], data: [], errors: [] };
    }

    const headers = this.getHeaders(lines, config);
    const data: Record<string, any>[] = [];
    const errors: string[] = [];
    const headerCount = headers.length;
    const startIndex = config.header ? 1 : 0;

    for (let i = startIndex; i < lines.length; i++) {
      const values = lines[i].split(config.delimiter);

      if (values.length !== headerCount) {
        errors.push(`Row ${i + 1} has ${values.length} columns, but header has ${headerCount}.`);
        continue;
      }

      const rowObject = headers.reduce((obj, header, index) => {
        obj[header] = values[index]?.trim() ?? '';
        return obj;
      }, {} as Record<string, any>);

      data.push(rowObject);
    }

    return { headers, data, errors };
  }

  private getHeaders(lines: string[], config: Required<CsvParserConfig>): string[] {
    if (config.header) {
      return lines[0].split(config.delimiter).map(h => h.trim());
    }

    const firstLine = lines[0] ?? '';
    const columnCount = firstLine.split(config.delimiter).length;
    return Array.from({ length: columnCount }, (_, i) => `Column ${i + 1}`);
  }
}
