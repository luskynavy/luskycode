using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf.parser;

namespace ReadPdf
{
    public class LocationTextExtractionStrategyEx : LocationTextExtractionStrategy
    {
        string _text;
        public LocationTextExtractionStrategyEx()
            : base()
        {
            string _text = "";
        }

        //Automatically called for each chunk of text in the PDF
        public override void RenderText(TextRenderInfo renderInfo)
        {
            base.RenderText(renderInfo);

            var chars = renderInfo.GetCharacterRenderInfos();
            foreach (var c in chars)
            {
                //Console.Write(c.PdfString);
                
                var startPoint = c.GetBaseline().GetStartPoint();

                if (startPoint[0] > 400 && startPoint[0] < 480 && startPoint[1] > 842 - 128 && startPoint[1] < 848 - 110)
                {
                    _text += c.PdfString;
                }
            }

        }

        public override string GetResultantText()
        {
            return _text;
        }
    }
}
