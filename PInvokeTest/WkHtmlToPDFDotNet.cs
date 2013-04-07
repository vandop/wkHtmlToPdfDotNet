using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace K4TUtils
{
    /// <summary>
    /// Simple Wrapper arruond wkHtmlToPDF
    /// http://code.google.com/p/wkhtmltopdf/
    /// 
    /// This just a initial release, but in future releases it will support much more functionality given by original wkhtmltopdf
    /// </summary>
    public unsafe class WkHtmlToPDFDotNet
    {
        [DllImport("dlls\\wkHtmlToPdfNativeWrapper", EntryPoint = "?ConvertPDFFromHtml@@YGXPAD0@Z", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern void ConvertPDFFromHtml(void* source, void* file);

        public static void ConvertPDF(string sourceName = @"C:\Users\Public\teste.html", string outputFile = @"C:\Users\Public\teste.pdf")
        {
            IntPtr sourceNamePtr = IntPtr.Zero;
            IntPtr outputFilePtr = IntPtr.Zero;
            try
            {
                sourceNamePtr = Marshal.StringToHGlobalAnsi(sourceName);
                outputFilePtr = Marshal.StringToHGlobalAnsi(outputFile);

                ConvertPDFFromHtml(sourceNamePtr.ToPointer(), outputFilePtr.ToPointer());
            }
            finally
            {
                if (sourceNamePtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(sourceNamePtr);
                if (outputFilePtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(outputFilePtr);
            }

        }
    }
}
