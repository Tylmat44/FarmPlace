using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrentCraftButtomTemplate : MonoBehaviour, IPointerEnterHandler
{
    public CraftableResource craftableResource;
    public CraftingStation craftingStation;

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
        RecipieHover._instance.setAndShowPopup(this.craftableResource, this.craftingStation, true);
    }
}
