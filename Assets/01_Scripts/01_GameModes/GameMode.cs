using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public static string nameGame;

    public static List<QCS.Gate> gates;
    public static List<QCS.Gate> customGates;
    public static List<QCS.Circuit> circuits;

    public static QCS.Circuit BaseCircuit()
    {
        return new QCS.Circuit(2, 2);
    }
}
