using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    [Header("---------------Audio Sources---------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------------Audio Clips------------------")]
    public AudioClip _death;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}

