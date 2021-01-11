using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIWinCanvas : MonoBehaviour
{
    [SerializeField]
    private GameDataObject data;

    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        StartCoroutine(EnableCanvas());
    }

    IEnumerator EnableCanvas()
    {
        yield return new WaitUntil(() => data.isGameWin == true);
        canvas.enabled = true;

        StartCoroutine(ReloadLevel());
    }

    IEnumerator ReloadLevel()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        SceneManager.LoadScene("SampleScene");
    }
}