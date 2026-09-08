#region Summary
/// <summary>
/// VolumeSliderBridge forwards Settings panel slider changes to whichever
/// AudioManager instance is currently alive. Sliders can't be wired directly
/// to AudioManager in the Inspector, because AudioManager is a persistent
/// singleton and each scene's local copy of it gets destroyed at runtime -
/// only AudioManager.Instance is guaranteed to still exist.
/// </summary>
#endregion

#region Phase 3 Sprint 12 - Volume Slider Bridge
using UnityEngine;

public class VolumeSliderBridge : MonoBehaviour
{
    #region Slider Callbacks
    public void OnMusicVolumeChanged(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void OnSfxVolumeChanged(float volume)
    {
        AudioManager.Instance.SetSfxVolume(volume);
    }
    #endregion
}
#endregion