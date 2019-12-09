using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YValue : MonoBehaviour
{
    public float yValue;
    public static YValue ins;
    // Start is called before the first frame update
    void Awake()
    {
        ins = this;
    }
}
