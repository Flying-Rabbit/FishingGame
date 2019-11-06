using TMPro;
using UnityEngine;

public class Prompt : MonoBehaviour
{
    private TextMeshProUGUI txtLevel;

    private void Start()
    {
        txtLevel = transform.Find("txtLevel").GetComponent<TextMeshProUGUI>();
        HideSelf();
    }

    public void ShowLevel(int level)
    {
        txtLevel.text = level.ToString();
        Invoke("HideSelf", 0.5f);
    }

    void HideSelf()
    {
        gameObject.SetActive(false);
    }
}
