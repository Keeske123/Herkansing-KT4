using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Sewco.Resources.Helper_classes;
using Sewco.Modules.ControlPanel;
using Sewco.Modules.MaterialsEditor;
using System.IO;
using System.Data;
using System.Windows.Data;
using System.Reflection;

namespace Sewco.Modules.MaterialsEditor
{


    public class ViewModelMaterials : ObservableObject
    {
        public ViewModelMaterials()
        {
            if (ViewModelControlPanel.xValidDatabaseConnection)
            {
                xEnableScreen = true;
                xEnableMaterialMenu = false;

                #region RelayCommands
                newMaterialCommand = new RelayCommand(
                        param => newMaterial(),
                        param => (!xEditMode)            // Only visible when not in New of Change mode
                    );
                saveMaterialCommand = new RelayCommand(
                        param => saveMaterial(),
                        param => (!xContentHasError && (xChangeMode || xNewMode))
                    );
                cancelMaterialCommand = new RelayCommand(
                        param => cancelMaterial(),                   // Cancel Material editing.
                        param => (xEditMode)                 // Is visible in Material edit mode.. (New and change mode)
                    );
                editMaterialCommand = new RelayCommand(
                        param => editMaterial(),             // Edit Material button  
                        param => (!xEditMode && materialsModel.obcMaterials.Count > 0)        // (!xEditMode) Visible when not in edit mode.         
                    );
                deleteMaterialCommand = new RelayCommand(
                        param => deleteMaterial(),           // Delete Material
                        param => (xChangeMode)       // Visible when not in edit mode.. TODO: Ook niet wanneer deze vaker gebruikt wordt in bovenliggende database..
                    );
                addMaterialTypeCommand = new RelayCommand(
                    param => addMaterialType(),
                    param => (true)
                );
                deleteMaterialTypeCommand = new RelayCommand(
                    param => deleteMaterialType(param),
                    param => (true)
                );
                firstMaterialCommand = new RelayCommand(
                    param => selectFirstMaterial(),
                    param => (!xNewMode && !xChangeMode)
                );

                lastMaterialCommand = new RelayCommand(
                        param => selectLastMaterial(),
                        param => (!xNewMode && !xChangeMode)
                    );
                copyMaterialCommand = new RelayCommand(
                        param => controlCopyMaterialPopUp(true),     // Show popup screen, to copy a current Material
                        param => (xEditMode)                 // Only visible when in edit mode
                    );
                cancelCopyMaterialCommand = new RelayCommand(
                        param => controlCopyMaterialPopUp(false),
                        param => (true)
                    );
                selectCopyMaterialCommand = new RelayCommand(
                        param => copyMaterial(),
                        param => (iCopyMaterialSelectedValue != 0)       // TODO
                    );

                selectImagePathFolderCommand = new RelayCommand(
                        param => openFileDialog(),
                        param => (true)
                    );

                #endregion
                materialsModel = new MaterialsBusinessObject();

                initializeMaterialsEditor();
            }
            else
            {
                xEnableScreen = false;
            }

            sImagePathLogo = Directory.GetCurrentDirectory() + "\\Images\\logo.png";

        }


        #region private properties declarations
        private bool _xEnableScreen;
        private int _iMaterialSelectedValue;
        private int _iCopyMaterialSelectedValue;
        private bool _xChangeMode;
        private bool _xNewMode;
        private bool _xEditMode;
        private string _sSearchMaterialList;
        private bool _xShowPopupCopyMaterial;
        private bool _xShowPopupEditMaterial;
        private string _sfilterPopupCopyMaterial;
        private bool _xDoubleOptionSelected; 
        private bool _xNoDeviceTypeSelected;
        private string _sImagePathLogo;
        #endregion

        #region relayCommands declarations
        public RelayCommand newMaterialCommand { get; set; }
        public RelayCommand saveMaterialCommand { get; set; }
        public RelayCommand cancelMaterialCommand { get; set; }
        public RelayCommand editMaterialCommand { get; set; }
        public RelayCommand deleteMaterialCommand { get; set; }
        public RelayCommand copyMaterialCommand { get; set; }
        public RelayCommand cancelCopyMaterialCommand { get; set; }
        public RelayCommand selectCopyMaterialCommand { get; set; }

