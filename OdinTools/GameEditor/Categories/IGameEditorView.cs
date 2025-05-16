namespace UniModules.GameEditor.Categories
{
    public interface IGameEditorView
    {
        public bool Active { get; }

        public void Focus();

        public void FocusLost();
    }
}