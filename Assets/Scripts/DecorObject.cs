using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorObject : MonoBehaviour
{
    private Decor decor;
    private Dictionary<int, Vector2> tileGroup;
    private bool isHover = false;

    public Decor Decor { get => decor; set => decor = value; }
    public Dictionary<int, Vector2> TileGroup { get => tileGroup; set => tileGroup = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
