using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    private Toggle togAudio;
    private Button btnClose;

    private void Start()
    {
        togAudio = transform.Find("togAudio").GetComponent<Toggle>();
        btnClose = transform.Find("btnClose").GetComponent<Button>();
        togAudio.isOn = GameManager.Instance.GameData.AudioIsOn;
        togAudio.onValueChanged.AddListener(OnValueChanged);
        btnClose.onClick.AddListener(() => gameObject.SetActive(false));       
    }

    void OnValueChanged(bool ison)
    {
        togAudio.isOn = ison;
        GameManager.Instance.SetAudio(ison);
    }

}
