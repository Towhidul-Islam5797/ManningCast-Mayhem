#region Summary
/// <summary>
/// BillboardManager plays the billboard video once while GameScene loads in
/// the background, showing the rules alongside it. Skip unlocks after a
/// short delay. Transitions to GameScene automatically once the video has
/// finished (or been skipped) and the scene is ready.
/// </summary>
#endregion

#region Phase 3 Sprint 11 - Billboard Manager
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class BillboardManager : MonoBehaviour
{
    #region Settings
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private float skipUnlockSeconds = 8f;
    #endregion

    #region References
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Button skipButton;
    #endregion

    #region Private State
    private AsyncOperation sceneLoad;
    private bool videoFinished;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        skipButton.interactable = false;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();

        sceneLoad = SceneManager.LoadSceneAsync(gameSceneName);
        sceneLoad.allowSceneActivation = false;

        Invoke(nameof(UnlockSkip), skipUnlockSeconds);
    }

    private void Update()
    {
        if (videoFinished && sceneLoad.progress >= 0.9f)
        {
            sceneLoad.allowSceneActivation = true;
        }
    }
    #endregion

    #region Video
    private void OnVideoFinished(VideoPlayer source)
    {
        videoFinished = true;
    }
    #endregion

    #region Skip
    private void UnlockSkip()
    {
        skipButton.interactable = true;
    }

    public void Skip()
    {
        videoPlayer.Stop();
        videoFinished = true;
    }
    #endregion
}
#endregion