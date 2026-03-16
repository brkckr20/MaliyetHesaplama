using MaliyeHesaplama.mvvm;
using System.Windows;
using System.Windows.Controls;
using Binding = System.Windows.Data.Binding;

namespace MaliyeHesaplama.userControls
{
    public partial class OrderGirisi : System.Windows.Controls.UserControl
    {
        MatrixViewModel ViewModel { get; set; }
        public OrderGirisi()
        {
            InitializeComponent();
            ViewModel = new MatrixViewModel();
            DataContext = ViewModel;
            CreateColumns();
            MatrixGrid.ItemsSource = ViewModel.Rows;
        }
        private void CreateColumns()
        {
            MatrixGrid.Columns.Clear();

            // Variant kolonları
            foreach (var variant in ViewModel.VariantColumns)
            {
                MatrixGrid.Columns.Add(new DataGridComboBoxColumn
                {
                    Header = variant,
                    ItemsSource = ViewModel.Colors,
                    SelectedItemBinding = new Binding($"[{variant}]")
                });
            }

            // Size kolonları
            foreach (var size in ViewModel.SizeColumns)
            {
                MatrixGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = size,
                    Binding = new Binding($"Cells[{size}]")
                });
            }
        }
        private void AddSize_Click(object sender, RoutedEventArgs e)
        {
            string newSize = "150x150";

            if (!ViewModel.SizeColumns.Contains(newSize))
            {
                ViewModel.SizeColumns.Add(newSize);

                CreateColumns();
            }
        }
        private void AddVariant_Click(object sender, RoutedEventArgs e)
        {
            string newVariant = "Astar";

            if (!ViewModel.VariantColumns.Contains(newVariant))
            {
                ViewModel.VariantColumns.Add(newVariant);

                CreateColumns();
            }
        }
    }
}
