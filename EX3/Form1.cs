using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EX3;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;

namespace EX3
{
    public partial class Form1 : Form
    {
        private EngineContextMenu1 _menu;
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Desktop);
            InitializeComponent();
            _menu = new EngineContextMenu1();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            axToolbarControl1.AddItem(new OpenDocument(axMapControl1), -1, -1, true, 0,
            esriCommandStyles.esriCommandStyleIconOnly);

            FullExtent fullExtentTool = new FullExtent();
            fullExtentTool.OnCreate(axMapControl1.Object);
            axToolbarControl1.AddItem(fullExtentTool, -1, -1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);

            ZoomIn zoomInTool = new ZoomIn();
            zoomInTool.OnCreate(axMapControl1.Object); // 关联地图控件
            axToolbarControl1.AddItem(zoomInTool, -1, -1, true, 0,
            esriCommandStyles.esriCommandStyleIconOnly);

            axToolbarControl1.AddItem(new SaveAsDocument(axMapControl1), -1, -1, true,
                0, esriCommandStyles.esriCommandStyleIconOnly);

            ICommand loadLayerCommand = new OpenCommand();
            // 将LoadLayerCommand添加到ToolbarControl1
            axToolbarControl1.AddItem(loadLayerCommand, -1, -1, false, 0,
              esriCommandStyles.esriCommandStyleIconOnly);
        }

        private void axToolbarControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IToolbarControlEvents_OnMouseDownEvent e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Form1_Click(object sender, EventArgs e)
        {

        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 2)
            {

                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap map = null;
                ILayer layer = null;
                object other = null;
                object index = null;
                axTOCControl1.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
                MessageBox.Show(layer.Name.ToString());
                var pass = (IMapControl3)axMapControl1.Object;
                pass.CustomProperty = layer; 
                _menu.SetHook(pass);
                _menu.PopupMenu(e.x, e.y, axTOCControl1.hWnd);
            }

        }
    }
}
