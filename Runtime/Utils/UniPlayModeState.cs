namespace UniGame.Utils
{
    /// <summary>
    ///         <para>Enumeration specifying a change in the Editor's play mode state.
    /// 
    /// See Also: PauseState, EditorApplication.playModeStateChanged, EditorApplication.isPlaying.</para>
    ///       </summary>
    public enum UniPlayModeState
    {
        /// <summary>
        ///   <para>Occurs during the next update of the Editor application if it is in edit mode and was previously in play mode.</para>
        /// </summary>
        EnteredEditMode,
        /// <summary>
        ///   <para>Occurs when exiting edit mode, before the Editor is in play mode.</para>
        /// </summary>
        ExitingEditMode,
        /// <summary>
        ///   <para>Occurs during the next update of the Editor application if it is in play mode and was previously in edit mode.</para>
        /// </summary>
        EnteredPlayMode,
        /// <summary>
        ///   <para>Occurs when exiting play mode, before the Editor is in edit mode.</para>
        /// </summary>
        ExitingPlayMode,
    }
}