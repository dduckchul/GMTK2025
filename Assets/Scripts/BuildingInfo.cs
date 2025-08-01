using System;
using TMPro;
using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    public BuildingManager buildingManager;
    public char buildingName;
    public TMP_Text text;
    public bool isOpen;

    public void SetBuildingManager(BuildingManager bm)
    {
        this.buildingManager = bm;
    }
    
    public void Start()
    {
        text.text = buildingName.ToString();        
    }
}
