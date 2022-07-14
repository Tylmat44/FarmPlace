using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlwaysRunning : MonoBehaviour
{
    public GameObject tileManager;
    public GameObject buttonTemplate;
    public TextMeshProUGUI selectionText;
    public static bool allowHover = true;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        DataManager.loadAll();
        AccountManager.loadAccountInfo();
        tileManager.GetComponent<TileManager>().generateMap();
        tileManager.GetComponent<TileManager>().loadMapFromFile();
        ActiveTileableObjects.loadActiveTileableObjects();
        Inventory.loadInventory();


    }

    // Update is called once per frame
    void Update()
    {
/*        if (ToolChest.PlowSelected)
        {
            selectionText.text = "Plow";
        }
        else if (ToolChest.ClearSelected)
        {
            selectionText.text = "Clear";
        } else if (ToolChest.SeedSelected != null)
        {
            selectionText.text = ToolChest.SeedSelected.Name;
        } else
        {
            selectionText.text = "Nothing";
        }*/
    }

    private void OnApplicationQuit()
    {
        tileManager.GetComponent<TileManager>().saveMapToFile();
        ActiveTileableObjects.saveActiveTileableObjects();
        AccountManager.saveAccountInfo();
        Inventory.saveInventory();
    }
}
