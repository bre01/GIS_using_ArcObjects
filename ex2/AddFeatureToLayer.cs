using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ex2
{
    /// <summary>
    /// Summary description for AddFeatureToLayer.
    /// </summary>
    [Guid("db209952-2561-418d-803e-e652b2dbbf27")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ex2.AddFeatureToLayer")]
    public sealed class AddFeatureToLayer : BaseCommand
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

        public AddFeatureToLayer()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "add feature";  //localizable text
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
            _mapControl = (IMapControl3)hook;
            if (hook == null)
                return;

            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            ILayer layer = (ILayer)_mapControl.CustomProperty;
            var od = new OpenFileDialog();
            if(od.ShowDialog()==DialogResult.OK)
            {
                AddFeaturesToExistingLayer(_mapControl, layer,od.FileName);

            }
            // TODO: Add AddFeatureToLayer.OnClick implementation
        }

        #endregion 
        public void AddFeaturesToExistingLayer(IMapControl3 mapControl,ILayer addToLayer,string shapefilePath)
        {
            // Get the feature layer you want to add features to.
            IFeatureLayer featureLayer = null;

            for (int i = 0; i < mapControl.LayerCount; i++)
            {
                var layer = mapControl.get_Layer(i);
                if (layer is IFeatureLayer && layer==addToLayer)
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
            mapControl.Refresh();
            MessageBox.Show("done");
        }

    }
}
