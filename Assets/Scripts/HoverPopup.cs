using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverPopup : MonoBehaviour
{
    public static HoverPopup _instance;

    public TextMeshProUGUI textComponent;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
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
        this.transform.position = Input.mousePosition;
    }

    public void setAndShowPopup(string text)
    {
        this.gameObject.SetActive(true);
        textComponent.text = text;
    }

    public void hidePopup()
    {
        this.gameObject.SetActive(false);
    }
}