
using System.Runtime.InteropServices;
using System;
namespace WkHtmlToPDFDotNet
{
    /// <summary>
    /// Simple Wrapper arruond wkHtmlToPDF
    /// http://code.google.com/p/wkhtmltopdf/
    /// 
    /// This just a initial release, but in future releases it will support much more functionality given by original wkhtmltopdf
    /// </summary>
    public static class PDFWrapper
    {
        [DllImport("wkHtmlToPdfNativeWrapper", EntryPoint = "?ConvertPDFFromHtml@@YGXPAD0@Z", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern void ConvertPDFFromHtml(IntPtr source, IntPtr file);
        [DllImport("wkHtmlToPdfNativeWrapper", EntryPoint = "?Init@@YGXXZ", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern void Initialize();
        [DllImport("wkHtmlToPdfNativeWrapper", EntryPoint = "?DeInit@@YGXXZ", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern void Destroy();

        /// <summary>
        /// Converts a HTML Page or an HTML file to PDF, saving the file at <paramref name="outputfile"/>
        /// </summary>
        /// <param name="sourceName">Name of the source.</param>
        /// <param name="outputFile">The output file.</param>
        public static void ConvertPDF(string sourceName, string outputFile)
        {
            IntPtr sourceNamePtr = IntPtr.Zero;
            IntPtr outputFilePtr = IntPtr.Zero;
            try
            {
                sourceNamePtr = Marshal.StringToHGlobalAnsi(sourceName);
                outputFilePtr = Marshal.StringToHGlobalAnsi(outputFile);

                ConvertPDFFromHtml(sourceNamePtr, outputFilePtr);
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
