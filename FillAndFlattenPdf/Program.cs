using System;
using pdftron;
using pdftron.Common;
using pdftron.Filters;
using pdftron.SDF;
using pdftron.PDF;

namespace FillAndFlattenPdf
{
    class Program
    {
        private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();

        // TODO: pass this sucker FullName, Address + City + State + Zip, DayNumberZip, DayNumberNumber, Email, String of TaxMapIDs, SignatureImgPath 
        static void Main(string[] args)
        {

            PDFNet.Initialize();

            // string input_path =  "../../../../TestFiles/";
            string output_path = @"C:\Users\dbaker.SDGLOCAL\Documents\Visual Studio 2013\Projects\FillAndFlattenPdf\FillAndFlattenPdf\pdf\";
                        
            // Fill-in forms / Modify values of existing fields.            
            // Search for specific fields in the document.            
            try
            {
                using (PDFDoc doc = new PDFDoc(output_path + "rp305r.pdf"))
                using (Stamper s = new Stamper(Stamper.SizeType.e_relative_scale, 0.3, 0.3))                
                {
                    doc.InitSecurityHandler();

                    Image img = Image.Create(doc, @"C:\Users\dbaker.SDGLOCAL\Documents\Visual Studio 2013\Projects\FillAndFlattenPdf\FillAndFlattenPdf\pdf\sig.png");
                    s.SetSize(Stamper.SizeType.e_relative_scale, 0.3, 0.3);                    
                    //set position of the image to the center, left of PDF pages
                    s.SetAlignment(Stamper.HorizontalAlignment.e_horizontal_left, Stamper.VerticalAlignment.e_vertical_bottom );
                    s.SetPosition(80, 110);
                    s.SetFontColor(new ColorPt(0, 0, 0, 0));
                    //s.SetRotation(180);
                    s.SetAsBackground(false);
                    //only stamp first 2 pages
                    s.StampImage(doc, img, new PageSet(1, 1));

                    FieldIterator itr;
                    for (itr = doc.GetFieldIterator(); itr.HasNext(); itr.Next())
                    {
                        Field field = itr.Current();
                        Console.WriteLine("Field name: {0}", field.GetName());
                        Console.WriteLine("Field partial name: {0}", field.GetPartialName());

                        Console.Write("Field type: ");
                        Field.Type type = field.GetType();
                        switch (type)
                        {
                            //case Field.Type.e_button:
                            //    Console.WriteLine("Button");
                            //    break;
                            //case Field.Type.e_radio:
                            //    Console.WriteLine("Radio button");
                            //    break;
                            //case Field.Type.e_check:
                            //    field.SetValue(true);
                            //    Console.WriteLine("Check box");
                            //    break;
                            case Field.Type.e_text:
                                {
                                    Console.WriteLine("Text");

                                    // Edit all variable text in the document
                                    String old_value = "none";
                                    if (field.GetValue() != null)
                                        old_value = field.GetValue().GetAsPDFText();

                                    string FieldName = field.GetName().ToString();
                                    Console.Write("\n-" + FieldName + "-\n");
                                    switch (FieldName)
                                    {
                                        case "Date":
                                            field.SetValue("CurrentDate");
                                            break;
                                        case "Name and mailing address of landowners 1":
                                            field.SetValue("Full Name");
                                            break;
                                        case "Name and mailing address of landowners 2":
                                            field.SetValue("Street Address");
                                            break;
                                        case "Name and mailing address of landowners 3":
                                            field.SetValue("City + State + Zip");
                                            break;
                                        //case "Name and mailing address of landowners 4":
                                        //    field.SetValue("You Shouldn't Need Me");
                                        //    break;
                                        case "Email address":
                                            field.SetValue("Email");
                                            break;
                                        case "Evening Number":
                                            field.SetValue("Evening Number");
                                            break;
                                        case "Day Number":
                                            field.SetValue("Day Number");
                                            break;
                                        //case "Day Area Code":
                                        //    field.SetValue("Don't Use");
                                        //    break;
                                        case "Year 2":
                                            field.SetValue("Lst");
                                            break;
                                        case "Year 1":
                                            field.SetValue("CrYr");
                                            break;
                                        //case "Following parcels - Tax Map Numbers1":
                                        //    field.SetValue("Don't Use Me. I'm too short");
                                        //    break;
                                        case "Following parcels - Tax Map Numbers2":
                                            field.SetValue(" Tax Map Numbers2");
                                            break;
                                        case "Following parcels - Tax Map Numbers3":
                                            field.SetValue(" Tax Map Numbers3");
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            //case Field.Type.e_choice:
                            //    Console.WriteLine("Choice");
                            //    break;
                            //case Field.Type.e_signature:
                            //    Console.WriteLine("Signature");
                            //    break;
                        }

                        Console.WriteLine("------------------------------");
                    }

                    // Search for a specific field
                    //Field fld = doc.GetField("employee.name.first");
                    //if (fld != null)
                    //{
                    //    Console.WriteLine("Field search for {0} was successful.", fld.GetName());
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Field search failed.");
                    //}

                    // Regenerate field appearances.
                    doc.RefreshFieldAppearances();
                    doc.Save(output_path + "forms_test_edit.pdf", 0);
                    Console.WriteLine("Done. Results saved to forms_test_edit.pdf");
                    //Console.ReadKey();
                }
            }
            catch (PDFNetException e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
