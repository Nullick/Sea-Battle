using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip _explosionAudioClip;
    [SerializeField] private AudioClip _missingAudioClip;
    [SerializeField] private AudioClip _shotAudioClip;

    private AudioSource _audioSource;

    public static AudioController Instance;

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayExplosionSound()
    {
        _audioSource.clip = _explosionAudioClip;
        _audioSource.Play();
    }

    public void PlayMissSound()
    {
        _audioSource.clip = _missingAudioClip;
        _audioSource.Play();
    }

    public void PlayShotSound() 
    {
        _audioSource.clip = _shotAudioClip;
        _audioSource.Play();
    }
}
