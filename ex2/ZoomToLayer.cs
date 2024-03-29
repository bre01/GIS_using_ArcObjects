using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ex2
{
    /// <summary>
    /// Summary description for ZoomToLayer.
    /// </summary>
    [Guid("075c6506-cb4c-44b2-ac4d-978eb604091b")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ex2.ZoomToLayer")]
    public sealed class ZoomToLayer : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion
        
        private IHookHelper m_hookHelper;

        private IMapControl3 _mapControl;

        public ZoomToLayer()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            base.m_caption = "Zoom to layer";//右键的时候会显示的命令标题
            
            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
           // The OnCreate method gives the command a hook into the application.

            //When implementing ICommand to create a custom command, use the OnCreate method to get a hook to the application.

            //override这个oncreate方法，在Command被创建的时候这个OnCreate会被调用
            //这个command命令会让这个命令挂一个Hook（钩子)到应用中（在这里就是Form1），
            //通过这个钩子可以获得应用中的对象

            _mapControl = (IMapControl3)hook;
            //就是通过这个
            // _TocRightClick.SetHook(_passMapControl);
            //来讲应用中的_passMapControl传递到命令中的，
            //同时_passMapControl有一个属性，CustomtyProperty，
            //通过这个属性的，将其cast成想要的类型，就可以得到选定的图层

            /*if (hook == null)
                return;

            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;*/

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            //通过cast得到选定的图层
            ILayer layer = (ILayer)_mapControl.CustomProperty;
            //将整个mapcontrol的范围设定为某个图层的AreaOfInterest
            //即可实现Zoom to layer的效果
            _mapControl.Extent = layer.AreaOfInterest;
            // TODO: Add ZoomToLayer.OnClick implementation
        }

        #endregion
    }
}
