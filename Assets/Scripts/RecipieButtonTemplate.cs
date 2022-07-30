using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipieButtonTemplate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public TextMeshProUGUI craftableResourceText;
    public CraftableResource craftableResource;
    public CraftingStation craftingStation;
    public CraftingStationObjectTemplate craftingStationObjectTemplate;
    public int canCraft;
    public bool hover = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!hover)
        {
            RecipieHover._instance.setAndShowPopup(this.craftableResource, this.craftingStation, false);
            hover = true;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        RecipieHover._instance.hidePopup();
        hover = false;
    }

    public void selectCraftableResource()
    {
        this.craftingStationObjectTemplate.setCraftableResource(this.craftableResource);
        RecipieHover._instance.hidePopup();
        this.hover = false;
        this.craftingStationObjectTemplate.closeRecipieMenu();
    }
}
