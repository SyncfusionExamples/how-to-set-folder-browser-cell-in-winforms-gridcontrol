# How to set folder browser cell in winforms gridcontrol

This example demonstrates how to set folder browser cell in [WinForms GridControl](https://www.syncfusion.com/winforms-ui-controls/grid-control).

To use the `FolderBrowser` cell type, create custom `FolderBrowserCellModel` and `FolderBrowserCellRenderer` derived from [GridTextBoxCellModel](https://help.syncfusion.com/cr/windowsforms/Syncfusion.Windows.Forms.Grid.GridTextBoxCellModel.html) and [GridTextBoxCellRenderer](https://help.syncfusion.com/cr/windowsforms/Syncfusion.Windows.Forms.Grid.GridTextBoxCellRenderer.html). The Folder Browser dialog box will be displayed in the [OnButtonClicked](https://help.syncfusion.com/cr/windowsforms/Syncfusion.Windows.Forms.Grid.GridCellRendererBase.html#Syncfusion_Windows_Forms_Grid_GridCellRendererBase_OnButtonClicked_System_Int32_System_Int32_System_Int32_) method.

### Creating CustomCellModel

``` c#
//Deriving  GridTextBoxCellModel. 
public class FolderBrowserCellModel : GridTextBoxCellModel
{
    protected FolderBrowserCellModel(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        //Set the button bar size.
        base.ButtonBarSize = new Size(20, 20);
    }
    //Constructor for the Derived Class
    public FolderBrowserCellModel(GridModel grid)
        : base(grid)
    { 
 
    }
    //Override the CreateRenderer() in the Base class.
    public override GridCellRendererBase CreateRenderer(GridControlBase control)
    {
        //Return the Custom Renderer Object
        return new FolderBrowserCellRenderer(control, this);
    }
}
```

### Creating CustomCellRenderer

``` c#
//Deriving the GridTextBoxCellRenderer.
public class FolderBrowserCellRenderer : GridTextBoxCellRenderer
{
    //FolderBrowser object declaration.
    private System.Windows.Forms.OpenFileDialog folderBrowser1;
    public FolderBrowserCellRenderer(GridControlBase grid, GridTextBoxCellModel cellModel)
        : base(grid, cellModel)
    {
        AddButton(new BrowseButton(this));
        //Initialize the folderBrowser1 object.
        this.folderBrowser1 = new System.Windows.Forms.OpenFileDialog();
    }
    #region [overrides]        
    protected override void OnButtonClicked(int rowIndex, int colIndex, int button)
    {
        base.OnButtonClicked(rowIndex, colIndex, button);
        if(folderBrowser1.ShowDialog()== DialogResult.OK)
        {
            string filePath = folderBrowser1.FileName;     
        }         
    }    
}
```

### Adding CellModel

``` c#
// Add the custom cell type to the CellModels of the GridControl.
this.gridControl1.CellModels.Add("FolderBrowser", new FolderBrowserCellModel(gridControl1.Model));
// Set the cell type to "FolderBrowser"
this.gridControl1[2, 3].Text = "Browse here";
this.gridControl1[2, 3].CellType = "FolderBrowser";
```