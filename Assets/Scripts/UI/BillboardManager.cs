#region Summary
/// <summary>
/// BillboardManager plays the billboard video once while GameScene loads in
/// the background, showing the rules alongside it. Skip unlocks after a
/// short delay. Transitions to GameScene automatically once the video has
/// finished (or been skipped) and the scene is ready.
/// </summary>
#endregion

#region Phase 3 Sprint 11 - Billboard Manager
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using UnityEngine.Video;

//public class BillboardManager : MonoBehaviour
//{
//    #region Settings
//    [SerializeField] private string gameSceneName = "GameScene";
//    [SerializeField] private float skipUnlockSeconds = 8f;
//    #endregion

//    #region References
//    [SerializeField] private VideoPlayer videoPlayer;
//    [SerializeField] private Button skipButton;
//    #endregion

//    #region Private State
//    private AsyncOperation sceneLoad;
//    private bool videoFinished;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        skipButton.interactable = false;
//        videoPlayer.loopPointReached += OnVideoFinished;
//        videoPlayer.Play();

//        sceneLoad = SceneManager.LoadSceneAsync(gameSceneName);
//        sceneLoad.allowSceneActivation = false;

//        Invoke(nameof(UnlockSkip), skipUnlockSeconds);
//    }

//    private void Update()
//    {
//        if (videoFinished && sceneLoad.progress >= 0.9f)
//        {
//            sceneLoad.allowSceneActivation = true;
//        }
//    }
//    #endregion

//    #region Video
//    private void OnVideoFinished(VideoPlayer source)
//    {
//        videoFinished = true;
//    }
//    #endregion

//    #region Skip
//    private void UnlockSkip()
//    {
//        skipButton.interactable = true;
//    }

//    public void Skip()
//    {
//        videoPlayer.Stop();
//        videoFinished = true;
//    }
//    #endregion
//}
#endregion

#region Summary
/// <summary>
/// BillboardManager plays the billboard video once while GameScene loads in
/// the background, showing the rules alongside it. Skip unlocks after a
/// short delay. Transitions to GameScene automatically once the video has
/// finished (or been skipped) and the scene is ready.
/// </summary>
#endregion

#region Phase 3 Sprint 11 - Billboard Manager + Error Handling
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using UnityEngine.Video;

//public class BillboardManager : MonoBehaviour
//{
//    #region Settings
//    [SerializeField] private string gameSceneName = "GameScene";
//    [SerializeField] private float skipUnlockSeconds = 8f;
//    #endregion

//    #region References
//    [SerializeField] private VideoPlayer videoPlayer;
//    [SerializeField] private Button skipButton;
//    #endregion

//    #region Private State
//    private AsyncOperation sceneLoad;
//    private bool videoFinished;
//    #endregion

//    #region Unity Lifecycle
//    private void Start()
//    {
//        skipButton.interactable = false;
//        videoPlayer.loopPointReached += OnVideoFinished;
//        videoPlayer.Play();

//        sceneLoad = SceneManager.LoadSceneAsync(gameSceneName);

//        if (sceneLoad == null)
//        {
//            Debug.LogError("BillboardManager: failed to start loading scene '" + gameSceneName + "'. Check the scene name and Build Settings.");
//            return;
//        }

//        sceneLoad.allowSceneActivation = false;

//        Invoke(nameof(UnlockSkip), skipUnlockSeconds);
//    }

//    private void Update()
//    {
//        if (sceneLoad == null) return;

//        if (videoFinished && sceneLoad.progress >= 0.9f)
//        {
//            sceneLoad.allowSceneActivation = true;
//        }
//    }

//    private void OnDestroy()
//    {
//        if (videoPlayer != null)
//        {
//            videoPlayer.loopPointReached -= OnVideoFinished;
//        }
//    }
//    #endregion

//    #region Video
//    private void OnVideoFinished(VideoPlayer source)
//    {
//        videoFinished = true;
//    }
//    #endregion

//    #region Skip
//    private void UnlockSkip()
//    {
//        skipButton.interactable = true;
//    }

//    public void Skip()
//    {
//        CancelInvoke(nameof(UnlockSkip));
//        videoPlayer.Stop();
//        videoFinished = true;
//    }
//    #endregion
//}
#endregion

#region Summary
/// <summary>
/// BillboardManager plays the billboard video once while GameScene loads in
/// the background, showing the rules alongside it. Skip unlocks after a
/// short delay. Transitions to GameScene automatically once the video has
/// finished (or been skipped) and the scene is ready.
/// </summary>
#endregion

#region Phase 3 Sprint 11 - Billboard Manager + Error Handling
using System.Collections;
using TMPro;
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
    [SerializeField] private TMP_Text skipButtonText;
    #endregion

    #region Private State
    private AsyncOperation sceneLoad;
    private bool videoFinished;
    private Coroutine skipCountdownRoutine;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        skipButton.interactable = false;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();

        sceneLoad = SceneManager.LoadSceneAsync(gameSceneName);

        if (sceneLoad == null)
        {
            Debug.LogError("BillboardManager: failed to start loading scene '" + gameSceneName + "'. Check the scene name and Build Settings.");
            return;
        }

        sceneLoad.allowSceneActivation = false;

        skipCountdownRoutine = StartCoroutine(SkipCountdown());
    }

    private void Update()
    {
        if (sceneLoad == null) return;

        if (videoFinished && sceneLoad.progress >= 0.9f)
        {
            sceneLoad.allowSceneActivation = true;
        }
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
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
    private IEnumerator SkipCountdown()
    {
        int secondsRemaining = Mathf.CeilToInt(skipUnlockSeconds);

        while (secondsRemaining > 0)
        {
            skipButtonText.text = "SKIP " + secondsRemaining;
            yield return new WaitForSeconds(1f);
            secondsRemaining--;
        }

        skipButtonText.text = "SKIP";
        skipButton.interactable = true;
    }

    public void Skip()
    {
        if (skipCountdownRoutine != null)
        {
            StopCoroutine(skipCountdownRoutine);
        }

        videoPlayer.Stop();
        videoFinished = true;
    }
    #endregion
}
#endregion