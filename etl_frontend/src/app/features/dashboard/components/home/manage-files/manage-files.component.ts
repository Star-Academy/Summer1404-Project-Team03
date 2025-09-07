import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';

interface CsvColumn {
  header: string;
  selectedType: string;
}

@Component({
  selector: 'app-manage-files',
  standalone: false,
  templateUrl: './manage-files.component.html',
  styleUrl: './manage-files.component.scss'
})
export class ManageFilesComponent {
  // --- STATE MANAGEMENT ---
  files: any[] = []; // List of all uploaded files
  selectedFileContent: { headers: string[], data: any[] } | null = null;
  uploadedFileColumns: CsvColumn[] = [];

  // Available database types for column mapping
  dbTypes = [
    { label: 'Text', value: 'TEXT' },
    { label: 'Integer', value: 'INTEGER' },
    { label: 'Decimal', value: 'DECIMAL' },
    { label: 'Date', value: 'DATE' },
    { label: 'Boolean', value: 'BOOLEAN' }
  ];

  constructor(private messageService: MessageService) { }

  ngOnInit() {
    // Mock initial file list (in a real app, you'd fetch this from a server)
    this.files = [
      { name: 'products.csv', size: '2.5 KB', lastModified: '2025-08-15' },
      { name: 'customer_data.csv', size: '10.1 KB', lastModified: '2025-08-20' },
    ];
  }

  // --- FILE LIST ACTIONS ---

  /**
   * Deletes a file from the list.
   * @param fileName The name of the file to delete.
   */
  deleteFile(fileName: string) {
    this.files = this.files.filter(f => f.name !== fileName);
    this.messageService.add({ severity: 'warn', summary: 'Deleted', detail: `File '${fileName}' removed.` });
    // If the deleted file was being viewed, clear the view
    if (this.selectedFileContent && fileName === 'currently_viewed_file.csv') { // Mock logic
      this.selectedFileContent = null;
    }
  }

  /**
   * Displays the content of a selected CSV file as a table.
   * @param file The file object to view.
   */
  viewFile(file: any) {
    // In a real app, you would fetch and parse the file content from your server.
    // Here we'll just mock the data for demonstration.
    this.selectedFileContent = {
      headers: ['ID', 'ProductName', 'Price', 'Stock'],
      data: [
        { ID: 1, ProductName: 'Laptop', Price: 1200, Stock: 50 },
        { ID: 2, ProductName: 'Mouse', Price: 25, Stock: 300 },
        { ID: 3, ProductName: 'Keyboard', Price: 75, Stock: 150 }
      ]
    };
    this.uploadedFileColumns = []; // Hide the upload view
  }

  // --- FILE UPLOAD AND PROCESSING ---

  /**
   * Handles the file upload event. Reads the CSV and prepares it for type mapping.
   * @param event The upload event containing the file.
   */
  onFileUpload(event: any) {
    const file = event.files[0];
    const reader = new FileReader();

    reader.onload = (e) => {
      const text = reader.result as string;
      const lines = text.split(/\r\n|\n/).filter(line => line.trim() !== ''); // Split lines and remove empty ones

      if (lines.length > 0) {
        const headers = lines[0].split(',');
        this.uploadedFileColumns = headers.map(header => ({
          header: header.trim(),
          selectedType: 'TEXT' // Default to TEXT
        }));

        // Add the new file to our list
        this.files.push({
          name: file.name,
          size: `${(file.size / 1024).toFixed(2)} KB`,
          lastModified: new Date().toISOString().split('T')[0]
        });

        this.selectedFileContent = null; // Hide file view
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'File uploaded and parsed.' });
      } else {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'The uploaded file is empty.' });
      }
    };

    reader.onerror = () => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to read file.' });
    };

    reader.readAsText(file);
  }

  /**
   * Finalizes the column type selection.
   */
  confirmColumnTypes() {
    // In a real app, you would send `this.uploadedFileColumns` to your backend
    // to save the mapping and process the data.
    console.log('Final column mapping:', this.uploadedFileColumns);
    this.messageService.add({ severity: 'info', summary: 'Confirmed', detail: 'Column types have been set.' });

    // Clear the view after confirming
    this.uploadedFileColumns = [];
  }

  /**
   * Returns to the main file list view.
   */
  goBack() {
    this.selectedFileContent = null;
    this.uploadedFileColumns = [];
  }
}
