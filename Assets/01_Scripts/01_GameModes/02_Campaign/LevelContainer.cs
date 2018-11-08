using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.IO;

[XmlRoot("LevelCollection")]
public class LevelContainer {

    [XmlArray("Levels")]

    [XmlArrayItem("Level")]
    public List<Level> Levels = new List<Level>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static LevelContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(LevelContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as LevelContainer;
        }
    }

}
