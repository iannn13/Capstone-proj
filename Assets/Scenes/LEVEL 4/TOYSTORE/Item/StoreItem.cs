using UnityEngine;

[CreateAssetMenu(fileName = "NewStoreItem", menuName = "Store/StoreItem")]
public class StoreItem : ScriptableObject
{
    public string itemName;              // Name of the item
    public Sprite itemImage;             // Image for the item
    public int scoreRequirement;         // Score requirement for the item
    public int cashRequirement;          
    public string itemToActivateName;   
    public bool isClaimed; 
}
