using Python.Runtime;

namespace WpfTemplateStudio.Core.Services
{
    public static class BackgroundRemoverService
    {
        public enum DeepLearningModel
        {
            u2net,
            u2netp,
            u2net_human_seg,
            silueta,
            isnet_general_use
        }

        public static byte[] RemoveBackground(string inputPath)
        {
            return RemoveBackground(inputPath, null, null);
        }

        public static byte[] RemoveBackground(string inputPath, string outputPath)
        {
            return RemoveBackground(inputPath, outputPath, null);
        }

        public static byte[] RemoveBackground(string inputPath, DeepLearningModel? model)
        {
            return RemoveBackground(inputPath, null, model);
        }

        /// <summary>
        /// Removes the background from an image.
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static byte[] RemoveBackground(string inputPath, string outputPath, DeepLearningModel? model)
        {
            if(String.IsNullOrEmpty(inputPath))
            {
                return null;
            }

            PythonInitializerService.Instance.Initialize();
            try
            {
                using (Py.GIL())
                {
                    dynamic rembg = Py.Import("rembg");
                    dynamic remove = rembg.remove;
                    dynamic new_session = rembg.new_session;

                    byte[] inputBytes = File.ReadAllBytes(inputPath);
                    byte[] outputBytes;

                    if (model == null)
                    {
                        outputBytes = (byte[])rembg.remove(inputBytes);
                    }
                    else
                    {
                        dynamic newSession = new_session(model.ToString());
                        outputBytes = (byte[])rembg.remove(inputBytes, session: newSession);
                    }

                    if (!String.IsNullOrEmpty(outputPath))
                    {
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
