using System.Windows.Forms;
using DevExpress.XtraNavBar;

namespace CQC.ConTest
{
    public partial class ucToolbox : UserControl
    {
        public ucToolbox()
        {
            InitializeComponent();
            NavBarGroup standardGroup = navBar.Groups.Add();
            standardGroup.Caption = "Standard";
            standardGroup.Name = "Standard";
            NavBarGroup devexpressGroup = navBar.Groups.Add();
            devexpressGroup.Caption = "DevExpress";
            devexpressGroup.Name = "DevExpress";
            devexpressGroup.Expanded = true;
            int index = 0;
            foreach (string key in toolboxSvgImages.Keys)
            {
                NavBarItem item = new NavBarItem();
                var caption = GetCaption(key);
                item.Caption = caption;
                item.Name = caption;
                item.ImageOptions.SmallImageIndex = index;
                navBar.Items.Add(item);
                if (!key.Contains("DX"))
                    standardGroup.ItemLinks.Add(item);
                else
                    devexpressGroup.ItemLinks.Add(item);
                index++;
            }
        }
        string GetCaption(string key)
        {
            return key.Contains("_") ? key.Substring(0, key.IndexOf('_')) : key;
        }
    }
}
