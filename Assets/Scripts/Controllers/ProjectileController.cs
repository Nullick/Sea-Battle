using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Animation _projectileAnimation;

    private bool _isPlayerAnimationOver = false;
    private bool _isEnemyAnimationOver = false;

    public static ProjectileController Instance;
    public bool IsPlayerAnimationOver => _isPlayerAnimationOver;
    public bool IsEnemyAnimationOver => _isEnemyAnimationOver;
    public Animation ProjectileAnimation => _projectileAnimation;

    public void Initialized()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SpawnPlayerProjectile(Tile tile)
    {
        _projectile = Instantiate<GameObject>(_projectile);
        _projectile.transform.position = new Vector3(tile.X - 2, 3.5f, tile.Z);
        _projectileAnimation = _projectile.GetComponentInChildren<Animation>();
        _projectileAnimation.Play("EnemyTileAnimation");
    }

    public void SpawnEnemyProjectile(Tile tile)
    {
        _projectile = Instantiate<GameObject>(_projectile);
        _projectile.transform.position = new Vector3(tile.X + 2, 3.5f, tile.Z);
        _projectileAnimation = _projectile.GetComponentInChildren<Animation>();
        _projectileAnimation.Play("PlayerTileAnimation");
    }
}