        public RelayCommand firstMaterialCommand { get; set; }
        public RelayCommand lastMaterialCommand { get; set; }

        public RelayCommand addMaterialTypeCommand { get; set; }
        public RelayCommand deleteMaterialTypeCommand { get; set; }

        public RelayCommand selectImagePathFolderCommand { get; set; }

        public RelayCommand cbbScanbackSelectedValueChanged { get; set; }

        #endregion

        #region public properties declarations

        public static MaterialsBusinessObject materialsModel { get; set; }

        private ObservableCollection<int> _obcSelectedMaterialTypeItems { get; set; } = new ObservableCollection<int>();

        public string sImagePathLogo
        {
            get
            {
                return _sImagePathLogo;
            }
            set
            {
                _sImagePathLogo = value;
            }
        }

        public ObservableCollection<int> obcSelectedMaterialTypeItems
        {
            get
            {
                return _obcSelectedMaterialTypeItems;
            }

            set
            {
                _obcSelectedMaterialTypeItems = value;
            }

        }


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

        
        public bool xNoDeviceTypeSelected
        {
            get
            {
                return _xNoDeviceTypeSelected;
            }
            set
            {
                _xNoDeviceTypeSelected = value;
                OnPropertyChanged("xNoDeviceTypeSelected");
            }
        }
        public bool xDoubleOptionSelected
        {
            get
            {
                return _xDoubleOptionSelected;
            }
            set
            {
                _xDoubleOptionSelected = value;
                OnPropertyChanged("xDoubleOptionSelected");
            }
        }
        public int iMaterialSelectedValue
        {
            get
            {
                return _iMaterialSelectedValue;
            }
            set
            {
                _iMaterialSelectedValue = value;
                materialIsChanged();

                OnPropertyChanged("iMaterialSelectedValue");
            }
        }
        public int iCopyMaterialSelectedValue
        {
            get
            {
                return _iCopyMaterialSelectedValue;
            }
            set
            {
                _iCopyMaterialSelectedValue = value;
                OnPropertyChanged("iCopyMaterialSelectedValue");
            }
        }
        public bool xChangeMode
        {
            get { return _xChangeMode; }
            set
            {
                _xChangeMode = value;
                xEditMode = value;
                OnPropertyChanged("xChangeMode");
            }
        }
        public bool xNewMode
        {
            get { return _xNewMode; }
            set
            {
                _xNewMode = value;
                xEditMode = value;
                OnPropertyChanged("xNewMode");
            }
        }
        public bool xEditMode
        {
            get { return _xEditMode; }
            set
            {
                _xEditMode = value;
                materialsModel.propertyValidation(value);
                OnPropertyChanged("xEditMode");
            }
        }
        public string sSearchMaterialList
        {
            get
            {
                return _sSearchMaterialList;
            }
            set
            {
                _sSearchMaterialList = value;
                OnPropertyChanged("sSearchMaterialList");
                fillMaterialList(value);
                updateMaterialDetails(iMaterialSelectedValue, false);
            }
        }
        public bool xShowPopupCopyMaterial
        {
            get
            {
                return _xShowPopupCopyMaterial;
            }
            set
            {
                _xShowPopupCopyMaterial = value;
                xEnableScreen = !value;
                sfilterPopupCopyMaterial = "<DoNothing>";
                OnPropertyChanged("xShowPopupCopyMaterial");
            }
        }
        public bool xShowPopupEditMaterial
        {
            get
            {
                return _xShowPopupEditMaterial;
            }
            set
            {
                _xShowPopupEditMaterial = value;
                xEnableScreen = !value;
                OnPropertyChanged("xShowPopupEditMaterial");
            }
        }
        public string sfilterPopupCopyMaterial
        {
            get
            {
                return _sfilterPopupCopyMaterial;
            }
            set
            {
                if (value != "<DoNothing>")
                {
                    controlCopyMaterialPopUp(xShowPopupCopyMaterial, value);
                    _sfilterPopupCopyMaterial = value;
                }
                else
                    _sfilterPopupCopyMaterial = "";
                OnPropertyChanged("sfilterPopupCopyMaterial");
            }
        }

