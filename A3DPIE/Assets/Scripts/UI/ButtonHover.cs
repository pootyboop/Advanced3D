using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ButtonHover : MonoBehaviour
{
    TMPro.TextMeshProUGUI text;
    public Color hoverColor = new Color(1.0f, 1.0f, 1.0f);
    Color originalColor;

    bool hovered = false;



    void Start()
    {
        text = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        originalColor = text.color;
    }



    private void OnDisable() {
        if (hovered)
            OnHoverStop();
    }



    public void OnHoverStart() {
        hovered = true;

        text.color = hoverColor;
    }



    public void OnHoverStop() {
        hovered = false;

        text.color = originalColor;
    }
}
