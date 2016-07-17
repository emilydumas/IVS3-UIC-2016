using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace tpzcat
{
    public class Program : MonoBehaviour
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: tpzcat.exe INFILE\nINFILE should be a TPZ file with attributes including \"length\" and \"word\".");
                Environment.Exit(1);
            }

            // rawstream is an object from which the compressed data can be read
            // decstream is an object that can decompress the data from rawstream
            // textreader presents the decompressed data as a file-like object
            // reader interprets the text as json

            // I am not sure why all four layers are needed; in other languages
            // there would be only two!

            using (FileStream rawstream = File.Open(args[0], FileMode.Open, FileAccess.Read))
                using (GZipStream decstream = new GZipStream(rawstream, CompressionMode.Decompress))
                    using (StreamReader textreader = new StreamReader(decstream))
                        using (JsonTextReader reader = new JsonTextReader(textreader))
                        {
                            // Convert the entire JSON file to a hierarchy of C# objects
                            // and make "obj" the root
                            JObject obj = (JObject)JToken.ReadFrom(reader);

                            // Retrieve the attribute names as a list of strings
                            string[] attrs = obj["attributes"].Values<string>().ToArray();

                            // Iterate over points
                            foreach (JObject pt in obj["points"]) {
                                // Make a dictionary out of the attributes of this point
                                var attrdict = Enumerable.Range(0, attrs.Length).ToDictionary(i => attrs[i], i => pt["a"][i]);
                                // Convert the "v" (=vector) field to an array of doubles
                                var vec = pt["v"].Values<double>().ToArray();

                                // Output
                                Console.WriteLine("Word {0} has length {1} and vector [{2}]",attrdict["word"],attrdict["length"],", ",vec.ToString());
                            }
                        }

        }
    }
}
