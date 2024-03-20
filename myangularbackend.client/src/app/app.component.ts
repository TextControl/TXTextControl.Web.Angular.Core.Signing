import { HttpClient } from '@angular/common/http';
import { Component, OnInit, HostListener } from '@angular/core';

declare const TXDocumentViewer: any;
interface DocumentData {
  document?: string;
  name?: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {

  public documentData: DocumentData = {};

  constructor(private http: HttpClient) { }

  @HostListener('window:documentViewerLoaded', ['$event'])
  onDocumentViewerLoaded() {

    TXDocumentViewer.signatures.setSubmitCallback(function (data: string) {
      var element = document.createElement('a');
      element.setAttribute('href', 'data:application/pdf;;base64,' + data);
      element.setAttribute('download', "results.pdf");
      document.body.appendChild(element);
      element.click();
    })

    var signatureSettings = {
      showSignatureBar: true,
      redirectUrlAfterSignature: 'https://localhost:7275/document/sign',
      ownerName: 'Paul',
      signerName: 'Jacob',
      signerInitials: 'PK',
      signatureBoxes: [{ name: 'txsign', signingRequired: true, style: 0 }]
    };

    TXDocumentViewer.loadDocument(this.documentData.document, this.documentData.name, signatureSettings);
  }

  ngOnInit() {
    this.getDocument();
  }

  getDocument() {
    this.http.get<DocumentData>('/document/load').subscribe(
      (result) => {
        this.documentData = result;
      }
    );
  }

  title = 'myangularbackend.client';
}
