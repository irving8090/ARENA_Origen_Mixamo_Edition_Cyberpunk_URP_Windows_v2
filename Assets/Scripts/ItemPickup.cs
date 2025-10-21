
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType { Fragment, Medkit, EnergyCore }
    public ItemType itemType;
    public int healthRestore = 25;
    bool collected = false;

    public void Collect()
    {
        if (collected) return;
        collected = true;
        Debug.Log("Collected: " + itemType);
        if (itemType == ItemType.Medkit)
        {
            var player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
            if (player != null) player.health = Mathf.Min(player.maxHealth, player.health + healthRestore);
            UI.HUDController.Instance?.UpdateHealth(player.health, player.maxHealth);
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Collect();
    }
}
