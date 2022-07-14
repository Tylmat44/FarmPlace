using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmScene : MonoBehaviour
{
    public TextMeshProUGUI selectionText;
    public GameObject seedSelectionPanel;
    public GameObject buttonTemplate;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public Slider xpBar;
    public TextMeshProUGUI coinsText;
    public static List<GameObject> catalogButtons = new List<GameObject>();
    public static TileableObjectType catalogTypeOpen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AccountManager.calculateLevel();
        this.levelText.text = AccountManager.Level.ToString();
        this.xpText.text = (DataManager.xpTable[(int)AccountManager.Level + 1] - (DataManager.xpTable[(int)AccountManager.Level + 1] - AccountManager.Xp)) + " / " + (DataManager.xpTable[(int)AccountManager.Level + 1] - DataManager.xpTable[(int)AccountManager.Level]);
        this.xpBar.minValue = (float)(DataManager.xpTable[(int)AccountManager.Level] - (DataManager.xpTable[(int)AccountManager.Level - 1]));
        this.xpBar.maxValue = (float)(DataManager.xpTable[(int)AccountManager.Level + 1] - DataManager.xpTable[(int)AccountManager.Level]);
        this.xpBar.value = AccountManager.Xp;
        this.coinsText.text = string.Format("{0:n0}", AccountManager.Coins);

    }

    public void selectTool(int selection)
    {
        ToolChest.deselectAll();
        switch (selection)
        {
            case 0:
                ToolChest.PlowSelected = true;
                selectionText.text = "Plow";
                break;
            case 1:
                ToolChest.ClearSelected = true;
                selectionText.text = "Clear";
                break;
            case 2:
                selectSeed();
                break;
            case 3:
                ToolChest.deselectAll();
                break;
            default:
                break;
        }

    }

    public void selectSeed()
    {
        seedSelectionPanel.SetActive(true);
        foreach (string item in Inventory.getListOfResourceType(ResourceType.seed))
        {
            GameObject button = Instantiate(buttonTemplate);
            button.SetActive(true);

            button.GetComponent<SeedListButton>().seedText.text = DataManager.resourceDB[item].Name;
            button.GetComponent<SeedListButton>().seedCount.text = Inventory.inventory[item].ToString();
            button.GetComponent<SeedListButton>().SeedKey = item;
            button.transform.SetParent(buttonTemplate.transform.parent, false);
        }
    }

    public void gainXp()
    {
        Inventory.addItem("WHEAT_SEEDS", 10);
        Inventory.addItem("ONION_SEEDS", 10);
        Inventory.addItem("CARROT_SEEDS", 10);
        Inventory.addItem("TOMATO_SEEDS", 10);
        Inventory.addItem("POTATO_SEEDS", 10);
        Inventory.addItem("CORN_SEEDS", 10);
        AccountManager.Coins += 1000;
    }

    public void changeCatalog(int selection)
    {
        foreach (GameObject gameObject in catalogButtons)
        {
            Destroy(gameObject);
        }

        catalogButtons = new List<GameObject>();

        switch(selection)
        {
            case 0:
                foreach (string seed in DataManager.getListOfResourceType(ResourceType.seed))
                {
                    GameObject button = Instantiate(buttonTemplate);
                    button.SetActive(true);

                    button.GetComponent<SeedListButton>().seedText.text = DataManager.resourceDB[seed].Name;
                    button.GetComponent<SeedListButton>().seedCount.text = DataManager.resourceDB[seed].BuyPrice.ToString();
                    button.GetComponent<SeedListButton>().SeedKey = seed;
                    button.transform.SetParent(buttonTemplate.transform.parent, false);
                    catalogButtons.Add(button);
                }
                catalogTypeOpen = TileableObjectType.crop;
                break;
            case 1:
                foreach (string animal in DataManager.getListOfResourceType(ResourceType.baby))
                {
                    GameObject button = Instantiate(buttonTemplate);
                    button.SetActive(true);

                    button.GetComponent<SeedListButton>().seedText.text = DataManager.resourceDB[animal].Name;
                    button.GetComponent<SeedListButton>().seedCount.text = DataManager.resourceDB[animal].BuyPrice.ToString();
                    button.GetComponent<SeedListButton>().SeedKey = animal;
                    button.transform.SetParent(buttonTemplate.transform.parent, false);
                    catalogButtons.Add(button);
                }
                catalogTypeOpen = TileableObjectType.animal;
                break;
            case 2:
                foreach (string useableBuilding in DataManager.getListOfResourceType(ResourceType.useableBuilding))
                {
                    GameObject button = Instantiate(buttonTemplate);
                    button.SetActive(true);

                    button.GetComponent<SeedListButton>().seedText.text = DataManager.resourceDB[useableBuilding].Name;
                    button.GetComponent<SeedListButton>().seedCount.text = DataManager.resourceDB[useableBuilding].BuyPrice.ToString();
                    button.GetComponent<SeedListButton>().SeedKey = useableBuilding;
                    button.transform.SetParent(buttonTemplate.transform.parent, false);
                    catalogButtons.Add(button);
                }
                catalogTypeOpen = TileableObjectType.useableBuilding;
                break;
            default:
                break;
        }
    }

}
