using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace WkHtmlToPDFDotNet
{

    public class HTMLConverterRequest
    {
        public string Input;
        public string Output;
        public ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
    }

    public sealed class HtmlToPDFConverter : IDisposable
    {
        #region Singleton Stuff
        /*Singleton Implemetation: http://www.yoda.arachsys.com/csharp/singleton.html
         * In the reference above its possible to check a understand why we didn't choose a Lazy implementation.
        */

        static readonly HtmlToPDFConverter instance = new HtmlToPDFConverter();


        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static HtmlToPDFConverter()
        {
        }

        private HtmlToPDFConverter()
        {
            this.Initialize();
        }

        /// <summary>
        /// Gets the UploadManager Singleton Instance
        /// </summary>
        public static HtmlToPDFConverter Instance
        {
            get
            {

                return instance;
            }
        }
        #endregion

        private  System.Collections.Concurrent.ConcurrentQueue<HTMLConverterRequest> _requestQueue = new System.Collections.Concurrent.ConcurrentQueue<HTMLConverterRequest>();
        private Task _consumerThread;
        CancellationTokenSource cancelationToken = new CancellationTokenSource();
        CancellationToken ct;
        public ManualResetEvent _mre= new ManualResetEvent(false);

        public void Initialize()
        {
            _consumerThread = Task.Factory.StartNew(StartConverter,cancelationToken.Token);
            ct = cancelationToken.Token;

        }

        private void StartConverter()
        {
            PDFWrapper.Initialize();
            while(true){
                ct.ThrowIfCancellationRequested();
                HTMLConverterRequest request;
                if (_requestQueue.TryDequeue(out request))
                {
                    Console.WriteLine("Dequeued: Converting");
                    PDFWrapper.ConvertPDF(request.Input, request.Output);
                    request.ManualResetEvent.Set();
                }
                else
                {
                    Console.WriteLine("Waiting to be feeded");
                    _mre.Reset();
                    _mre.WaitOne();   
                }
            }
        }

        public void EnqueuePDFConversion(HTMLConverterRequest request)
        {
            _requestQueue.Enqueue(request);
            _mre.Set();
        }

        #region IDisposable Members

        private void Destroy()
        {
            cancelationToken.Cancel();
            PDFWrapper.Destroy();
        }

        public void Dispose()
        {
            Destroy();
            GC.SuppressFinalize(this);
        }

        ~HtmlToPDFConverter() // To be called by GC when Dispose is not called
        {
            Destroy();
        }

        #endregion
    }
}
