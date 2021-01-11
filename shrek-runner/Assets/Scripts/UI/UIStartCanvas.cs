using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartCanvas : MonoBehaviour
{
    [SerializeField]
    private GameDataObject data;

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (!data.isGamePlayed && Input.GetMouseButtonDown(0))
        {
            canvas.enabled = false;
            data.StartGame();
        }
    }
}