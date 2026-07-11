#region Summary
/// <summary>
/// GameManager class is responsible for managing the game state, including player lives and game over conditions.
/// Usage: 
/// 1. Attach this script to a GameObject in your Unity scene.
/// 2. Set the startingLives field in the Inspector to define how many lives the player starts with.
/// 3. Call the PlayerHitObstacle() method when the player collides with an obstacle to decrement lives and check for game over.
/// 
/// Note: This class is implemented as a singleton to ensure only one instance exists throughout the game.
#endregion

#region Phase 1 Sprint 4 - Character Selection Implementation
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    #region Scene Settings
    [SerializeField] private string mainSceneName = "MainScene";
    #endregion

    #region Selection Handlers
    public void SelectPeyton()
    {
        CharacterSelection.SelectedCharacter = CharacterSelection.Character.Peyton;
        SceneManager.LoadScene(mainSceneName);
    }

    public void SelectEli()
    {
        CharacterSelection.SelectedCharacter = CharacterSelection.Character.Eli;
        SceneManager.LoadScene(mainSceneName);
    }
    #endregion
}
#endregion