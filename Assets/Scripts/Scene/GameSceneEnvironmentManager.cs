using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneEnvironmentManager : MonoBehaviour
{
    [SerializeField] private GameEvent onGameStarted;
    [SerializeField] private GameObject lights;
    [SerializeField] private GameObject ui;
    private DateTime startTime;
    private bool isRunning = true;

    void Awake()
    {
        if (!Application.isMobilePlatform)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Start()
    {
        startTime = DateTime.Now;
    }

    void Update()
    {
        if (!isRunning) return;

        if ((DateTime.Now - startTime).TotalSeconds >= 5)
        {
            ui.SetActive(false);
            lights.SetActive(false);
            isRunning = false;
            onGameStarted.Raise(this, null);
        }
    }
}
