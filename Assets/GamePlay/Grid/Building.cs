using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building",menuName = "Build")]
public class Building : ScriptableObject
{
    public GameObject prefab;
    public int width;
    public int height;
}
