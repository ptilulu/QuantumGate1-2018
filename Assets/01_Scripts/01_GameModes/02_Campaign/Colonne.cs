using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

/*public class Porte
{
    public int Porte;
}*/

public class Colonne {

    [XmlAttribute("nColonne")]
    public int nColonne;

    public int BitDefaut;

    [XmlArray("Portes"), XmlArrayItem("Porte")]
    public List<int> Portes;
}
