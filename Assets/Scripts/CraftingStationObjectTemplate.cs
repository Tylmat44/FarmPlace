using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingStationObjectTemplate : MonoBehaviour
{
    public TextMeshProUGUI craftingStationName;
    public TextMeshProUGUI recipieName;
    public Text quantity;
    public CraftingStation craftingStation;
    public CraftableResource craftableResource;
    private UseableBuilding useableBuilding;
    public GameObject buttonTemplate;
    public Button craftingStationButton;
    private int slot;

    public UseableBuilding UseableBuilding { get => useableBuilding; set => useableBuilding = value; }
    public int Slot { get => slot; set => slot = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (craftingStation != null)
        {
            craftingStationName.text = craftingStation.Name;
        } else
        {
            craftingStationName.text = "None";
        }

        if (craftableResource != null)
        {
            recipieName.text = craftableResource.Name;
        } else
        {
            recipieName.text = "None";
        }

        quantity.text = "0";

        craftingStationButton.onClick.AddListener(openCraftingStationCatalog);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void openCraftingStationCatalog()
    {
        foreach (GameObject gameObject in FarmScene.catalogButtons)
        {
            Destroy(gameObject);
        }
        foreach (CraftingType type in this.UseableBuilding.CraftingTypes)
        {
            this.addTypeToCatalog(type);
        }
    }

    public void setCraftingStation(CraftingStation craftingStation, int slot, UseableBuilding useableBuilding)
    {
        this.craftingStation = craftingStation;
        this.craftingStationName.text = this.craftingStation.Name;
        if (this.craftingStation.CraftableResource != null)
        {
            this.craftableResource = this.craftingStation.CraftableResource;
            this.recipieName.text = this.craftableResource.Name;
        }
        this.quantity.text = this.craftingStation.QuantityLeft.ToString();
        this.Slot = slot;
        this.UseableBuilding = useableBuilding;
    }

    private void addTypeToCatalog(CraftingType type)
    {
        switch (type)
        {
            case CraftingType.food:
                foreach (string crafting_station_food in DataManager.getListOfResourceType(ResourceType.crafting_station_food))
                {
                    GameObject button = Instantiate(buttonTemplate);
                    button.SetActive(true);

                    button.GetComponent<SeedListButton>().seedText.text = DataManager.resourceDB[crafting_station_food].Name;
                    button.GetComponent<SeedListButton>().seedCount.text = DataManager.resourceDB[crafting_station_food].BuyPrice.ToString();
                    button.GetComponent<SeedListButton>().SeedKey = crafting_station_food;
                    button.GetComponent<SeedListButton>().IsCraftingStation = true;
                    button.GetComponent<SeedListButton>().UseableBuilding = this.useableBuilding;
                    button.GetComponent<SeedListButton>().Slot = this.Slot;
                    button.transform.SetParent(buttonTemplate.transform.parent, false);
                    FarmScene.catalogButtons.Add(button);
                }
                break;
            case CraftingType.alcohol:
                foreach (string crafting_staion_alcohol in DataManager.getListOfResourceType(ResourceType.crafting_staion_alcohol))
                {
                    GameObject button = Instantiate(buttonTemplate);
                    button.SetActive(true);

                    button.GetComponent<SeedListButton>().seedText.text = DataManager.resourceDB[crafting_staion_alcohol].Name;
                    button.GetComponent<SeedListButton>().seedCount.text = DataManager.resourceDB[crafting_staion_alcohol].BuyPrice.ToString();
                    button.GetComponent<SeedListButton>().SeedKey = crafting_staion_alcohol;
                    button.GetComponent<SeedListButton>().IsCraftingStation = true;
                    button.GetComponent<SeedListButton>().UseableBuilding = this.useableBuilding;
                    button.GetComponent<SeedListButton>().Slot = this.Slot;
                    button.transform.SetParent(buttonTemplate.transform.parent, false);
                    FarmScene.catalogButtons.Add(button);
                }
                break;
            default:
                break;
        }
    }
}


