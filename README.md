# How to customize edit mode behavior of GridCell in SfDataGrid


In <b>SfDataGrid</b>, you can directly go to the edit mode in Grid Cell by pressing any letter or digit as an input from the keyboard.

By default, <b>SfDataGrid</b> does not allow the Grid Cell to go to edit mode while pressing the Minus (-) key or any special character. You can overcome this behavior by customizing the SfDataGrid class, and overriding its OnTextInput () method.
<i>
```
Note:By default, GridTemplateColumn goes to edit mode while pressing the F2 key when you load the EditTemplate for it. No other letters or digits allow edit mode for GridTemplateColumn, GridCheckBoxColumn, GridImageColumn, GridHyperlinkColumn and GridUnboundColumn.
```
</i>
The following code example illustrates how to override the SfDataGrid class and customize the editing behavior in OnTextInput method.

## C#

```C#
public class SfDataGridExt : SfDataGrid
{
     public SfDataGridExt() : base()
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
```

In the above code example, the OnTextInput () method is fired when the input is received from the keyboard for Grid Column’s cell. With the help of <b>CurrentRowColumnIndex</b> in <b>CurrentCellManager</b> of <b>SelectionController</b>, you can get the current column from VisibleColumns collection that receives the input from keyboard.

The editing is skipped when the current column is <b>GridTemplateColumn</b> and it is already in edit mode. With the existing condition, you need to include the condition to allow negative number also.

The above customized <b>SfDataGrid</b> should be loaded in Xaml for implementing the editing behavior in Grid Cell. Refer the following code example.

## XAML

```XAML
<local:SfDataGridExt x:Name="grid"
                           ItemsSource="{Binding Students}"
                           AllowResizingColumns="True"
                           AllowResizingHiddenColumns="False"
                           AllowDraggingColumns="True" 
                           GridValidationMode="InView"
                           AutoGenerateColumns="False"                          
                           AllowEditing="True">
            <local:SfDataGridExt.Columns>
                <syncfusion:GridTextColumn HeaderText="Id" MappingName="Id"/>
                <syncfusion:GridTextColumn HeaderText="Name" MappingName="Name"  
                                                               TextWrapping="Wrap"  />
                <syncfusion:GridNumericColumn HeaderText="Salary"  
                                                                      MappingName="Salary"     />
                <syncfusion:GridNumericColumn HeaderText="GradePoint Average" 
                                                                    MappingName="GradePointAverage" />
            </local:SfDataGridExt.Columns>
</local:SfDataGridExt >
```
