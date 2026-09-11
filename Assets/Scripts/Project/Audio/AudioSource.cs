using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Efectos")]
    [SerializeField] private AudioClip buttonClip;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip transformClip;

    [Header("Volúmenes")]
    [Range(0f, 1f)][SerializeField] private float buttonVolume = 0.7f;
    [Range(0f, 1f)][SerializeField] private float damageVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float jumpVolume = 0.8f;
    [Range(0f, 1f)][SerializeField] private float transformVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // El audio sobrevive al cambio MainMenu → EscenaGato
        DontDestroyOnLoad(gameObject);

        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();
    }

    private void Play(AudioClip clip, float volume)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayButton()
    {
        Play(buttonClip, buttonVolume);
    }

    public void PlayDamage()
    {
        Play(damageClip, damageVolume);
    }

    public void PlayJump()
    {
        Play(jumpClip, jumpVolume);
    }

    public void PlayTransform()
    {
        Play(transformClip, transformVolume);
    }
}