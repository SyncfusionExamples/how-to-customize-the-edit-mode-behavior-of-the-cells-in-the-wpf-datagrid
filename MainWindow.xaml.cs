using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Cells;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            // Removes Default UpDown renderer. 
            this.grid.CellRenderers.Remove("Numeric");
            //Adds Custom renderer.
            this.grid.CellRenderers.Add("Numeric", new GridCellNumericRendererExt());
        }

    }

    public class GridCellNumericRendererExt : GridCellNumericRenderer
    {
        protected override void OnEditElementLoaded(object sender, RoutedEventArgs e)
        {
            var uiElement = ((DoubleTextBox)sender);
            uiElement.ValueChanged += OnValueChanged;

            uiElement.Focus();

            if ((this.DataGrid.EditorSelectionBehavior == EditorSelectionBehavior.SelectAll || this.DataGrid.IsAddNewIndex(this.CurrentCellIndex.RowIndex)) && PreviewInputText == null)
            {
                uiElement.SelectAll();
            }
            else
            {
                if (PreviewInputText == null || char.IsLetter(PreviewInputText.ToString(), 0))
                {
                    var index = uiElement.Text.Length;
                    uiElement.Select(index + 1, 0);
                    return;
                }
                double value;
                double.TryParse(PreviewInputText.ToString(), out value);
                uiElement.Value = value;
                uiElement.Text = PreviewInputText.ToString();
                if (PreviewInputText == ".")
                    uiElement.Text = "0.00";
                var caretIndex = uiElement.Text.IndexOf(PreviewInputText.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal);
                uiElement.Select(caretIndex + 1, 0);
            }
            PreviewInputText = null;
       }

        private void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            base.CurrentRendererValueChanged();
        }

        protected override void OnWireEditUIElement(DoubleTextBox uiElement)
        {
            uiElement.ValueChanged -= OnValueChanged;
        }
    }

    public class SfDataGridExt : SfDataGrid
    {
        public SfDataGridExt()
            : base()
        {
        }       
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            if (!SelectionController.CurrentCellManager.HasCurrentCell)
            {
                base.OnTextInput(e);
                return;
            }
            var rowColumnIndex = SelectionController.CurrentCellManager.CurrentRowColumnIndex;
            var row = this.ResolveToRowIndex(rowColumnIndex.RowIndex);       
            var dataRow = this.RowGenerator.Items.FirstOrDefault(item => item.RowIndex == rowColumnIndex.RowIndex);                              
            if (dataRow != null && dataRow is DataRow)
            {
                var dataColumn = dataRow.VisibleColumns.FirstOrDefault(column => column.ColumnIndex == rowColumnIndex.ColumnIndex);                
                char text;
                char.TryParse(e.Text, out text);
                if (dataColumn != null && !(dataColumn.GridColumn is GridTemplateColumn) && !dataColumn.IsEditing && SelectionController.CurrentCellManager.BeginEdit())
                    dataColumn.Renderer.PreviewTextInput(e);
            }
            base.OnTextInput(e);
        }
    }
}
