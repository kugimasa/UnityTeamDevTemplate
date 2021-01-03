using SilCilSystem.Internals;
using UnityEditor;

namespace SilCilSystem.Editors
{
    internal static class NotificationVector2Generator
    {
        private const string MenuPath = "Struct/Vector2";

        [MenuItem(EditorConstants.CreateVariableMenuPath + MenuPath, false, 0)]
        private static void CreateVariableAsset()
        {
            CustomEditorUtil.CreateVariableAsset<NotificationVector2>("NewVariable.asset", typeof(ReadonlyVector2Value), typeof(EventVector2), typeof(EventVector2Listener));
        }

        [MenuItem(EditorConstants.CreateGameEventMenuPath + MenuPath, false, 0)]
        private static void CreateEventAsset()
        {
            CustomEditorUtil.CreateVariableAsset<EventVector2>("NewEvent.asset", typeof(EventVector2Listener));
        }
    }
}