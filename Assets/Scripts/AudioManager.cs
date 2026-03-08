using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    private AudioSource source;

    public AudioClip playerJump;
    public AudioClip whistle;
    public AudioClip objectFreeze;
    public AudioClip objectUnfreeze;
    public AudioClip gateButtonPress;
    public AudioClip gateOpen;
    public AudioClip gateClose;
    public AudioClip gateTicking;
    public AudioClip successScene;
    public AudioClip failScene;
    public AudioClip gameWin;
    public AudioClip menuButton;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        instance = this;

        source = GetComponent<AudioSource>();
        source.spatialBlend = 0f;
    }

    public static void PlayPlayerJump() => instance.PlayAudio(instance.playerJump);
    public static void PlayWhistle() => instance.PlayAudio(instance.whistle);
    public static void PlayObjectFreeze() => instance.PlayAudio(instance.objectFreeze);
    public static void PlayObjectUnfreeze() => instance.PlayAudio(instance.objectUnfreeze);
    public static void PlayGateButtonPress() => instance.PlayAudio(instance.gateButtonPress);
    public static void PlayGateOpen() => instance.PlayAudio(instance.gateOpen);
    public static void PlayGateClose() => instance.PlayAudio(instance.gateClose);
    public static void PlayGateTicking() => instance.PlayAudio(instance.gateTicking);
    public static void PlaySuccessScene() => instance.PlayAudio(instance.successScene);
    public static void PlayFailScene() => instance.PlayAudio(instance.failScene);
    public static void PlayGameWin() => instance.PlayAudio(instance.gameWin);
    public static void PlayMenuButton() => instance.PlayAudio(instance.menuButton);

    private void PlayAudio(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
