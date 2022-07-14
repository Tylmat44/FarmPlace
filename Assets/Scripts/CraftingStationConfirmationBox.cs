using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingStationConfirmationBox : MonoBehaviour
{
    public static CraftingStationConfirmationBox _instance;

    public Button yesButton;
    public Button noButton;
    public TextMeshProUGUI text;
    public static bool selection = false;
    public bool boxOpen = false;
    public bool selectionChosen = false;
    private string name;

    public string Name { get => name; set => name = value; }

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
       if (this.boxOpen)
        {
            if (this.selectionChosen)
            {
                this.boxOpen = false;
                this.gameObject.SetActive(false);
                this.selectionChosen = false;
            }
        }
    }


    public IEnumerator Dialog()
    {
        this.boxOpen = true;
        this.text.text = "Are you sure you want to replace " + this.Name + "?";
        this.gameObject.SetActive(true);
        var waitForButton = new WaitForUIButtons(yesButton, noButton);
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == yesButton)
        {
            selection = true;
        }
        else
        {
            selection = false;
        }

        this.selectionChosen = true;
        
    }


}