using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [Header("--------AudioSource---------")] 

    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("--------AudioClips---------")]
    public AudioClip MainAmbiance;


    void Start()
    {
        MusicSource.clip = MainAmbiance;
        MusicSource.Play();
    }
}
 