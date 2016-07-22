using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;




public class DataPlot
{
    public string[] attributes { get; set; }
    public Point[] points { get; set; }
}

public class Point
{
    public object[]  a { get; set; }
    public string[] t { get; set; }
    public float[] v { get; set; }
}


public class LoadJson
{
    private LoadJson(){}

    private static LoadJson _instance;

    public static LoadJson Instance
    {
        get { return _instance ?? (_instance = new LoadJson()); }
    }

    // this is called during initialization on ParticlePlots. It converts a JSON
    // string to a collection of DataPlot objects.
    public DataPlot Load(string tpzstring)
    {
        DataPlot dataPlot = new DataPlot();

        try
        {
            dataPlot = JsonConvert.DeserializeObject<DataPlot>(tpzstring);
            }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        return dataPlot;
    }

    //this is called when loading a new JSON file.
    public DataPlot LoadFromFile(string path)
    {
        DataPlot dataPlot = new DataPlot();
        GameObject.FindGameObjectWithTag("Finish").GetComponent<Text>().text = "Loading " + path;
        StreamReader _reader = null;
        try
        {
          using (FileStream rawstream = File.Open(path, FileMode.Open, FileAccess.Read))
              using (GZipStream decstream = new GZipStream(rawstream, CompressionMode.Decompress))
              // figure out whether it's working.
              // YES: textreader = new StreamReader(decstream))
              // NO: textreader = new StreamReader(args[0])
                  using (StreamReader reader = new StreamReader(decstream))
                      {
                          dataPlot = Load(reader.ReadToEnd());
                      }

        }
        catch (Exception e)
        {
            GameObject.FindGameObjectWithTag("Finish").GetComponent<Text>().text = e.Message;
        }
        finally
        {
            if (_reader != null)
                _reader.Dispose();
        }
        return dataPlot;
    }
}