        private bool _xEnableMaterialMenu;
        public bool xEnableMaterialMenu
        {
            get
            {
                return _xEnableMaterialMenu;
            }
            set
            {
                _xEnableMaterialMenu = value;
                OnPropertyChanged("xEnableMaterialMenu");
            }
        }

        private static bool _xScanbackSelectionChanged;
        public static bool xScanbackSelectionChanged
        {
            get
            {
                return _xScanbackSelectionChanged;
            }
            set
            {
                if ((_xScanbackSelectionChanged != value))
                {
                    test();
                }
                _xScanbackSelectionChanged = value;


                //OnPropertyChanged("xScanbackSelectionChanged");
            }
        }
        #endregion

        #region private functions
        private bool xContentHasError
        {
            get
            {
                return materialsModel.validationStatus();   
            }
        }
        private static void test()
        {
            //materialsModel.reloadFilteredComboboxes();
            //materialsModel.reloadDatabase();
        }
        private void initializeMaterialsEditor()
        {
            reloadMaterialsEditor();
            selectFirstMaterial();
            materialIsChanged();
        }
        private void newMaterial()
        {
            materialsModel.emptyMaterial();
            xNewMode = true;
        }
        private void editMaterial()
        {
            xChangeMode = true;         // Make input field editable. 
            materialIsChanged();
        }
        private void deleteMaterial()
        {
            if (!materialsModel.deleteMaterial(materialsModel.materialId))
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase"));
            }

