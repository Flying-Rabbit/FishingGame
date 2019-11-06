using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI txtGold;
    public TextMeshProUGUI txtLitterTimer;
    public TextMeshProUGUI txtLevel;
    public Image imgExp;   
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtBigTimer;
    public TextMeshProUGUI txtBulletCost;

    public Button btnReward;
    public Button btnBack;
    public Button btnSetting;
    public Button btnBulletLvDown;
    public Button btnBulletLvUp;

    public GameObject imgPanelSetting;
    public GameObject imgPrompt;
    private GameData gameData;

    private void Start()
    {
        GameManager.Instance.UIDataChanged += this.UpdateUI;
        GameManager.Instance.LevelUpEvent += this.LevelUp;
        GameManager.Instance.UIBulletCostChanged += this.SetBulletCost;

        btnBulletLvUp.onClick.AddListener(ClickBulletLvUp);
        btnBulletLvDown.onClick.AddListener(ClickBulletLvDown);
        btnReward.onClick.AddListener(ClickReward);
        btnBack.onClick.AddListener(ClickBack);
        btnSetting.onClick.AddListener(ClickSetting);
    }

    private void OnDestroy()
    {
        GameManager.Instance.UIDataChanged -= this.UpdateUI;
        GameManager.Instance.UIBulletCostChanged -= this.SetBulletCost;
    }

    public void Init()
    {
        gameData = GameManager.Instance.GameData;
        UpdateUI();

        StartCoroutine(LitteTimer());
        StartCoroutine(BigTimer());
    }

    IEnumerator BigTimer()
    {
        int bigTimer = 10;
        txtBigTimer.gameObject.SetActive(true);
        btnReward.gameObject.SetActive(false);
        txtBigTimer.text = bigTimer.ToString();
        yield return null;
        while (bigTimer > 0)
        {
            bigTimer -= 1;
            yield return new WaitForSeconds(1);
            txtBigTimer.text = bigTimer.ToString();
        }
        txtBigTimer.gameObject.SetActive(false);
        btnReward.gameObject.SetActive(true);
    }

    IEnumerator LitteTimer()
    {
        yield return null;
        int litteTimer = 99;
        txtLitterTimer.text = litteTimer / 10 + " " + litteTimer % 10;

        while (true)
        {
            litteTimer -= 1;
            yield return new WaitForSeconds(1);
            txtLitterTimer.text = litteTimer / 10 + " " + litteTimer % 10;
            if (litteTimer < 0)
            {
                GameManager.Instance.AddCoins((GameManager.Instance.GameData.LittleTimerReward));
                litteTimer = 99;
            }
        }        
    }

    void UpdateUI()
    {
        txtLevel.text = gameData.Level.ToString();
        txtGold.text = "$" + gameData.Gold.ToString();
        imgExp.fillAmount = gameData.Exp * 0.01f / (gameData.Level * gameData.Level);
        txtTitle.text = GameManager.Instance.GetTitle();
    }

    private void SetBulletCost(int cost)
    {
        this.txtBulletCost.text = "$" + cost;
    }

    void LevelUp(int level)
    {
        imgPrompt.SetActive(true);
        imgPrompt.GetComponent<Prompt>().ShowLevel(level);
    }


    void ClickReward()
    {
        StartCoroutine(BigTimer());
        GameManager.Instance.AddCoins(GameManager.Instance.GameData.BigTimerReward);
        AudioManager.Instance.PlayAudio(ACName.Gold);
    }

    void ClickBack()
    {
        GameManager.Instance.SaveGame();
        SceneManager.LoadScene(0);
    }

    void ClickSetting()
    {
        imgPanelSetting.SetActive(true);
    }

    void ClickBulletLvUp()
    {
        GameManager.Instance.GunLevelChanged(1);
    }

    void ClickBulletLvDown()
    {
        GameManager.Instance.GunLevelChanged(-1);
    }
}
