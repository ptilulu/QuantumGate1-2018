using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class Level {

    [XmlAttribute("id")]
    public int id;

    [XmlAttribute("name")]
    public string name;

    public int NbColonnes;

    public int ColonneHeight;

    public string Resultats;

    [XmlArray("Colonnes")]
    [XmlArrayItem("Colonne")]
    public List<Colonne> colonnes = new List<Colonne>();
    
}
