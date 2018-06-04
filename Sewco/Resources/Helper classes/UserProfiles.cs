using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;

namespace Sewco.Resources
{
    class UserProfiles : INotifyPropertyChanged
    {
        #region Private Properties

        private string rightsName;
        private bool productions;
        private bool machineConfig;
        private bool products;
        private bool users;
        private bool reset;
        private bool reprint;
        private bool maintenance;
        private bool find;
        private bool desktop;
        private bool materials;
        private bool productProfile;
        private bool labelEditor;
        private bool bobbinMonitor;
        private bool bobbinTracer;

        LinqToSQLDataContext db = new LinqToSQLDataContext();
        #endregion

        #region Public Properties

        public string sRightsName
        {
            get
            {
                return rightsName;
            }
            set
            {
                if (rightsName != value)
                {
                    rightsName = value;
                    RaisePropertyChanged("sRightsName");
                }
            }
        }
        public bool xProductions
        {
            get
            {
                var query =
                    from t in db.tbl_UserProfiles
                    where t.Userprofile == rightsName
                    select t.Productions;

                foreach (var q in query)
                {
                    productions = q.Value;
                    return q.Value;
                }
                return productions;
            }
            set
            {
                if (productions != value)
                {
                    productions = value;
                    RaisePropertyChanged("xProductions");
                }
            }
        }
        public bool xMachineConfig
        {
            get
            {
                return machineConfig;
            }
            set
            {
                if (machineConfig != value)
                {
                    machineConfig = value;
                    RaisePropertyChanged("xMachineConfig");
                }
            }
        }
        public bool xProducts
        {
            get
            {                
                return products;
            }
            set
            {
                if (products != value)
                {
                    products = value;
                    RaisePropertyChanged("xProducts");
                }
            }
        }
        public bool xUsers
        {
            get
            {
                var query =
                    from t in db.tbl_UserProfiles
                    where t.Userprofile == rightsName
                    select t.Users;

                foreach (var q in query)
                {
                    users = q.Value;
                    return q.Value;
                }
                return users;
            }
            set
            {
                if (users != value)
                {
                    users = value;
                    RaisePropertyChanged("xUsers");
                }
            }
        }
        public bool xReset
        {
            get
            {
                var query =
                    from t in db.tbl_UserProfiles
                    where t.Userprofile == rightsName
                    select t.Reset;

                foreach (var q in query)
                {
                    reset = q.Value;
                    return q.Value;
                }
                return reset;
            }
            set
            {
                if (reset != value)
                {
                    reset = value;
                    RaisePropertyChanged("xReset");
                }
            }
        }
        public bool xReprint
        {
            get
            {
                var query =
                    from t in db.tbl_UserProfiles
                    where t.Userprofile == rightsName
                    select t.Reprint;

                foreach (var q in query)
                {
                    reprint = q.Value;
                    return q.Value;
                }
                return reprint;
            }
            set
            {
                if (reprint != value)
                {
                    reprint = value;
                    RaisePropertyChanged("xReprint");
                }
            }
        }
        public bool xMaintenance
        {
            get
            {
                var query =
                    from t in db.tbl_UserProfiles
                    where t.Userprofile == rightsName
                    select t.Maintenance;

                foreach (var q in query)
                {
                    maintenance = q.Value;
                    return q.Value;
                }
                return maintenance;
            }
            set
            {
                if (maintenance != value)
                {
                    maintenance = value;
                    RaisePropertyChanged("xMaintenance");
                }
            }
        }
        public bool xFind
        {
            get
            {
                var query =
                   from t in db.tbl_UserProfiles
                   where t.Userprofile == rightsName
                   select t.Find;

                foreach (var q in query)
                {
                    find = q.Value;
                    return q.Value;
                }
                return find;
            }
            set
            {
                if (find != value)
                {
                    find = value;
                    RaisePropertyChanged("xFind");
                }
            }
        }
        public bool xDesktop
        {
            get
            {
                var query =
                   from t in db.tbl_UserProfiles
                   where t.Userprofile == rightsName
                   select t.Desktop;

                foreach (var q in query)
                {
                    desktop = q.Value;
                    return q.Value;
                }
                return desktop;
            }
            set
            {
                if (desktop != value)
                {
                    desktop = value;
                    RaisePropertyChanged("xDesktop");
                }
            }
        }
        public bool xMaterials
        {
            get
            {
                var query =
                   from t in db.tbl_UserProfiles
                   where t.Userprofile == rightsName
                   select t.Materials;

                foreach (var q in query)
                {
                    materials = q.Value;
                    return q.Value;
                }
                return materials;
            }
            set
            {
                if (materials != value)
                {
                    materials = value;
                    RaisePropertyChanged("xMaterials");
                }
            }
        }
        public bool xProductProfile
        {
            get
            {
                var query =
                   from t in db.tbl_UserProfiles
                   where t.Userprofile == rightsName
                   select t.ProductProfile;

                foreach (var q in query)
                {
                    productProfile = q.Value;
                    return q.Value;
                }
                return productProfile;
            }
            set
            {
                if (productProfile != value)
                {
                    productProfile = value;
                    RaisePropertyChanged("xProductProfile");
                }
            }
        }
        public bool xLabelEditor
        {
            get
            {
                var query =
                   from t in db.tbl_UserProfiles
                   where t.Userprofile == rightsName
                   select t.LabelEditor;

                foreach (var q in query)
                {
                    labelEditor = q.Value;
                    return q.Value;
                }

                return labelEditor;
            }
            set
            {
                if (labelEditor != value)
                {
                    labelEditor = value;
                    RaisePropertyChanged("xLabelEditor");
                }
            }
        }
        public bool xBobbinMonitor
        {
            get
            {
                var query =
                   from t in db.tbl_UserProfiles
                   where t.Userprofile == rightsName
                   select t.BobbinMonitor;

                foreach (var q in query)
                {
                    bobbinMonitor = q.Value;
                    return q.Value;
                }
                return bobbinMonitor;
            }
            set
            {
                if (bobbinMonitor != value)
                {
                    bobbinMonitor = value;
                    RaisePropertyChanged("xBobbinMonitor");
                }
            }
        }
        public bool xBobbinTracer
        {
            get
            {
                var query =
                   from t in db.tbl_UserProfiles
                   where t.Userprofile == rightsName
                   select t.BobbinTracer;

                foreach (var q in query)
                {
                    bobbinTracer = q.Value;
                    return q.Value;
                }

                return bobbinTracer;
            }
            set
            {
                if (bobbinTracer != value)
                {
                    bobbinTracer = value;
                    RaisePropertyChanged("xBobbinTracer");
                }
            }
        }

        #endregion

        public void SetRights(string rightsName)
        {
            var query =
                from t in db.tbl_UserProfiles
                where t.Userprofile == rightsName
                select t;

            foreach (var t in query)
            {
                sRightsName = t.Userprofile;
                xProductions = t.Productions.Value;
                xMachineConfig = t.MachineConfig.Value;
                xProducts = t.Products.Value;
                xUsers = t.Users.Value;
                xReset = t.Reset.Value;
                xReprint = t.Reprint.Value;
                xMaintenance = t.Maintenance.Value;
                xFind = t.Find.Value;
                xDesktop = t.Desktop.Value;
                xMaterials = t.Materials.Value;
                xProductProfile = t.ProductProfile.Value;
                xLabelEditor = t.LabelEditor.Value;
                xBobbinMonitor = t.BobbinMonitor.Value;
                xBobbinTracer = t.BobbinTracer.Value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}
