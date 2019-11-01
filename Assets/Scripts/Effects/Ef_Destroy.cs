using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_Destroy : MonoBehaviour
{
    private void Start()
    {
        Invoke("SelfDestroy", 0.3f);
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
