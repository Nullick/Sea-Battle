using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Create Buttons")]
    [SerializeField] private Button _createOneTileShipButton;
    [SerializeField] private Button _createTwoTileShipButton;
    [SerializeField] private Button _createThreeTileShipButton;
    [SerializeField] private Button _createFourTileShipButton;
    [SerializeField] private ReadyButton _readyButton;
    [Header("Text")]
    [SerializeField] private Text _oneTileShipText;
    [SerializeField] private Text _twoTileShipText;
    [SerializeField] private Text _threeTileShipText;
    [SerializeField] private Text _fourTileShipText;
    [SerializeField] private Text _endText;
    [SerializeField] private Text _turnText;
    [Header("Other")]
    [SerializeField] private ShipController _enemyShipController;

    public static UIController Instance;

    public void Initialize(ShipController enemyShipController)
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _readyButton.Initialize();
        _readyButton.PlayerReady += OnPlayerReady;

        _oneTileShipText.gameObject.SetActive(false);
        _twoTileShipText.gameObject.SetActive(false);
        _threeTileShipText.gameObject.SetActive(false);
        _fourTileShipText.gameObject.SetActive(false);

        _endText.gameObject.SetActive(false);
        _turnText.gameObject.SetActive(false);

        _enemyShipController = enemyShipController;
        _enemyShipController.ShipDestroyed += OnShipDestroyed;
    }

    private void OnShipDestroyed()
    {
        _oneTileShipText.text = "One Tile Ships: " + _enemyShipController.ShipsForUI[ShipType.OneTile];
        _twoTileShipText.text = "Two Tile Ships: " + _enemyShipController.ShipsForUI[ShipType.TwoTile];
        _threeTileShipText.text = "Three Tile Ships: " + _enemyShipController.ShipsForUI[ShipType.ThreeTile];
        _fourTileShipText.text = "Four Tile Ships: " + _enemyShipController.ShipsForUI[ShipType.FourTile];
    }

    private void OnPlayerReady()
    {
        _createOneTileShipButton.gameObject.SetActive(false);
        _createTwoTileShipButton.gameObject.SetActive(false);
        _createThreeTileShipButton.gameObject.SetActive(false);
        _createFourTileShipButton.gameObject.SetActive(false);

        _readyButton.gameObject.SetActive(false);

        _oneTileShipText.gameObject.SetActive(true);
        _twoTileShipText.gameObject.SetActive(true);
        _threeTileShipText.gameObject.SetActive(true);
        _fourTileShipText.gameObject.SetActive(true);

        _turnText.gameObject.SetActive(true);
    }

    public void PlayerWon()
    {
        _endText.gameObject.SetActive(true);
        _endText.text = "YOU WON!";
        _endText.color = new Color(14f, 222f, 0f);
    }

    public void EnemyWon()
    {
        _endText.gameObject.SetActive(true);
        _endText.text = "COMPUTER WON";
        _endText.color = new Color(255f, 0f, 0f);
    }

    public void PlayerMove()
    {
        _turnText.text = "Your turn";
    }

    public void EnemyMove()
    {
        _turnText.text = "Computer turn";
    }

    private void OnDestroy()
    {
        _readyButton.PlayerReady -= OnPlayerReady;
        _enemyShipController.ShipDestroyed -= OnShipDestroyed;
    }
}