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
            //创建一个新的实现ITool的类
            //通过add item，然后选择Base Tool

            ITool customTool = new ZoomIn();
            MessageBox.Show(customTool.GetHashCode().ToString());
            axToolbarControl1.AddItem(customTool);
            //还创建了一个base command用来比较command和tool的不同

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
            //首先通过GetItem来获得第二个Item，第一个是通过右击属性来手动添加的Zoom in Tool
            //第二个即Add Item添加到Tool
            //GetItem（）返回一个实现了IToobarItem的对象
            //IToolbarItem有成员属性Command 
            //这是一个只读属性，当尝试获取IToolbarItem.Command时，会获得一个对象，这个对象实现了ICommand接口
            //这里文档的描述是
            //IToolbarItem.Command Property:
            //Returns the ICommand object used by the Command, Tool or ToolControl item. If the item is using a Menu, an IMultiItem object or Palette Nothing is returned.
            //所以说如果如果在IToolbarItem这个里面的"东西"是一个Tool类的实例，那么读取Command属性，
            //会得到一个Tool类实例（对象），这个Tool实例当然是实现了ICommand的，(神奇吧，Tool是实现了ICommand的)
            //同时，这个Tool类实例，也是实现了ITool的,
            // Actually,这个ITool实例就是我们在56行创建的customTool，
            //如果你按F12，进入ZoomIn的定义，会发现我们生成的ZoomIn base tool，
            //是继承了BaseTool这个抽象类的，
            //而如果继续按F12看BaseTool的定义 
            //可以看到BaseTool实现了接口Itool
            //同时还继承了BaseCommand,而BaseCommand实现了接口额ICommand
            //所以这个ZoomIn类，是实现了ITool和ICommand的
            //如果在文档中搜索Tool class也可以得到佐证

            //至于如何确定读取IToolbarItem.Command属性时，得到的是我们在56行通过ZoomIn类来
            //得到的CustomTool       " ITool customTool = new ZoomIn();"
            //可以得到新的对象customTool之后打印其哈希码 
            //对比调用Command的得到的值的哈希码来确定 这两个对象是完全一样的
            //这两个打印的代码分别在57行和114行，取消注释之后即可在打开应用和点击mapControl的时候分别弹出
            //两次的哈希码
            MessageBox.Show(axToolbarControl1.GetItem(1).Command.GetHashCode().ToString());
            if (axToolbarControl1.GetItem(1).Command.Enabled)
            {
                //这里有一个坑就是Command属性
                //在这里，调用Command的属性，得到的是一个ZoomIn实例
                //这个实例
                //有Selected和Enabled两个属性，
                //在使用应用时，如果点击一个Tool对应的区域，区域会变暗
                //这时Command的Enabled属性会被设为True，
                //   ！！！竟然是Enabled，而不是Selected！！！
                //这是一个很大的confusion

                                //ZoomIn实例和Command1实例的比较



                //仅仅继承了BaseCommand类的ToolbarItem和继承了BaseTool
                //(继承BaseTool即实现了ITool和ICommand,而如果只是继承BaseCommand就只实现了了
                //ICommand)的ToolbarItem有一个显著区别就是,虽然都能够被添加到ToolbarControl中

                //但是只是继承了BaseCommand的类不能被选中，只能被点击

                //这个项目中的第一二个是Tool，而第三个是Command，

                

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
