using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;

    public int click;
    public bool inGameBtn;

    public List<TabButton> anotherInGameBtn;

    public Transform topPos;
    public Transform bottomPos;
    public Transform pagePos;
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);

        if (inGameBtn)
        {
            click += 1;
            if (click == 1)
            {
                for (int i = 0; i < anotherInGameBtn.Count; i++)
                {
                    anotherInGameBtn[i].click = 0;
                }
                pagePos.position = topPos.position;
            }
            if (click == 2)
            {
                for (int i = 0; i < anotherInGameBtn.Count; i++)
                {
                    anotherInGameBtn[i].click = 0;
                }
                pagePos.position = bottomPos.position;
                click = 0;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    private void Start()
    {
        tabGroup.Subscribe(this);
    }
}
