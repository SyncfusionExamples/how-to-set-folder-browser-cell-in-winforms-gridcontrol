using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using Syncfusion.Diagnostics;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.GridHelperClasses;
using System.IO;

namespace CellGrid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.gridControl1.DefaultColWidth = 120;
            this.gridControl1.DefaultRowHeight = 35;

            // Add the custom cell type to the CellModels of the GridControl.
            // The name we give at the argument is used to set the CellType name.
            this.gridControl1.CellModels.Add("FolderBrowser", new FolderBrowserCellModel(gridControl1.Model));
            // Set the cell type to "FolderBrowser"
            this.gridControl1[2, 3].Text = "Browse here";
            this.gridControl1[2, 3].CellType = "FolderBrowser";
        }
       
    }

    #region Browse Button
    // Draws the button with "..." style
    public class BrowseButton : GridCellButton
    {
        static GridIconPaint icon;

        static BrowseButton()
        {
            icon = new GridIconPaint("InteractiveCellDemo.", typeof(BrowseButton).Assembly);
        }

        public BrowseButton(GridTextBoxCellRenderer control)
            :base(control)
        {}

        public override void Draw(Graphics g, int rowIndex, int colIndex, bool bActive, GridStyleInfo style)
        {
            TraceUtil.TraceCurrentMethodInfo(rowIndex, colIndex, bActive, style.CellValue);

            base.Draw(g, rowIndex, colIndex, bActive, style);

            bool hovering = IsHovering(rowIndex, colIndex);
            bool mouseDown = IsMouseDown(rowIndex, colIndex);
            bool disabled = !style.Clickable;

            ButtonState buttonState = ButtonState.Normal;
            if (disabled)
                buttonState |= ButtonState.Inactive | ButtonState.Flat;

            else if (!hovering && !mouseDown)
                buttonState |= ButtonState.Flat;

            Point ptOffset = Point.Empty;
            if (mouseDown)
            {
                ptOffset = new Point(1, 1);
                buttonState |= ButtonState.Pushed;
            }

            DrawButton(g, Bounds, buttonState, style);

            Bitmap bitmapName = new Bitmap(Image.FromFile(@"..\..\Browse.bmp"));
            icon.PaintIcon(g, Bounds, ptOffset, bitmapName, Color.White);

        }
    }
    #endregion

    #region FolderBrowserCellModel
    //Deriving GridTextBoxCellModel 
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
        { }
        //Override the CreateRenderer() in the Base class.
        public override GridCellRendererBase CreateRenderer(GridControlBase control)
        {
            //Return the Custom Renderer Object
            return new FolderBrowserCellRenderer(control, this);
        }
    }
    #endregion

    #region FolderBrowserCellRenderer
    //Deriving the GridTextBoxCellRenderer class
    public class FolderBrowserCellRenderer : GridTextBoxCellRenderer
    {
        //FolderBrowser object decleration.
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
            if(folderBrowser1.ShowDialog() == DialogResult.OK)
            {
                string filePath = folderBrowser1.FileName;
            }
        }
        /// <override/>
        protected override Rectangle OnLayout(int rowIndex, int colIndex, GridStyleInfo style, Rectangle innerBounds, Rectangle[] buttonsBounds)
        {
            // Trace the properties of the cell
            TraceUtil.TraceCurrentMethodInfo(rowIndex, colIndex, style, innerBounds, buttonsBounds);
            Rectangle rightArea;
            if (this.Grid.IsRightToLeft())
            {
                rightArea = Rectangle.FromLTRB(innerBounds.Left, innerBounds.Top, innerBounds.Left + 20, innerBounds.Bottom);
                innerBounds.X += 20;

            }
            else
            {
                // Here you specify where the button should be drawn within the cell.
                rightArea = Rectangle.FromLTRB(innerBounds.Right - 20, innerBounds.Top, innerBounds.Right, innerBounds.Bottom);

            }
            innerBounds.Width -= 20;
            buttonsBounds[0] = GridUtil.CenterInRect(rightArea, new Size(20, 20));
            
            // Here you return the area where the text should be drawn/edited.

            return innerBounds;
        }
        #endregion
    }
    #endregion
}
