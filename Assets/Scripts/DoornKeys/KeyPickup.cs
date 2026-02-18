using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public string keyColor; // ตั้งเป็น "Blue" หรือ "Red"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KeyInventory inv = other.GetComponent<KeyInventory>();
            if (inv != null)
            {
                inv.AddKey(keyColor);
                Destroy(gameObject);
            }
        }
    }
}
