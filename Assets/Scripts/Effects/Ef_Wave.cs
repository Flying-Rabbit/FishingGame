using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_Wave : MonoBehaviour
{
    public Texture2D[] waves;
    private int index;
    private int count;
    private Material mat;

    private void Start()
    {
        index = 0;
        count = waves.Length;
        mat = GetComponent<Renderer>().material;
        mat.mainTexture = waves[index];
        InvokeRepeating("ChangeTexture", 0.1f, 0.1f);
    }

    void ChangeTexture()
    {
        index++;
        if (index >= count)
        {
            index = 0;
        }
        mat.mainTexture = waves[index];
    }


}
