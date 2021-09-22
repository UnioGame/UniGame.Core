using UnityEditor;

namespace UniModules.UniGame.Core.Editor.LifeTimeTools
{
    public static class LifeTimeEditorCommands
    {
        public const string ReportingMenu = "UniGame/TimeLine/Enable Reporting";
        
        public static bool IsReportingEnabled
        {
            get => EditorPrefs.GetBool(ReportingMenu, true);
            set => EditorPrefs.SetBool(ReportingMenu, value);
        }
        
        [MenuItem(ReportingMenu)]
        public static void UpdateReporting()
        {
            IsReportingEnabled = !IsReportingEnabled;
        }
        
        [MenuItem(ReportingMenu, true)]
        public static bool UpdateReportingValidate() {
            Menu.SetChecked(ReportingMenu, IsReportingEnabled);
            return IsReportingEnabled;
        }
    }
}
