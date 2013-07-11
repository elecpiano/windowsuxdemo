using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace WindowsUXDemo.Utility
{
    public class UIHelper
    {
        public static DependencyObject FindAncestorOfType<T>(DependencyObject root)
        {
            while ((root != null) && !(root is T))
            {
                root = VisualTreeHelper.GetParent(root);
            }
            return root;
        }

        public static T FindChildOfType<T>(DependencyObject instance) where T : DependencyObject
        {
            T control = default(T);

            if (instance != null)
            {

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(instance); i++)
                {
                    if ((control = VisualTreeHelper.GetChild(instance, i) as T) != null)
                    {
                        break;
                    }
                    control = FindChildOfType<T>(VisualTreeHelper.GetChild(instance, i));
                }
            }
            return control;
        }

    }

}
