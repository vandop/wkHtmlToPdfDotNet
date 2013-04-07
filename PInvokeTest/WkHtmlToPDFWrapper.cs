
using System.Runtime.InteropServices;
using System;
namespace PInvokeTest
{
	unsafe class WkHtmlToPDFWrapper
	{
		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_init")]
		private static extern int wkhtmltopdf_init(int use_graphics);
		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_deinit")]
		private static extern int wkhtmltopdf_deinit();
		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_extended_qt")]
		private static extern int wkhtmltopdf_extended_qt();
		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_version")]
		private static extern IntPtr wkhtmltopdf_version();


		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_create_global_settings")]
		private static extern IntPtr wkhtmltopdf_create_global_settings();
		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_create_object_settings")]
		private static extern IntPtr wkhtmltopdf_create_object_settings();

		
		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_set_global_setting")]
        private static extern int wkhtmltopdf_set_global_setting(IntPtr settings, IntPtr name, IntPtr value);

		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_get_global_setting")]
		private static extern int wkhtmltopdf_get_global_setting(IntPtr settings, IntPtr name, IntPtr value, int vs);

		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_set_object_setting")]
		private static extern int wkhtmltopdf_set_object_setting(IntPtr settings, IntPtr name, IntPtr value);

		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_get_object_setting")]
		private static extern int wkhtmltopdf_get_object_setting(IntPtr settings, IntPtr name, IntPtr value, int vs);

		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_create_converter")]
		private static extern IntPtr wkhtmltopdf_create_converter(IntPtr settings);
		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_destroy_converter")]
		private static extern void wkhtmltopdf_destroy_converter(IntPtr converter);

		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_convert")]
		private static extern int wkhtmltopdf_convert(IntPtr converter);
		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_add_object")]
		private static extern void wkhtmltopdf_add_object(IntPtr converter, IntPtr setting, IntPtr data);

		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_http_error_code")]
		private static extern int wkhtmltopdf_http_error_code(IntPtr converter);

		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_get_output")]
		private static extern long wkhtmltopdf_get_output(IntPtr converter, IntPtr output);

		[DllImport("wkhtmltox", EntryPoint = "wkhtmltopdf_progress_string")]
		private static extern IntPtr wkhtmltopdf_progress_string(IntPtr converter);

		//private static extern void wkhtmltopdf_set_warning_callback(wkhtmltopdf_converter * converter, wkhtmltopdf_str_callback cb);
		//private static extern void wkhtmltopdf_set_error_callback(wkhtmltopdf_converter * converter, wkhtmltopdf_str_callback cb);
		//private static extern void wkhtmltopdf_set_phase_changed_callback(wkhtmltopdf_converter * converter, wkhtmltopdf_void_callback cb);
		//private static extern void wkhtmltopdf_set_progress_changed_callback(wkhtmltopdf_converter * converter, wkhtmltopdf_int_callback cb);
		//private static extern void wkhtmltopdf_set_finished_callback(wkhtmltopdf_converter * converter, wkhtmltopdf_int_callback cb);


		#region Private Fields
		private static int GetHttpErrorCode(IntPtr converter)
		{
			return wkhtmltopdf_http_error_code(converter);
		}

		private static string GetProgressString(IntPtr converter)
		{
			IntPtr _strPTr = IntPtr.Zero ;
			try
			{
				
				_strPTr = wkhtmltopdf_progress_string(converter);

				return Marshal.PtrToStringAnsi(_strPTr);
			}
			finally
			{
				//if (_strPTr != IntPtr.Zero)
				//    Marshal.FreeHGlobal(_strPTr);
			}
		}

		private static byte[] GetOutput(IntPtr converter)
		{
			IntPtr refPtr = IntPtr.Zero;
			IntPtr outPtr = IntPtr.Zero;
			byte[] outFile;
			try
			{
				refPtr = Marshal.AllocHGlobal(IntPtr.Size);
				long size = wkhtmltopdf_get_output(converter, refPtr);
				outFile = new byte[size];
				
				if (size < 0)
				{
					throw new ExternalException("Exception when trying to read output");
				}
				outPtr = Marshal.ReadIntPtr(refPtr);
				Marshal.Copy(outPtr, outFile, 0, (int)size);
				return outFile;
			}
			finally
			{
				if (refPtr != IntPtr.Zero)
					Marshal.FreeHGlobal(refPtr);
			}

		}
		#endregion


		public static void Init(bool useGraphics)
		{
			if (wkhtmltopdf_init(useGraphics ? 0 : 1) != 1)
			{
				throw new ExternalException("Exception occurs when calling init");
			}
		}

		public static void DeInit()
		{
			if (wkhtmltopdf_deinit() != 1)
			{
				throw new ExternalException("Exception occurs when calling DeInit");
			}
		}

		public static void ExcentendQt()
		{
			if (wkhtmltopdf_extended_qt() != 0)
			{
				throw new ExternalException("Exception occurs when calling ExcentendQt");
			}
		}

		public static string GetVersion()
		{
			try {
			IntPtr _versionPTr = wkhtmltopdf_version();
			
			return Marshal.PtrToStringAnsi(_versionPTr);
			}
			finally {

			}
		}

		

