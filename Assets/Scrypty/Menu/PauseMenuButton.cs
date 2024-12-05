using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;

public class PauseMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Color hoverColor = Color.red; // Color to change to when hovered
    private Color originalColor; // Original color of the text
    public TextMeshProUGUI textComponent;

    void Awake()
    {
        if (textComponent != null)
        {
            originalColor = textComponent.color;
        }
        else
        {
            Debug.LogError("Text component not found.");
        }
    }

    private void OnEnable()
    {
        textComponent.color = originalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textComponent != null)
        {
            textComponent.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textComponent != null)
        {
            textComponent.color = originalColor;
        }
    }
}

