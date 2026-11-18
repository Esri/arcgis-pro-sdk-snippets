using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
/*

   Copyright 2025 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       https://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using ArcGIS.Core.OCR;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace ProSnippets.OCRSnippets
{
  /// <summary>
  /// Provides examples of OCR operations using the ArcGIS Pro SDK.
  /// </summary>
  /// <remarks>
  /// The code snippets in this class are intended to be used as examples of how to work with the OCR Manager in ArcGIS Pro.
  /// Each region in the method contains a specific code snippet that demonstrates a particular functionality.
  /// Note that some methods may require to be launched on the ArcGIS Pro's Main CIM thread. These methods are marked in the code comments as requiring a QueuedTask to run.
  /// CRefs are used for internal purposes only. Please ignore them in the context of this example.
  /// </remarks>
  public static partial class ProSnippetsOCR
  {
    public static async void OCRProSnippets()
    {
      #region ignore - Variable initialization

      OCRTSVOutput tsvOutput = null;

      #endregion

      #region ProSnippet Group: OCR
      #endregion

      // cref: ArcGIS.Core.OCR.OCRManager.IsValidDocumentForOCR(System.String)
      // cref: ArcGIS.Core.OCR.OCRManager.Instance
      #region Determine if a document is valid for OCR
      {
        string docFileName1 = "c:\\MyData\\MyDocument.pdf";
        string docFileName2 = "c:\\MyData\\MyDocument.bmp";
        string docFileName3 = "c:\\MyData\\MyDocument.txt";

        var isValid = OCRManager.Instance.IsValidDocumentForOCR(docFileName1);
        // isValid = true

        isValid = OCRManager.Instance.IsValidDocumentForOCR(docFileName2);
        // isValid = true

        isValid = OCRManager.Instance.IsValidDocumentForOCR(docFileName3);
        // isValid = false
      }
      #endregion

      // cref: ArcGIS.Core.OCR.OCRManager.GetDocumentPageCount(System.String)
      // cref: ArcGIS.Core.OCR.OCRManager.Instance
      #region Get number of pages in a document
      {
        string docFileName = "c:\\MyData\\MyDocument.pdf";
        var pageCount = OCRManager.Instance.GetDocumentPageCount(docFileName);
      }
      #endregion

      // cref: ArcGIS.Core.OCR.OCRManager.ExtractTextFromDocumentAsync(System.String)
      // cref: ArcGIS.Core.OCR.OCRManager.Instance
      #region Extract text from a document
      {
        string docFileName = "c:\\MyData\\MyDocument.pdf";

        var text = await OCRManager.Instance.ExtractTextFromDocumentAsync(docFileName);


        // or extract from a specific page
        //int pageIdx = 1;
        //var text = await OCRManager.Instance.ExtractTextFromDocumentAsync(deedFileName, pageIdx);
      }
      #endregion

      // cref: ArcGIS.Core.OCR.OCRManager.ExtractTextFromDocumentAsync(System.String,System.Threading.CancellationToken)
      // cref: ArcGIS.Core.OCR.OCRManager.Instance
      #region Extract text from a document using a CancellationTracker
      {
        string docFileName = "c:\\MyData\\MyDocument.pdf";

        var text = "";
        bool canceled = false;
        using (CancelableProgressorSource cps = new CancelableProgressorSource("Reading document", "Canceled"))
        {
          canceled = await QueuedTask.Run<bool>(async () =>
          {
            try
            {
              text = await ArcGIS.Core.OCR.OCRManager.Instance.ExtractTextFromDocumentAsync(docFileName, cps.Progressor.CancellationToken);
            }
            catch (OperationCanceledException cex)
            {
              return true;
            }
            catch (Exception ex)
            {
            }
            return false;
          }, cps.Progressor);
        }

        if (canceled)
        {
          // to do - some cancel action
        }
        else
        {
          // to do - something with text
        }
      }

      #endregion

      // cref: ArcGIS.Core.OCR.OCROptions
      // cref: ArcGIS.Core.OCR.OCROutputType
      // cref: ArcGIS.Core.OCR.OCRManager.ExtractFromDocumentAsync(ArcGIS.Core.OCR.OCROptions)
      // cref: ArcGIS.Core.OCR.OCROutput
      // cref: ArcGIS.Core.OCR.OCRTextOutput
      // cref: ArcGIS.Core.OCR.OCRTextOutput.Text
      // cref: ArcGIS.Core.OCR.OCRManager.Instance
      // cref: ArcGIS.Core.OCR.OCROptions.OutputType
      #region Extract text from a document using OCROptions
      {
        string docFileName = "c:\\MyData\\MyDocument.tiff";

        var ocrOptions = new OCROptions(docFileName);
        ocrOptions.OutputType = OCROutputType.Text;

        // set a specific page if necessary
        // (if not set, then all pages are scanned)
        // ocrOptions.Page = pageIdx;

        // set the resolution if necessary
        // ocrOptions.Resolution = 400;

        var output = await OCRManager.Instance.ExtractFromDocumentAsync(ocrOptions);
        var textOutput = output as OCRTextOutput;

        var text = textOutput.Text;
      }

      #endregion

      // cref: ArcGIS.Core.OCR.OCROptions
      // cref: ArcGIS.Core.OCR.OCROutputType
      // cref: ArcGIS.Core.OCR.OCROptions.Model
      // cref: ArcGIS.Core.OCR.OCRManager.ExtractFromDocumentAsync(ArcGIS.Core.OCR.OCROptions)
      // cref: ArcGIS.Core.OCR.OCROutput
      // cref: ArcGIS.Core.OCR.OCRTextOutput
      // cref: ArcGIS.Core.OCR.OCRTextOutput.Text
      // cref: ArcGIS.Core.OCR.Exceptions.OCRException
      // cref: ArcGIS.Core.OCR.OCRManager.Instance
      // cref: ArcGIS.Core.OCR.OCROptions.OutputType
      #region Extract text from a document using a different model
      {
        string docFileName = "c:\\MyData\\MyDocument.jpg";

        var ocrOptions = new OCROptions(docFileName);
        ocrOptions.OutputType = OCROutputType.Text;

        // set a specific page if necessary
        // (if not set, then all pages are scanned)
        // ocrOptions.Page = pageIdx;

        // set the resolution if necessary
        // ocrOptions.Resolution = 400;

        // set the language
        // (de.traineddata should be installed at (arcgis_install_folder)\Resources\OCR)
        // if file cannot be found an exception is thrown
        ocrOptions.Model = "de";

        try
        {
          var output = await OCRManager.Instance.ExtractFromDocumentAsync(ocrOptions);
          var textOutput = output as OCRTextOutput;

          var text = textOutput.Text;
        }
        catch (ArcGIS.Core.OCR.Exceptions.OCRException ex)
        {
        }
      }

      #endregion

      // cref: ArcGIS.Core.OCR.OCROptions
      // cref: ArcGIS.Core.OCR.OCROutputType
      // cref: ArcGIS.Core.OCR.OCRManager.ExtractFromDocumentAsync(ArcGIS.Core.OCR.OCROptions)
      // cref: ArcGIS.Core.OCR.OCROutput
      // cref: ArcGIS.Core.OCR.OCRTSVOutput
      // cref: ArcGIS.Core.OCR.OCRTSVOutput.Pages
      // cref: ArcGIS.Core.OCR.OCRPage
      // cref: ArcGIS.Core.OCR.OCRPage.Blocks
      // cref: ArcGIS.Core.OCR.OCRBlock
      // cref: ArcGIS.Core.OCR.OCRBlock.Paragraphs
      // cref: ArcGIS.Core.OCR.OCRParagraph
      // cref: ArcGIS.Core.OCR.OCRParagraph.Lines
      // cref: ArcGIS.Core.OCR.OCRLine
      // cref: ArcGIS.Core.OCR.OCRLine.Words
      // cref: ArcGIS.Core.OCR.OCRWord
      // cref: ArcGIS.Core.OCR.OCRWord.Content
      // cref: ArcGIS.Core.OCR.OCRWord.Confidence
      // cref: ArcGIS.Core.OCR.OCRManager.Instance
      // cref: ArcGIS.Core.OCR.OCROptions.OutputType
      #region Extract TSV output from a document
      {
        string docFileName = "c:\\MyData\\MyDocument.bmp";

        var ocrOptions = new OCROptions(docFileName);
        ocrOptions.OutputType = OCROutputType.TSV;

        // set a specific page if necessary
        // (if not set, then all pages are scanned)
        // ocrOptions.Page = pageIdx;

        // set the resolution if necessary
        // ocrOptions.Resolution = 400;

        var output = await OCRManager.Instance.ExtractFromDocumentAsync(ocrOptions);
        if (output is OCRTSVOutput ocr_tsvOutput)
        {
          // process the tsv output
          foreach (var page in ocr_tsvOutput.Pages)
          {
            foreach (var block in page.Blocks)
            {
              foreach (var para in block.Paragraphs)
              {
                foreach (var line in para.Lines)
                {
                  foreach (var word in line.Words)
                  {
                    var content = word.Content;
                    var confidence = word.Confidence;

                    // process content according to confidence
                  }
                }
              }
            }
          }
        }
      }

      #endregion

      // cref: ArcGIS.Core.OCR.OCRTSVOutput.Pages
      // cref: ArcGIS.Core.OCR.OCRPage
      // cref: ArcGIS.Core.OCR.OCRPage.Blocks
      // cref: ArcGIS.Core.OCR.OCRBlock
      // cref: ArcGIS.Core.OCR.OCRBlock.Paragraphs
      // cref: ArcGIS.Core.OCR.OCRParagraph
      // cref: ArcGIS.Core.OCR.OCRParagraph.Lines
      // cref: ArcGIS.Core.OCR.OCRLine
      // cref: ArcGIS.Core.OCR.OCRLine.Words
      // cref: ArcGIS.Core.OCR.OCRWord.Content
      #region Get text from TSVOutput (simple)
      {
        string text = "";

        var flowDocument = new FlowDocument();

        foreach (var page in tsvOutput.Pages)
        {
          List<Paragraph> pageParagraphs = new();

          foreach (var block in page.Blocks)
          {
            foreach (var paragraph in block.Paragraphs)
            {
              foreach (var line in paragraph.Lines)
              {
                string lineWords = string.Join(" ", line.Words.Select(w => w.Content));
                pageParagraphs.Add(new Paragraph(new Span(new Run(lineWords))));
              }
            }
          }

          flowDocument.Blocks.AddRange(pageParagraphs);
          flowDocument.Blocks.Add(new Paragraph(new Span(new Run(Environment.NewLine))));
        }

        var startPointer = flowDocument?.ContentStart;
        var endPointer = flowDocument?.ContentEnd;

        if (startPointer != null && endPointer != null)
        {
          text = new TextRange(startPointer, endPointer).Text;
        }
      }
      #endregion

    }
  }
}
