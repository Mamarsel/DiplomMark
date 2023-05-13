using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiplomMark.Classes.HelpClasses
{
    public static class UIElementExtensions
    {
        public static int GetGroupID(DependencyObject obj)
        {
            return (int)obj.GetValue(GroupIDProperty);
        }

        public static void SetGroupID(DependencyObject obj, int value)
        {
            obj.SetValue(GroupIDProperty, value);
        }
        public static readonly DependencyProperty GroupIDProperty =
            DependencyProperty.RegisterAttached("GroupID", typeof(int), typeof(UIElementExtensions), new UIPropertyMetadata(null));
    }
}
