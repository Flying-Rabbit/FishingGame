using UnityEngine;
using UnityEngine.UI;

public class Ef_SwithSprite : MonoBehaviour
{
    public Sprite[] loadings;
    private int index;
    private int count;
    private Image img;  

    private void Start()
    {
        index = 0;
        count = loadings.Length;
        img = GetComponent<Image>();
        img.sprite = loadings[index];
        InvokeRepeating("ChangeTexture", 0.1f, 0.1f);
    }

    void ChangeTexture()
    {
        index++;
        if (index >= count)
        {
            index = 0;
        }
        img.sprite = loadings[index];
    }
}
