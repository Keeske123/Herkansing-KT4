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
using Sewco.Modules.ProjectsEditor;
using Sewco.Modules.ControlPanel;
using Sewco.Resources.Helper_classes;




namespace Sewco.Modules.ProductsEditor
{
    public class clIOPosition : ObservableObject
    {
        public clIOPosition(int _argiIOPosition, int _argiMaterialType, string _argsMaterialType)
        {
            iIOPositionSelectedValuePath    =   _argiIOPosition - 1;    // Array index is IO position - 1
            iMaterialType                   =   _argiMaterialType;
            sMaterialTypeName               =   _argsMaterialType;

            sMaterialTypeName               =   _argiIOPosition.ToString() + " " + sMaterialTypeName;
        }


        private int _iMaterialSelectedvalue;
        private int _iIOPositionSelectedValuePath;
        private string _sMaterialTypeName;

        public ObservableCollection<clCbbFilltype1>     obcAddedMaterials      { get; set; } = new ObservableCollection<clCbbFilltype1>();
        //public ObservableCollection<clCbbFilltype1> _obcCurrentMaterials    { get; set; } = new ObservableCollection<clCbbFilltype1>();     // Consists the current shown materials on the screen

        public int iMaterialType
        {
            get
            {
                return _iMaterialSelectedvalue;
            }
            set
            {
                _iMaterialSelectedvalue = value;
                OnPropertyChanged("iMaterialType");
            }
        }
        public int iIOPositionSelectedValuePath
        {
            get
            {
                return _iIOPositionSelectedValuePath;
            }
            set
            {
                _iIOPositionSelectedValuePath = value;
                OnPropertyChanged("iIOPositionSelectedValuePath");
            }
        }

        public string sMaterialTypeName
        {
            get
            {
                return _sMaterialTypeName;
            }
            set
            {
                _sMaterialTypeName = value;
                OnPropertyChanged("sMaterialTypeName");
            }
        }

    }

    public class ProductsBusinessObject : Product //, IDataErrorInfo
    {
        public ProductsBusinessObject()
        {
            _projectsConnectDB              = new projectsConnectLocalDB();
            _productsConnectDB              = new productsConnectLocalDB();
            _profilesConnectDB              = new profilesConnectLocalDB();
            _coversConnectDB                = new coverConnectLocalDB();
            _typesConnectDB                 = new typeConnectLocalDB();
            _materialProductConnectDB       = new materialProductConnectLocalDB();
            _materialTypeConnectDB          = new materialTypeConnectLocalDB();
            _materialConnectDB              = new materialConnectLocalDB();
            _materialTypeMaterialConnectDB  = new materialTypeMaterialConnectLocalDB();
            _materialIOPositionConnectDB    = new materialIOPositionConnectLocalDB();


            fillDefaultComboboxes();
            //fillCbbProjectOptions();

            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__ProductCode")));
            sCode = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__ProductName")));
            sProductName    = new clTextBoxValidation(clInputErrors);
        }

        private ObservableCollection<clErrors> clInputErrors = new ObservableCollection<clErrors>();

        public clTextBoxValidation sProductName
        {
            get; set;
        }
        public clTextBoxValidation sCode
        {
            get; set;
        }

        private void fillDefaultComboboxes()
        {
            fillCbbCoverOptions();
            fillCbbTypeOptions();
            fillCbbProfileOptions();
        }

