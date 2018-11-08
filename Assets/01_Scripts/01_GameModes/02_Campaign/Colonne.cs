using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class Colonne {

    [XmlAttribute("nColonne")]
    public int nColonne;

    public int BitDefaut;

    public int Porte;
}
