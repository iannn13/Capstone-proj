using UnityEngine;

[CreateAssetMenu(fileName = "NewStoreItem", menuName = "Store/StoreItem")]
public class StoreItem : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int scoreRequirement;
    public int cashRequirement;
    public GameObject itemPrefab; // The item to place in the player's room
}
