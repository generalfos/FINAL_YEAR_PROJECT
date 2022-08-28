using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProperties : MonoBehaviour
{
    public enum UnitClass
    {
        Engineer,
        Scientist
    };

    public string Name;
    public UnitClass Class;
}
