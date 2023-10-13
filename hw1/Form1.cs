using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
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

namespace hw1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Desktop);
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
                //the axMapControls is created by draging mapControl to form
                //in form designer
                axMapControl1.AddShapeFile(System.IO.Path.GetDirectoryName(fileName), System.IO.Path.GetFileName(fileName));



            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmdLoadshpf_Click(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            ITool customTool = new ZoomIn();
            axToolbarControl1.AddItem(new ZoomIn());
            axToolbarControl1.AddItem(new Command1());
        }
        /*
        private void mapControl_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 1) // Left mouse button
            {
                IEnvelope newExtent = axMapControl1.Extent; // Get the current extent

                // Define a zoom factor (adjust this as needed)
                double zoomFactor = 0.5;

                // Calculate the new extent based on the click point
                double centerX = (e.mapX + axMapControl1.Extent.XMin) / 2;
                double centerY = (e.mapY + axMapControl1.Extent.YMin) / 2;
                newExtent.XMin = centerX - (newExtent.Width * zoomFactor / 2);
                newExtent.XMax = centerX + (newExtent.Width * zoomFactor / 2);
                newExtent.YMin = centerY - (newExtent.Height * zoomFactor / 2);
                newExtent.YMax = centerY + (newExtent.Height * zoomFactor / 2);

                // Set the new extent to the MapControl
                axMapControl1.Extent = newExtent;
            }
        }
        */
        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            if (axToolbarControl1.GetItem(1).Command.Enabled)
            {
                /*if (e.button == 1)
                {
                    axMapControl1.Extent = axMapControl1.TrackRectangle();
                }*/
                if (e.button == 1) // Left mouse button
                {
                    IEnvelope newExtent = axMapControl1.Extent; // Get the current extent

                    // Define a zoom factor (adjust this as needed)
                    double zoomFactor = 0.5;

                    // Calculate the new extent based on the click point
                    double centerX = e.mapX ;
                    double centerY = e.mapY;
                    newExtent.XMin = centerX - (newExtent.Width * zoomFactor / 2);
                    newExtent.XMax = centerX + (newExtent.Width * zoomFactor / 2);
                    newExtent.YMin = centerY - (newExtent.Height * zoomFactor / 2);
                    newExtent.YMax = centerY + (newExtent.Height * zoomFactor / 2);
                    axMapControl1.Extent = newExtent;

                }

            }
        }

        
       

        private void button1_Click_1(object sender, EventArgs e)
        {
            cmdLoadshpf_Click(sender,e);
        }
    }

}
