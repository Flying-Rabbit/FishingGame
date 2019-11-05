using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScr : MonoBehaviour
{
    public Vector3 dir;

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            transform.right = dir;
            Debug.Log("transform.right: " + transform.right);
        }
    }
}
