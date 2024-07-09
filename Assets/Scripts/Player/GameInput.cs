using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private const string GAME_INPUT_BINDINGS = "GameInputBindings";

    private GameControl gameControl;

    public event EventHandler OnLeftButtonAction;

    public enum BindingType
    {
        Up,
        Down,
        Left,
        Right,
        Scroll,
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        gameControl = new GameControl();
        if (PlayerPrefs.HasKey(GAME_INPUT_BINDINGS))
        {
            // 重新绑定信息
            gameControl.LoadBindingOverridesFromJson(PlayerPrefs.GetString(GAME_INPUT_BINDINGS));
        }
        gameControl.Camera.Enable();

    }

    public void Start()
    {
        gameControl.Camera.LeftButton.performed += LeftButton_Performed;
    }

    private void LeftButton_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => OnLeftButtonAction?.Invoke(this, EventArgs.Empty);

    private void OnDestroy()
    {
        gameControl.Dispose();
    }


    public Vector3 GetMovementDirectionNormalized()
    {
        // 获得对应actions值gameControl.[mapName].[actionsName].Enable()
        Vector2 inputVector2 = gameControl.Camera.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(inputVector2.x, 0, inputVector2.y);

        direction = direction.normalized;

        return direction;
    }

    public Vector3 GetScrollDirectionNormalized()
    {
        // 从输入操作中读取鼠标滚轮的输入值
        float scrollInput = -gameControl.Camera.Scroll.ReadValue<float>();

        Vector3 direction = new Vector3(0, scrollInput, 0);

        return direction;
    }


    public void ReBinding(BindingType bindingType, Action onComplete)
    {
        InputAction inputAction = null;
        int index = -1;
        switch (bindingType)
        {
            case BindingType.Up:
                index = 1;
                inputAction = gameControl.Camera.Move;
                break;
            case BindingType.Down:
                index = 2;
                inputAction = gameControl.Camera.Move;
                break;
            case BindingType.Left:
                index = 3;
                inputAction = gameControl.Camera.Move;
                break;
            case BindingType.Right:
                index = 4;
                inputAction = gameControl.Camera.Move;
                break;
            case BindingType.Scroll:
                index = 0;
                inputAction = gameControl.Camera.Scroll;
                break;
            default:
                break;
        }

        gameControl.Camera.Disable();
        inputAction.PerformInteractiveRebinding(index).OnComplete(callback =>
        {
            callback.Dispose();
            gameControl.Camera.Enable();
            onComplete?.Invoke();

            // gameControl.SaveBindingOverridesAsJson(); 获得绑定的JSON数据（不会保存）
            PlayerPrefs.SetString(GAME_INPUT_BINDINGS, gameControl.SaveBindingOverridesAsJson());
            // 手动保存（不手动也一样会自动保存，但是只有当数量达到一定值以后才会自动保存）
            PlayerPrefs.Save();
        }).Start();
    }

    public string GetBindingDisplayString(BindingType bindingType)
    {
        switch (bindingType)
        {
            case BindingType.Up:
                return gameControl.Camera.Move.bindings[1].ToDisplayString();
            case BindingType.Down:
                return gameControl.Camera.Move.bindings[2].ToDisplayString();
            case BindingType.Left:
                return gameControl.Camera.Move.bindings[3].ToDisplayString();
            case BindingType.Right:
                return gameControl.Camera.Move.bindings[4].ToDisplayString();
            case BindingType.Scroll:
                return gameControl.Camera.Scroll.bindings[0].ToDisplayString();
            default:
                return string.Empty;
        }
    }
}