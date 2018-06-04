using System;
using System.Linq;
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

namespace Sewco.Modules.ProjectsEditor
{
    public class clIOPosition : Material_IOPosition
    {
        public clIOPosition(int _argiProjectId, int _argiIOPosition, int _argiMaterialType, int _argiDeviceTypeId, ObservableCollection<clCbbFilltype1> _argobcMaterialTypeOptions, ObservableCollection<clCbbFilltype1> _argobcDeviceTypeOptions)
        {
            this.projectId = _argiProjectId;
            this.materialIOPosition = _argiIOPosition;
            this.iSelectedvalueMaterialType = _argiMaterialType;
            this.iSelectedvalueDeviceType = _argiDeviceTypeId;
            this.obcMaterialTypeOptions = _argobcMaterialTypeOptions;
            this.obcDeviceTypeOptions = _argobcDeviceTypeOptions;
            this.xIsLastItem = false;
        }

        


        private int _iSelectedvaluePath;
        public int iSelectedvaluePath
        {
            get
            {
                return _iSelectedvaluePath;
            }
            set
            {
                _iSelectedvaluePath = value;
                SendPropertyChanged("iSelectedvaluePath");
            }
        }
        
        private int _iSelectedvalueMaterialType;
        public int iSelectedvalueMaterialType
        {
            get
            {
                return _iSelectedvalueMaterialType;
            }
            set
            {
                _iSelectedvalueMaterialType = value;

                if (_iSelectedvalueMaterialType == 0)
                {
                    iBorderThicknessMaterialType = 2;
                }
                else
                {
                    iBorderThicknessMaterialType = 0;
                }

                SendPropertyChanged("iSelectedvalueMaterialType");
            }
        }

        private int _iSelectedvalueDeviceType;
        public int iSelectedvalueDeviceType
        {
            get
            {
                return _iSelectedvalueDeviceType;
            }
            set
            {
                _iSelectedvalueDeviceType = value;

                if (_iSelectedvalueDeviceType == 0)
                {
                    iBorderThicknessDeviceType = 2;
                }
                else
                {
                    iBorderThicknessDeviceType = 0;
                }
             
                SendPropertyChanged("iSelectedvalueDeviceType");
            }
        }

        private int _iBorderThicknessMaterialType;
        public int iBorderThicknessMaterialType
        {
            get
            {
                return _iBorderThicknessMaterialType;
            }
            set
            {
                _iBorderThicknessMaterialType = value;
                SendPropertyChanged("iBorderThicknessMaterialType");
            }
        }

        private int _iBorderThicknessDeviceType;
        public int iBorderThicknessDeviceType
        {
            get
            {
                return _iBorderThicknessDeviceType;
            }
            set
            {
                _iBorderThicknessDeviceType = value;
                SendPropertyChanged("iBorderThicknessDeviceType");
            }
        }


        private bool _xIsLastItem;
        public bool xIsLastItem
        {
            get
            {
                return _xIsLastItem;
            }
            set
            {
                _xIsLastItem = value;
                SendPropertyChanged("xIsLastItem");
            }
        }

        public ObservableCollection<clCbbFilltype1> obcMaterialTypeOptions { get; set; } = new ObservableCollection<clCbbFilltype1>();
        public ObservableCollection<clCbbFilltype1> obcDeviceTypeOptions { get; set; } = new ObservableCollection<clCbbFilltype1>();
    }

    public class clProducts : Product
    {
        public clProducts(int _argiProjectId, int _argiProfileId, int _argiProductId, bool _argxActive, string _argsName, int _argiCoverId, int _argiTypeId)
        {
            this.projectId  = _argiProjectId;
            this.profileId  = _argiProfileId;
            this.productId  = _argiProductId;
            this.active     = _argxActive;
            this.name       = _argsName;
            this.coverId    = _argiCoverId;
            this.typeId     = _argiTypeId;


        }

        private int _iSelectedvaluePath;
        public int iSelectedvaluePath
        {
            get
            {
                return _iSelectedvaluePath;
            }
            set
            {
                _iSelectedvaluePath = value;
                SendPropertyChanged("iSelectedvaluePath");
            }
        }
        
    }

    /*
    public static class clAvailable 
    {

        public static ObservableCollection<int> list;

       public static void listUsedComboboxItems()
       {

           if (ProjectsBusinessObject.clOption1Scanback.iSelectedValue != 0)
           {
                list[0] = _clOption1Scanback.iSelectedValue;
           }
           if (_clOption3Bobbin.iSelectedValueItem1 != 0)
           {
                list[1] = _clOption3Bobbin.iSelectedValueItem1;
           }
           if (_clOption3Bobbin.iSelectedValueItem2 != 0)
           {
                list[2] = _clOption3Bobbin.iSelectedValueItem2;
           }

           int i = 0;
           foreach (var IOPos in _obcIOPositions)
           {
                if (IOPos.iSelectedvalueDeviceType != 0)
                {
                    list[3 + i] = IOPos.iSelectedvalueDeviceType;
                }
                i++;
           }
       }

       public static ObservableCollection<clCbbFilltype1> filter(int cbbNumber)
       {
            listUsedComboboxItems();

           ObservableCollection<clCbbFilltype1> obcFilteredDeviceTypeOptions = new ObservableCollection<clCbbFilltype1>();

           // Fill combobox
           foreach (var q in getAvailableDeviceTypes())
           {
               //obcFilteredDeviceTypeOptions.Add(new clCbbFilltype1(q.deviceName, q.deviceTypeId));
               obcFilteredDeviceTypeOptions.Add(new clCbbFilltype1(q.sDisplayName, q.iSelectedvaluePath));
           }

           // Filter combobox with unused devicetypes and selected.
           foreach (var selectedValue in obcFilteredDeviceTypeOptions.ToList())
           {
               foreach (var usedSelectedValue in list)
               {
                   // Remove all in array except selected value of this combobox.
                   if (((int)usedSelectedValue != list[cbbNumber]) &&                  // if selected value not selected by this combobox
                       (selectedValue.iSelectedvaluePath != 0) &&                      // if selected value not equal to '0' (not used), always visible
                       (selectedValue.iSelectedvaluePath == (int)usedSelectedValue))   // if selected value is equal to value in used values list
                   {
                       // Remove devicetype because it's selected in another combobox.
                       obcFilteredDeviceTypeOptions.Remove(selectedValue);
                   }
               }
           }
           return obcFilteredDeviceTypeOptions;
       } 

    }*/

