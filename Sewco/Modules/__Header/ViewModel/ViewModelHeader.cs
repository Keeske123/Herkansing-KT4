using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sewco.Resources.Helper_classes;

namespace Sewco.Modules.Header
{
    

    public class ViewModelHeader : ObservableObject
    {
        public ViewModelHeader()
        {
            //header = new clHeader();
        }

        //public clHeader header { get; set; }
        private string _sLabelContent;
        public string sLabelContent
        {
            get
            {
                return _sLabelContent;
            }
            set
            {
                _sLabelContent = value;
                OnPropertyChanged("sLabelContent");
            }
        }

        void doSomething()
        {
            sLabelContent = sLabelContent + "voilàHeader";

            //header.sModuleName += "hoi";
        }
    }
}
