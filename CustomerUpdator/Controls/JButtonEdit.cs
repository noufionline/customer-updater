using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.Xpf.Editors.Themes;

namespace CustomerUpdator.Controls
{
    public class JButtonEdit : ButtonEdit
    {
        public bool IsWaitIndicatorVisible
        {
            get => (bool)GetValue(IsWaitIndicatorVisibleProperty);
            set => SetValue(IsWaitIndicatorVisibleProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsWaitIndicatorVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsWaitIndicatorVisibleProperty =
            DependencyProperty.Register("IsWaitIndicatorVisible", typeof(bool), typeof(JButtonEdit),
                new PropertyMetadata(false,
                    (o, args) =>
                    {
                        FrameworkElement item = (FrameworkElement)o;

                        if(item is BaseEdit buttonEdit) buttonEdit.IsReadOnly=(bool)args.NewValue;
                       
                    }));

        protected override void InsertCommandButtonInfo(IList<ButtonInfoBase> collection)
        {
            base.InsertCommandButtonInfo(collection);

            ButtonInfoBase button =CreateLoadingButtonInfo();
            collection.Insert(0, button);
        }


        protected virtual ButtonInfoBase CreateLoadingButtonInfo() {
            ButtonInfo info = new ButtonInfo
            {
                ButtonKind = ButtonKind.Repeat,
                Template = new DataTemplate(){DataType = typeof(ColumnWaitIndicator)}
            };
            info.SetBinding(ButtonInfoBase.VisibilityProperty, new Binding("IsWaitIndicatorVisible") { Source = this, Converter = new BoolToVisibilityConverter() });
            return info;
        }
    }
}