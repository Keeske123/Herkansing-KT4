using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sewco.Modules.ControlPanel;
using System.Windows.Forms;
using Sewco.Resources.Helper_classes;
using System.Collections.ObjectModel;
using System.IO;

namespace Sewco.Modules.ProductsEditor
{
    class ViewModelProducts : ObservableObject, ifSewcoMenu
    {
        public ViewModelProducts()
        {
          
            if (ViewModelControlPanel.xValidDatabaseConnection)
            {
                xEnableScreen       = true;
                xEnableProductMenu  = false;
                #region RelayCommands
                newObjectCommand = new RelayCommand(
                        param => newObject(),      
                        param => (!xEditObjectMode && (productsModel.clProjects.obcItems.Count > 0) && ViewControlPanel.ViewModelControlPanel.header.xUserLoggedIn)            // Only visible when not in New of Change mode
                    );
                saveObjectCommand = new RelayCommand(
                        param => saveObject(),
                        param => ((xChangeObjectMode || xNewObjectMode) && !xContentHasError) && ViewControlPanel.ViewModelControlPanel.header.xUserLoggedIn
                    );
                cancelObjectCommand = new RelayCommand(
                        param => cancelObject(),                 
                        param => (xEditObjectMode) && ViewControlPanel.ViewModelControlPanel.header.xUserLoggedIn           // Is visible in Project edit mode.. (New and change mode)
                    );
                editObjectCommand = new RelayCommand(
                        param => editObject(),        
                        param => (!xEditObjectMode && productsModel.obcProducts.Count > 0) && ViewControlPanel.ViewModelControlPanel.header.xUserLoggedIn           // (!xEditMode) Visible when not in edit mode.         
                    );
                deleteObjectCommand = new RelayCommand(
                        param => deleteObject(), 
                        param => (xChangeObjectMode) && ViewControlPanel.ViewModelControlPanel.header.xUserLoggedIn          // Visible when not in edit mode..
                    );
                copyObjectCommand = new RelayCommand(
                        param => controlCopyPopUpObject(true),     // Show popup screen, to copy a current Project
                        param => (xEditObjectMode) && ViewControlPanel.ViewModelControlPanel.header.xUserLoggedIn                // Only visible when in edit mode
                    );
                cancelCopyObjectCommand = new RelayCommand(
                        param => controlCopyPopUpObject(false),
                        param => ViewControlPanel.ViewModelControlPanel.header.xUserLoggedIn
                    );
                selectCopyObjectCommand = new RelayCommand(
                        param => copyObject(),
                        param => (iCopyObjectSelectedValue != 0) && ViewControlPanel.ViewModelControlPanel.header.xUserLoggedIn       // TODO
                    );
                deleteAddedMaterialCommand = new RelayCommand(
                        param => deleteAddedMaterial(),
                        param => xEditObjectMode && ViewControlPanel.ViewModelControlPanel.header.xUserLoggedIn
                    );
                addNewMaterialCommand = new RelayCommand(
                        param => addNewMaterial(),
                        param => xEditObjectMode && ViewControlPanel.ViewModelControlPanel.header.xUserLoggedIn
                    );
                selectImagePathFolderCommand = new RelayCommand(
                        param => openFileDialog(),
                        param => ViewControlPanel.ViewModelControlPanel.header.xUserLoggedIn
                    );

                #endregion RelayCommands
                productsModel = new ProductsBusinessObject();

                initializeProductsEditor();
                xEnableCbbProductsCode = true;
            }
            else
            {
                xEnableScreen = false;
            }
        }

        private bool    xContentHasError
        {
            get
            {
                return productsModel.validationStatus(); //productsModel.sProductName.xHasError;
            }
        }


        #region private properties declarations
        private bool    _xEnableScreen;
        private int     _iProjectSelectedValue;
        private int     _iObjectSelectedValue;
        private int     _iIOPositionSelectedvalue;
        private int     _iAvailableNewMaterialSelectedValue;
        private int     _iAddedMaterialSelectedValue;
        private bool    _xChangeObjectMode;
        private bool    _xNewObjectMode;
        private bool    _xEditObjectMode;
        private bool    _xShowPopupCopyObject;
        private bool    _xShowPopupEditObject;
        private int     _iCopyObjectSelectedValue;
        private string  _sfilterPopupCopyObject;
        private string  _sSearchProjectList;
        private bool    _xEnableCbbProductsCode;
        private bool    _xEnableProductMenu;

