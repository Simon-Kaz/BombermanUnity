using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")] [SerializeField] private GameObject bombPrefab;
    [SerializeField] private KeyCode inputKey = KeyCode.Space;
    [SerializeField] private float bombFuseTimeInSeconds = 3f;
    [SerializeField] private int bombAmount = 1;
    private int _bombsRemaining;
    private WaitForSeconds _waitForBombToExplode;

    [Header("Explosion")] public int explosionRadius = 1;
    [SerializeField] private Explosion explosionPrefab;
    [SerializeField] private float explosionDuration = 1f;
    [SerializeField] private LayerMask explosionLayerMask;

    [Header("Destructible")] 
    [SerializeField] private Tilemap destructibleTiles;
    [SerializeField] private Destructible destructiblePrefab;


    #region Monobehaviour events

    private void OnEnable()
    {
        _bombsRemaining = bombAmount;
        _waitForBombToExplode = new WaitForSeconds(bombFuseTimeInSeconds);
    }

    private void Update()
    {
        if (_bombsRemaining > 0 && Input.GetKeyDown(inputKey))
            StartCoroutine(PlaceBomb());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
            other.isTrigger = false;
    }

    #endregion

    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        _bombsRemaining--;

        yield return _waitForBombToExplode;

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        _bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
            return;

        position += direction;

        // Did we hit a wall etc.? Destructible or otherwise            
        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1);
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);
        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        _bombsRemaining++;
    }
}