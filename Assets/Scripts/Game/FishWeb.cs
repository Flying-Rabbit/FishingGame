using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishWeb : MonoBehaviour
{
    public int Damage { get; set; }

    private void Start()
    {
        Invoke("SelfDestroy", 2f);
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
