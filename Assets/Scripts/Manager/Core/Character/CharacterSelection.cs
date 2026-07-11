#region Summary
/// <summary>
/// This static class manages the character selection in the game. It provides an enumeration of available characters and a property to store the currently selected character.
/// </summary>
#endregion
#region Phase 1 Sprint 4 - Character Selection
public static class CharacterSelection
{
    #region Character Options
    public enum Character
    {
        Peyton,
        Eli
    }
    #endregion

    #region Selected Character
    public static Character SelectedCharacter { get; set; } = Character.Peyton;
    #endregion
}
#endregion