		/// <summary>
		/// Convert a HTML link or File to PDF. We are following the wkHtmlToPDF C bindings samples, named: pdf_c_api.c
		/// </summary>
		/// <param name="sourceName"></param>
		/// <param name="outputFile"></param>
		/// <returns></returns>
		public static bool ConvertPDF(string sourceName = "http://doc.trolltech.com/4.6/qstring.html", string outputFile = "teste.pdf")
		{
			IntPtr fileNamePtr      = IntPtr.Zero;
			IntPtr outOptionNamePtr = IntPtr.Zero;
			IntPtr cookieJarPTr     = IntPtr.Zero;
			IntPtr jarNamePtr          = IntPtr.Zero;
			IntPtr gs = IntPtr.Zero;
			IntPtr os = IntPtr.Zero;
			IntPtr converter = IntPtr.Zero;
			IntPtr pageSettingStr = IntPtr.Zero;
			IntPtr sourceNamePtr = IntPtr.Zero;
			try
			{

				/* Init wkhtmltopdf in graphics less mode */
				Init(false);

				/*
				 * Create a global settings object used to store options that are not
				 * related to input objects, note that control of this object is parsed to
				 * the converter later, which is then responsible for freeing it
				 */

				
				gs = wkhtmltopdf_create_object_settings(); //create global settings

				outOptionNamePtr = Marshal.StringToHGlobalAnsi("out");
				fileNamePtr = Marshal.StringToHGlobalAnsi(outputFile);
				cookieJarPTr = Marshal.StringToHGlobalAnsi("load.cookieJar");
				jarNamePtr = Marshal.StringToHGlobalAnsi("myjar.jar");

				int success;
				/* We want the result to be storred in the file called test.pdf */
				success = wkhtmltopdf_set_global_setting(gs, outOptionNamePtr, fileNamePtr);

				success = wkhtmltopdf_set_global_setting(gs, cookieJarPTr, jarNamePtr);

				/*
				 * Create a input object settings object that is used to store settings
				 * related to a input object, note again that control of this object is parsed to
				 * the converter later, which is then responsible for freeing it
				 */
				os = wkhtmltopdf_create_object_settings();
				sourceNamePtr = Marshal.StringToHGlobalAnsi(sourceName);
				pageSettingStr = Marshal.StringToHGlobalAnsi("page");
				/* We want to convert to convert the qstring documentation page */
				success = wkhtmltopdf_set_object_setting(os, pageSettingStr, sourceNamePtr);

				/* Create the actual converter object used to convert the pages */
				IntPtr c = wkhtmltopdf_create_converter(gs);

				#region Callback Functions NOT YET IMPLEMENTED
				/* Call the progress_changed function when progress changes */
				//wkhtmltopdf_set_progress_changed_callback(c, progress_changed);

				/* Call the phase _changed function when the phase changes */
				//wkhtmltopdf_set_phase_changed_callback(c, phase_changed);

				/* Call the error function when an error occures */
				//wkhtmltopdf_set_error_callback(c, error);

				/* Call the waring function when a warning is issued */
				//wkhtmltopdf_set_warning_callback(c, warning);
				#endregion

				/*
				 * Add the the settings object describing the qstring documentation page
				 * to the list of pages to convert. Objects are converted in the order in which
				 * they are added
				 */
				wkhtmltopdf_add_object(c, os,IntPtr.Zero);

                string progress = GetProgressString(c);
                Console.WriteLine(progress);

				/* Perform the actual convertion */
				success = wkhtmltopdf_convert(c);
				//if (success < 1)
				//{
				//    Console.WriteLine("Convertion Failed");
				//    Console.ReadKey();
				//    return false;
				//}
				progress = GetProgressString(c);
				Console.WriteLine(progress);

                //ByteArrayToFile(outputFile, GetOutput(c));

				/* Output possible http error code encountered */
				Console.WriteLine("Error Code: ", GetHttpErrorCode(c));

				/* Destroy the converter object since we are done with it */
				wkhtmltopdf_destroy_converter(c);

				/* We will no longer be needing wkhtmltopdf funcionality */
				DeInit();

				return true;
			}
			finally
			{
				if (fileNamePtr != IntPtr.Zero)
					Marshal.FreeHGlobal(fileNamePtr);
				if (outOptionNamePtr != IntPtr.Zero)
					Marshal.FreeHGlobal(outOptionNamePtr);
				if (cookieJarPTr != IntPtr.Zero)
					Marshal.FreeHGlobal(cookieJarPTr);
				if (jarNamePtr != IntPtr.Zero)
					Marshal.FreeHGlobal(jarNamePtr);
				if (gs != IntPtr.Zero)
					Marshal.FreeHGlobal(gs);
				if (pageSettingStr != IntPtr.Zero)
					Marshal.FreeHGlobal(pageSettingStr);
				if (sourceNamePtr != IntPtr.Zero)
					Marshal.FreeHGlobal(sourceNamePtr);
			}
		}

		/// <summary>
		/// Function to save byte array to a file
		/// </summary>
		/// <param name="_FileName">File name to save byte array</param>
		/// <param name="_ByteArray">Byte array to save to external file</param>
		/// <returns>Return true if byte array save successfully, if not return false</returns>
		public static bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
		{
			try
			{
				// Open file for reading
				System.IO.FileStream _FileStream = new System.IO.FileStream(_FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);

				// Writes a block of bytes to this stream using data from a byte array.
				_FileStream.Write(_ByteArray, 0, _ByteArray.Length);

				// close file stream
				_FileStream.Close();

				return true;
			}
			catch (Exception _Exception)
			{
				// Error
				Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
			}

			// error occured, return false
			return false;
		}






		
	}
}
