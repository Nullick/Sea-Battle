using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
