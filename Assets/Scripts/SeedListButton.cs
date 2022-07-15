using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeedListButton : MonoBehaviour
{
    public TextMeshProUGUI seedText;
    public TextMeshProUGUI seedCount;
    public TextMeshProUGUI selectedText;
    private string seedKey;
    private bool isCraftingStation;
    private UseableBuilding useableBuilding;
    private int slot;

    public string SeedKey { get => seedKey; set => seedKey = value; }
    public bool IsCraftingStation { get => isCraftingStation; set => isCraftingStation = value; }
    public UseableBuilding UseableBuilding { get => useableBuilding; set => useableBuilding = value; }
    public int Slot { get => slot; set => slot = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectSeed()
    {
        if (!isCraftingStation)
        {
            ToolChest.deselectAll();
            if (FarmScene.catalogTypeOpen == TileableObjectType.crop)
            {
                ToolChest.SeedSelected = DataManager.resourceDB[seedKey];
                selectedText.text = DataManager.resourceDB[seedKey].Name;
            }
            else if (FarmScene.catalogTypeOpen == TileableObjectType.animal)
            {
                ToolChest.AnimalSelected = DataManager.animalDB[seedKey];
                selectedText.text = DataManager.animalDB[seedKey].Name;
            }
            else if (FarmScene.catalogTypeOpen == TileableObjectType.useableBuilding)
            {
                ToolChest.UseableBuildingSelected = DataManager.useableBuildingDB[seedKey];
                selectedText.text = DataManager.useableBuildingDB[seedKey].Name;
            }
        } else
        {
            if (DataManager.craftingStationDB[seedKey].Copy().Cost < AccountManager.Coins)
            {
                StartCoroutine(craftingStationDialogue(DataManager.craftingStationDB[seedKey].Copy(), this.slot));
            }
        }

    }

    public IEnumerator craftingStationDialogue(CraftingStation craftingStation, int slot)
    {
        CraftingStationConfirmationBox._instance.Name = craftingStation.Name;

        yield return StartCoroutine(CraftingStationConfirmationBox._instance.Dialog());

        if (CraftingStationConfirmationBox.selection)
        {
            this.UseableBuilding.purchaseCraftingStation(craftingStation, slot);
        }

    }
}
