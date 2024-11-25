using UnityEngine;

public class SceneTile : MonoBehaviour
{
    [SerializeField] private Tile _tile;

    [Header("Materials")]
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _hitMaterial;
    [SerializeField] private Material _missMaterial;

    public Tile Tile => _tile;

    public void Initialize(Tile tile)
    {
        _tile = tile;
    }

    public void ChangeMaterialHit()
    {
        //_defaultMaterial = _hitMaterial;
        this.GetComponent<Renderer>().sharedMaterial = _hitMaterial;
    }

    public void ChangeMaterialMiss()
    {
        //_defaultMaterial = _missMaterial;
        this.GetComponent<Renderer>().sharedMaterial = _missMaterial;
    }

    private void OnMouseExit()
    {
        if (GameController.Instance.IsPlayerTurn)
        {
            this.GetComponent<Renderer>().materials[1] = _defaultMaterial;
        }
    }

    private void OnMouseOver()
    {

    }
}
