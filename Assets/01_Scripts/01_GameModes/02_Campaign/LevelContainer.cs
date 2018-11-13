using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.IO;
using System.Xml;

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
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            var serializer = new XmlSerializer(typeof(LevelContainer));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as LevelContainer;
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            var www = new WWW(path);
            while (!www.isDone) { } // Wait for the reader to finish reading

            if (!string.IsNullOrEmpty(www.error))
                Debug.LogError("Can't read");
            
            XmlSerializer serializer = new XmlSerializer(typeof(LevelContainer));
            MemoryStream stream = new MemoryStream(www.bytes);

            LevelContainer levelContainer = new LevelContainer();
            if (stream != null)
                levelContainer = serializer.Deserialize(stream) as LevelContainer;

            stream.Close();
            return levelContainer;
        }
        return new LevelContainer();

    }

}
