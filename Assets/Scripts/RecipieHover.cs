using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipieHover : MonoBehaviour
{
    public static RecipieHover _instance;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI timeText;
    public List<TextMeshProUGUI> recipieTexts;
    public List<RawImage> recipieImages;
    public bool selection = false;
    public Color RED = Color.red;
    public Color GREEN = Color.green;
    public CraftingStation craftingStation;
    public CraftableResource craftableResource;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(this.selection)
        {
            if (this.craftingStation != null && this.craftingStation.IsCrafting)
            {
                this.nameText.text = "Time Remaining: " + craftingStation.TimeLeft.ToString(@"hh\:mm\:ss");
                this.timeText.text = "Total Time Remaining: " + TimeSpan.FromMinutes(((craftableResource.CraftingTime.TotalMinutes / craftingStation.SpeedMultiplier) * (craftingStation.QuantityLeft - 1)) + craftingStation.TimeLeft.TotalMinutes).ToString(@"hh\:mm\:ss");
            }
        }
    }

    public void setAndShowPopup(CraftableResource craftableResource, CraftingStation craftingStation, bool selection)
    {
        if (!selection)
        {
            this.selection = false;
            this.gameObject.transform.localPosition = new Vector3(0f, 75f, 0f);
            this.gameObject.SetActive(true);

            int position = 0;
            int quantityInv = 0;
            int quantityNeeded = 0;

            foreach (string key in craftableResource.Recipie.Keys)
            {
                quantityInv = Inventory.getItemAmount(key);
                quantityNeeded = craftableResource.Recipie[key];
                this.recipieTexts[position].text = quantityInv + "/" + quantityNeeded;

                if (quantityInv < quantityNeeded)
                {
                    this.recipieTexts[position].color = RED;
                }
                else
                {
                    this.recipieTexts[position].color = GREEN;
                }
            }

            for (int i = position + 1; i < 8; i++)
            {
                this.recipieTexts[i].gameObject.SetActive(false);
                this.recipieImages[i].gameObject.SetActive(false);
            }

            this.nameText.text = craftableResource.Name;
            this.timeText.text = TimeSpan.FromMinutes(craftableResource.CraftingTime.TotalMinutes / craftingStation.SpeedMultiplier).ToString(@"hh\:mm\:ss");
        }
        else
        {
            if (craftableResource != null && craftableResource.Key != "NONE")
            {
                this.craftableResource = craftableResource;
                this.craftingStation = craftingStation;
                this.selection = true;
                this.gameObject.transform.localPosition = new Vector3(74f, -78f, 0f);
                this.gameObject.SetActive(true);

                int position = 0;
                int quantityInv = 0;
                int quantityNeeded = 0;

                foreach (string key in craftableResource.Recipie.Keys)
                {
                    quantityInv = Inventory.getItemAmount(key);
                    quantityNeeded = craftableResource.Recipie[key];
                    this.recipieTexts[position].text = quantityInv + "/" + quantityNeeded;

                    this.recipieTexts[position].gameObject.GetComponent<ResourceHoverPopupTemplate>().setResourceName(DataManager.resourceDB[key].Name);
                    this.recipieImages[position].gameObject.GetComponent<ResourceHoverPopupTemplate>().setResourceName(DataManager.resourceDB[key].Name);

                    if (quantityInv < quantityNeeded)
                    {
                        this.recipieTexts[position].color = RED;
                    }
                    else
                    {
                        this.recipieTexts[position].color = GREEN;
                    }
                }

                for (int i = position + 1; i < 8; i++)
                {
                    this.recipieTexts[i].gameObject.SetActive(false);
                    this.recipieImages[i].gameObject.SetActive(false);
                }

                if (craftingStation.IsCrafting)
                {
                    this.nameText.text = "Time Remaining: " + craftingStation.TimeLeft.ToString(@"hh\:mm\:ss");
                    this.timeText.text = "Total Time Remaining: " + TimeSpan.FromMinutes(((craftableResource.CraftingTime.TotalMinutes / craftingStation.SpeedMultiplier) * (craftingStation.QuantityLeft - 1)) + craftingStation.TimeLeft.TotalMinutes).ToString(@"hh\:mm\:ss");
                }
                else
                {
                    this.nameText.text = "";
                    this.timeText.text = "";
                }
            }
        }
    }

    public void hidePopup()
    {
        this.gameObject.SetActive(false);
    }
}
