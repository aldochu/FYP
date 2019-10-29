using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTransparancy : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Color temp;

    void Start()
    {
        temp = GetComponent<Image>().color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        temp.a = 0.80f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        temp.a = 0.10f;
    }
}
