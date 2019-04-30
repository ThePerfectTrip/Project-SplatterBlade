using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeLabelColor : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image hoverIcon;
    Button button;
    Text label;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        label = GetComponentInChildren<Text>();
	}

    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Do something.
        hoverIcon.enabled = true;
        label.color = button.colors.highlightedColor;
    }
    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        // Do something.
        label.color = button.colors.pressedColor;
    }

    // When highlighted with mouse.
    public void OnPointerExit(PointerEventData eventData)
    {
        // Do something.
        hoverIcon.enabled = false;
        label.color = button.colors.normalColor;
    }
}
