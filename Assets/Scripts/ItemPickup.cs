using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease
    }

    [SerializeField] private ItemType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            OnItemPickup(other.gameObject);
    }

    private void OnItemPickup(GameObject player)
    {
        switch (type)
        {
            case ItemType.BlastRadius:
                player.GetComponent<BombController>().AddBomb();
                break;
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().explosionRadius++;
                break;
            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().speed++;
                break;
        }

        Destroy(gameObject);
    }
}