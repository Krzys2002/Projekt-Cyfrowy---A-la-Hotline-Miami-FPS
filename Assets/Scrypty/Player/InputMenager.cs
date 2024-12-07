using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-100)]
public class InputMenager : MonoBehaviour
{
    public static InputMenager inputMenager;
    
    PlayerInput playerInput;
    PlayerInput.OnFootActions onFoot;

    public UnityAction<Vector2> movmentInput;
    public UnityAction pauseAction;
    public UnityAction interationAction;
    public UnityAction reloadAction;

    public void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.onFoot;
        inputMenager = this;
    }

    public void Update()
    {
        if (onFoot.PauseMenu.WasPressedThisFrame())
        {
            pauseAction?.Invoke();
        }
        
        if (onFoot.Interation.WasPressedThisFrame())
        {
            interationAction?.Invoke();
        }
        
        if (onFoot.Reload.WasPressedThisFrame())
        {
            reloadAction?.Invoke();
        }
    }

    public void FixedUpdate()
    {
        movmentInput?.Invoke(onFoot.Movment.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
