using Syncfusion.UI.Xaml.Grid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MaliyeHesaplama.helpers.controller
{
    public class GridButtonEditColumn : GridTemplateColumn
    {
        public GridButtonEditColumn()
        {
            CellTemplate = CreateCellTemplate();
            EditTemplate = CreateEditTemplate();
        }
        private DataTemplate CreateCellTemplate()
        {
            var template = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(DockPanel));
            var text = new FrameworkElementFactory(typeof(TextBlock));
            text.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding(MappingName));
            text.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            text.SetValue(TextBlock.MarginProperty, new Thickness(0, 0, 5, 0));

            var button = new FrameworkElementFactory(typeof(Button));
            button.SetValue(Button.ContentProperty,"...");
            button.SetValue(Button.WidthProperty, 25.0);
            button.SetValue(Button.HeightProperty,20.0);
            button.SetBinding(Button.CommandProperty, new System.Windows.Data.Binding("DataContext.AcButonCommand")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(SfDataGrid), 1)
            });
            button.SetBinding(Button.CommandParameterProperty, new System.Windows.Data.Binding());
            factory.AppendChild(text);
            factory.AppendChild(button);
            template.VisualTree = factory;
            return template;
        }
        private DataTemplate CreateEditTemplate()
        {
            var template = new DataTemplate();

            var factory = new FrameworkElementFactory(typeof(DockPanel));

            var textBox = new FrameworkElementFactory(typeof(TextBox));
            textBox.SetBinding(TextBox.TextProperty, new System.Windows.Data.Binding(MappingName)
            {
                Mode = System.Windows.Data.BindingMode.TwoWay,
                UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged
            });
            textBox.SetValue(TextBox.VerticalAlignmentProperty, VerticalAlignment.Center);
            textBox.SetValue(TextBox.MarginProperty, new Thickness(0, 0, 5, 0));

            var button = new FrameworkElementFactory(typeof(Button));
            button.SetValue(Button.ContentProperty, "...");
            button.SetValue(Button.WidthProperty, 25.0);
            button.SetValue(Button.HeightProperty, 20.0);
            button.SetBinding(Button.CommandProperty, new System.Windows.Data.Binding("DataContext.AcButonCommand")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(SfDataGrid), 1)
            });
            button.SetBinding(Button.CommandParameterProperty, new System.Windows.Data.Binding());

            factory.AppendChild(textBox);
            factory.AppendChild(button);

            template.VisualTree = factory;
            return template;
        }
    }
}
