using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ReadyButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private ShipClickController _shipController;

    public event Action PlayerReady;

    public void Initialize()
    {
        _shipController.AllShipsPlaced += OnAllShipsPlaced;
        _shipController.NotAllShipsPlaced += OnNotAllShipsPlaced;
        this.transform.gameObject.SetActive(false);
    }

    private void OnAllShipsPlaced()
    {
        this.transform.gameObject.SetActive(true);
    }

    private void OnNotAllShipsPlaced()
    {
        this.transform.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerReady?.Invoke();
    }

    private void OnDestroy()
    {
        _shipController.AllShipsPlaced -= OnAllShipsPlaced;
        _shipController.AllShipsPlaced -= OnNotAllShipsPlaced;
    }
}
