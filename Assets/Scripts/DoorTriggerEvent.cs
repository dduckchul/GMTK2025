using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerEvent : MonoBehaviour
{
    private BuildingInfo _buildingInfo;
    private void Awake()
    {
        _buildingInfo = transform.parent.GetComponent<BuildingInfo>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        _buildingInfo.buildingManager.TriggerBuilding(_buildingInfo, other);
    }
}