        private void fillCbbCoverOptions()
        {
            // get cover types.. Not implemented yet   
            List<Object> CoverQuery = new List<Object>(_coversConnectDB.select(""));

            foreach (Covers cover in CoverQuery)
            {
                clCover.obcItems.Add(new clCbbFilltype1(cover.code, cover.coverId));
            }

            //clCover.obcItems.Add(new clCbbFilltype1("Cover1", 1));
            //clCover.obcItems.Add(new clCbbFilltype1("Cover2", 2));
        }
        private void fillCbbTypeOptions()
        {
            List<Object> TypeQuery = new List<Object>(_typesConnectDB.select(""));

            foreach (Types type in TypeQuery)
            {
                clType.obcItems.Add(new clCbbFilltype1(type.code, type.typeId));
            }

            //clType.obcItems.Add(new clCbbFilltype1("Type1", 1));
            //clType.obcItems.Add(new clCbbFilltype1("Type2", 2));
        }
        /*private void fillCbbProjectOptions()
        {
            List<Object> ProjectQuery = new List<Object>(_projectsConnectDB.select(""));

            foreach (Project project in ProjectQuery)
            {
                clChosenProject.obcItems.Add(new clCbbFilltype1(project.projectName, project.projectId));    
            }
        }*/
        private void fillCbbProfileOptions()
        {
            List<Object> ProfileQuery = new List<Object>(_profilesConnectDB.select(""));

            foreach (profile profile in ProfileQuery)
            {
                clProfile.obcItems.Add(new clCbbFilltype1(profile.name, profile.profileId));
            }
        }
        private clcbbOptions    _clCover                { get; set; } = new clcbbOptions();
        private clcbbOptions    _clType                 { get; set; } = new clcbbOptions();
        //private clcbbOptions    _clChosenProject        { get; set; } = new clcbbOptions();
        private clcbbOptions    _clProjects             { get; set; } = new clcbbOptions();
        private clcbbOptions    _clProfile              { get; set; } = new clcbbOptions();
        private clcbbOptions    _clShowAddedMaterials   { get; set; } = new clcbbOptions();
        private clcbbOptions    _clAvailableMaterials   { get; set; } = new clcbbOptions();
        private clcbbOptions _clAvailableMaterials_temp { get; set; } = new clcbbOptions();


        //private ObservableCollection<clCbbFilltype1>    _obcProjects        { get; set; } = new ObservableCollection<clCbbFilltype1>();
        private ObservableCollection<clCbbFilltype1>    _obcProducts        { get; set; } = new ObservableCollection<clCbbFilltype1>();
        private ObservableCollection<clCbbFilltype1>    _obcCopyProducts    { get; set; } = new ObservableCollection<clCbbFilltype1>();
        private ObservableCollection<clCbbFilltype1>    _obcCode            { get; set; } = new ObservableCollection<clCbbFilltype1>();
        private ObservableCollection<clIOPosition>      _obcIOPositions     { get; set; } = new ObservableCollection<clIOPosition>();
        


        private int _iIOPositionSelectedvalue;
        public int iIOPositionSelectedvalue
        {
            get
            {
                return _iIOPositionSelectedvalue;
            }
            set
            {
                _iIOPositionSelectedvalue = value;
                SendPropertyChanged("iIOPositionSelectedvalue");
            }
        }

        private projectsConnectLocalDB              _projectsConnectDB;
        private productsConnectLocalDB              _productsConnectDB;
        private profilesConnectLocalDB              _profilesConnectDB;
        private coverConnectLocalDB                 _coversConnectDB;
        private typeConnectLocalDB                  _typesConnectDB;
        private materialProductConnectLocalDB       _materialProductConnectDB;
        private materialTypeConnectLocalDB          _materialTypeConnectDB;
        private materialConnectLocalDB              _materialConnectDB;
        private materialTypeMaterialConnectLocalDB  _materialTypeMaterialConnectDB;
        private materialIOPositionConnectLocalDB    _materialIOPositionConnectDB;
        /*
        #region Input error handling
        private string validateInput(string value)
        {
            string sResult = "";
            int iNumberOfErrors = 0;

            if (value == "")
            {
                iNumberOfErrors++;
                if (iNumberOfErrors > 1)
                    sResult += "\n";
                sResult += clLanguages.getName("__InputIsEmpty");
            }

            this.validateResultOK = (sResult == "");
            return sResult;
        }
        string IDataErrorInfo.this[string sPropertyName]
        {
            get
            {

                // Type myType = typeof(clSeamTemplateModel);
                // This part gets automaticly the right property. 
                // sPropertyName contains the name of the property which value is updated. 

                // This gets the property by name, so we dont need any if statements like: if(sPropertyName == "sStichcount") etc etc. 
                var vGetProperty = this.GetType().GetProperty(sPropertyName);

                // Get the current (new) value of this property
                string sPropertyValue = vGetProperty.GetValue(this, null) as string;
                // Validate this value
                string sResult = validateInput(sPropertyValue);
                // This property name correspondents to the database name. 
                // Example: sBlindTens = property name
                //          blindTens = database name. 
                // sBlindTens   -> Cut the first s              = BlindTens
                // BlindTens    -> Change 'B' to lowercase      = blindTens = database property
                string sSaveName = sPropertyName.Substring(1);
                sSaveName = char.ToLower(sSaveName[0]) + sSaveName.Substring(1);

                // Get database property (blindTens)
                var vSaveProperty = this.GetType().GetProperty(sSaveName);


                if (sResult != "")
                {
                    // If validate result is not good, set value to 0
                    vSaveProperty.SetValue(this, "");

                    if (!list.Contains(sSaveName))
                    {
                        list.Add(sSaveName);
                    }
                    xContentError = (list.Count() > 0);
                    // Result error tekst.
                    return sResult;
                }
                else
                {
                    if (list.Contains(sSaveName))
                    {
                        list.Remove(sSaveName);
                    }
                    xContentError = (list.Count() > 0);
                    // Validate result is good. Set new value to database property
                    vSaveProperty.SetValue(this, sPropertyValue);
                }
                return null;
            }

        }
        public bool validateResultOK { get; set; }
        public bool xContentError;
        string IDataErrorInfo.Error
        {
            get { return null; }
        }
        List<string> list = new List<string>();
        #endregion
        */

