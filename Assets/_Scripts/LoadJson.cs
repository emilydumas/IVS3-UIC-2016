using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Unity.IO.Compression;
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
    public string[]  a { get; set; } // TODO: What will it do if the attribute is a composite like an object?
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


    //this is called when loading a new JSON file.
    public DataPlot LoadFromFile(string path)
    {
        DataPlot dataPlot = new DataPlot();
        var serializer = new JsonSerializer();

        if(path.EndsWith("tpz"))
        {
          using (FileStream rawstream = File.Open(path, FileMode.Open, FileAccess.Read))
            using (GZipStream decstream = new GZipStream(rawstream, CompressionMode.Decompress))
              using (var tr = new JsonTextReader(new StreamReader(decstream)))
              {
                dataPlot = serializer.Deserialize<DataPlot>(tr);
              }
        }
        else if (path.EndsWith("tpc"))
        {
          using (var tr = new JsonTextReader(new StreamReader(path)))
          {
            dataPlot = serializer.Deserialize<DataPlot>(tr);
          }
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ParticlePlot>()._infoText.text = "Please submit TPZ or TPC file format." ;
        }



        return dataPlot;
    }
}
