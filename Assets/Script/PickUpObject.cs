using TMPro;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    /* The value of the object which have this script */
    public int objectValue = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            switch (gameObject.tag)
            {
                case "Gem":
                    Inventory.Instance.UpdateGems(objectValue);
                    break;
                case "Heal":
                    Inventory.Instance.UpdateHeart(objectValue);
                    break;
                default:
                    Debug.Log("Nothing to do with this object" + gameObject.tag);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
