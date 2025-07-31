using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    private char _buildingName;
    private bool _isOpen;

    public char BuildingName
    {
        get => _buildingName;
        set => _buildingName = value;
    }

    public bool IsOpen
    {
        get => _isOpen;
        set => _isOpen = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
