using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class Level {

    [XmlAttribute("id")]
    public int id;

    public int NbColonnes;

    public int ColonneHeight;

    [XmlArray("Colonnes")]
    [XmlArrayItem("Colonne")]
    public List<Colonne> colonnes = new List<Colonne>();
    
}
