using ESRI.ArcGIS.Carto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EX3
{
    public partial class Property : Form
    {
        ILayer _layer;

        public Property(ILayer layer)
        {
            _layer = layer;
            InitializeComponent();
            textBox1.Text = layer.Name;
            if (layer is IFeatureLayer)
                textBox2.Text = ((IFeatureLayer2)layer).ShapeType.ToString();
            else
            {
                textBox2.Text = "Raster";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
                     
            
        }
    }
}
