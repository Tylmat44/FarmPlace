using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingStationObjectTemplate : MonoBehaviour
{
    public TextMeshProUGUI craftingStationName;
    public TextMeshProUGUI recipieName;
    public TMP_InputField quantity;
    public CraftingStation craftingStation;
    public CraftableResource craftableResource;
    private UseableBuilding useableBuilding;
    public GameObject buttonTemplate;
    public Button craftingStationButton;
    public static List<GameObject> recipieButtons;
    public GameObject recipieButton;
    public GameObject currentCraftButton;
    private int slot;
    public int oldCraftingNumber = 0;

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

        quantity.text  = "0";

        craftingStationButton.onClick.AddListener(openCraftingStationCatalog);

        recipieButtons = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.oldCraftingNumber != this.craftingStation.QuantityLeft)
        {
            this.oldCraftingNumber = this.craftingStation.QuantityLeft;
            this.quantity.text = this.craftingStation.QuantityLeft.ToString();
        }
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

    public void openRecipieSelection()
    {
        foreach (GameObject gameObject in recipieButtons)
        {
            Destroy(gameObject);
        }

        switch (this.craftingStation.Type)
        {
            case CraftingType.food:
                foreach (string food in DataManager.getListOfResourceType(ResourceType.food))
                {
                    GameObject button = Instantiate(recipieButton);
                    button.SetActive(true);

                    button.GetComponent<RecipieButtonTemplate>().craftableResourceText.text = DataManager.resourceDB[food].Name;
                    button.GetComponent<RecipieButtonTemplate>().craftableResource = DataManager.craftableResourceDB[food].Copy();
                    button.GetComponent<RecipieButtonTemplate>().craftingStation = this.craftingStation;
                    button.GetComponent<RecipieButtonTemplate>().craftingStationObjectTemplate = this;
                    button.transform.SetParent(recipieButton.transform.parent, false);
                    recipieButtons.Add(button);
                }
                break;
            case CraftingType.alcohol:
                foreach (string alcohol in DataManager.getListOfResourceType(ResourceType.alcohol))
                {
                    GameObject button = Instantiate(recipieButton);
                    button.SetActive(true);

                    button.GetComponent<RecipieButtonTemplate>().craftableResourceText.text = DataManager.resourceDB[alcohol].Name;
                    button.GetComponent<RecipieButtonTemplate>().craftableResource = DataManager.craftableResourceDB[alcohol].Copy();
                    button.GetComponent<RecipieButtonTemplate>().craftingStation = this.craftingStation;
                    button.GetComponent<RecipieButtonTemplate>().craftingStationObjectTemplate = this;
                    button.transform.SetParent(recipieButton.transform.parent, false);
                    recipieButtons.Add(button);
                }
                break;
            default:
                break;
        }
    }

    public void submitCraftingNumber(string num)
    {
        if (this.craftableResource != null)
        {
            int n = int.Parse(num);
            if (this.craftingStation.IsCrafting)
            {
                if (n < this.craftingStation.QuantityLeft)
                {
                    this.craftingStation.refundResources(this.craftingStation.QuantityLeft - n);
                }
                else if (n > this.craftingStation.QuantityLeft)
                {
                    this.craftingStation.increaseQuantity(n - this.craftingStation.QuantityLeft);
                }
            }
            else
            {
                this.craftingStation.startCrafting(this.craftableResource, n);
            }
        }
    }

    public void setCraftableResource(CraftableResource craftableResource)
    {
        this.craftableResource = craftableResource;
        this.recipieName.text = craftableResource.Name;
        this.currentCraftButton.GetComponent<CurrentCraftButtomTemplate>().craftableResource = craftableResource;
        this.currentCraftButton.GetComponent<CurrentCraftButtomTemplate>().craftingStation = this.craftingStation;
    }

    public void closeRecipieMenu()
    {
        foreach (GameObject gameObject in recipieButtons)
        {
            Destroy(gameObject);
        }
    }

    public void craftingStationButtons(int selection)
    {
        switch (selection)
        {
            case 0:
                this.submitCraftingNumber("0");
                break;
            case 1:
                this.submitCraftingNumber((this.craftingStation.QuantityLeft + 1).ToString());
                break;
            case 2:
                this.submitCraftingNumber((this.craftingStation.QuantityLeft - 1).ToString());
                break;
            default:
                break;
        }
    }
}