        #region public properties declararions

        public clcbbOptions clCover
        {
            get
            {
                return _clCover;
            }
            set
            {
                _clCover = value;
                SendPropertyChanged("clCover");
            }
        }
        public clcbbOptions clType
        {
            get
            {
                return _clType;
            }
            set
            {
                _clType = value;
                SendPropertyChanged("clType");
            }
        }
        /*public clcbbOptions clChosenProject
        {
            get
            {
                return _clChosenProject;
            }
            set
            {
                _clChosenProject = value;
                SendPropertyChanged("clChosenProject");
            }
        }*/
        public clcbbOptions clProfile
        {
            get
            {
                return _clProfile;
            }
            set
            {
                _clProfile = value;
                SendPropertyChanged("clProfile");
            }
        }
        public clcbbOptions clShowAddedMaterials
        {
            get
            {
                return _clShowAddedMaterials;
            }
            set
            {
                _clShowAddedMaterials = value;
                SendPropertyChanged("clShowAddedMaterials");
            }
        }
        public clcbbOptions clAvailableMaterials
        {
            get
            {
                return _clAvailableMaterials;
            }
            set
            {
                _clAvailableMaterials = value;
                SendPropertyChanged("clAvailableMaterials");
            }
        }
        public clcbbOptions clAvailableMaterials_temp
        {
            get
            {
                return _clAvailableMaterials_temp;
            }
            set
            {
                _clAvailableMaterials_temp = value;
                SendPropertyChanged("clAvailableMaterials_temp");
            }
        }
        public ObservableCollection<clIOPosition>   obcIOPositions
        {
            get
            {
                return this._obcIOPositions;
            }
            set
            {
                this._obcIOPositions = value;
                SendPropertyChanged("obcIOPositions");
            }
        }
        /*public ObservableCollection<clCbbFilltype1> obcProjects
        {
            get
            {
                return this._obcProjects;
            }
            set
            {
                this._obcProjects = value;
                SendPropertyChanged("obcProjects");
            }
        }*/
        public clcbbOptions clProjects
        {
            get
            {
                return _clProjects;
            }
            set
            {
                _clProjects = value;
                SendPropertyChanged("clProjects");
            }
        }
        public ObservableCollection<clCbbFilltype1> obcProducts
        {
            get
            {
                return _obcProducts;
            }
            set
            {
                _obcProducts = value;
                SendPropertyChanged("obcProducts");
            }
        }
        public ObservableCollection<clCbbFilltype1> obcCopyProducts
        {
            get
            {
                return _obcCopyProducts;
            }
            set
            {
                _obcCopyProducts = value;
                SendPropertyChanged("obcCopyProducts");
            }
        }
        
        #endregion

