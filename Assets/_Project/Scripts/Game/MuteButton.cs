using UnityEngine;

public class MuteButton : MonoBehaviour
{
    public Sprite aciveSprite, muteSprite;
    public UnityEngine.UI.Image icon;

    private void Awake()
    {
        AudioManager.onMuteRequested += AudioManager_onMuteRequested;
    }

    private void OnDestroy()
    {
        AudioManager.onMuteRequested -= AudioManager_onMuteRequested;
    }

    private void AudioManager_onMuteRequested(bool p_mute)
    {
        icon.sprite = p_mute ? muteSprite : aciveSprite;
    }
}
