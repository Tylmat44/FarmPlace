using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingMenuPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static BuildingMenuPopup _instance;

    public TextMeshProUGUI textComponent;
    public UseableBuilding useableBuilding;
    public List<GameObject> craftingStationObjects = new List<GameObject>();
    public GameObject craftingStationTemplate;

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

    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        AlwaysRunning.allowHover = false;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        AlwaysRunning.allowHover = true;
    }

    public void setAndShowPopup(UseableBuilding useableBuilding)
    {
        this.gameObject.SetActive(true);
        this.transform.position = Input.mousePosition;
        this.useableBuilding = useableBuilding;

        foreach (GameObject gameObject in this.craftingStationObjects)
        {
            Destroy(gameObject);
        }

        this.craftingStationObjects = new List<GameObject>();

        int count = 1;
        foreach (int craftingStation in this.useableBuilding.CraftingStations.Keys)
        {
            GameObject station = Instantiate(craftingStationTemplate);
            station.SetActive(true);

            station.GetComponent<CraftingStationObjectTemplate>().setCraftingStation(this.useableBuilding.CraftingStations[craftingStation], count, this.useableBuilding);
            station.transform.SetParent(craftingStationTemplate.transform.parent, false);
            this.craftingStationObjects.Add(station);
            count++;
        }


        while (count <= this.useableBuilding.Slots)
        {
            GameObject station = Instantiate(craftingStationTemplate);
            station.GetComponent<CraftingStationObjectTemplate>().UseableBuilding = this.useableBuilding;
            station.GetComponent<CraftingStationObjectTemplate>().Slot = count;
            station.SetActive(true);
            station.transform.SetParent(craftingStationTemplate.transform.parent, false);
            this.craftingStationObjects.Add(station);
            count++;
        }


    }

    public void hidePopup()
    {
        this.gameObject.SetActive(false);
    }
}