            selectFirstMaterial();
            cancelMaterial();
        }
        private void copyMaterial()
        {
            controlCopyMaterialPopUp(false);
            updateMaterialDetails(iCopyMaterialSelectedValue, true);
            //xNewMode = true;
            iCopyMaterialSelectedValue = 0;
        }
        private void saveMaterial()
        {
            fillUsedMaterialTypes();
            if (xDoubleOptionSelected)
            {
                MessageBox.Show(clLanguages.getName("__ErrorMaterialTypeAlreadyInUse"));
            }
            else if(xNoDeviceTypeSelected)
            {
                MessageBox.Show(clLanguages.getName("__ErrorMaterialTypeNotSelected"));
            }
            else
            {
                if (xNewMode)
                {
                    xNewMode = false;            // Disable new Material mode
                    if (!materialsModel.saveNewMaterial())
                    {
                        MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase"));
                    }

                    reloadMaterialsEditor();

                    selectLastMaterial();        // Select last row from database, because an new Material is added, which is the last entry
                    materialIsChanged();         // Update all values
                }
                if (xChangeMode)
                {
                    xChangeMode = false;
                    if (!materialsModel.saveEditedMaterial())
                    {
                        MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase"));
                    }
                    reloadMaterialsEditor();
                    materialIsChanged();
                }
            }
        }
        private void cancelMaterial()
        {
            xNewMode = false;
            xChangeMode = false;

            iCopyMaterialSelectedValue = 0;

            reloadMaterialsEditor();                     // Must be called, so reinitialize the combobox.. A possible search is used
            materialIsChanged();
        }
        private void selectFirstMaterial()
        {
            int iFirstItemId = materialsModel.getFirstMaterialId();

            if (iFirstItemId > 0)
            {
                _iMaterialSelectedValue = iFirstItemId;
                OnPropertyChanged("iMaterialSelectedValue");
            }
        }
        private void selectLastMaterial()        // select last row from database
        {
            int iLastItemId = materialsModel.getLastMaterialId();

            if (iLastItemId > 0)
            {
                _iMaterialSelectedValue = iLastItemId;
                OnPropertyChanged("iMaterialSelectedValue");
            }
        }
        private void materialIsChanged()
        {
            updateMaterialDetails(iMaterialSelectedValue, false);

            _sSearchMaterialList = "";
            OnPropertyChanged("sSearchMaterialList");
        }
        private void updateMaterialDetails(int iIdNumber, bool xIsNew)   // Update profile details from database. 
        {
            materialsModel.updateMaterialDetails(iIdNumber, xIsNew);

            fillMaterialType(iIdNumber);
        }
        private void fillMaterialType(int iIdNumber)
        {
            materialsModel.getMaterialTypes(iIdNumber);
        }
        private void reloadMaterialsEditor()
        {
            if (!materialsModel.reloadDatabase())
            {
                System.Windows.Forms.MessageBox.Show(clLanguages.getName("__NoDatabaseConnection"));
            }

            fillMaterialList();
            fillMaterialType(_iMaterialSelectedValue);
            fillUsedMaterialTypes();
        }
        private void fillUsedMaterialTypes()
        {

            xDoubleOptionSelected = false;
            xNoDeviceTypeSelected = false;
            foreach (var x in obcSelectedMaterialTypeItems.ToList())
            {
                obcSelectedMaterialTypeItems.Remove(x);
            }

           

            var i = 0;


            foreach (var x in materialsModel.obcMaterialType.ToList())
            {
                obcSelectedMaterialTypeItems.Insert(i, x.iSelectedvalueMaterialType);
                i++;
            }

            var dict = new Dictionary<int, int>();

            foreach (var value in obcSelectedMaterialTypeItems.ToList())
            {
                if (value == 0)
                {
                    xNoDeviceTypeSelected = true;
                }

                if (dict.ContainsKey(value) && value != 0)
                {
                    dict[value]++;
                    xDoubleOptionSelected = true;
                }
                else
                {
                    dict[value] = 1;
                }
            }
        }
        private void fillMaterialList(string sFilter = "")
        {
            materialsModel.getMaterials(sFilter);
            if(materialsModel.obcMaterials.Count() > 0)
            { 
                _iMaterialSelectedValue = materialsModel.obcMaterials[0].iSelectedvaluePath;      // Set value to first search item, but only when something has been found during search
                OnPropertyChanged("iMaterialSelectedValue");
            } else {
                _iMaterialSelectedValue = -1;
                OnPropertyChanged("iMaterialSelectedValue");
            }
        }
        /*
        private void fillMaterialType(int iMaterialId)
        {
            if (materialsModel.getMaterialTypes(iMaterialId) == null)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingDatabase"));
            }
        }
   */
        private void addMaterialType()
        {
            materialsModel.addMaterialType();
        }
        private void deleteMaterialType(object oSelectedValue)
        {
            int iSelectedValue = Convert.ToInt32(oSelectedValue);
            materialsModel.deleteMaterialType(iSelectedValue);
        }
        private void controlCopyMaterialPopUp(bool xStatus, string sFilter = "")          // Control copy Material popup
        {
            if (xStatus) // Is enabled, select first item
            {
                if (materialsModel.getFilteredCopyMaterial(sFilter) == null)
                {
                    MessageBox.Show(clLanguages.getName("__ErrorReadingDatabase"));
                }

                if (materialsModel.obcCopyMaterials.Count() > 0)
                    iCopyMaterialSelectedValue = materialsModel.obcCopyMaterials[0].iSelectedvaluePath;
                else
                    iCopyMaterialSelectedValue = 0;
            }
            xShowPopupCopyMaterial = xStatus;
        }
        private void openFileDialog()             // Open browse folder, to select folder where the label is located 
        {
            OpenFileDialog fbdImagePath = new OpenFileDialog();
            fbdImagePath.DefaultExt = "jpg";
            fbdImagePath.Filter = "jpg files (*.jpg)|*.jpg";
            string sCurrentDirectory = Directory.GetCurrentDirectory();

            fbdImagePath.InitialDirectory = sCurrentDirectory + "\\Images\\Material\\";   // Select initial path to folder browser 
            if (fbdImagePath.ShowDialog() == System.Windows.Forms.DialogResult.OK)      // When 'OK' buttun is pressed
            {
                materialsModel.imagePath = fbdImagePath.FileName;                    // Save path to current template
            }
        }
        #endregion
    }

}
