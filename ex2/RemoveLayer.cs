using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ex2
{
    /// <summary>
    /// Summary description for RemoveLayer.
    /// </summary>
    [Guid("644080d2-d119-41c2-8c6b-91e2b0210db1")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ex2.RemoveLayer")]
    public sealed class RemoveLayer : BaseCommand
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

        public RemoveLayer()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "Remove layer";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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
            if (hook == null)
                return;

            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
            _mapControl = (IMapControl3)hook;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add RemoveLayer.OnClick implementation
            //可以使用customproperty来直接传递图层的index，然后直接用index即可修改图层
            //但是因为在命令被点击之前并不知道是要zoom layer还是remove layer，所以不知道到底往custompropery里面放图层还是图层index
            //这里就只能比较传入进来的图层和mapcontrol里面的那个图层匹配，然后就删除对应图层
            int i = 0;
            int count=_mapControl.LayerCount;
            ILayer layer = (ILayer)_mapControl.CustomProperty;
            while (i < count)
            {
                if (layer== _mapControl.Layer[i])
                {
                    _mapControl.DeleteLayer(i);
                    break;
                    //如果不break的话
                    //因为count还是被删除之前的count，所以会循环到不存在的图层导致下标越界 index out of range
                }
                i++;

            }

        }

        #endregion
    }
}
