using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MinitelToArduino
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Convert .vdt Minitel file to c++ header file.");

            /*foreach(string file in Directory.GetFiles(".","*.mie", SearchOption.AllDirectories))
            {
                Console.WriteLine(file);
                string hpp = file.Replace(".mie", ".hpp");
                if(File.Exists(hpp))
                {
                    continue;
                }
                using (StreamReader reader = File.OpenText(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                    int cpt = 0;
                    foreach(var x in o)
                    {
                        foreach (var t in x.Value)
                        {
                            if (t["data"].HasValues)
                            {
                                Console.WriteLine("{0}/ {1} = {2}", cpt++, t["type"], t["data"]["miedit-value"]);
                            }
                            else
                            {
                                Console.WriteLine("{0}/ {1}", cpt++, t["type"]);
                            }
                        }
                    }
                    Console.WriteLine("cpt={0}", cpt);
                }
            }*/

            foreach (string file in Directory.GetFiles(".", "*.vdt", SearchOption.AllDirectories))
            {
                int posPoint = file.LastIndexOf(".");
                int posStart = file.LastIndexOf("\\");
                if (posStart < 0)
                    posStart = file.LastIndexOf("/");
                string name = file.Substring(posStart + 1, posPoint - posStart - 1);

                Console.Write("Working on file: {0}", name);
                string hpp = file.Replace(".vdt", ".hpp");
                if (File.Exists(hpp))
                {
                    //continue;
                }
                byte[] bytes = File.ReadAllBytes(file);
                var x = File.CreateText(hpp);
                StringBuilder hex = new StringBuilder(bytes.Length * 6);
                bool isFirst = true;
                foreach (byte b in bytes)
                {
                    if(isFirst)
                    {
                        isFirst = !isFirst;
                        hex.AppendFormat("0x{0:x2}", b);
                    } else {
                        hex.AppendFormat(", 0x{0:x2}", b);
                    }
                }
                string result = "#define " + name.ToUpper() + "_SIZE " + bytes.Length.ToString() + "\r\nconst PROGMEM uint8_t " + name + "[] = {" + hex.ToString() + "};";
                x.Write(result);
                x.Close();
                Console.WriteLine(" DONE");
            }

            Console.WriteLine("Press a key to exit.");
            Console.ReadKey();
        }
    }
}