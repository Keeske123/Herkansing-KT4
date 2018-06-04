using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Sewco.Modules.ControlPanel;
using Sewco.Resources.Helper_classes;




namespace Sewco.Modules.TypesEditor
{
    public class TypesBusinessObject : ObservableObject
    {
        public TypesBusinessObject()
        {
            _projectsConnectDB              = new projectsConnectLocalDB();
            _productsConnectDB              = new productsConnectLocalDB();
            _profilesConnectDB              = new profilesConnectLocalDB();
            _materialProductConnectDB       = new materialProductConnectLocalDB();
            _materialTypeConnectDB          = new materialTypeConnectLocalDB();
            _materialConnectDB              = new materialConnectLocalDB();
            _materialTypeMaterialConnectDB  = new materialTypeMaterialConnectLocalDB();
            _materialIOPositionConnectDB    = new materialIOPositionConnectLocalDB();
            _typesConnectDB                 = new typeConnectLocalDB();

            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__ProductCode")));
            sCode = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__TypeName")));
            sTypeName    = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__Comment")));
            sComment = new clTextBoxValidation(clInputErrors);
        }
        private int         iTypeId;
        private string      _sImagePath;
        private bool        _xActive;
        private ObservableCollection<clErrors> clInputErrors = new ObservableCollection<clErrors>();

        public clTextBoxValidation sTypeName
        {
            get; set;
        }
        public clTextBoxValidation sCode
        {
            get; set;
        }
        public clTextBoxValidation sComment
        {
            get; set;
        }
        public string sImagePath
        {
            get
            {
                return _sImagePath;
            }
            set
            {
                _sImagePath = value;
                OnPropertyChanged("sImagePath");
            }
        }
        public bool xActive
        {
            get
            {
                return _xActive;
            }
            set
            {
                _xActive = value;
                OnPropertyChanged("xActive");
            }
        }
        private int _iObjectSelectedValue;
        public int iObjectSelectedValue
        {
            get
            {
                return _iObjectSelectedValue;
            }
            set
            {
                _iObjectSelectedValue = value;
                OnPropertyChanged("iObjectSelectedValue");
            }
        }

        private ObservableCollection<clCbbFilltype1>    _obcTypes        { get; set; } = new ObservableCollection<clCbbFilltype1>();
        private ObservableCollection<clCbbFilltype1>    _obcCopyTypes    { get; set; } = new ObservableCollection<clCbbFilltype1>();

        
        private projectsConnectLocalDB              _projectsConnectDB;
        private productsConnectLocalDB              _productsConnectDB;
        private profilesConnectLocalDB              _profilesConnectDB;
        private materialProductConnectLocalDB       _materialProductConnectDB;
        private materialTypeConnectLocalDB          _materialTypeConnectDB;
        private materialConnectLocalDB              _materialConnectDB;
        private materialTypeMaterialConnectLocalDB  _materialTypeMaterialConnectDB;
        private materialIOPositionConnectLocalDB    _materialIOPositionConnectDB;
        private typeConnectLocalDB                  _typesConnectDB;

        #region public properties declararions

        public ObservableCollection<clCbbFilltype1> obcTypes
        {
            get
            {
                return _obcTypes;
            }
            set
            {
                _obcTypes = value;
                OnPropertyChanged("obcTypes");
            }
        }
        public ObservableCollection<clCbbFilltype1> obcCopyTypes
        {
            get
            {
                return _obcCopyTypes;
            }
            set
            {
                _obcCopyTypes = value;
                OnPropertyChanged("obcCopyTypes");
            }
        }

        #endregion
        clImage typeImage = new clImage();

        #region public functions
        public int saveNewType()
        {
            Types saveNewType    = new Types();

            saveNewType.code        = sCode.sInput;
            saveNewType.name        = sTypeName.sInput;
            saveNewType.comment     = sComment.sInput;
            saveNewType.active      = xActive;
            saveNewType.imagePath   = sImagePath;

            int iNewTypeId          = _typesConnectDB.insertAndSubmit(saveNewType);

            return iNewTypeId;
        }
        public int saveEditedType()
        {
            Types editedType        = new Types();            // get current type from database
            editedType              = _typesConnectDB.selectWhere(iTypeId);

            editedType.code         = sCode.sInput;
            editedType.name         = sTypeName.sInput; 
            editedType.comment      = sComment.sInput;
            editedType.active       = xActive;
            editedType.imagePath    = sImagePath;

            int iEditedTypeId       = _typesConnectDB.editInsertAndSubmit(editedType);   // insert edited type to database

            return iEditedTypeId;
        }
        public void deleteType(int iDeleteTypeId)
        {
            _typesConnectDB.deleteWhereTypeId(iDeleteTypeId);
        }
        public int getFirstTypeId()
        {
            if (this.obcTypes.Count > 0)
            {
                return this.obcTypes[0].iSelectedvaluePath;
            }
            else
            {
                return -1;
            }
        }
        public int getLastTypeId()
        {
            if (this.obcTypes.Count > 0)
            {
                return this.obcTypes[this.obcTypes.Count - 1].iSelectedvaluePath;
            }
            else
            {
                return -1;
            }
        }
        public Types getType(int iNewTypeId)
        {
            return _typesConnectDB.selectWhere(iNewTypeId);
        }
        public void getTypes(string sFilter = "", int iProfileId = 0)
        {
            foreach (var types in this.obcTypes.ToList())          // Delete all items in this mirrow database list
            {
                this.obcTypes.Remove(types);                       // Remove all database data from observableobject. 
            }

            List<Object> TypeQuery = new List<Object>(_typesConnectDB.select(sFilter));


            foreach (Types types in TypeQuery)
            {
                this.obcTypes.Add(new clCbbFilltype1(types.code, types.typeId));      // Add database data to observableobject. 
            }
        }
        public void getFilteredCopyType(string sFilter = "")
        {
            foreach (var types in obcCopyTypes.ToList())          // Delete all items in this mirrow database list
            {
                obcCopyTypes.Remove(types);                       // Remove all database data from observableobject. 
            }
            List<Object> TypeQuery = new List<Object>(_typesConnectDB.select(sFilter));

            foreach (Types types in TypeQuery)
            {
                obcCopyTypes.Add(new clCbbFilltype1(clLanguages.getName("__Code") + ":\t\t" + types.code + "\n" + clLanguages.getName("__Name") + ":\t" + types.name, types.typeId));
            }
        }
        public void updateTypeDetails(int iNewTypeId, bool xIsNew)
        {
            Types type = getType(iNewTypeId);

            if(type != null)
            {
                if (!xIsNew)                                             // Do not copy name, comment and code to current class when a class is copied. 
                {
                    iTypeId             = type.typeId;
                    sTypeName.sInput    = type.name; 
                    sCode.sInput        = type.code;
                    sComment.sInput     = type.comment;
                }
                sImagePath              = typeImage.checkImagePath(type.imagePath);
                xActive                 = type.active;
            }
            else
            {
                emptyType();
            }
        }
        public void emptyType()
        {
            iTypeId             = -1;
            sTypeName.sInput    = "";
            sCode.sInput        = "";
            sComment.sInput     = "";
            sImagePath          = clImage.sDefaultImage;
            xActive             = true;
        }
        public void propertyValidation(bool xEnable)    
        {
            sTypeName.xValidateActive   = xEnable;
            sCode.xValidateActive       = xEnable;
            sComment.xValidateActive    = xEnable;
        }
        public bool validationStatus()
        {
            return (sTypeName.xHasError || sCode.xHasError || sComment.xHasError);
        }

        #endregion
        
    }
}
