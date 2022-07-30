using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResourceHoverPopupTemplate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string resourceName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        ResourceHoverPopup._instance.setAndShowPopup(this.resourceName);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        ResourceHoverPopup._instance.hidePopup();
    }

    public void setResourceName(string resourceName)
    {
        this.resourceName = resourceName;
    }
}
