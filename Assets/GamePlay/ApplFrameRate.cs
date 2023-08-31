using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplFrameRate : MonoBehaviour //改帧数上限
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }
}
