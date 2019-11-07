using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    private Button btnNewGame;
    private Button btnGoOnGame;
    private GameObject loadingCanvas;

    void Start()
    {
        btnNewGame = transform.Find("btnNewGame").GetComponent<Button>();
        btnGoOnGame = transform.Find("btnGoOnGame").GetComponent<Button>();
        btnNewGame.onClick.AddListener(OnClickNewGame);
        btnGoOnGame.onClick.AddListener(OnClickGoOnGame);
        loadingCanvas = GameObject.Find("LoadingCanvas");
        loadingCanvas.SetActive(false);
    }

    void OnClickNewGame()
    {
        InitData();
        StartCoroutine(LoadNewScene());
    }

    void OnClickGoOnGame()
    {
        StartCoroutine(LoadNewScene());
    }

    IEnumerator LoadNewScene()
    {
        yield return null;
        loadingCanvas.SetActive(true);
        AsyncOperation op = SceneManager.LoadSceneAsync(1);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            yield return new WaitForSeconds(0.5f);
          
            if (op.progress >= 0.899f)
            {
                break;
            }
        }
        op.allowSceneActivation = true;
    }

    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    void InitData()
    {
        PlayerPrefs.SetInt("LEVEL", 0);
        PlayerPrefs.SetInt("EXP", 0);
        PlayerPrefs.SetInt("Gold", 10000);
        PlayerPrefs.SetInt("AUDIO", 1);
    }
}
