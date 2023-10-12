using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;

namespace ex2
{
    public partial class Form1 : Form
    {
        private ITOCControl2 _passTOCControl;
        private IMapControl3 _passMapControl;
        private IToolbarMenu m_menuMap;
        private IToolbarMenu _TocRightClick;
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            InitializeComponent();
        }

        private void cmdLoadshpf_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "shp files(*.shp)|*.shp";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("ok");
                Console.WriteLine(openFileDialog1);
                string fileName = openFileDialog1.FileName;
                System.IO.Path.GetFileName(fileName);
                if (axMapControl1.LayerCount != 1)
                {

                    axMapControl1.AddShapeFile(System.IO.Path.GetDirectoryName(fileName), System.IO.Path.GetFileName(fileName));


                }
                else
                {
                    //adding feature to a exsiting layer would take siginificant effort
                    //Maybe i just add a new layer;
                    var layer = axMapControl1.get_Layer(0);
                    Console.WriteLine(axMapControl1.LayerCount);
                    axMapControl1.AddShapeFile(System.IO.Path.GetDirectoryName(fileName), System.IO.Path.GetFileName(fileName));
                    Console.WriteLine(axMapControl1.LayerCount);
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            axMapControl1.ClearLayers();
        }
        public void AddFeaturesToExistingLayer(string shapefilePath)
        {
            // Get the feature layer you want to add features to.
            IFeatureLayer featureLayer = null;

            for (int i = 0; i < axMapControl1.LayerCount; i++)
            {
                var layer = axMapControl1.get_Layer(i);
                if (layer is IFeatureLayer && layer.Name == "YourLayerName")
                {
                    featureLayer = (IFeatureLayer)layer;
                    break;
                }
            }

            if (featureLayer == null)
            {
                // Handle the case where the layer with the specified name is not found.
                return;
            }

            // Open an edit session.
            IWorkspaceEdit workspaceEdit = ((IDataset)featureLayer.FeatureClass).Workspace as IWorkspaceEdit;
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();

            try
            {
                // Create a feature cursor for inserting features.
                IFeatureCursor featureCursor = featureLayer.FeatureClass.Insert(true);
                IFeatureBuffer featureBuffer = featureLayer.FeatureClass.CreateFeatureBuffer();

                // Iterate through your shapefile and add features to the feature class.
                // You will need to open and read your shapefile to get the feature geometries and attributes.

                // Example:
                // IFeature feature = ... // Create a new feature with the appropriate geometry and attributes.
                // featureBuffer.Shape = feature.Shape; // Set the geometry of the feature buffer.
                // featureBuffer.set_Value(index, value); // Set attribute values.

                // Insert the feature into the feature class.
                featureCursor.InsertFeature(featureBuffer);
                featureCursor.Flush();

                // Commit the edit operation and stop the edit session.
                workspaceEdit.StopEditOperation();
                workspaceEdit.StopEditing(true);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the editing process.
                // Roll back the edit operation if necessary.
                workspaceEdit.AbortEditOperation();
                workspaceEdit.StopEditing(false);
            }

            // Refresh the view to display the added features.
            axMapControl1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            openFileDialog1.Filter="mdx files(*.mxd)|*.mxd";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {


                IMapDocument mapDocument = new MapDocumentClass();
                /*if (mapDocument != null)
                {
                    mapDocument.Close();
                }*/
                mapDocument.Open(openFileDialog1.FileName);
                axMapControl1.Map = mapDocument.ActiveView as IMap;



            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveDocumentAs();
        }
        private void SaveDocument()
        {
            IMxdContents mxd = axMapControl1.Map as IMxdContents;
            IMapDocument mapDocument = new MapDocumentClass();
            mapDocument.New("C:/Users/bre/Documents/ForGisDev/a.mxd");
            mapDocument.ReplaceContents(mxd);
            mapDocument.Save(true, true);
            MessageBox.Show("saved !");
        }
        private void SaveDocumentAs()
        {
            saveFileDialog1.Filter="mdx files(*.mxd)|*.mxd";
            IMxdContents mxd = axMapControl1.Map as IMxdContents;
            IMapDocument mapDocument = new MapDocumentClass();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mapDocument.New(saveFileDialog1.FileName);
                mapDocument.ReplaceContents(mxd);
                mapDocument.Save(true, true);
                MessageBox.Show("saved !");
            }

        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button != 2)
            {
                return;
            }
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap map = null;
            ILayer layer = null;
            object other = null;
            object index = null;
            _passTOCControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
            if (item == esriTOCControlItem.esriTOCControlItemMap)
            {
                _passTOCControl.SelectItem(map, null);
            }

            else
            {
                _passTOCControl.SelectItem(layer, null);
            }
            _passMapControl.CustomProperty = layer;
                
            if (item == esriTOCControlItem.esriTOCControlItemLayer)
                _TocRightClick.PopupMenu(e.x, e.y, _passTOCControl.hWnd);


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //what's the point of this?
            _passTOCControl = (ITOCControl2)axTOCControl1.Object;
            _passMapControl = (IMapControl3)axMapControl1.Object;
            //  
            _TocRightClick = new ToolbarMenuClass();
            _TocRightClick.AddItem(new RemoveLayer(), -1, 0, false, esriCommandStyles.esriCommandStyleTextOnly);
            _TocRightClick.AddItem(new ZoomToLayer(), -1, 1, true, esriCommandStyles.esriCommandStyleTextOnly);
            //why make a additional variable? it's only used when passing
            // I can cast it when passing 
            _TocRightClick.SetHook(_passMapControl);
            //or I can pass the axMapControl1 and cast it when I need the more specific controls(TOCControl ,MapControl ...)

        }

        private void axTOCControl1_OnEndLabelEdit(object sender, ITOCControlEvents_OnEndLabelEditEvent e)
        {
            if ((string)e.newLabel == "")
            {
                e.canEdit = false;
                MessageBox.Show("can not be empty");
            }
        }
    }
}
