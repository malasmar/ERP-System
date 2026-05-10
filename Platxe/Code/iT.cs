using System;
using System.Linq;
using CLiCore;

namespace Platxe
{
    public class iT
    {
        public static string T(string Language, string Key, string Default = "No Label")
        {
            string result = "No Label";
            if (Key == "")
                return Default;

            try
            {
                if (Language == null || Language == "" || Language == "en")
                {
                    if (iCore.iLL.Count(x => x.Key == Key) > 0)
                    {
                        result = iCore.iLL.Where(x => x.Key == Key).FirstOrDefault().English;
                    }
                }
                else
                {
                    if (iCore.iLL.Count(x => x.Key == Key) > 0)
                    {
                        result = iCore.iLL.Where(x => x.Key == Key).FirstOrDefault().Arabic;
                    }

                }
                return result ?? "No Label";
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    }
}

