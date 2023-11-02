using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace EX3
{
    /// <summary>
    /// Summary description for MyMenu.
    /// </summary>
    [Guid("b3900182-7d1e-4e5f-8a07-d84dc2b32d3b")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("EX3.MyMenu")]
    public sealed class MyMenu : BaseMenu
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
            ControlsMenus.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsMenus.Unregister(regKey);
        }

        #endregion
        #endregion

        public MyMenu()
        {
            //
            // TODO: Define your menu here by adding items
            //            
            //AddItem("esriControls.ControlsMapZoomInFixedCommand");
            //BeginGroup(); //Separator
            //AddItem("{380FB31E-6C24-4F5C-B1DF-47F33586B885}"); //undo command
            //AddItem(new Guid("B0675372-0271-4680-9A2C-269B3F0C01E8")); //redo command
        }

        public override string Caption
        {
            get
            {
                //TODO: Replace bar caption
                return "My C# Menu";
            }
        }
        public override string Name
        {
            get
            {
                //TODO: Replace bar ID
                return "MyMenu";
            }
        }
    }
}