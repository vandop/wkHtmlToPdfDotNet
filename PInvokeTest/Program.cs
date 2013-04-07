using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using WkHtmlToPDFDotNet;

namespace PInvokeTest
{
    class Program
    {
        static ManualResetEvent mre = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            //Console.WriteLine(WkHtmlToPDFWrapper.GetVersion());
            //WkHtmlToPDFDotNet.HtmlToPDFConverter.Initialize();
            //WkHtmlToPDFDotNet.HtmlToPDFConverter.ConvertPDF("C:\\Users\\Public\\htmlView.html", string.Format("C:\\Users\\Public\\test0{0}.pdf", 1));
            //WkHtmlToPDFDotNet.HtmlToPDFConverter.ConvertPDF("C:\\Users\\Public\\htmlView.html", string.Format("C:\\Users\\Public\\test0{0}.pdf", 2));



            Thread last = null;
            for (int i = 0; i < 10; i++)
            {
                var thread = new Thread(Program.ConvertPDFs);
                thread.Start(i);
                last = thread;
            }


            if(last != null)
                last.Join();

            Console.WriteLine("First Row completed");
            Console.ReadKey();

            for (int i = 10; i < 20; i++)
            {
                var thread = new Thread(Program.ConvertPDFs);
                thread.Start(i);
                last = thread;
            }

            Console.WriteLine("Second Row completed");

            Console.ReadKey();

            for (int i = 10; i < 30; i++)
            {
                var thread = new Thread(Program.ConvertPDFs);
                thread.Start(i);
                last = thread;
            }

            Console.WriteLine("Third Row completed");




            //WkHtmlToPDFDotNet.HtmlToPDFConverter.Dispose();
            //WkHtmlToPDFWrapper.ConvertPDF(outputFile:"C:\\Users\\Public\\test.pdf");
            Console.WriteLine("Converted - Press any key to exit");
            
        }

        static void ConvertPDFs(object data)
        {
            int seed = (int)data;
            var request0 = new HTMLConverterRequest() { Input = "test.html", Output = string.Format(@"test{0}.pdf", seed) };
            HtmlToPDFConverter.Instance.EnqueuePDFConversion(request0);
            request0.ManualResetEvent.WaitOne();
            Console.WriteLine("PDF Converted seed: " + seed);
        }

    }
}
