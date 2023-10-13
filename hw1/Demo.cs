using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw1
{

    interface IToolbarControl
    {
        IToolbarItem IToolbarItem { get; set; }

        IToolbarItem GetItem(int index);

    }
    class Item : IToolbarItem
    {
        public ICommand Command { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ToolbarMenu Menu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
    class ToolbarContorl1 : IToolbarControl
    {
        public IToolbarItem IToolbarItem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        internal Item Item
        {
            get => default;
            set
            {
            }
        }

        public IToolbarItem GetItem(int index)
        {
            return new Item();
        }
    }
    class Command : ICommand
    {
        public bool Checked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Selected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }




    interface IToolbarItem
    {
        ICommand Command { get; set; }
        ToolbarMenu Menu { get; set; }

    }
    interface ICommand
    {
        bool Checked { get; set; }
        bool Selected { get; set; }
    }
}
