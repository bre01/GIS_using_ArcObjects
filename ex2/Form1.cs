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

//在使用arcobjects的控件的时候，如果右键然后点击property并不能到其控件的C#相关属性
//只会被跳转到arcobjects的属性
//但是为控件挂载属性需要到C#相关属性
//所以先右键属性一个C#原生的控件，右侧菜单出现之后再点击相关的arcobjects的控件，右侧菜单就会发生变化，变成arcobjects控件的C#相关属性
//这是即可再里面挂载相关的event handler
//例如之后需要挂载TocControl的 OnLabelEditEnd事件
//Form1.cs的最底部
namespace ex2
{
    
    public partial class Form1 : Form
    {
        //those private variables will be used later 
        //for passing data to command

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
                //the axMapControls is created by draging mapControl to form
                //in form designer
                    axMapControl1.AddShapeFile(System.IO.Path.GetDirectoryName(fileName), System.IO.Path.GetFileName(fileName));



            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            axMapControl1.ClearLayers();
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
                //just create a mapDocument and import the data from mxd file
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
            //now using SaveDocumentAs() method
            //the hard code filename method has been deprecated and
            //only left here for example
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
            //check if it is the right click
            if (e.button != 2)
            {
                return;
            }
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap map = null;
            ILayer layer = null;
            object other = null;
            object index = null;
            //using the hitTest method to get the clickded item, or map, or layer ,etc
            //using the ref keyword to make the value stored in the variable declared before
            //使用hittest方法来得到点击中的item或者map，或者layer等
            //这里先在外面声明变量，然后使用ref关键词传递地址进去，
            //hittest方法会将结果的值放在传入的地址，之后就可以在外面直接使用之前声明的变量来得到值，
            //而不用接受函数的返回值
            _passTOCControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
            if (item == esriTOCControlItem.esriTOCControlItemMap)
            {
                //check if it's map being selected
                _passTOCControl.SelectItem(map, null);
            }

            else
            {
                //or layer
                
                _passTOCControl.SelectItem(layer, null);
            }
            //store the selected layer into CustomProperty,
            //when the layer is needed, cast it out 
            //!!!!!!!!
            //！！！！！！
            //将选定的图层存在CustomProperty里面
            //CustomProperty是一个Ojbect类型的值，可以存任何类型的值，
            //但是在需要使用的时候需要cast成期待的值
            _passMapControl.CustomProperty = layer;
                
            //pop up the Menu at the right clicked place
            //在右键的位置弹出菜单
            if (item == esriTOCControlItem.esriTOCControlItemLayer)
                _TocRightClick.PopupMenu(e.x, e.y, _passTOCControl.hWnd);


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //what's the point of this?
            //I get it now, when can not pass axTOCControl to hook
            //in that way we can not pass the layer stored in the customProperty 
            
            //虽然我们有了axMapcontrol变量，这个变量在designer里面通过拖拽创建，
            //但是因为AxMapControl类型没有customProperty, 我们没有办法传递选中的图层，
            //所以需要一个IMapControl3类型的图层，这个图层又axMapControl cast而来，
            //有轻微的不同，之后我们在图层中都使用这个变量而不是直接使用axMapControl，（Form1_Load在窗口启动是就会运行，就像是初始化——
            _passTOCControl = (ITOCControl2)axTOCControl1.Object;
            _passMapControl = (IMapControl3)axMapControl1.Object;


            //make a new Menu
            //创建一个新菜单
            _TocRightClick = new ToolbarMenuClass();
            //将我们创建的命令添加到菜单中
            
            //and add the Menu command created to the menu
            _TocRightClick.AddItem(new RemoveLayer(), -1, 0, false, esriCommandStyles.esriCommandStyleTextOnly);
            _TocRightClick.AddItem(new ZoomToLayer(), -1, 1, true, esriCommandStyles.esriCommandStyleTextOnly);

            //我试了试添加多个要素到一个图层，很不容易实现，算了
            //these adding multiple feature to one layer thing is tricky
            //maybe not use it 
            _TocRightClick.AddItem(new AddFeatureToLayer(), -1, 2, true, esriCommandStyles.esriCommandStyleTextOnly);


            //why make a additional variable? it's only used when passing
            // I can cast it when passing 
            //.... yeah, i can not pass the custom property if I just cast when passing;
            //the additional variable _passMapControl, _passTocControl is needed...
            //
            //让这个_passMapControl被菜单里面的命令被钩住，从而命令就可以访问这个Object

            _TocRightClick.SetHook(_passMapControl);
            //set _passMapControl to hook, (passing data)
            //so that i can use the passed data(_passMapControl) in command
            //eg:
            /*
                public override void OnCreate(object hook)
                {
                        _mapControl = (IMapControl3)hook;

                }*/
        }
        private void axTOCControl1_OnEndLabelEdit(object sender, ITOCControlEvents_OnEndLabelEditEvent e)
        {
            //如果结果为空，则马上打断 
            //这个事件处理器需要挂载到axTOCContro的OnEndLableEdit上面
            if ((string)e.newLabel == "")
            {
                e.canEdit = false;
                
                MessageBox.Show("can not be empty");
            }
        }
    }
}