    public class ProjectsBusinessObject : Project, IDataErrorInfo
    {
        public ProjectsBusinessObject()
        {
            this.clProductionModeOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName(clConstants.ProductionMode[0]), clConstants.PRODUCTION_SINGLE_MODE));
            this.clProductionModeOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName(clConstants.ProductionMode[1]), clConstants.PRODUCTION_BATCH_MODE));
            this.clProductionModeOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName(clConstants.ProductionMode[2]), clConstants.PRODUCTION_UNLIMITED_MODE));

            this.clCounterResetOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName(clConstants.ResetOptions[0]), clConstants.RESET_DAILY));
            this.clCounterResetOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName(clConstants.ResetOptions[1]), clConstants.RESET_WEEKLY));
            this.clCounterResetOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName(clConstants.ResetOptions[2]), clConstants.NO_RESET));

            this.clProductionCodesOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName(clConstants.ResetOptions[0]), clConstants.FORD));
            this.clProductionCodesOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName(clConstants.ResetOptions[1]), clConstants.VOLVO));

            this.clLanguageList.obcItems.Add(new clCbbFilltype1("Dutch",    0));
            this.clLanguageList.obcItems.Add(new clCbbFilltype1("English",  1));

            clInputErrors   = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__ProjectName")));
            clInputErrors.Add(new clErrors(clConstants.STRINGCHECK, clLanguages.getName("__ProjectName")));
            sProjectName    = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__Comment")));
            clInputErrors.Add(new clErrors(clConstants.STRINGCHECK, clLanguages.getName("___Comment")));
            sComment        = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.STRINGCHECK, clLanguages.getName("__MachineId")));
            sMachineId      = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.STRINGCHECK, clLanguages.getName("__PlantId")));
            sPlantId        = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.STRINGCHECK, clLanguages.getName("__PlantInfo") ));
            sPlantInfo      = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.STRINGCHECK, clLanguages.getName("__CustomerId") ));
            sCustomerId     = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.STRINGCHECK, clLanguages.getName("__CustomerInfo")));
            sCustomerInfo   = new clTextBoxValidation(clInputErrors);

            getLanguages();
        }

        #region private properties declarations
        private ObservableCollection<clIOPosition>   _obcIOPositions                { get; set; } = new ObservableCollection<clIOPosition>();
        private ObservableCollection<clCbbFilltype1> _obcProjects                   { get; set; } = new ObservableCollection<clCbbFilltype1>();
        private ObservableCollection<clCbbFilltype1> _obcCopyProjects               { get; set; } = new ObservableCollection<clCbbFilltype1>();
        private ObservableCollection<clProducts> _obcProducts                       { get; set; } = new ObservableCollection<clProducts>();
        private ObservableCollection<clCbbFilltype1> _obcAvailableMaterialTypes     { get; set; } = new ObservableCollection<clCbbFilltype1>();
        private ObservableCollection<clCbbFilltype1> _obcAvailableDeviceTypes       { get; set; } = new ObservableCollection<clCbbFilltype1>();

        //private int[] ariDeviceTypeUsed                                                           = new int[10];

        private clcbbOptions _clProductionModeOptions                               { get; set; } = new clcbbOptions();
        private clcbbOptions _clCounterResetOptions                                 { get; set; } = new clcbbOptions();
        private clcbbOptions _clProductionCodesOptions                              { get; set; } = new clcbbOptions();
        private clcbbProjectOptions _clOption1Scanback                              { get; set; } = new clcbbProjectOptions(0, true);
        private clcbbProjectOptions _clOption2VWcode                                { get; set; } = new clcbbProjectOptions(0, false);
        private clcbbProjectOptions2 _clOption3Bobbin                               { get; set; } = new clcbbProjectOptions2(1, 2, true);

        private clcbbOptions _clLanguageList { get; set; } = new clcbbOptions();

        


        private string  _sProjectName;
        private string  _sComment;
        private string  _sMachineId;
        private string  _sPlantId;
        private string  _sPlantInfo;
        private string  _sCustomerId;
        private string  _sCustomerInfo;

        public static  ObservableCollection<int> usedDeviceTypelist;

        private static int _staticSelectedValueScanback;
        public static int staticSelectedValueScanback
        {
            get
            {
                return _staticSelectedValueScanback;
            }
            set {
                _staticSelectedValueScanback = value;
            }

        }

        /*
        public static void listUsedComboboxItems()
        {

            if (clOption1Scanback.iSelectedValue != 0)
            {
                usedDeviceTypelist[0] = _clOption1Scanback.iSelectedValue;
            }
            if (_clOption3Bobbin.iSelectedValueItem1 != 0)
            {
                usedDeviceTypelist[1] = _clOption3Bobbin.iSelectedValueItem1;
            }
            if (_clOption3Bobbin.iSelectedValueItem2 != 0)
            {
                usedDeviceTypelist[2] = _clOption3Bobbin.iSelectedValueItem2;
            }
            /*
            int i = 0;
            foreach (var IOPos in _obcIOPositions)
            {
                if (IOPos.iSelectedvalueDeviceType != 0)
                {
                    ariDeviceTypeUsed[3 + i] = IOPos.iSelectedvalueDeviceType;
                }
                i++;
            }
        }
        
        public static  ObservableCollection<clCbbFilltype1> filter(int cbbNumber, ObservableCollection<clCbbFilltype1> _obcFilteredDeviceTypeOptions)
        {
            //listUsedComboboxItems();
            //_obcFilteredDeviceTypeOptions = null;
            // Fill combobox
            foreach (var q in getStaticAvailableDeviceTypes())
            {
                //obcFilteredDeviceTypeOptions.Add(new clCbbFilltype1(q.deviceName, q.deviceTypeId));
                _obcFilteredDeviceTypeOptions.Add(new clCbbFilltype1(q.sDisplayName, q.iSelectedvaluePath));
            }

            if (_obcFilteredDeviceTypeOptions != null)
            {
                // Filter combobox with unused devicetypes and selected.
                foreach (var selectedValue in _obcFilteredDeviceTypeOptions.ToList())
                {
                    if (usedDeviceTypelist != null) {
                        foreach (var usedSelectedValue in usedDeviceTypelist.ToList())
                        {
                            // Remove all in array except selected value of this combobox.
                            if (((int)usedSelectedValue != usedDeviceTypelist[cbbNumber]) &&                  // if selected value not selected by this combobox
                                (selectedValue.iSelectedvaluePath != 0) &&                      // if selected value not equal to '0' (not used), always visible
                                (selectedValue.iSelectedvaluePath == (int)usedSelectedValue))   // if selected value is equal to value in used values list
                            {
                                // Remove devicetype because it's selected in another combobox.
                                _obcFilteredDeviceTypeOptions.Remove(selectedValue);
                            }
                        }
                    }
                }
                
            }
            return _obcFilteredDeviceTypeOptions;
        }
        */
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

        #endregion

        #region public properties declararions

        public clcbbProjectOptions clOption1Scanback
        {
            get
            {
                return _clOption1Scanback;
            }
            set
            {
                _clOption1Scanback = value;
               
                
                SendPropertyChanged("clOption1Scanback");
            }
        }
        public clcbbProjectOptions clOption2VWcode
        {
            get
            {
                return _clOption2VWcode;
            }
            set
            { 
                _clOption2VWcode = value;
                SendPropertyChanged("clOption2VWcode");
            }
        }
        public clcbbProjectOptions2 clOption3Bobbin
        {
            get
            {
                return _clOption3Bobbin;
            }
            set
            {
                _clOption3Bobbin = value;
                SendPropertyChanged("clOption3Bobbin");
            }
        }

        public clcbbOptions clLanguageList
        {
            get
            {
                return _clLanguageList;
            }
            set
            {
                _clLanguageList = value;
                SendPropertyChanged("clLanguageList");
            }
        }

        public bool validateResultOK { get; set; }
        public bool xContentError;
        string IDataErrorInfo.Error
        {
            get { return null; }
        }
        List<string> list = new List<string>();
        public ObservableCollection<clIOPosition> obcIOPositions
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
        public ObservableCollection<clCbbFilltype1> obcProjects
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
        }
        public ObservableCollection<clCbbFilltype1> obcCopyProjects
        {
            get
            {
                return this._obcCopyProjects;
            }
            set
            {
                this._obcCopyProjects = value;
                SendPropertyChanged("obcCopyProjects");
            }
        }
        public ObservableCollection<clProducts> obcProducts
        {
            get
            {
                return this._obcProducts;
            }
            set
            {
                this._obcProducts = value;
                SendPropertyChanged("obcProducts");
            }
        }
        public ObservableCollection<clCbbFilltype1> obcAvailableMaterialTypes
        {
            get
            {
                return this._obcAvailableMaterialTypes;
            }
            set
            {
                this._obcAvailableMaterialTypes = value;
                SendPropertyChanged("obcAvailableMaterialTypes");
            }
        }
        private static ObservableCollection<clCbbFilltype1> _static_obcAvailableDeviceTypes;
        public static ObservableCollection<clCbbFilltype1> static_obcAvailableDeviceTypes
        {
            get
            {
                return _static_obcAvailableDeviceTypes;
            }
            set
            {
                _static_obcAvailableDeviceTypes = value;
            }
        }
        public  ObservableCollection<clCbbFilltype1> obcAvailableDeviceTypes
        {
            get
            {
                return _obcAvailableDeviceTypes;
            }
            set
            {
                _obcAvailableDeviceTypes = value;
                _static_obcAvailableDeviceTypes = value;
                SendPropertyChanged("obcAvailableDeviceTypes");
            }
        }
        public clcbbOptions clProductionModeOptions
        {
            get
            {
                return _clProductionModeOptions;
            }
            set
            {
                _clProductionModeOptions = value;
                SendPropertyChanged("clProductionModeOptions");
            }
        }
        public clcbbOptions clCounterResetOptions
        {
            get
            {
                return _clCounterResetOptions;
            }
            set
            {
                _clCounterResetOptions = value;
                SendPropertyChanged("clCounterResetOptions");
            }
        }
        public clcbbOptions clProductionCodesOptions
        {
            get
            {
                return _clProductionCodesOptions;
            }
            set
            {
                _clProductionCodesOptions = value;
                SendPropertyChanged("clProductionCodesOptions");
            }
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

           
                if (sResult != "" )
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
       /* public string sProjectName
        {
            get
            {
                return _sProjectName;
            }
            set
            {
                _sProjectName = value;
                SendPropertyChanged("sProjectName");
            }
        }
        public string sComment
        {
            get
            {
                return _sComment;
            }
            set
            {
                _sComment = value;
                SendPropertyChanged("sComment");
            }
        }
        public string sMachineId
        {
            get
            {
                return _sMachineId;
            }
            set
            {
                _sMachineId = value;
                SendPropertyChanged("sMachineId");
            }
        }
        public string sPlantId
        {
            get
            {
                return _sPlantId;
            }
            set
            {
                _sPlantId = value;
                SendPropertyChanged("sPlantId");
            }
        }
        public string sPlantInfo
        {
            get
            {
                return _sPlantInfo;
            }
            set
            {
                _sPlantInfo = value;
                SendPropertyChanged("sPlantInfo");
            }
        }
        public string sCustomerId
        {
            get
            {
                return _sCustomerId;
            }
            set
            {
                _sCustomerId = value;
                SendPropertyChanged("sCustomerId");
            }
        }
        public string sCustomerInfo
        {
            get
            {
                return _sCustomerInfo;
            }
            set
            {
                _sCustomerInfo = value;
                SendPropertyChanged("sCustomerInfo");
            }
        }*/
        /*
       public bool getSelectedValueChanged()
       {
           return _selectedValueChanged;
       }

       private static bool _selectedValueChanged;
       public static bool selectedValueChanged
       {
           get
           {
               return _selectedValueChanged;
           }
           set
           {
               _selectedValueChanged = value;
               ViewModelProjects.xScanbackSelectionChanged = value;
               if (_selectedValueChanged)
               {
                   _selectedValueChanged = false;
                   //reloadFilteredComboboxes();
                   // ProjectsBusinessObject newProj = new ProjectsBusinessObject();
                   // newProj.reloadFilteredComboboxes();

                   //_scanbackSelectedValueChanged = false;

               }
           }
       }*/

        private ObservableCollection<clErrors> clInputErrors = new ObservableCollection<clErrors>();
        public clTextBoxValidation sProjectName
        {
            get; set;
        }
        public clTextBoxValidation sComment
        {
            get; set;
        }
        public clTextBoxValidation sMachineId
        {
            get; set;
        }
        public clTextBoxValidation sPlantId
        {
            get; set;
        }
        public clTextBoxValidation sPlantInfo
        {
            get; set;
        }
        public clTextBoxValidation sCustomerId
        {
            get; set;
        }
        public clTextBoxValidation sCustomerInfo
        {
            get; set;
        }


        private int _iHighestIOPosition;
        #endregion
        clImage projectImage = new clImage();
        #region public functions
        public int saveNewProject()
        {
            Project saveNewProject          = new Project();                          // Create a new instance of Project

            saveNewProject.projectName      = sProjectName.sInput;
            saveNewProject.comment          = sComment.sInput;
            saveNewProject.active           = active;
            saveNewProject.machineId        = sMachineId.sInput;
            saveNewProject.plantId          = sPlantId.sInput;
            saveNewProject.plantInfo        = sPlantInfo.sInput;
            saveNewProject.customerId       = sCustomerId.sInput;
            saveNewProject.customerInfo     = sCustomerId.sInput;
            saveNewProject.productMode      = productMode;
            saveNewProject.counterReset     = counterReset;
            saveNewProject.prodCodesTable   = prodCodesTable;
            saveNewProject.imagePath        = imagePath;

            ViewModelControlPanel.DBDataClass.Projects.InsertOnSubmit(saveNewProject);     // Insert new Project   
            ViewModelControlPanel.DBDataClass.SubmitChanges();

            int iNewProjectId = saveNewProject.projectId;

            // Upload MaterialIOPositions to database
            foreach (var obcIOOptions in this.obcIOPositions.ToList())
            {
                Material_IOPosition newMaterial_IOPosition  = new Material_IOPosition();

                newMaterial_IOPosition.projectId            = iNewProjectId;
                newMaterial_IOPosition.materialIOPosition   = obcIOOptions.materialIOPosition;
                newMaterial_IOPosition.materialTypeId       = obcIOOptions.iSelectedvalueMaterialType;
                newMaterial_IOPosition.deviceTypeId         = obcIOOptions.iSelectedvalueDeviceType;

                ViewModelControlPanel.DBDataClass.Material_IOPositions.InsertOnSubmit(newMaterial_IOPosition);
            }
            ViewModelControlPanel.DBDataClass.SubmitChanges();

            Option option = new Option();

            if (clOption1Scanback.xActive)
            {
                option                  = new Option();
                option.projectId        = iNewProjectId;
                option.optionId         = 1;
                option.deviceTypeId1    = clOption1Scanback.iSelectedValue;
                option.deviceTypeId2    = 0;
                ViewModelControlPanel.DBDataClass.Options.InsertOnSubmit(option);
            }
            if (clOption2VWcode.xActive)
            {
                option                  = new Option();
                option.projectId        = iNewProjectId;
                option.optionId         = 2;
                option.deviceTypeId1    = 0;
                option.deviceTypeId2    = 0;
                ViewModelControlPanel.DBDataClass.Options.InsertOnSubmit(option);
            }
            if (clOption3Bobbin.xActive)
            {
                option                  = new Option();
                option.projectId        = iNewProjectId;
                option.optionId         = 3;
                option.deviceTypeId1    = clOption3Bobbin.iSelectedValueItem1;
                option.deviceTypeId2    = clOption3Bobbin.iSelectedValueItem2;
                ViewModelControlPanel.DBDataClass.Options.InsertOnSubmit(option);
            }

            ViewModelControlPanel.DBDataClass.SubmitChanges();

            // Upload MaterialIOPositions to database
            foreach (var obcProducts in this.obcProducts.ToList())
            {
                Product newProduct = new Product();

                newProduct.projectId            = iNewProjectId;
                newProduct.profileId            = obcProducts.profileId;
                //newProduct.productId            = obcProducts.productId; //Auto increment
                newProduct.name                 = obcProducts.name;
                newProduct.active               = obcProducts.active;
                newProduct.coverId              = obcProducts.coverId;
                newProduct.typeId               = obcProducts.typeId;

                ViewModelControlPanel.DBDataClass.Products.InsertOnSubmit(newProduct);
            }
            ViewModelControlPanel.DBDataClass.SubmitChanges();

            return iNewProjectId;
        }
        public int saveEditedProject()
        {
            Project editedProject = ViewModelControlPanel.DBDataClass.Projects.Single(c => c.projectId == projectId);  // Get the correct project

            editedProject.projectName   = sProjectName.sInput;
            editedProject.comment       = sComment.sInput;
            editedProject.active        = active;
            editedProject.machineId     = sMachineId.sInput;
            editedProject.plantId       = sPlantId.sInput;
            editedProject.plantInfo     = sPlantInfo.sInput;
            editedProject.customerId    = sCustomerId.sInput;
            editedProject.customerInfo  = sCustomerId.sInput;
            editedProject.productMode   = (short)clProductionModeOptions.iSelectedValue;
            editedProject.counterReset  = (short)clCounterResetOptions.iSelectedValue;
            editedProject.prodCodesTable = "";
            editedProject.imagePath     = imagePath;

            int iEditedProjectId        = editedProject.projectId;

            // First delete all current items 
            var deleteIOPositionsQuery =    from   x in ViewModelControlPanel.DBDataClass.Material_IOPositions
                                            where  x.projectId == iEditedProjectId
                                            select x;

            foreach (var q in deleteIOPositionsQuery)
            {
                ViewModelControlPanel.DBDataClass.Material_IOPositions.DeleteOnSubmit(q);
            }
            ViewModelControlPanel.DBDataClass.SubmitChanges();

            // Upload new values to database
            foreach (var obcIOOptions in this.obcIOPositions.ToList())
            {
                Material_IOPosition newMaterial_IOPosition = new Material_IOPosition();

                newMaterial_IOPosition.projectId            = iEditedProjectId;
                newMaterial_IOPosition.materialIOPosition   = obcIOOptions.materialIOPosition;
                newMaterial_IOPosition.materialTypeId       = obcIOOptions.iSelectedvalueMaterialType;
                newMaterial_IOPosition.deviceTypeId         = obcIOOptions.iSelectedvalueDeviceType;


                ViewModelControlPanel.DBDataClass.Material_IOPositions.InsertOnSubmit(newMaterial_IOPosition);
            }
            ViewModelControlPanel.DBDataClass.SubmitChanges();

            // First delete all current items 
            var deleteOptionsQuery = from   x in ViewModelControlPanel.DBDataClass.Options
                                        where  x.projectId == iEditedProjectId
                                        select x;

            foreach (var q in deleteOptionsQuery)
            {
                ViewModelControlPanel.DBDataClass.Options.DeleteOnSubmit(q);
            }
            ViewModelControlPanel.DBDataClass.SubmitChanges();


            Option option = new Option();

            if (clOption1Scanback.xActive)
            {
                option                  = new Option();
                option.projectId        = iEditedProjectId;
                option.optionId         = 1;
                option.deviceTypeId1    = clOption1Scanback.iSelectedValue;
                option.deviceTypeId2    = 0;
                ViewModelControlPanel.DBDataClass.Options.InsertOnSubmit(option);
            }
            if (clOption2VWcode.xActive)
            {
                option                  = new Option();
                option.projectId        = iEditedProjectId;
                option.optionId         = 2;
                option.deviceTypeId1    = 0;
                option.deviceTypeId2    = 0;
                ViewModelControlPanel.DBDataClass.Options.InsertOnSubmit(option);
            }
            if (clOption3Bobbin.xActive)
            {
                option                  = new Option();
                option.projectId        = iEditedProjectId;
                option.optionId         = 3;
                option.deviceTypeId1    = clOption3Bobbin.iSelectedValueItem1;
                option.deviceTypeId2    = clOption3Bobbin.iSelectedValueItem2;
                ViewModelControlPanel.DBDataClass.Options.InsertOnSubmit(option);
            }

            var languageQuery = from x in ViewModelControlPanel.DBDataClass.Settings
                                where x.Id == 0
                                orderby x.sLanguage ascending
                                select x;

            foreach (var l in languageQuery)
            {
                l.sLanguage = this.clLanguageList.obcItems[clLanguageList.iSelectedValue].sDisplayName;
            }

            ViewModelControlPanel.DBDataClass.SubmitChanges();

            return iEditedProjectId;
        }
        public bool deleteProject(int iProjectId)
        {
            try
            {
                // Delete I/O positions from project
                var deleteIOPositionsQuery = from x in ViewModelControlPanel.DBDataClass.Material_IOPositions
                                             where x.projectId == iProjectId
                                             select x;
                foreach (var q in deleteIOPositionsQuery)
                {
                    ViewModelControlPanel.DBDataClass.Material_IOPositions.DeleteOnSubmit(q);
                }

                ViewModelControlPanel.DBDataClass.SubmitChanges();

                // Delete products from project
                var deleteProductsQuery = from x in ViewModelControlPanel.DBDataClass.Products
                                             where x.projectId == iProjectId
                                             select x;
                foreach (var q in deleteProductsQuery)
                {
                    ViewModelControlPanel.DBDataClass.Products.DeleteOnSubmit(q);
                }

                ViewModelControlPanel.DBDataClass.SubmitChanges();

                // Delete Project
                var deleteProjectQuery  =   from   x in ViewModelControlPanel.DBDataClass.Projects
                                            where  x.projectId == iProjectId
                                            select x;

                foreach (var q in deleteProjectQuery)
                {
                    ViewModelControlPanel.DBDataClass.Projects.DeleteOnSubmit(q);
                }

                ViewModelControlPanel.DBDataClass.SubmitChanges();

                return true;
            } catch
            {
                return false;
            }

        }
        public int getFirstProjectId()
        {
            if (this.obcProjects.Count > 0)
            {
                return this.obcProjects[0].iSelectedvaluePath;
            }
            else
            {
                return -1;
            }
        }
        public int getLastProjectId()
        {
            if (this.obcProjects.Count > 0)
            {
                return this.obcProjects[this.obcProjects.Count - 1].iSelectedvaluePath;
            }
            else
            {
                return -1;
            }
        }      
        public bool reloadDatabase()
        {
            try
            {
                ViewModelControlPanel.reloadDatabase();
                this.getProjects();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Project getProject(int iProjectId)
        {
            Project getProject = new Project();
            try
            {
                getProject = ViewModelControlPanel.DBDataClass.Projects.Single(c => c.projectId == iProjectId);
                return getProject;
            }
            catch
            {
                return null;
            }
        }
        public void getProjects(string sFilter = "")
        {
            foreach (var projects in this.obcProjects.ToList())          // Delete all items in this mirrow database list
            {
                this.obcProjects.Remove(projects);                       // Remove all database data from observableobject. 
            }

            try
            {
                var ProjectQuery = from     x in ViewModelControlPanel.DBDataClass.Projects
                                   where    x.projectName.Contains(sFilter) || x.comment.Contains(sFilter)
                                   orderby  x.projectId ascending
                                   select   x;
                foreach (var projects in ProjectQuery)
                {
                    this.obcProjects.Add(new clCbbFilltype1(projects.projectName + ",  " + projects.comment, projects.projectId));      // Add database data to observableobject. 
                }
            }
            catch
            {

            }
        }
        public void getLanguages()
        {
            try
            {
                var languageQuery = from x in ViewModelControlPanel.DBDataClass.Settings
                                    orderby x.sLanguage ascending
                                    select x;
                int i = 0;
                foreach (var l in languageQuery)
                {
                    if (l.sLanguage == "Dutch")
                    {
                        this.clLanguageList.iSelectedValue = 0;
                    }
                    else
                    {
                        this.clLanguageList.iSelectedValue = 1;
                    }
                    
                    //this.obcLanguageList.Add(new clCbbFilltype1(l.sLanguage, i));
                    i++;
                }
            }
            catch
            {
            }
        }
        public ObservableCollection<clIOPosition> getIOPositions(int iProjectId)
        {
            // This is only possible when an observable list doesn't have to be a property
            this.obcAvailableMaterialTypes  = new ObservableCollection<clCbbFilltype1>();
            this.obcAvailableDeviceTypes    = new ObservableCollection<clCbbFilltype1>();

            try
            {
                var materialTypesQuery = from       x in ViewModelControlPanel.DBDataClass.MaterialTypes
                                         orderby    x.materialTypeId ascending
                                         select     x;

                foreach (var m in materialTypesQuery)
                {
                    this.obcAvailableMaterialTypes.Add(new clCbbFilltype1(m.name, m.materialTypeId));
                }

                var deviceTypeQuery = from      x in ViewModelControlPanel.DBDataClass.DeviceTypes
                                      orderby   x.deviceTypeId ascending
                                      select    x;

                // Get all available device types from database
                foreach (var d in deviceTypeQuery)
                {
                    this.obcAvailableDeviceTypes.Add(new clCbbFilltype1(d.deviceName, d.deviceTypeId));
                }

                var IOPosQuery = from       x in ViewModelControlPanel.DBDataClass.Material_IOPositions
                                 where      x.projectId == iProjectId
                                 orderby    x.materialIOPosition ascending
                                 select     x;

                foreach (var q in this.obcIOPositions.ToList())
                {
                    this.obcIOPositions.Remove(q);
                }
                foreach (var q in IOPosQuery)
                {
                    // Get all available IO position from database. 
                    this.obcIOPositions.Add(new clIOPosition(q.projectId, q.materialIOPosition, q.materialTypeId, q.deviceTypeId, this.obcAvailableMaterialTypes, this.obcAvailableDeviceTypes));
                }

                _iHighestIOPosition = this.obcIOPositions.Count();      // Highest positions equals count(), starts at 1
                if (_iHighestIOPosition > 0)
                {
                    this.obcIOPositions[_iHighestIOPosition - 1].xIsLastItem = true;
                }
                return this.obcIOPositions;
            }
            catch
            {
                return null;
            }
        }
        public ObservableCollection<clProducts> getProducts(int iProjectId)
        {
           try
           {
                
                foreach (var itemToRemove in this.obcProducts.ToList())
                {
                    this.obcProducts.Remove(itemToRemove);
                }
                var productsQuery = from    x in ViewModelControlPanel.DBDataClass.Products
                                    where   x.projectId == iProjectId
                                    orderby x.productId ascending
                                    select  x;
                foreach (var p in productsQuery)
                {
                    this.obcProducts.Add(new clProducts((int)p.projectId, (int)p.profileId, p.productId, (bool)p.active, p.name, (int)p.coverId, (int)p.typeId));
                }
                return this.obcProducts;
           }
           catch
           {
                return null;
           }
        }
        public ObservableCollection<clCbbFilltype1> getFilteredCopyProject(string sFilter = "")
        {
            try
            {
                foreach (var project in this.obcCopyProjects.ToList())          // Delete all items in this mirrow database list
                {
                    this.obcCopyProjects.Remove(project);                       // Remove all database data from observableobject. 
                }
                var ProjectQuery = from     x in ViewModelControlPanel.DBDataClass.Projects
                                   where    x.projectName.Contains(sFilter) || x.comment.Contains(sFilter)
                                   orderby  x.projectId ascending
                                   select   x;
                foreach (var project in ProjectQuery)
                {
                    this.obcCopyProjects.Add(new clCbbFilltype1(clLanguages.getName("__Name") + ":\t\t" + project.projectName + "\n" + clLanguages.getName("__Comment") + ":\t" + project.comment, project.projectId));
                }
                return this.obcCopyProjects;
            }
            catch
            {
                return null;
            }
        }
        public ObservableCollection<clCbbFilltype1> getAvailableMaterialTypes()
        {
            return this.obcAvailableMaterialTypes;
        }
        public  ObservableCollection<clCbbFilltype1> getAvailableDeviceTypes()
        {
            return this.obcAvailableDeviceTypes;
        }
        public static ObservableCollection<clCbbFilltype1> getStaticAvailableDeviceTypes()
        {

            return _static_obcAvailableDeviceTypes;
        }
        public void getProjectOptions(int iProjectId)
        {
            try
            {
                // TODO, bekijken of dat dit naar .ToList kan gedaan worden ViewModelControlPanel.DBDataClass.Options.ToList(c => c.projectId == iProjectId); 
                var optionsQuery = from     x in ViewModelControlPanel.DBDataClass.Options
                                   where    x.projectId == iProjectId
                                   select   x;

                clOption1Scanback.xActive   = false;
                clOption2VWcode.xActive     = false;
                clOption3Bobbin.xActive     = false;


                

                clOption1Scanback.iSelectedValue        = 0;
                clOption3Bobbin.iSelectedValueItem1     = 0;
                clOption3Bobbin.iSelectedValueItem2     = 0;

                foreach (var o in optionsQuery)
                {
                    switch (o.optionId)
                    {
                        case 1:
                            clOption1Scanback.xActive                     = true;
                            clOption1Scanback.iSelectedValue    = (int)o.deviceTypeId1;
                            break;

                        case 2:
                            clOption2VWcode.xActive = true;
                            break;

                        case 3:
                            clOption3Bobbin.xActive                   = true;
                            clOption3Bobbin.iSelectedValueItem1 = (int)o.deviceTypeId1;
                            clOption3Bobbin.iSelectedValueItem2 = (int)o.deviceTypeId2;
                            break;

                        default:
                            break;
                    }
                }
                reloadFilteredComboboxes();

            } catch
            {

            }
        }
        public void addIOPosition()
        {
            _iHighestIOPosition = this.obcIOPositions.Count();
            if (_iHighestIOPosition > 0)
            {
                this.obcIOPositions[_iHighestIOPosition - 1].xIsLastItem = false;
            }
            _iHighestIOPosition += 1;
            this.obcIOPositions.Add(new clIOPosition(projectId, _iHighestIOPosition, 0, 0, getAvailableMaterialTypes(), getAvailableDeviceTypes()));
        
            this.obcIOPositions[_iHighestIOPosition - 1].xIsLastItem = true;
        }
        public void deleteIOPosition(int iSelectedItem)
        {
            // Delete selected IO position
            this.obcIOPositions.RemoveAt(iSelectedItem-1);

            // re-position
            for (int i = 0; i < this.obcIOPositions.Count(); i++)
            {
                this.obcIOPositions[i].materialIOPosition   = i+1;
            }            
        }
        public void updateProjectDetails(int iIdNumber, bool xIsNew)
        {
            Project project = getProject(iIdNumber);
            getProjectOptions(iIdNumber);

            if (project != null)
            {
                if (!xIsNew)                                             // Do not copy name and comment to current class when a class is copied. 
                {
                    projectId           = project.projectId;
                    sProjectName.sInput = project.projectName;
                    sComment.sInput     = project.comment;
                }

                active                  = project.active;
                sMachineId.sInput       = project.machineId;
                sPlantId.sInput         = project.plantId;
                sPlantInfo.sInput       = project.plantInfo;
                sCustomerId.sInput      = project.customerId;
                sCustomerInfo.sInput    = project.customerInfo;
                productMode             = project.productMode;
                counterReset            = project.counterReset;
                prodCodesTable          = project.prodCodesTable;
                imagePath               = projectImage.checkImagePath(project.imagePath);

                string sSewcoPath       = Directory.GetCurrentDirectory();

                reloadFilteredComboboxes();

                clProductionModeOptions.iSelectedValue  = ((int)productMode);
                clCounterResetOptions.iSelectedValue    = ((int)counterReset);
                clProductionCodesOptions.iSelectedValue = 0;

            } else
            {
                emptyProject();
            }
        }
        public void emptyProject()
        {
            sProjectName.sInput     = "";
            sComment.sInput         = "";
            active                  = true;
            sMachineId.sInput       = "";
            sPlantId.sInput         = "";
            sPlantInfo.sInput       = "";
            sCustomerId.sInput      = "";
            sCustomerInfo.sInput    = "";
            productMode             = 0;
            counterReset            = 0;
            prodCodesTable          = "";
            imagePath               = clImage.sDefaultImage;
            clOption1Scanback.xActive = false;
            clOption2VWcode.xActive   = false;
            clOption3Bobbin.xActive   = false;

            // Set comboboxes to default value
            clProductionModeOptions.iSelectedValue  = 0;
            clCounterResetOptions.iSelectedValue    = 0;
            clProductionCodesOptions.iSelectedValue = 0;

            clOption1Scanback.iSelectedValue        = 0;
            clOption3Bobbin.iSelectedValueItem1         = 0;
            clOption3Bobbin.iSelectedValueItem2         = 0;

            foreach (var q in this.obcProducts.ToList())
            {
                this.obcProducts.Remove(q);
            }

            foreach (var v in this.obcIOPositions.ToList())
            {
                this.obcIOPositions.Remove(v);
            }
            _iHighestIOPosition = this.obcIOPositions.Count();            // Set new highest position, which should be zero. 
        }
        public void reloadFilteredComboboxes()
        {
            clOption1Scanback.addSelectedValueToList(0, clOption1Scanback.iSelectedValue);
            clOption3Bobbin.addSelectedValueToList(1, clOption3Bobbin.iSelectedValueItem1);
            clOption3Bobbin.addSelectedValueToList(2, clOption3Bobbin.iSelectedValueItem2);

            //clOption1Scanback.cbbFilter(0, getAvailableDeviceTypes());
            //clOption3Bobbin.cbbFilter(1, getAvailableDeviceTypes());
            //clOption3Bobbin.cbbFilter(2, getAvailableDeviceTypes());



            //clOption1Scanback.obcItems  = filter(0, _obcScanback);
            //clOption3Bobbin.obcItems1   = filter(1, _obcBobbin1);
            //clOption3Bobbin.obcItems2   = filter(2, _obcBobbin2);

            clOption1Scanback.obcItems = getAvailableDeviceTypes(); //clOption1Scanback.getObcFilteredDevices();
            clOption3Bobbin.obcItems1 = getAvailableDeviceTypes(); // clOption3Bobbin.getObcFilteredDevices();
            clOption3Bobbin.obcItems2 = getAvailableDeviceTypes(); // clOption3Bobbin.getObcFilteredDevices();
            
            

        }

        public void propertyValidation(bool xEnable)
        {
            sProjectName.xValidateActive    = xEnable;
            sComment.xValidateActive        = xEnable;
            sMachineId.xValidateActive      = xEnable;
            sPlantId.xValidateActive        = xEnable;
            sPlantInfo.xValidateActive      = xEnable;
            sCustomerId.xValidateActive     = xEnable;
            sCustomerInfo.xValidateActive     = xEnable;
        }
        public bool validationStatus()
        {
            return (sProjectName.xHasError  || 
                    sComment.xHasError      || 
                    sMachineId.xHasError    || 
                    sPlantInfo.xHasError    || 
                    sPlantId.xHasError      || 
                    sCustomerId.xHasError   || 
                    sCustomerInfo.xHasError);
        }

        #endregion
    } // End class ProjectsBusinessObject
}

