// PDFNativeTester.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <wkHtmlToPdfNativeWrapper.h>
#include <stdio.h>
#include <wkhtmltox\pdf.h>

int _tmain(int argc, _TCHAR* argv[])
{
	Init();
	ConvertPDFFromHtml("C:\\Users\\Public\\htmlView.html","C:\\Users\\Public\\test.pdf");
	ConvertPDFFromHtml("C:\\Users\\Public\\htmlView.html","C:\\Users\\Public\\test1.pdf");
	ConvertPDFFromHtml("C:\\Users\\Public\\htmlView.html","C:\\Users\\Public\\test2.pdf");
	ConvertPDFFromHtml("C:\\Users\\Public\\htmlView.html","C:\\Users\\Public\\test3.pdf");
	ConvertPDFFromHtml("C:\\Users\\Public\\htmlView.html","C:\\Users\\Public\\test4.pdf");
	DeInit();

	//wkhtmltopdf_global_settings * gs;
	//wkhtmltopdf_object_settings * os;
	//wkhtmltopdf_converter * c;

	////printf("Begin\n");
	///* Init wkhtmltopdf in graphics less mode */
	//wkhtmltopdf_init(false);

	///*
	// * Create a global settings object used to store options that are not
	// * related to input objects, note that control of this object is parsed to
	// * the converter later, which is then responsible for freeing it
	// */
	//gs = wkhtmltopdf_create_global_settings();
	///* We want the result to be storred in the file called test.pdf */
	//wkhtmltopdf_set_global_setting(gs, "out", "teste.pdf");

	//wkhtmltopdf_set_global_setting(gs, "load.cookieJar", "myjar.jar");
	///*
	// * Create a input object settings object that is used to store settings
	// * related to a input object, note again that control of this object is parsed to
	// * the converter later, which is then responsible for freeing it
	// */
	//os = wkhtmltopdf_create_object_settings();
	///* We want to convert to convert the qstring documentation page */
	//wkhtmltopdf_set_object_setting(os, "page", "http://www.google.com");

	///* Create the actual converter object used to convert the pages */
	//c = wkhtmltopdf_create_converter(gs);

	/////* Call the progress_changed function when progress changes */
	////wkhtmltopdf_set_progress_changed_callback(c, progress_changed);

	/////* Call the phase _changed function when the phase changes */
	////wkhtmltopdf_set_phase_changed_callback(c, phase_changed);

	/////* Call the error function when an error occures */
	////wkhtmltopdf_set_error_callback(c, error);

	/////* Call the waring function when a warning is issued */
	////wkhtmltopdf_set_warning_callback(c, warning);

	///*
	// * Add the the settings object describing the qstring documentation page
	// * to the list of pages to convert. Objects are converted in the order in which
	// * they are added
	// */
	//wkhtmltopdf_add_object(c, os, NULL);

	///* Perform the actual convertion */
	//int code = wkhtmltopdf_convert(c);
	//if (!code)
	//	fprintf(stderr, "Convertion failed!");

	///* Output possible http error code encountered */
	//printf("httpErrorCode: %d  Conversion Code: %d \n", wkhtmltopdf_http_error_code(c),code);

	///* Destroy the converter object since we are done with it */
	//wkhtmltopdf_destroy_converter(c);

	///* We will no longer be needing wkhtmltopdf funcionality */
	//wkhtmltopdf_deinit();

	//printf("Done");

	getchar();
	return 0;
}