        #region public functions
        public int saveNewProduct(int iProjectId)
        {
            Product saveNewProduct      = new Product();

            saveNewProduct.code         = sCode.sInput;
            saveNewProduct.name         = sProductName.sInput;     
            saveNewProduct.active       = active;
            saveNewProduct.coverId      = clCover.iSelectedValue;
            saveNewProduct.typeId       = clType.iSelectedValue;
            saveNewProduct.projectId    = iProjectId;
            saveNewProduct.profileId    = clProfile.iSelectedValue;
            saveNewProduct.imagePath    = imagePath;

            int iNewProductId = _productsConnectDB.insertAndSubmit(saveNewProduct);

            if(iNewProductId != -1)
            {
                MaterialProduct newMaterialProduct;
                // for every IO position
                foreach (var ioPosition in _obcIOPositions.ToList())
                {
                    // for every material in every IO position
                    foreach (var material in ioPosition.obcAddedMaterials.ToList())
                    {
                        newMaterialProduct                      = new MaterialProduct();
                        newMaterialProduct.productId            = iNewProductId;
                        newMaterialProduct.materialIOPosition   = ioPosition.iIOPositionSelectedValuePath + 1;
                        newMaterialProduct.materialId           = material.iSelectedvaluePath;

                        _materialProductConnectDB.insert(newMaterialProduct);
                    }
                }
                _materialProductConnectDB.submit();
            }

            return iNewProductId;
        }
        public int saveEditedProduct(int iProjectId)
        {
            Product editedProduct = _productsConnectDB.selectProduct(productId);            // get current product from database

            editedProduct.code          = sCode.sInput;
            editedProduct.name          = sProductName.sInput; 
            editedProduct.active        = active;
            editedProduct.coverId       = clCover.iSelectedValue;
            editedProduct.typeId        = clType.iSelectedValue;
            editedProduct.projectId     = iProjectId; 
            editedProduct.profileId     = clProfile.iSelectedValue;
            editedProduct.imagePath     = imagePath;

            int iEditedProductId = _productsConnectDB.editInsertAndSubmit(editedProduct);   // insert edited product to database

            _materialProductConnectDB.deleteWhereProductId(productId);

            if(iEditedProductId != -1)
            {
                MaterialProduct newMaterialProduct;
                // for every IO position
                foreach (var ioPosition in _obcIOPositions.ToList())
                {
                    // for every material in every IO position
                    foreach (var material in ioPosition.obcAddedMaterials.ToList())
                    {
                        newMaterialProduct                      = new MaterialProduct();
                        newMaterialProduct.productId            = iEditedProductId;
                        newMaterialProduct.materialIOPosition   = ioPosition.iIOPositionSelectedValuePath + 1;
                        newMaterialProduct.materialId           = material.iSelectedvaluePath;

                        _materialProductConnectDB.insert(newMaterialProduct);                       
                    }
                }
                _materialProductConnectDB.submit();
            }
            return iEditedProductId;
        }
        public void deleteProduct(int iProductId)
        {
            _productsConnectDB.deleteWhereProductId(productId);
        }
        public int getFirstProductId()
        {
            if (this.obcProducts.Count > 0)
            {
                return this.obcProducts[0].iSelectedvaluePath;
            }
            else
            {
                return -1;
            }
        }
        public int getLastProductId()
        {
            if (this.obcProducts.Count > 0)
            {
                return this.obcProducts[this.obcProducts.Count - 1].iSelectedvaluePath;
            }
            else
            {
                return -1;
            }
        }
        public Product getProduct(int iProductId)
        {
            return _productsConnectDB.selectProduct(iProductId);
        }
        public void getProjects(string sFilter = "")
        {
            foreach (var projects in this.clProjects.obcItems.ToList())          // Delete all items in this mirrow database list
            {
                this.clProjects.obcItems.Remove(projects);                       // Remove all database data from observableobject. 
            }

            List<Object> ProjectQuery = new List<Object>(_projectsConnectDB.selectWhere("", 0, sFilter));

            foreach (Project projects in ProjectQuery)
            {
                this.clProjects.obcItems.Add(new clCbbFilltype1(projects.projectName + ",  " + projects.comment, projects.projectId));      // Add database data to observableobject. 
            }
        }
        public void getProducts(string sFilter = "", int iProfileId = 0)
        {
            foreach (var products in this.obcProducts.ToList())          // Delete all items in this mirrow database list
            {
                this.obcProducts.Remove(products);                       // Remove all database data from observableobject. 
            }

            List<Object> ProductQuery = new List<Object>(_productsConnectDB.selectWhere("", iProfileId, sFilter));

            foreach (Product product in ProductQuery)
            {
                this.obcProducts.Add(new clCbbFilltype1(product.code, product.productId));      // Add database data to observableobject. 
            }
        }
        public void getIOPositions(int iProjectId, int iProductId)
        {
            int i = -1;
            int iIndexCounter = -1;
            // query naar MaterialProduct
            foreach (var ioPosition in _obcIOPositions.ToList())          // Delete all items in this mirrow database list
            {
                _obcIOPositions.Remove(ioPosition);                       // Remove all database data from observableobject. 
            }

            List<Object> IOPositionsQuery = new List<Object>(_materialIOPositionConnectDB.selectWhere(iProjectId));

            foreach (Material_IOPosition ioPosition in IOPositionsQuery)    // for every found IO position..
            {
                if (i != ioPosition.materialIOPosition)     // if previous IO position is different from current IO position. Add a new IO position
                {
                    // Add new IO position to list. 
                    clIOPosition newclIOPosition = new clIOPosition(ioPosition.materialIOPosition, ioPosition.materialTypeId, ioPosition.MaterialType.name);
                    _obcIOPositions.Add(newclIOPosition);

                    iIndexCounter++;    // Increase index counter. 
                    i = ioPosition.materialIOPosition;
                }
                List<Object> MaterialProductQuery = new List<Object>(_materialProductConnectDB.selectWhereProductMaterialIOPosition(iProductId, ioPosition.materialIOPosition));
                foreach (MaterialProduct material in MaterialProductQuery)
                {
                    // Add a new material for every found material from database
                    Material MaterialQuery;
                    MaterialQuery = _materialConnectDB.selectWhere("", material.materialId, "");

                    if (MaterialQuery != null)
                    {
                        // If necesary this can be adjusted to: Found index of: iIOPositionSelectedValuePath == ioPosition.materialIOPosition AND sMaterialTypeName == MaterialTypeQuery.MaterialType.name
                        _obcIOPositions[iIndexCounter].obcAddedMaterials.Add(new clCbbFilltype1(MaterialQuery.code + ", " + MaterialQuery.name, material.materialId));
                    }
                }
            }
        }
        public void getAddedMaterials(int iSelectedIOPosition)
        {
            // getAddedMaterials() copies the added materials from the _obcIOPositions list to the clShowAddedMaterials. The clShowAddedMaterials is show on the screen. 
            // _obcIOPositions[iSelectedIOPosition].obcAddedMaterials is a temporary database. Is is not possible the show a array with index location on the screen. 
            foreach (var material in this.clShowAddedMaterials.obcItems.ToList())         
            {
                clShowAddedMaterials.obcItems.Remove(material);                      
            }

            if((_obcIOPositions.Count() > 0) && (iSelectedIOPosition < _obcIOPositions.Count()))
            {
                // Copy added materials to clShowAddedMaterials class to show on the screen. This is only a mirror of the original class and used to show data on the screen.
                foreach (var material in _obcIOPositions[iSelectedIOPosition].obcAddedMaterials.ToList())
                {
                    clShowAddedMaterials.obcItems.Add(new clCbbFilltype1(material.sDisplayName, material.iSelectedvaluePath));
                }
            }
        }     
        public void getAvailableMaterials(int iSelectedIOPosition)
        {
            // getAvailableMaterials() gets the available materials from the database. 
            // When the user selects a different IO position, this function will be called to update the available materials. 
            foreach (var availableMaterial in this.clAvailableMaterials.obcItems.ToList())
            {
                clAvailableMaterials.obcItems.Remove(availableMaterial);
            }
            foreach (var availableMaterial in this.clAvailableMaterials_temp.obcItems.ToList())
            {
                clAvailableMaterials_temp.obcItems.Remove(availableMaterial);
            }

            if ((_obcIOPositions.Count() > 0) && (iSelectedIOPosition < _obcIOPositions.Count()))
            {
                // Select all materials depending on selected IO position. MaterialType is saved within class. 
                List<Object> availableMaterialsQuery = new List<Object>(_materialTypeMaterialConnectDB.selectWhereMaterialTypeId(_obcIOPositions[iSelectedIOPosition].iMaterialType));

                foreach (MaterialTypeMaterial availableMaterial in availableMaterialsQuery)
                {
                    // Do not add MaterialId to obcAddedMaterials list when material exist in obcAddedMaterials
                    if (!_obcIOPositions[iSelectedIOPosition].obcAddedMaterials.Any(c => c.iSelectedvaluePath == availableMaterial.materialId))
                    {
                        clAvailableMaterials.obcItems.Add(new clCbbFilltype1(availableMaterial.Material.code + ", " + availableMaterial.Material.name, availableMaterial.materialId));
                        clAvailableMaterials_temp.obcItems.Add(new clCbbFilltype1(availableMaterial.Material.code + ", " + availableMaterial.Material.name, availableMaterial.materialId));
                    }
                }
            }
        }
        public void addNewMaterial(int iSelectedIOPosition, int iAvailableNewMaterialSelectedValue)
        {
            // addNewMaterial() adds a new material to the obcAddedMaterials list. 
            Material MaterialQuery;
            MaterialQuery = _materialConnectDB.selectWhere("", iAvailableNewMaterialSelectedValue, "");

            if (MaterialQuery != null && iSelectedIOPosition < _obcIOPositions.Count())
            {
                _obcIOPositions[iSelectedIOPosition].obcAddedMaterials.Add(new clCbbFilltype1(MaterialQuery.code + ", " + MaterialQuery.name, iAvailableNewMaterialSelectedValue));
            }
            getAddedMaterials(iSelectedIOPosition);                             // Refresh the 'show' database
            deleteAvailableMaterialItem(iAvailableNewMaterialSelectedValue);    // Delete the added material from the available material list, because it is already added
        }
        public void deleteAddedMaterial(int iSelectedIOPosition, int iAddedMaterialSelectedValue)
        {
            // deleteAddedMaterial() deletes and added material from the obcAddedMaterials list. 
            try
            {
                if (iSelectedIOPosition < _obcIOPositions.Count())
                {
                    _obcIOPositions[iSelectedIOPosition].obcAddedMaterials.Remove(_obcIOPositions[iSelectedIOPosition].obcAddedMaterials.First(c => c.iSelectedvaluePath == iAddedMaterialSelectedValue));
                }
            } catch
            {
                // Reeks bevat geen elementen  
            }
            getAddedMaterials(iSelectedIOPosition);                     // Refresh the 'show' database 
            addAvailableMaterialItem(iAddedMaterialSelectedValue);      // Add the deleted material to the available material list, because the user deleted it, so it is available to add
        }
        private void addAvailableMaterialItem(int iAvailableNewMaterialSelectedValue)
        {
            // addAvailableMaterialItem() adds an item to the clAvailableMaterials list. The user deleted it from the obcAddedMaterials list. 
            Material MaterialQuery;
            MaterialQuery = _materialConnectDB.selectWhere("", iAvailableNewMaterialSelectedValue, "");
            if (MaterialQuery != null)
            {
                clAvailableMaterials.obcItems.Add(new clCbbFilltype1(MaterialQuery.code + ", " + MaterialQuery.name, iAvailableNewMaterialSelectedValue));
            }

            clcbbOptions temp  = new clcbbOptions();

            if (clAvailableMaterials_temp.obcItems.Count > 0)
            {
                temp = clAvailableMaterials_temp;
                /*foreach (var availableMaterial in this.clAvailableMaterials_temp.obcItems.ToList())
                {
                    clAvailableMaterials_temp.obcItems.Remove(availableMaterial);
                }*/
                int i = 0;
                /*foreach (var availableMaterial in temp.obcItems.ToList())
                {
                    if (i > 0)
                    {
                        clAvailableMaterials_temp.obcItems.Add(temp.obcItems[i - 1]);
                    }
                    i++;
                }*/
                clAvailableMaterials_temp.obcItems.Remove(temp.obcItems.First());

                clAvailableMaterials_temp.obcItems.Add(temp.obcItems.First());
            }
        }
        private void deleteAvailableMaterialItem(int iAvailableNewMaterialSelectedValue)
        {
            // deleteAvailableMaterialItem() deletes an item from the clAvailableMaterials list. The user added this material to the obcAddedMaterials list.  
            try
            {
                clAvailableMaterials.obcItems.Remove(clAvailableMaterials.obcItems.First(c => c.iSelectedvaluePath == iAvailableNewMaterialSelectedValue));
            }
            catch
            {
                // Reeks bevat geen elementen  
            }
        }
        public void getFilteredCopyProduct(string sFilter = "")
        {
            foreach (var products in obcCopyProducts.ToList())          // Delete all items in this mirrow database list
            {
                obcCopyProducts.Remove(products);                       // Remove all database data from observableobject. 
            }
            List<Object> ProductQuery = new List<Object>(_productsConnectDB.select(sFilter));

            foreach (Product product in ProductQuery)
            {
                obcCopyProducts.Add(new clCbbFilltype1(clLanguages.getName("__Name") + ":\t\t" + product.code + "\n" + clLanguages.getName("__Comment") + ":\t" + product.name, product.productId));
            }
        }
        public int getFirstAddedMaterialItem(int iSelectedIOPosition)
        {
            int iFirstAddedMaterialItem = -1;

            if(iSelectedIOPosition < _obcIOPositions.Count())
            { 
                if(_obcIOPositions[iSelectedIOPosition].obcAddedMaterials.Count() > 0)
                {
                    iFirstAddedMaterialItem = _obcIOPositions[iSelectedIOPosition].obcAddedMaterials[0].iSelectedvaluePath;
                }
            }
            //iFirstAddedMaterialItem = _obcIOPositions[iSelectedIOPosition].obcAddedMaterials.First(c => c.iSelectedvaluePath == iAvailableNewMaterialSelectedValue).iSelectedvaluePath; 

            return iFirstAddedMaterialItem;
        }
        public int getFirstAvailableMaterialItem()
        {
            int iFirstAvailableMaterialItem = -1;

            if (clAvailableMaterials.obcItems.Count() > 0)
            {
                iFirstAvailableMaterialItem = clAvailableMaterials.obcItems[0].iSelectedvaluePath;
            }

            //iFirstAvailableMaterialItem = clAvailableMaterials.obcItems.First(c => c.iSelectedvaluePath == iAvailableNewMaterialSelectedValue).iSelectedvaluePath;

            return iFirstAvailableMaterialItem;
        }
        public void updateProductDetails(int iProductId, bool xIsNew)
        {
            Product product = getProduct(iProductId);

            if(product != null)
            {
                if (!xIsNew)                                             // Do not copy name and comment to current class when a class is copied. 
                {
                    productId           = product.productId;
                    sProductName.sInput = product.name.ToString(); 
                    sCode.sInput        = product.code.ToString();
                }
                imagePath                       = product.imagePath;
                active                          = product.active;
                clCover.iSelectedValue          = (int)product.coverId;
                clType.iSelectedValue           = (int)product.typeId;
                //clProjects.iSelectedValue       = (int)product.projectId;
                //clChosenProject.iSelectedValue  = (int)product.projectId;
                clProfile.iSelectedValue        = (int)product.profileId;

                // extend of neccesary with new items
            }
            else
            {
                emptyProduct();
            }
        }
        public void emptyProduct()
        {
            productId           = -1;
            sProductName.sInput = ""; 
            sCode.sInput        = "";
            imagePath           = "/Resources/Images/DefaultImage/No_image_selected.png";
            active              = true;

            if (clCover.obcItems.Count() > 0)
                clCover.iSelectedValue = clCover.obcItems[0].iSelectedvaluePath;
            else
                clCover.iSelectedValue          = 0;

            if (clType.obcItems.Count() > 0)
                clType.iSelectedValue = clType.obcItems[0].iSelectedvaluePath;
            else
                clType.iSelectedValue           = 0;

            if (clProfile.obcItems.Count() > 0)
                clProfile.iSelectedValue = clProfile.obcItems[0].iSelectedvaluePath;
            else
                clProfile.iSelectedValue = 0;
            //clChosenProject.iSelectedValue  = 0;



            //_obcIOPositions[iSelectedIOPosition].obcAddedMaterials.ToList())

            foreach (var ioPosition in _obcIOPositions.ToList())
            {
                foreach (var addedMaterial in ioPosition.obcAddedMaterials.ToList())
                { 
                    ioPosition.obcAddedMaterials.Remove(addedMaterial);
                }
            }
            foreach (var addedMaterial in clShowAddedMaterials.obcItems.ToList())
            {
                clShowAddedMaterials.obcItems.Remove(addedMaterial);
            }
            foreach (var availableMaterial in clAvailableMaterials.obcItems.ToList())
            {
                clAvailableMaterials.obcItems.Remove(availableMaterial);
            }

        }
        public void propertyValidation(bool xEnable)    
        {
            sProductName.xValidateActive    = xEnable;
            sCode.xValidateActive           = xEnable;
        }
        public bool validationStatus()
        {
            return (sProductName.xHasError || sCode.xHasError);
        }

        #endregion
    }
}
