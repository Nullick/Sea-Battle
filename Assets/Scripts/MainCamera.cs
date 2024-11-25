using TreeEditor;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private ReadyButton _readyButton;
    private Vector3 _smoothVelocity;
    public void Initialize()
    {
        transform.position = new Vector3(2, 20f, -5f);
        transform.rotation = Quaternion.Euler(77f, 0f, 0f);
        _readyButton.PlayerReady += OnPlayerReady;
    }

    private void OnPlayerReady()
    {
        transform.position = new Vector3(18f, 26f, 0f);
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(16f, 26f, 0f), ref _smoothVelocity, 5f);
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 15f;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private void OnDestroy()
    {
        _readyButton.PlayerReady -= OnPlayerReady;
    }
}
