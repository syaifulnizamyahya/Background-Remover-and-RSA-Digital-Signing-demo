using Newtonsoft.Json;
using Python.Runtime;
using System.IO;
using System.Text;

namespace WpfTemplateStudio.Core.Services
{
    public static class BackgroundRemoverService
    {
        public static byte[] RemoveBackground(string inputPath)
        {
            return RemoveBackground(inputPath, null);
        }

        public static byte[] RemoveBackground(string inputPath, string outputPath)
        {
            //input_path = 'D:\\Dev\\bestinet\\BackgroundRemoverAi\\ConsoleRembg\\Input\\000272_4_000272_MALE_25.jpg'
            //output_path = 'D:\\Dev\\bestinet\\BackgroundRemoverAi\\ConsoleRembg\\Output\\000272_4_000272_MALE_25_output.png'

            //with open(input_path, 'rb') as i:
            //    with open(output_path, 'wb') as o:
            //        input = i.read()

            //        output = remove(input)
            //        o.write(output)
            //#######################################################

            PythonInitializerService.Instance.Initialize();
            try
            {
                using (Py.GIL())
                {
                    Console.WriteLine("Importing rembg module...");
                    dynamic rembg = Py.Import("rembg");
                    dynamic remove = rembg.remove;

                    Console.WriteLine("Reading input image...");
                    byte[] inputBytes = File.ReadAllBytes(inputPath);

                    Console.WriteLine("Calling rembg.remove function...");
                    byte[] outputBytes = remove(inputBytes);

                    if (outputPath != null)
                    {
                        Console.WriteLine("Writing output image...");
                        File.WriteAllBytes(outputPath, outputBytes);
                    }

                    return outputBytes;
                }
            }
            catch (Exception)
            {
                throw new Exception(WpfTemplateStudio.Core.Properties.Resources.BackgroundRemoverService_RemoveBackground_FailedToRemoveBackgroud);
            }
        }
    }
}