        #endregion private properties declarations

        #region relayCommands declarations
        public RelayCommand newObjectCommand            { get; set; }
        public RelayCommand saveObjectCommand           { get; set; }
        public RelayCommand cancelObjectCommand         { get; set; }
        public RelayCommand editObjectCommand           { get; set; }
        public RelayCommand deleteObjectCommand         { get; set; }
        public RelayCommand copyObjectCommand           { get; set; }
        public RelayCommand cancelCopyObjectCommand     { get; set; }
        public RelayCommand selectCopyObjectCommand     { get; set; }
        public RelayCommand deleteAddedMaterialCommand  { get; set; }
        public RelayCommand addNewMaterialCommand       { get; set; }
        public RelayCommand selectImagePathFolderCommand { get; set; }

        #endregion relay commands

        public static ProductsBusinessObject productsModel { get; set; }
        public bool xEnableScreen
        {
            get
            {
                return _xEnableScreen;
            }
            set
            {
                _xEnableScreen = value;
                OnPropertyChanged("xEnableScreen");
            }
        }
        public bool xChangeObjectMode
        {
            get { return _xChangeObjectMode; }
            set
            {
                _xChangeObjectMode = value;
                xEditObjectMode = value;
                OnPropertyChanged("xChangeObjectMode");
            }
        }
        public bool xNewObjectMode
        {
            get { return _xNewObjectMode; }
            set
            {
                _xNewObjectMode = value;
                xEditObjectMode = value;
                OnPropertyChanged("xNewObjectMode");
            }
        }
        public bool xEditObjectMode
        {
            get { return _xEditObjectMode; }
            set
            {
                _xEditObjectMode = value;
                productsModel.propertyValidation(value);
                OnPropertyChanged("xEditObjectMode");
            }
        }
        public bool xEnableCbbProductsCode
        {
            get { return _xEnableCbbProductsCode; }
            set
            {
                _xEnableCbbProductsCode = value;
                OnPropertyChanged("xEnableCbbProductsCode");
            }
        }
        public int iProjectSelectedValue
        {
            get
            {
                return _iProjectSelectedValue;
            }
            set
            {
                _iProjectSelectedValue = value;
                OnPropertyChanged("iProjectSelectedValue");
                projectIsChanged();
                
            }
        }
        public int iObjectSelectedValue
        {
            get
            {
                return _iObjectSelectedValue;
            }
            set
            {
                _iObjectSelectedValue = value;
                /*if(_iObjectSelectedValue < 0 && !xEditObjectMode)
                {
                    xEnableCbbProductsCode = false;
                }
                else { 
                    xEnableCbbProductsCode = true;
                }*/
                objectIsChanged();
                OnPropertyChanged("iObjectSelectedValue");
            }
        }
        public int iCopyObjectSelectedValue
        {
            get
            {
                return _iCopyObjectSelectedValue;
            }
            set
            {
                _iCopyObjectSelectedValue = value;
                OnPropertyChanged("iCopyObjectSelectedValue");
            }
        }
        public int iIOPositionSelectedValue
        {
            get
            {
                return _iIOPositionSelectedvalue;
            }
            set
            {
                _iIOPositionSelectedvalue = value;
                OnPropertyChanged("iIOPositionSelectedValue");

                fillAddedMaterials();
            }
        }
        public int iAddedMaterialSelectedValue
        {
            get
            {
                return _iAddedMaterialSelectedValue;
            }
            set
            {
                _iAddedMaterialSelectedValue = value;
                OnPropertyChanged("iAddedMaterialSelectedValue");
            }
        }
        public int iAvailableNewMaterialSelectedValue
        {
            get
            {
                return _iAvailableNewMaterialSelectedValue;
            }
            set
            {
                _iAvailableNewMaterialSelectedValue = value;
                OnPropertyChanged("iAvailableNewMaterialSelectedValue");
            }
        }
        public bool xShowPopupCopyObject
        {
            get
            {
                return _xShowPopupCopyObject;
            }
            set
            {
                _xShowPopupCopyObject   = value;
                xEnableScreen           = !value;
                sfilterPopupCopyObject = "<DoNothing>";
                OnPropertyChanged("xShowPopupCopyObject");
            }
        }
        public bool xShowPopupEditObject
        {
            get
            {
                return _xShowPopupEditObject;
            }
            set
            {
                _xShowPopupEditObject = value;
                xEnableScreen = !value;
                OnPropertyChanged("xShowPopupEditObject");
            }
        }
        public string sSearchProjectList
        {
            get
            {
                return _sSearchProjectList;
            }
            set
            {
                _sSearchProjectList = value;
                OnPropertyChanged("sSearchProjectList");

                fillProjectList(_sSearchProjectList);
                fillObjectList("", _iProjectSelectedValue);
                objectIsChanged();
            }
        }
        public string sfilterPopupCopyObject
        {
            get
            {
                return _sfilterPopupCopyObject;
            }
            set
            {
                if (value != "<DoNothing>")
                {
                    controlCopyPopUpObject(xShowPopupCopyObject, value);
                    _sfilterPopupCopyObject = value;
                }
                else
                    _sfilterPopupCopyObject = "";
                OnPropertyChanged("sfilterPopupCopyObject");
            }
        }
        public void newObject()
        {
            productsModel.emptyProduct();

            fillAddedMaterials();
            xNewObjectMode = true;
            xEnableCbbProductsCode = false;
        }
        public void editObject()
        {
            xChangeObjectMode = true;         // Make input field editable. 
            //projectIsChanged();
            xEnableCbbProductsCode = false;
        }
        public void deleteObject()
        {
            productsModel.deleteProduct(productsModel.productId);
 
            selectFirstObject();
            cancelObject();
        }
        public void copyObject()
        {
            controlCopyPopUpObject(false);
            updateObjectDetails(iCopyObjectSelectedValue, true);
            iCopyObjectSelectedValue = -1;
        }
        public void cancelObject()
        {
            int iTempObjectSelectedValue = iObjectSelectedValue;
            xNewObjectMode      = false;
            xChangeObjectMode   = false;

            iCopyObjectSelectedValue = 0;
            xEnableCbbProductsCode = true; ;
            //fillObjectList("", _iProjectSelectedValue);
            //iObjectSelectedValue = iTempObjectSelectedValue;

            sSearchProjectList = "";
            //objectIsChanged();
        }
        public void saveObject()
        {
            int newSelectedObjectValue = 0;
            if (xNewObjectMode)
            {
                xNewObjectMode = false;            // Disable new Project mode

                newSelectedObjectValue  = productsModel.saveNewProduct(_iProjectSelectedValue);

                xEnableCbbProductsCode  = true;
                fillObjectList("", _iProjectSelectedValue);
            }
            if (xChangeObjectMode)
            {
                xChangeObjectMode = false;

                newSelectedObjectValue  = productsModel.saveEditedProduct(_iProjectSelectedValue);

                xEnableCbbProductsCode  = true;
                fillObjectList("", _iProjectSelectedValue);
            }
            iObjectSelectedValue = newSelectedObjectValue;
        }
        private void selectFirstObject()
        {
            int iFirstItemId = productsModel.getFirstProductId();

            if (iFirstItemId > 0)
            {
                _iProjectSelectedValue = iFirstItemId;
                OnPropertyChanged("iProjectSelectedValue");
            }
        }
        private void selectLastObject()        // select last row from database
        {
            int iLastItemId = productsModel.getLastProductId();

            if (iLastItemId > 0)
            {
                iObjectSelectedValue = iLastItemId;
            }
        }
        private void addNewMaterial()
        {
            productsModel.addNewMaterial(iIOPositionSelectedValue, iAvailableNewMaterialSelectedValue);

            iAddedMaterialSelectedValue         = productsModel.getFirstAddedMaterialItem(iIOPositionSelectedValue);
            iAvailableNewMaterialSelectedValue  = productsModel.getFirstAvailableMaterialItem();
        }
        private void deleteAddedMaterial()
        {
            productsModel.deleteAddedMaterial(iIOPositionSelectedValue, iAddedMaterialSelectedValue);

            iAddedMaterialSelectedValue         = productsModel.getFirstAddedMaterialItem(iIOPositionSelectedValue);
            iAvailableNewMaterialSelectedValue  = productsModel.getFirstAvailableMaterialItem();
        }
        public bool xEnableProductMenu
        {
            get
            {
                return _xEnableProductMenu;
            }
            set
            {
                _xEnableProductMenu = value;
                OnPropertyChanged("xEnableProductMenu");
            }
        }
        private void initializeProductsEditor()
        {
            //reloadProductsEditor();
            fillProjectList();
            fillObjectList("", _iProjectSelectedValue);
            objectIsChanged();
        }
        private void projectIsChanged()
        {
            fillObjectList("", _iProjectSelectedValue);
            objectIsChanged();

            _sSearchProjectList = "";
            OnPropertyChanged("sSearchProjectList");
        }
        private void objectIsChanged()
        {
            updateObjectDetails(iObjectSelectedValue, false);
        }
        private void fillObjectList(string sFilter = "", int iProjectId = 0)
        {
            productsModel.getProducts(sFilter, iProjectId);

            if (productsModel.obcProducts.Count > 0)
            {
                xEnableCbbProductsCode  = true;
                _iObjectSelectedValue   = productsModel.obcProducts[0].iSelectedvaluePath;
                OnPropertyChanged("iObjectSelectedValue");
            } else
            {
                _iObjectSelectedValue = -1;
                OnPropertyChanged("iObjectSelectedValue");
            }
        }
        private void fillAddedMaterials()
        {
            productsModel.getAddedMaterials(iIOPositionSelectedValue);       // Get all added materials depending on currently selected IO position
            productsModel.getAvailableMaterials(iIOPositionSelectedValue);

            iAddedMaterialSelectedValue         = productsModel.getFirstAddedMaterialItem(iIOPositionSelectedValue);
            iAvailableNewMaterialSelectedValue  = productsModel.getFirstAvailableMaterialItem();
        }
        private void updateObjectDetails(int iObjectId, bool xIsNew)   // Update profile details from database. 
        {
            int iObject = 0;
            if(xEditObjectMode) // When in edit mode, use selected item from copy list. 
            {
                iObject = iCopyObjectSelectedValue;
            } else
            {
                iObject = iObjectSelectedValue;
            }
            productsModel.updateProductDetails(iObjectId, xIsNew);
            productsModel.getIOPositions(iProjectSelectedValue, iObject);
            productsModel.getAddedMaterials(iIOPositionSelectedValue);       // Get all added materials depending on currently selected IO position
            productsModel.getAvailableMaterials(iIOPositionSelectedValue);   // Get all available materials depending on currently selected IO position

            iAddedMaterialSelectedValue         = productsModel.getFirstAddedMaterialItem(iIOPositionSelectedValue);
            iAvailableNewMaterialSelectedValue  = productsModel.getFirstAvailableMaterialItem();

            _iIOPositionSelectedvalue = 0;
            OnPropertyChanged("iIOPositionSelectedValue");

        }
        /*private void reloadProductsEditor()
        {
            if (!productsModel.reloadDatabase())
            {
                MessageBox.Show(clLanguages.getName("__NoDatabaseConnection"));
            }
        }*/
        private void fillProjectList(string sFilter = "")
        {
            productsModel.getProjects(sFilter);

            if (productsModel.clProjects.obcItems.Count > 0)
            {
                _iProjectSelectedValue = productsModel.clProjects.obcItems[0].iSelectedvaluePath;
                OnPropertyChanged("iProjectSelectedValue");
            }
            else
            {
                _iProjectSelectedValue = -1;
                OnPropertyChanged("iProjectSelectedValue");
            }
        }
        private void dummyFunction()
        {

        }
        private void controlCopyPopUpObject(bool xStatus, string sFilter = "")          // Control copy Project popup
        {
            if(xStatus) // Is enabled, select first item
            {
                productsModel.getFilteredCopyProduct(sFilter);
                if (productsModel.obcCopyProducts.Count() > 0)
                    iCopyObjectSelectedValue = productsModel.obcCopyProducts[0].iSelectedvaluePath;
                else
                    iCopyObjectSelectedValue = 0;
            }

            xShowPopupCopyObject = xStatus;
        }
        private void openFileDialog()             // Open browse folder, to select folder where the label is located 
        {
            OpenFileDialog fbdImagePath     = new OpenFileDialog();
            fbdImagePath.DefaultExt         = "jpg";
            fbdImagePath.Filter             = "jpg files (*.jpg)|*.jpg";
            string sCurrentDirectory        = Directory.GetCurrentDirectory();

            fbdImagePath.InitialDirectory = sCurrentDirectory + "\\Images\\Products\\";   // Select initial path to folder browser 
            if (fbdImagePath.ShowDialog() == DialogResult.OK)       // When 'OK' buttun is pressed
            {
                productsModel.imagePath = fbdImagePath.FileName;    // Save path to current template
            }
        }
    }
}
