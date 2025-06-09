namespace UniGame.Core.Runtime
{
#if !ODIN_INSPECTOR
    public interface ISearchFilterable
    {
        bool IsMatch(string filter);
    }
    
    public class ValueDropdownItem<TValue>
    {
        public string Text = string.Empty;
        public TValue Value = default;
        
        public ValueDropdownItem(){}
        
        public ValueDropdownItem(string text, TValue value)
        {
            Text = text;
            Value = value;
        }
    }
    
#endif

}