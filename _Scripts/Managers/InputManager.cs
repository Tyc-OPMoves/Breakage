using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameStateManager;

public class InputManager : StaticInstance<InputManager>
{
    [SerializeField]MSCameraController _cameraController;
    bool _isCursorVisible;
    private void Start()
    {
        _cameraController.enabled = false;
    }
    private void Update()
    {
        AutoRun();
        ToggleCursorVisibility();
        OpenShopSpaceKey();
        RerollKey();
        OpenHandEKey();
    }

    void AutoRun()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameStateManager.Instance.AutoRun(true);
            GameStateManager.Instance.UpdateGameState(GameState.AllySpawning);
        }
    }

    void ToggleCursorVisibility()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(Cursor.visible)
            {
                _cameraController.enabled = true;
                Cursor.lockState= CursorLockMode.Confined;
                Cursor.visible = false;
            }
            else
            {
                _cameraController.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    
    void OpenShopSpaceKey()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            GUIManager.Instance.ToggleShop();
        }
    }

    void RerollKey()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            GUIManager.Instance.Reroll();
        }
    }

    void OpenHandEKey()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GUIManager.Instance.ToggleBottomBar();
        }
    }
}
