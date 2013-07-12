using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Utility
{
    public static class CoolPopupContainerExtension
    {
        public static void CloseMeAsPopup(this FrameworkElement sender)
        {
            CoolPopupContainer container = UIHelper.FindAncestorOfType<CoolPopupContainer>(sender) as CoolPopupContainer;
            if (container!=null)
            {
                container.Hide();
            }
        }
    }
}
