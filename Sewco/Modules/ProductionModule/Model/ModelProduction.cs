using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sewco.Resources.Helper_classes;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Globalization;
using System.Windows.Data;
using System.Xml.Linq;
using Sewco.Modules.ControlPanel;
using FP_CONNECTLib;
using System.Windows.Forms;

namespace Sewco.Modules.ProductionModule
{
    public static class staticProductionSettings
    {
        public static int iProjectId;
        public static int iProductId;
        public static int iProductionMode;              // Defines the production mode. Batch, unlimited or single 
        public static bool xBatchMode;                  // 
        public static int iBatchDone;                   // Counts the number of produced products within a batch
        public static int iBatchSize;                   // Defines the batch size. When iBatchDone = iBatchSize, the batch is finished 
        public static DateTime timeProductionStart;     // Is actually not used. Only check in program, but not visible. Can be removed?? TODO
        public static bool xUseLotNumber;               // This number will be added to label information.. TODO, checken of dit zo is
        public static int iCounterValue;                // Is used to count produced products. Will be resetted according iCounterResetMode.
        public static int iCounterResetMode;            // defines when iCounterValue must be resetted

        /*public static clTextBoxValidation batchDone { get; set; }
        public static clTextBoxValidation batchSize { get; set; }*/


        public static int iPCProductionStep;
        // enzovoorts met batchnummer etc

        public static int iUserId
        {
            get
            {
                return ViewControlPanel.ViewModelControlPanel.header.iUserId;
            }
        }
        public static int iUserLevel
        {
            get
            {
                return ViewControlPanel.ViewModelControlPanel.header.iUserLevel;
            }
        }

    }

    /// <summary>
    /// ProductionSettings contains the global production settings, like batch information, production mode, project id etc. 
    /// </summary>
    public class ProductionSettings : ObservableObject
    {

        public ProductionSettings() { }

        public int iProjectId
        {
            get
            {
                return staticProductionSettings.iProjectId;
            }
            set
            {
                staticProductionSettings.iProjectId = value;
                OnPropertyChanged("iProjectId");
            }
        }
        public int iProductId
        {
            get
            {
                return staticProductionSettings.iProductId;
            }
            set
            {
                staticProductionSettings.iProductId = value;
                OnPropertyChanged("iProductId");
            }
        }
        public int iUserId
        {
            get
            {
                return staticProductionSettings.iUserId;
            }
            /*set
            {
                staticProductionSettings.iUserId = value;
                OnPropertyChanged("iUserId");
            }*/
        }
        public int iUserLevel
        {
            get
            {
                return staticProductionSettings.iUserLevel;
            }
            /*set
            {
                staticProductionSettings.iUserLevel = value;
                OnPropertyChanged("iUserLevel");
            }*/
        }
        public int iProductionMode
        {
            get
            {
                return staticProductionSettings.iProductionMode;
            }
            set
            {
                staticProductionSettings.iProductionMode = value;
                OnPropertyChanged("iProductionMode");
            }
        }
        public bool xBatchMode
        {
            get
            {
                return staticProductionSettings.xBatchMode;
            }
            set
            {
                staticProductionSettings.xBatchMode = value;
                OnPropertyChanged("xBatchMode");
            }
        }
        public int iBatchDone
        {
            get
            {
                return staticProductionSettings.iBatchDone;
            }
            set
            {
                staticProductionSettings.iBatchDone = value;
                OnPropertyChanged("iBatchDone");
            }
        }
        public int iBatchSize
        {
            get
            {
                return staticProductionSettings.iBatchSize;
            }
            set
            {
                staticProductionSettings.iBatchSize = value;
                OnPropertyChanged("iBatchSize");
            }
        }
        public DateTime timeProductionStart
        {
            get
            {
                return staticProductionSettings.timeProductionStart;
            }
            set
            {
                staticProductionSettings.timeProductionStart = value;
                OnPropertyChanged("timeProductionStart");
            }
        }
        public bool xUseLotNumber
        {
            get
            {
                return staticProductionSettings.xUseLotNumber;
            }
            set
            {
                staticProductionSettings.xUseLotNumber = value;
                OnPropertyChanged("xUseLotNumber");
            }
        }
        public int iCounterValue
        {
            get
            {
                return staticProductionSettings.iCounterValue;
            }
            set
            {
                staticProductionSettings.iCounterValue = value;
                OnPropertyChanged("iCounterValue");
            }
        }
        public int iCounterResetMode
        {
            get
            {
                return staticProductionSettings.iCounterResetMode;
            }
            set
            {
                staticProductionSettings.iCounterResetMode = value;
                OnPropertyChanged("iCounterResetMode");
            }
        }

    }

    public class ProjectInfo : ObservableObject
    {
        private string _sMachineId;
        private string _sPlantId;
        private string _sPlantInfo;
        private string _sCustomerId;
        private string _sCustomerInfo;
        private string _sProductionMode;
        private string _sCounterReset;
        private string _sProductionCode;
        private string _sProjectImagePath;
        private bool _xUseLotNumber;
        private int _iProjectId;
        private int _iProductionMode;
        private bool _xBatchMode;
        private int _iCounterResetMode;
        private int _iBatchDone;
        private int _iBatchSize;

        public string sMachineId
        {
            get
            {
                return _sMachineId;
            }
            set
            {
                _sMachineId = value;
                OnPropertyChanged("sMachineId");
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
                OnPropertyChanged("sPlantId");
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
                OnPropertyChanged("sPlantInfo");
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
                OnPropertyChanged("sCustomerId");
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
                OnPropertyChanged("sCustomerInfo");
            }
        }
        public string sProductionMode
        {
            get
            {
                return _sProductionMode;
            }
            set
            {
                _sProductionMode = value;
                OnPropertyChanged("sProductionMode");
            }
        }
        public string sCounterReset
        {
            get
            {
                return _sCounterReset;
            }
            set
            {
                _sCounterReset = value;
                OnPropertyChanged("sCounterReset");
            }
        }
        public string sProductionCode
        {
            get
            {
                return _sProductionCode;
            }
            set
            {
                _sProductionCode = value;
                OnPropertyChanged("sProductionCode");
            }
        }
        public string sProjectImagePath
        {
            get
            {
                return _sProjectImagePath;
            }
            set
            {
                _sProjectImagePath = value;
                OnPropertyChanged("sProjectImagePath");
            }
        }
        public bool xUseLotNumber
        {
            get
            {
                return _xUseLotNumber;
            }
            set
            {
                _xUseLotNumber = value;
                OnPropertyChanged("xUseLotNumber");
            }
        }
        public int iProjectId
        {
            get
            {
                return _iProjectId;
            }
            set
            {
                _iProjectId = value;
                OnPropertyChanged("iProjectId");
            }
        }
        public int iProductionMode
        {
            get
            {
                return _iProductionMode;
            }
            set
            {
                _iProductionMode = value;

                xBatchMode = (value == clConstants.PRODUCTION_BATCH_MODE);     // Determine if batch mode
                OnPropertyChanged("iProductionMode");
            }
        }
        public bool xBatchMode
        {
            get
            {
                return _xBatchMode;
            }
            set
            {
                _xBatchMode = value;
                OnPropertyChanged("xBatchMode");
            }
        }
        public int iCounterResetMode
        {
            get
            {
                return _iCounterResetMode;
            }
            set
            {
                _iCounterResetMode = value;
                OnPropertyChanged("iCounterResetMode");
            }
        }
        public int iBatchDone
        {
            get
            {
                return _iBatchDone;
            }
            set
            {
                _iBatchDone = value;
                OnPropertyChanged("iBatchDone");
            }
        }
        public int iBatchSize
        {
            get
            {
                return _iBatchSize;
            }
            set
            {
                _iBatchSize = value;
                OnPropertyChanged("iBatchSize");
            }
        }
    }

    public class ProductInfo : ObservableObject
    {
        private string _sName = "";
        private string _sCover = "";
        private string _sType;
        private string _sProfile;
        private string _sProductImagePath;
        private int _iProductId;
        public string sName
        {
            get
            {
                return _sName;
            }
            set
            {
                _sName = value;
                OnPropertyChanged("sName");
            }
        }
        public string sCover
        {
            get
            {
                return _sCover;
            }
            set
            {
                _sCover = value;
                OnPropertyChanged("sCover");
            }
        }
        public string sType
        {
            get
            {
                return _sType;
            }
            set
            {
                _sType = value;
                OnPropertyChanged("sType");
            }
        }
        public string sProfile
        {
            get
            {
                return _sProfile;
            }
            set
            {
                _sProfile = value;
                OnPropertyChanged("sProfile");
            }
        }
        public string sProductImagePath
        {
            get
            {
                return _sProductImagePath;
            }
            set
            {
                _sProductImagePath = value;
                OnPropertyChanged("sProductImagePath");
            }
        }
        public int iProductId
        {
            get
            {
                return _iProductId;
            }
            set
            {
                _iProductId = value;
                OnPropertyChanged("iProductId");
            }
        }
    }


    public class clProductionSeams : ObservableObject
    {
        public ObservableCollection<clSeam> obcItems { get; set; } = new ObservableCollection<clSeam>();

        private int _iStartPos;
        public int iStartPos
        {
            get
            {
                return _iStartPos;
            }
            set
            {
                _iStartPos = value;
                OnPropertyChanged("iStartPos");
            }
        }

        private int _iSelectedValue;
        public int iSelectedValue
        {
            get
            {
                return _iSelectedValue;
            }
            set
            {
                _iSelectedValue = value;

                OnPropertyChanged("iSelectedValue");
            }

        }
    }
    public class clSeam : ObservableObject
    {
        public clSeam(string _argiSeamNumber, string _argiMinStitches, string _argiMaxStitches, string _argiMinTension, string _argiMaxTension, string _argiActStitches, string _argdStitchLength, bool _argxActive, int _argiSelectedValuePath)
        {

            this.iSeamNumber = _argiSeamNumber;
            this.iMinStitches = _argiMinStitches;
            this.iMaxStitches = _argiMaxStitches;
            this.iMinTension = _argiMinTension;
            this.iMaxTension = _argiMaxTension;
            this.iActStitches = _argiActStitches;
            this.dStitchLength = _argdStitchLength;
            this.xActive = _argxActive;
            this.iSelectedValuePath = _argiSelectedValuePath;
        }

        private int _iSelectedValuePath;
        private string _iSeamNumber;
        private string _iMinStitches;
        private string _iMaxStitches;
        private string _iMinTension;
        private string _iMaxTension;
        private string _iActStitches;
        private string _dStitchLength;
        private bool _xActive;
        private string _sStyleName = "cust1";
        private int _iListboxItemHeight = 60;

        public string sStyleName
        {
            get
            {
                return _sStyleName;

            }
            set
            {
                _sStyleName = value;
                OnPropertyChanged("sStyleName");
            }
        }

        public int iListboxItemHeight
        {
            get
            {
                return _iListboxItemHeight;

            }
            set
            {
                _iListboxItemHeight = value;
                OnPropertyChanged("iListboxItemHeight");
            }
        }

        private Style _style;
        public Style style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
                OnPropertyChanged("style");
            }
        }

        public int iSelectedValuePath
        {
            get
            {
                return _iSelectedValuePath;
            }
            set
            {
                _iSelectedValuePath = value;
                OnPropertyChanged("iSelectedValuePath");
            }
        }

        public string iSeamNumber
        {
            get
            {
                return _iSeamNumber;
            }
            set
            {
                _iSeamNumber = value;
                OnPropertyChanged("iSeamNumber");
            }
        }

        public string iMinStitches
        {
            get
            {
                return _iMinStitches;
            }
            set
            {
                _iMinStitches = value;
                OnPropertyChanged("iMinStitches");
            }
        }

        public string iMaxStitches
        {
            get
            {
                return _iMaxStitches;
            }
            set
            {
                _iMaxStitches = value;
                OnPropertyChanged("iMaxStitches");
            }
        }

        public string iMinTension
        {
            get
            {
                return _iMinTension;
            }
            set
            {
                _iMinTension = value;
                OnPropertyChanged("iMinTension");
            }
        }

        public string iMaxTension
        {
            get
            {
                return _iMaxTension;
            }
            set
            {
                _iMaxTension = value;
                OnPropertyChanged("iMaxTension");
            }
        }

        public string iActStitches
        {
            get
            {
                return _iActStitches;
            }
            set
            {
                _iActStitches = value;
                OnPropertyChanged("iActStitches");
            }
        }

        public string dStitchLength
        {
            get
            {
                return _dStitchLength;
            }
            set
            {
                _dStitchLength = value;
                OnPropertyChanged("dStitchLength");
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
    }
    public class clStatus : ObservableObject
    {
        public clStatus(int _argiID, bool _argxActive, string arg_sImagePath = "")
        {
            xActive = _argxActive;
            iID = _argiID;
            sImagePath = arg_sImagePath;
        }

        private bool _xActive;
        private int _iID;
        private string _sImagePath;

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
        public int iID
        {
            get
            {
                return _iID;
            }
            set
            {
                _iID = value;
                OnPropertyChanged("iID");
            }
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
    }
    public class clError : ObservableObject
    {
        public clError(int _argiID, bool _argxActive, string _argsErrorName, string _argsErrorDescription)
        {
            iID = _argiID;
            xActive = _argxActive;
            sErrorName = _argsErrorName;
            sErrorDescription = _argsErrorDescription;
        }

        private bool _xActive;
        private int _iID;
        private string _sErrorName;
        private string _sErrorDescription;

        public int iID
        {
            get
            {
                return _iID;
            }
            set
            {
                _iID = value;
                OnPropertyChanged("iID");
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
        public string sErrorName
        {
            get
            {
                return _sErrorName;

            }
            set
            {
                _sErrorName = value;
                OnPropertyChanged("sErrorName");
            }
        }
        public string sErrorDescription
        {
            get
            {
                return _sErrorDescription;

            }
            set
            {
                _sErrorDescription = value;
                OnPropertyChanged("sErrorDescription");
            }
        }
    }

    /// <summary>
    /// Productlive contains the live data of the sewing process
    /// </summary>
    public class Productlive : ObservableObject
    {
        public int iActStitches;        // Actual number of stitches 
        public int iActTension;         // Actual tension during sewing process

        public uint iFatalErrors;        // Contains fatal errors of sewing process
        public uint iNonFatalErrors;     // Contains non-fatal errors of sewing process.

        public int iStatus;             // Contains status of sewing process.

        public int iActiveSeam;         // Contains the actual active seam number
    }
    /// <summary>
    /// ProductSettings contains the neccesary data to produce a product 
    /// </summary>
    public class ProductSettings : ObservableObject
    {
        private string _sCode;                 // Product code
        private string _sName;                 // Product name
        private string _sProfileName;          // Connected profile name product
        private string _sProfileComment;       // Connected profile comment product

        public string sCode
        {
            get
            {
                return _sCode;
            }
            set
            {
                _sCode = value;
                OnPropertyChanged("sCode");
            }
        }
        public string sName
        {
            get
            {
                return _sName;
            }
            set
            {
                _sName = value;
                OnPropertyChanged("sName");
            }
        }
        public string sProfileName
        {
            get
            {
                return _sProfileName;
            }
            set
            {
                _sProfileName = value;
                OnPropertyChanged("sProfileName");
            }
        }
        public string sProfileComment
        {
            get
            {
                return _sProfileComment;
            }
            set
            {
                _sProfileComment = value;
                OnPropertyChanged("sProfileComment");
            }
        }

        public ObservableCollection<ProdSeamTemplate> obcSeams { get; set; } = new ObservableCollection<ProdSeamTemplate>();       // Seam in this product
        public ObservableCollection<ProdIOPosition> obcIOPositions { get; set; } = new ObservableCollection<ProdIOPosition>();         // IO positions, with scanners + materials 

        // Eventueel uitbreiden met Covers, types
        // Ook moet hier nog de printer settings bijkomen. Welke com-poort etc.
    }
    public class ProdSeamTemplate : ObservableObject
    {
        // TODO, dit veranderen naar normale properties. Hoeven geen OnPropertyChanged dingen te zijn, ivm kopieren in de ViewModel
        private string _sName;
        private string _sComment;
        private bool _xPrintActive;
        private string _sLabelFile;
        private int _iLabelPrintPos;
        private int _iMinTens;
        private int _iMaxTens;
        private int _iBlindTens;
        private int _iTensFilter;
        private int _iStitchCount;
        private int _iMinStitch;
        private int _iMaxStitch;
        private int _iBlindArea;
        private int _iSeamLength;
        private int _iStitchLength;
        private int _iStartBtFw;
        private int _iStartBtBw;
        private int _iEndBtFw;
        private int _iEndBtBw;
        private int _iSpeed;
        private int _iPar;
        private int _iGeneral;
        private int _iFunctions;
        private int _iMonitoring;
        private int _iActStitch;

        public int iActStitch
        {
            get
            {
                return _iActStitch;
            }
            set
            {
                _iActStitch = value;
                OnPropertyChanged("iActStitch");
            }
        }

        public string sName
        {
            get
            {
                return _sName;
            }
            set
            {
                _sName = value;
                OnPropertyChanged("sName");
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
                OnPropertyChanged("sComment");
            }
        }
        public bool xPrintActive
        {
            get
            {
                return _xPrintActive;
            }
            set
            {
                _xPrintActive = value;
                OnPropertyChanged("xPrintActive");
            }
        }
        public string sLabelFile
        {
            get
            {
                return _sLabelFile;
            }
            set
            {
                _sLabelFile = value;
                OnPropertyChanged("sLabelFile");
            }
        }
        public int iLabelPrintPos
        {
            get
            {
                return _iLabelPrintPos;
            }
            set
            {
                _iLabelPrintPos = value;
                OnPropertyChanged("iLabelPrintPos");
            }
        }
        public int iMinTens
        {
            get
            {
                return _iMinTens;
            }
            set
            {
                _iMinTens = value;
                OnPropertyChanged("iMinTens");
            }
        }
        public int iMaxTens
        {
            get
            {
                return _iMaxTens;
            }
            set
            {
                _iMaxTens = value;
                OnPropertyChanged("iMaxTens");
            }
        }
        public int iBlindTens
        {
            get
            {
                return _iBlindTens;
            }
            set
            {
                _iBlindTens = value;
                OnPropertyChanged("iBlindTens");
            }
        }
        public int iTensFilter
        {
            get
            {
                return _iTensFilter;
            }
            set
            {
                _iTensFilter = value;
                OnPropertyChanged("iTensFilter");
            }
        }
        public int iStitchCount
        {
            get
            {
                return _iStitchCount;
            }
            set
            {
                _iStitchCount = value;
                OnPropertyChanged("iStitchCount");
            }
        }
        public int iMinStitch
        {
            get
            {
                return _iMinStitch;
            }
            set
            {
                _iMinStitch = value;
                OnPropertyChanged("iMinStitch");
            }
        }
        public int iMaxStitch
        {
            get
            {
                return _iMaxStitch;
            }
            set
            {
                _iMaxStitch = value;
                OnPropertyChanged("iMaxStitch");
            }
        }
        public int iBlindArea
        {
            get
            {
                return _iBlindArea;
            }
            set
            {
                _iBlindArea = value;
                OnPropertyChanged("iBlindArea");
            }
        }
        public int iSeamLength
        {
            get
            {
                return _iSeamLength;
            }
            set
            {
                _iSeamLength = value;
                OnPropertyChanged("iSeamLength");
            }
        }
        public int iStitchLength
        {
            get
            {
                return _iStitchLength;
            }
            set
            {
                _iStitchLength = value;
                OnPropertyChanged("iStitchLength");
            }
        }
        public int iStartBtFw
        {
            get
            {
                return _iStartBtFw;
            }
            set
            {
                _iStartBtFw = value;
                OnPropertyChanged("iStartBtFw");
            }
        }
        public int iStartBtBw
        {
            get
            {
                return _iStartBtBw;
            }
            set
            {
                _iStartBtBw = value;
                OnPropertyChanged("iStartBtBw");
            }
        }
        public int iEndBtFw
        {
            get
            {
                return _iEndBtFw;
            }
            set
            {
                _iEndBtFw = value;
                OnPropertyChanged("iEndBtFw");
            }
        }
        public int iEndBtBw
        {
            get
            {
                return _iEndBtBw;
            }
            set
            {
                _iEndBtBw = value;
                OnPropertyChanged("iEndBtBw");
            }
        }
        public int iSpeed
        {
            get
            {
                return _iSpeed;
            }
            set
            {
                _iSpeed = value;
                OnPropertyChanged("iSpeed");
            }
        }
        public int iPar
        {
            get
            {
                return _iPar;
            }
            set
            {
                _iPar = value;
                OnPropertyChanged("iPar");
            }
        }
        public int iGeneral
        {
            get
            {
                return _iGeneral;
            }
            set
            {
                _iGeneral = value;
                OnPropertyChanged("iGeneral");
            }
        }
        public int iFunctions
        {
            get
            {
                return _iFunctions;
            }
            set
            {
                _iFunctions = value;
                OnPropertyChanged("iFunctions");
            }
        }
        public int iMonitoring
        {
            get
            {
                return _iMonitoring;
            }
            set
            {
                _iMonitoring = value;
                OnPropertyChanged("iMonitoring");
            }
        }
    }

    public class ProdIOPosition
    {
        public string sMaterialTypeName;    // Ondergaren or bovengaren etc
        public string sDeviceName;          // Device name. (Scanback scanner / bobbin scanner enz.
        public string sDeviceAddress;       // COM4, COM1, 192.168.5.2 enz
        public string sInterfaceType;       // Serial / ethernet
        public bool xMaterialsRequired      // Product contains materials, so only specific materials can be used. 
        {
            get
            {
                return (obcAllowedMaterials.Count() > 0);
            }
        }

        public ObservableCollection<ProdMaterials> obcAllowedMaterials = new ObservableCollection<ProdMaterials>();   // Allowed material at this IO position
    }

    public class ProdMaterials
    {
        public int iMaterialId;
        public string sMaterialName;
        public string sMaterialCode;
        public int iMinLength;
        public string sCodeRange;
    }

    public class PanasonicCommunication
    {
        FP_CONNECT panasonicComm = new FP_CONNECT();


        void Initialize()
        {
            panasonicComm.SetLanguage(1);   // 1 = English
        }

        void OpenPort()
        {
            panasonicComm.PortOpen(0, 0);
        }
        void ClosePort()
        {
            panasonicComm.PortClose();
        }
        
        void writeData()
        {

        }

    }


    public class CommClass
    {
        //public const int iTxSettingsCount           = 9;
        public const int iTxMaxSeams                    = 16;
        public const int iTotalSettingsSize             = 1143;
        public const int iGeneralArrayAddress           = 0;
        public const int iMonitoringArrayAddress        = 15;
        public const int iStandardFunctionsArrayAddress = 32;
        public const int iHWInputArrayAddress           = 64;
        public const int iHWOutputArrayAddress          = 96;


        public int[] TxDataSettings = new int[iTotalSettingsSize];
        public void addSettings(Object _argobSettings, int iSettingType, int iSeamIndex)
        {
            switch (iSettingType)
            {
                case iGeneralArrayAddress:
                    GeneralSettings newGeneralSetting                   = (GeneralSettings)_argobSettings;
                    TxDataSettings[iSettingType + iSeamIndex]           = newGeneralSetting.iGeneral;
                    break;

                case iMonitoringArrayAddress:
                    MonitoringSettings newMonitoringSetting             = (MonitoringSettings)_argobSettings;
                    TxDataSettings[iSettingType + iSeamIndex]           = newMonitoringSetting.iMonitoring;
                    break;

                case iStandardFunctionsArrayAddress:
                    StandardSewingFunctions newStandardFunctionSetting      = (StandardSewingFunctions)_argobSettings;
                    TxDataSettings[iSettingType + iSeamIndex]               = newStandardFunctionSetting.iStandardFunctionsW1;
                    TxDataSettings[iSettingType + iSeamIndex + iTxMaxSeams] = newStandardFunctionSetting.iStandardFunctionsW2;
                    break;

                case iHWInputArrayAddress:
                    HWInputs newHWInputs                                    = (HWInputs)_argobSettings;
                    TxDataSettings[iSettingType + iSeamIndex]               = newHWInputs.iInputsW1;
                    TxDataSettings[iSettingType + iSeamIndex + iTxMaxSeams] = newHWInputs.iInputsW2;
                    break;

                case iHWOutputArrayAddress:
                    HWOutputs newHWOutputs                                      = (HWOutputs)_argobSettings;
                    TxDataSettings[iSettingType + iSeamIndex]                   = newHWOutputs.iOutputsW1;
                    TxDataSettings[iSettingType + iSeamIndex + iTxMaxSeams]     = newHWOutputs.iOutputsW2;
                    TxDataSettings[iSettingType + iSeamIndex + iTxMaxSeams*2]   = newHWOutputs.iOutputsW3;
                    break;
            }
        }

        public PanasonicCommunication plcComm = new PanasonicCommunication();


        public void test()
        {
            // bla bla query naar instellingen
            GeneralSettings generalSettings = new GeneralSettings();

            generalSettings.xActive = true;
            addSettings(generalSettings, 0, CommClass.iGeneralArrayAddress);
        }

        public class GeneralSettings
        {
            public GeneralSettings()
            {
                iGeneral = 0;
            }
            public bool xActive
            {
                get
                {
                    return (Def.getBit(iGeneral, 0));
                }
                set
                {
                    iGeneral = Def.setBit(iGeneral, 0, value);
                }
            }
            public int iTransit
            {
                get
                {
                    return ((iGeneral >> 1) & 3);
                }
                set
                {
                    iGeneral = Def.setBit(iGeneral, 1, value);
                }
            }
            public bool xRecording
            {
                get
                {
                    return (Def.getBit(iGeneral, 3));
                }
                set
                {
                    iGeneral = Def.setBit(iGeneral, 3, value);
                }
            }
            public int iScanback
            {
                get
                {
                    return Def.getBit(iGeneral, 4, 3);
                }
                set
                {
                    iGeneral = Def.setBit(iGeneral, 4, value);
                }
            }
            public bool xPrintLabel
            {
                get
                {
                    return (Def.getBit(iGeneral, 6));
                }
                set
                {
                    iGeneral = Def.setBit(iGeneral, 6, value);
                }
            }
            public bool xReserve1;
            public int iErrorHandling
            {
                get
                {
                    return Def.getBit(iGeneral, 8, 3);
                }
                set
                {
                    iGeneral = Def.setBit(iGeneral, 8, value, 3);
                }
            }
            public int iReserve1;

            public int iGeneral;

            public const int iPLCStartAddress = 1000;
        }
        public class MonitoringSettings
        {
            public MonitoringSettings()
            {
                iMonitoring = 0;
            }
            public int iBobbinThreadMonitor
            {
                get
                {
                    return Def.getBit(iMonitoring, 0, 3);
                }
                set
                {
                    iMonitoring = Def.getBit(iMonitoring, 0, 3);
                }
            }
            public int iCount
            {
                get
                {
                    return Def.getBit(iMonitoring, 2, 3);
                }
                set
                {
                    iMonitoring = Def.getBit(iMonitoring, 2, 3);
                }
            }
            public int iTopThreadMonitor
            {
                get
                {
                    return Def.getBit(iMonitoring, 4, 3);
                }
                set
                {
                    iMonitoring = Def.getBit(iMonitoring, 4, 3);
                }
            }
            public int iGuide
            {
                get
                {
                    return Def.getBit(iMonitoring, 6, 3);
                }
                set
                {
                    iMonitoring = Def.getBit(iMonitoring, 6, 3);
                }
            }
            public int iCoverPlateMonitor
            {
                get
                {
                    return Def.getBit(iMonitoring, 8, 3);
                }
                set
                {
                    iMonitoring = Def.getBit(iMonitoring, 8, 3);
                }
            }
            public int iBackTack
            {
                get
                {
                    return Def.getBit(iMonitoring, 10, 3);
                }
                set
                {
                    iMonitoring = Def.getBit(iMonitoring, 10, 3);
                }
            }

            public int iStitchLengthMonitor
            {
                get
                {
                    return Def.getBit(iMonitoring, 12, 3);
                }
                set
                {
                    iMonitoring = Def.getBit(iMonitoring, 12, 3);
                }
            }
            public int iHandWheelMonitor
            {
                get
                {
                    return Def.getBit(iMonitoring, 14, 3);
                }
                set
                {
                    iMonitoring = Def.getBit(iMonitoring, 14, 3);
                }
            }

            public int iMonitoring;
        }
        public class StandardSewingFunctions
        {
            public StandardSewingFunctions()
            {
                iStandardFunctionsW1 = 0;
                iStandardFunctionsW2 = 0;
            }
            public int iPedal
            {
                get
                {
                    return Def.getBit(iStandardFunctionsW1, 0, 7);
                }
                set
                {
                    iStandardFunctionsW1 = Def.getBit(iStandardFunctionsW1, 0, 7);
                }
            }
            public int iStartBackTack
            {
                get
                {
                    return Def.getBit(iStandardFunctionsW1, 6, 3);
                }
                set
                {
                    iStandardFunctionsW1 = Def.getBit(iStandardFunctionsW1, 6, 3);
                }
            }
            public int iEndBackTack
            {
                get
                {
                    return Def.getBit(iStandardFunctionsW1, 8, 3);
                }
                set
                {
                    iStandardFunctionsW1 = Def.getBit(iStandardFunctionsW1, 8, 3);
                }
            }
            public int iInterBackTack       // Intermediate backtack
            {
                get
                {
                    return Def.getBit(iStandardFunctionsW1, 10, 3);
                }
                set
                {
                    iStandardFunctionsW1 = Def.getBit(iStandardFunctionsW1, 10, 3);
                }
            }
            public int i2ndFootlift
            {
                get
                {
                    return Def.getBit(iStandardFunctionsW1, 12, 3);
                }
                set
                {
                    iStandardFunctionsW1 = Def.getBit(iStandardFunctionsW1, 12, 3);
                }
            }
            public int i2ndStitchLength
            {
                get
                {
                    return Def.getBit(iStandardFunctionsW1, 14, 3);
                }
                set
                {
                    iStandardFunctionsW1 = Def.getBit(iStandardFunctionsW1, 14, 3);
                }
            }

            public int iStandardFunctionsW1;

            public int i2ndThreadTension
            {
                get
                {
                    return Def.getBit(iStandardFunctionsW2, 0, 3);
                }
                set
                {
                    iStandardFunctionsW2 = Def.getBit(iStandardFunctionsW2, 0, 3);
                }
            }
            public int i2ndFootPressure
            {
                get
                {
                    return Def.getBit(iStandardFunctionsW2, 2, 3);
                }
                set
                {
                    iStandardFunctionsW2 = Def.getBit(iStandardFunctionsW2, 2, 3);
                }
            }
            public int iNeedleCooling
            {
                get
                {
                    return Def.getBit(iStandardFunctionsW2, 4, 3);
                }
                set
                {
                    iStandardFunctionsW2 = Def.getBit(iStandardFunctionsW2, 4, 3);
                }
            }
            public int iStandardFunctionsW2;
        }
        public class HWInputs
        {
            public HWInputs()
            {
                iInputsW1 = 0;
                iInputsW2 = 0;
            }
            public int iInput1
            {
                get
                {
                    return Def.getBit(iInputsW1, 0, 3);
                }
                set
                {
                    iInputsW1 = Def.getBit(iInputsW1, 0, 3);
                }
            }
            public int iInput2
            {
                get
                {
                    return Def.getBit(iInputsW1, 2, 3);
                }
                set
                {
                    iInputsW1 = Def.getBit(iInputsW1, 2, 3);
                }
            }
            public int iInput3
            {
                get
                {
                    return Def.getBit(iInputsW1, 4, 3);
                }
                set
                {
                    iInputsW1 = Def.getBit(iInputsW1, 4, 3);
                }
            }
            public int iInput4
            {
                get
                {
                    return Def.getBit(iInputsW1, 6, 3);
                }
                set
                {
                    iInputsW1 = Def.getBit(iInputsW1, 6, 3);
                }
            }
            public int iInput5
            {
                get
                {
                    return Def.getBit(iInputsW1, 8, 3);
                }
                set
                {
                    iInputsW1 = Def.getBit(iInputsW1, 8, 3);
                }
            }
            public int iInput6
            {
                get
                {
                    return Def.getBit(iInputsW1, 10, 3);
                }
                set
                {
                    iInputsW1 = Def.getBit(iInputsW1, 10, 3);
                }
            }
            public int iInput7
            {
                get
                {
                    return Def.getBit(iInputsW1, 12, 3);
                }
                set
                {
                    iInputsW1 = Def.getBit(iInputsW1, 12, 3);
                }
            }
            public int iInput8
            {
                get
                {
                    return Def.getBit(iInputsW1, 14, 3);
                }
                set
                {
                    iInputsW1 = Def.getBit(iInputsW1, 14, 3);
                }
            }
            public int iInputsW1;

            public int iInput9
            {
                get
                {
                    return Def.getBit(iInputsW2, 0, 3);
                }
                set
                {
                    iInputsW2 = Def.getBit(iInputsW2, 0, 3);
                }
            }
            public int iInput10
            {
                get
                {
                    return Def.getBit(iInputsW2, 2, 3);
                }
                set
                {
                    iInputsW2 = Def.getBit(iInputsW2, 2, 3);
                }
            }
            public int iAnalogInput1
            {
                get
                {
                    return Def.getBit(iInputsW2, 4, 3);
                }
                set
                {
                    iInputsW2 = Def.getBit(iInputsW2, 4, 3);
                }
            }
            public int iAnalogInput2
            {
                get
                {
                    return Def.getBit(iInputsW2, 6, 3);
                }
                set
                {
                    iInputsW2 = Def.getBit(iInputsW2, 6, 3);
                }
            }
            public int iInputsW2;
        }
        public class HWOutputs
        {
            public HWOutputs()
            {
                iOutputsW1 = 0;
                iOutputsW2 = 0;
                iOutputsW3 = 0;
            }
            public int iOutput1
            {
                get
                {
                    return Def.getBit(iOutputsW1, 0, 3);
                }
                set
                {
                    iOutputsW1 = Def.getBit(iOutputsW1, 0, 3);
                }
            }
            public int iOutput2
            {
                get
                {
                    return Def.getBit(iOutputsW1, 2, 3);
                }
                set
                {
                    iOutputsW1 = Def.getBit(iOutputsW1, 2, 3);
                }
            }
            public int iOutput3
            {
                get
                {
                    return Def.getBit(iOutputsW1, 4, 3);
                }
                set
                {
                    iOutputsW1 = Def.getBit(iOutputsW1, 4, 3);
                }
            }
            public int iOutput4
            {
                get
                {
                    return Def.getBit(iOutputsW1, 6, 3);
                }
                set
                {
                    iOutputsW1 = Def.getBit(iOutputsW1, 6, 3);
                }
            }
            public int iOutput5
            {
                get
                {
                    return Def.getBit(iOutputsW1, 8, 3);
                }
                set
                {
                    iOutputsW1 = Def.getBit(iOutputsW1, 8, 3);
                }
            }
            public int iOutput6
            {
                get
                {
                    return Def.getBit(iOutputsW1, 10, 3);
                }
                set
                {
                    iOutputsW1 = Def.getBit(iOutputsW1, 10, 3);
                }
            }
            public int iOutput7
            {
                get
                {
                    return Def.getBit(iOutputsW1, 12, 3);
                }
                set
                {
                    iOutputsW1 = Def.getBit(iOutputsW1, 12, 3);
                }
            }
            public int iOutput8
            {
                get
                {
                    return Def.getBit(iOutputsW1, 14, 3);
                }
                set
                {
                    iOutputsW1 = Def.getBit(iOutputsW1, 14, 3);
                }
            }
            public int iOutputsW1;

            public int iOutput9
            {
                get
                {
                    return Def.getBit(iOutputsW2, 0, 3);
                }
                set
                {
                    iOutputsW2 = Def.getBit(iOutputsW2, 0, 3);
                }
            }
            public int iOutput10
            {
                get
                {
                    return Def.getBit(iOutputsW2, 2, 3);
                }
                set
                {
                    iOutputsW2 = Def.getBit(iOutputsW2, 2, 3);
                }
            }
            public int iOutput11
            {
                get
                {
                    return Def.getBit(iOutputsW2, 4, 3);
                }
                set
                {
                    iOutputsW2 = Def.getBit(iOutputsW2, 4, 3);
                }
            }
            public int iOutput12
            {
                get
                {
                    return Def.getBit(iOutputsW2, 6, 3);
                }
                set
                {
                    iOutputsW2 = Def.getBit(iOutputsW2, 6, 3);
                }
            }
            public int iOutput13
            {
                get
                {
                    return Def.getBit(iOutputsW2, 8, 3);
                }
                set
                {
                    iOutputsW2 = Def.getBit(iOutputsW2, 8, 3);
                }
            }
            public int iOutput14
            {
                get
                {
                    return Def.getBit(iOutputsW2, 10, 3);
                }
                set
                {
                    iOutputsW2 = Def.getBit(iOutputsW2, 10, 3);
                }
            }
            public int iOutput15
            {
                get
                {
                    return Def.getBit(iOutputsW2, 12, 3);
                }
                set
                {
                    iOutputsW2 = Def.getBit(iOutputsW2, 12, 3);
                }
            }
            public int iOutput16
            {
                get
                {
                    return Def.getBit(iOutputsW2, 14, 3);
                }
                set
                {
                    iOutputsW2 = Def.getBit(iOutputsW2, 14, 3);
                }
            }
            public int iOutputsW2;

            public int iOutput17
            {
                get
                {
                    return Def.getBit(iOutputsW3, 0, 3);
                }
                set
                {
                    iOutputsW3 = Def.getBit(iOutputsW3, 0, 3);
                }
            }
            public int iOutput18
            {
                get
                {
                    return Def.getBit(iOutputsW3, 2, 3);
                }
                set
                {
                    iOutputsW3 = Def.getBit(iOutputsW3, 2, 3);
                }
            }
            public int iOutput19
            {
                get
                {
                    return Def.getBit(iOutputsW3, 4, 3);
                }
                set
                {
                    iOutputsW3 = Def.getBit(iOutputsW3, 4, 3);
                }
            }
            public int iOutput20
            {
                get
                {
                    return Def.getBit(iOutputsW3, 6, 3);
                }
                set
                {
                    iOutputsW3 = Def.getBit(iOutputsW3, 6, 3);
                }
            }
            public int iOutput21
            {
                get
                {
                    return Def.getBit(iOutputsW3, 8, 3);
                }
                set
                {
                    iOutputsW3 = Def.getBit(iOutputsW3, 8, 3);
                }
            }
            public int iOutput22
            {
                get
                {
                    return Def.getBit(iOutputsW3, 10, 3);
                }
                set
                {
                    iOutputsW3 = Def.getBit(iOutputsW3, 10, 3);
                }
            }
            public int iOutput23
            {
                get
                {
                    return Def.getBit(iOutputsW3, 12, 3);
                }
                set
                {
                    iOutputsW3 = Def.getBit(iOutputsW3, 12, 3);
                }
            }
            public int iOutput24
            {
                get
                {
                    return Def.getBit(iOutputsW3, 14, 3);
                }
                set
                {
                    iOutputsW3 = Def.getBit(iOutputsW3, 14, 3);
                }
            }
            public int iOutputsW3;
        }

        //public Int16[] iTimeOut = new Int16[iTxMaxSeams];

        public int[] iTimeOut
        {
            set
            {
                setData(value);
            }
        }

        public int[] iNormalSpeed
        {
            set
            {
                setData(value);
            }
        }
        void setData(int[] itest)
        {

        }

    }

    public class ModelProductSelection : ObservableObject
    {
        public ModelProductSelection()
        {
            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__BatchDone")));
            clInputErrors.Add(new clErrors(clConstants.INTCHECK, clLanguages.getName("__BatchDone")));
            clInputErrors.Add(new clErrors(clConstants.MINVALCHECK, clLanguages.getName("__BatchDone"), _sValidCharacters: "0"));
            clInputErrors.Add(new clErrors(clConstants.MAXVALCHECK, clLanguages.getName("__BatchDone"), _sValidCharacters: "0"));
            batchDone = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__BatchSize")));
            clInputErrors.Add(new clErrors(clConstants.INTCHECK, clLanguages.getName("__BatchSize")));
            clInputErrors.Add(new clErrors(clConstants.MINVALCHECK, clLanguages.getName("__BatchSize"), _sValidCharacters: "0"));
            batchSize = new clTextBoxValidation(clInputErrors);

            /*
            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__sBatchDone")));
            clInputErrors.Add(new clErrors(clConstants.MINVALCHECK, "", "", "1"));
            clInputErrors.Add(new clErrors(clConstants.MAXVALCHECK, "", "", iBatchSize.ToString()));
            sBatchDone = new clTextBoxValidation(clInputErrors);
            */

        batchSize.sInput = "";
            batchDone.sInput = "";
        }

        #region private variablen/properties
        private string _sTest;
        private ObservableCollection<clErrors> clInputErrors { get; set; } = new ObservableCollection<clErrors>();

        private projectsConnectLocalDB _projectsConnectDB = new projectsConnectLocalDB();
        private productsConnectLocalDB _productsConnectDB = new productsConnectLocalDB();
        private productionConnectLocalDB _productionConnectDB = new productionConnectLocalDB();
        private coverConnectLocalDB _coversConnectDB = new coverConnectLocalDB();
        private typeConnectLocalDB _typesConnectDB = new typeConnectLocalDB();
        private profilesConnectLocalDB _profilesConnectDB = new profilesConnectLocalDB();

        private clcbbOptions _clAvailableProjects { get; set; } = new clcbbOptions();
        private clcbbOptions _clAvailableProducts { get; set; } = new clcbbOptions();
        #endregion

        public clcbbOptions clAvailableProjects
        {
            get
            {
                return _clAvailableProjects;
            }
            set
            {
                _clAvailableProjects = value;
                OnPropertyChanged("clAvailableProjects");
            }
        }
        public clcbbOptions clAvailableProducts
        {
            get
            {
                return _clAvailableProducts;
            }
            set
            {
                _clAvailableProducts = value;
                OnPropertyChanged("clAvailableProducts");
            }
        }

        //private ProductInfo _clSelectedProductInfo { get; set; } = new ProductInfo();

        public ProjectInfo selectedProjectInfo { get; set; } = new ProjectInfo();
        public ProductInfo selectedProductInfo { get; set; } = new ProductInfo();

        private clTextBoxValidation _batchDone { get; set; }
        private clTextBoxValidation _batchSize { get; set; }

        public clTextBoxValidation batchDone
        {
            get
            {
                return _batchDone;
            }
            set
            {
                _batchDone = value;
                OnPropertyChanged("batchDone");
            }
        }

        public clTextBoxValidation batchSize
        {

            get
            {
                return _batchSize;
            }
            set
            {
                _batchSize = value;
                OnPropertyChanged("batchSize");
            }
        }

        public string sBatchDone
        {
            get
            {
                return selectedProjectInfo.iBatchDone.ToString();
            }
            set
            {
                batchDone.sInput = value;
                
                clInputErrors = new ObservableCollection<clErrors>();
                clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__BatchSize")));
                clInputErrors.Add(new clErrors(clConstants.INTCHECK, clLanguages.getName("__BatchSize")));
                clInputErrors.Add(new clErrors(clConstants.MINVALCHECK, clLanguages.getName("__BatchSize"), _sValidCharacters: value));
                batchSize = new clTextBoxValidation(clInputErrors, true);
                batchSize.sInput = sBatchSize;

                int iTempVar;
                Int32.TryParse(value, out iTempVar);
                selectedProjectInfo.iBatchDone = iTempVar;

                OnPropertyChanged("sBatchDone");
            }
        }

        public string sBatchSize
        {
            get
            {
                return selectedProjectInfo.iBatchSize.ToString();
            }
            set
            {
                batchSize.sInput = value;
             
                clInputErrors = new ObservableCollection<clErrors>();
                clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__BatchDone")));
                clInputErrors.Add(new clErrors(clConstants.INTCHECK, clLanguages.getName("__BatchDone")));
                clInputErrors.Add(new clErrors(clConstants.MINVALCHECK, clLanguages.getName("__BatchDone"), _sValidCharacters: "0"));
                clInputErrors.Add(new clErrors(clConstants.MAXVALCHECK, clLanguages.getName("__BatchDone"), _sValidCharacters: value));
                batchDone = new clTextBoxValidation(clInputErrors, true);
                batchDone.sInput = sBatchDone;

                int iTempVar;
                Int32.TryParse(value, out iTempVar);
                selectedProjectInfo.iBatchSize = iTempVar;

                OnPropertyChanged("sBatchSize");

            }
        }


        private void addAvailableProduct(List<Object> _argProductsQuery)
        {
            foreach (var projects in this.clAvailableProducts.obcItems.ToList())          // Delete all items in this mirrow database list
            {
                this.clAvailableProducts.obcItems.Remove(projects);                       // Remove all database data from observableobject. 
            }

            this.clAvailableProducts.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__NoProductSelected"), -1));      // Always an empty row

            foreach (Product products in _argProductsQuery)
            {
                this.clAvailableProducts.obcItems.Add(new clCbbFilltype1(products.code, products.productId));      // Add database data to observableobject. 
            }
        }
        private void getProjectInfo(Project _argProject)
        {
            if (_argProject != null)
            {
                selectedProjectInfo.sMachineId          = _argProject.machineId;
                selectedProjectInfo.sPlantId            = _argProject.plantId;
                selectedProjectInfo.sPlantInfo          = _argProject.plantInfo;
                selectedProjectInfo.sCustomerId         = _argProject.customerId;
                selectedProjectInfo.sCustomerInfo       = _argProject.customerInfo;
                selectedProjectInfo.sProductionMode     = clConstants.ProductionMode[_argProject.productMode];
                selectedProjectInfo.sCounterReset       = clConstants.ResetOptions[_argProject.counterReset];
                selectedProjectInfo.sProductionCode     = _argProject.prodCodesTable;

                selectedProjectInfo.xUseLotNumber       = _argProject.useLotNumber;
                selectedProjectInfo.iProjectId          = _argProject.projectId;
                selectedProjectInfo.iProductionMode     = _argProject.productMode;
                selectedProjectInfo.iCounterResetMode   = _argProject.counterReset;
            }
            else
            {
                //selectedProjectInfo = new ProjectInfo();
                selectedProjectInfo.sMachineId      = "";
                selectedProjectInfo.sPlantId        = "";
                selectedProjectInfo.sPlantInfo      = "";
                selectedProjectInfo.sCustomerId     = "";
                selectedProjectInfo.sCustomerInfo   = "";
                selectedProjectInfo.sProductionMode = "";
                selectedProjectInfo.sCounterReset   = "";
                selectedProjectInfo.sProductionCode = "";

                selectedProjectInfo.xUseLotNumber        = false;
                selectedProjectInfo.iProjectId           = -1;
                selectedProjectInfo.iProductionMode      = 0;            // Default = single mode
                selectedProjectInfo.iCounterResetMode    = clConstants.NO_RESET;
            }

        }
        private void getProductInfo(Product _argProduct)
        {
            if (_argProduct != null)
            {
                selectedProductInfo.sName       = _argProduct.name;
                selectedProductInfo.sCover      = _coversConnectDB.selectWhere((int)_argProduct.coverId).code;
                selectedProductInfo.sType       = _typesConnectDB.selectWhere((int)_argProduct.typeId).name;
                selectedProductInfo.sProfile    = _profilesConnectDB.selectWhere((int)_argProduct.profileId).name;
                selectedProductInfo.iProductId  = _argProduct.productId;
            }
            else
            {
                selectedProductInfo.sName       = "";
                selectedProductInfo.sCover      = "";
                selectedProductInfo.sType       = "";
                selectedProductInfo.sProfile    = "";
            }

        }
        private Production getBatchInfo(int _argiProjectId, int _argiProductId)
        {
            // query naar production tabel
            Production production;
            production = _productionConnectDB.selectSingle(_argiProjectId, _argiProductId);

            return production;
        }

        public string sTest
        {
            get
            {
                return _sTest;
            }
            set
            {
                _sTest = value;
            }
        }
        public void filterProjects(string _argsFilterProject = "")
        {
            List<Object> ProjectQuery = new List<Object>(_projectsConnectDB.selectWhere("", 0, _argsFilterProject));

            foreach (var projects in this.clAvailableProjects.obcItems.ToList())          // Delete all items in this mirrow database list
            {
                this.clAvailableProjects.obcItems.Remove(projects);                       // Remove all database data from observableobject. 
            }

            this.clAvailableProjects.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__NoProjectSelected"), -1));  // Always an empty row

            foreach (Project projects in ProjectQuery)
            {
                this.clAvailableProjects.obcItems.Add(new clCbbFilltype1(projects.projectName, projects.projectId));      // Add database data to observableobject. 
            }
        }
        public void filterProjects(int _argsProjectId)
        {
            Project selectedProject;
            selectedProject = _projectsConnectDB.selectSingle(_argsProjectId);

            foreach (var projects in this.clAvailableProjects.obcItems.ToList())          // Delete all items in this mirrow database list
            {
                this.clAvailableProjects.obcItems.Remove(projects);                       // Remove all database data from observableobject. 
            }

            this.clAvailableProjects.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__NoProjectSelected"), -1));  // Always an empty row
            this.clAvailableProjects.obcItems.Add(new clCbbFilltype1(selectedProject.projectName, selectedProject.projectId));      // Add database data to observableobject. 
        }
        public void filterProducts(int _argiProjectId, string _argsFilterProducts = "")     // Get products by projectId
        {
            List<Object> ProductsQuery = new List<Object>(_productsConnectDB.selectWhere("", _argiProjectId, _argsFilterProducts));

            addAvailableProduct(ProductsQuery);
        }
        public void filterProducts(string _argsFilterProducts) // Get products by productname
        {
            List<Object> ProductsQuery = new List<Object>(_productsConnectDB.select(_argsFilterProducts));
            addAvailableProduct(ProductsQuery);
        }

        /// <summary>
        /// Get production settings/information from database which are necesary to display on the productselection screen
        /// </summary>
        /// <param name="_argiProjectId">Project ID</param>
        /// <param name="_argiProductId">Product ID</param>
        public void getProductionSettings(int _argiProjectId, int _argiProductId)
        {
            Project project = new Project();
            Product product = new Product();

            project = _projectsConnectDB.selectSingle(_argiProjectId);      // read project info from database
            getProjectInfo(project);                                        // Set project properties
            product = _productsConnectDB.selectProduct(_argiProductId);     // read product info from database
            getProductInfo(product);                     // Set product properties

            if(product != null)
            {
                if (selectedProjectInfo.xBatchMode)  // Only when project is in batch mode.. 
                {
                    Production production;
                    production = getBatchInfo(_argiProjectId, product.productId);  // get current batch info from database if exist. 
                    if (production != null)
                    {
                        sBatchDone = production.batchDone.ToString();
                        sBatchSize = production.batchTotal.ToString();
                    }
                    else                            // Record does not exist. Set default settings                                      
                    {
                        sBatchDone = "0";
                        sBatchSize = "1";
                    }
                }
            }
            else
            {
                sBatchDone = "0";
                sBatchSize = "1";
            }
        }
        public Product getProduct(string sProductCode)
        {
            Product selectedProduct = _productsConnectDB.selectProduct(sProductCode);            // get current product from database
            return selectedProduct;
        }
        public Product getFilteredProduct(string sProductFilter)
        {
            List<Object> selectedProduct = _productsConnectDB.select(sProductFilter);            // get current product from database
            Product _returnProduct = null;

            if(selectedProduct.Count > 0)
                _returnProduct = (Product)selectedProduct.First();

            return _returnProduct;
        }
    }

    

    public class ModelProduction
    {
        public ModelProduction(int _argIProjectId, int _argIProductId)
        {
            productionSettings.iProjectId = _argIProjectId;
            productionSettings.iProductId = _argIProductId;

            getProductSettings();       // Get product settings of selected product
        }



        private productsConnectLocalDB              _productsConnectDB              = new productsConnectLocalDB();
        private seamTemplateConnectLocalDB          _seamTemplateConnectDB          = new seamTemplateConnectLocalDB();
        private materialIOPositionConnectLocalDB    _materialIOPositionConnectDB    = new materialIOPositionConnectLocalDB();
        private materialProductConnectLocalDB       _materialProductConnectDB       = new materialProductConnectLocalDB();
        private productionCounterConnectLocalDB     _productionCounterConnectDB     = new productionCounterConnectLocalDB();
        private productionConnectLocalDB            _productionConnectDB            = new productionConnectLocalDB();
        private projectsConnectLocalDB              _projectConnectDB               = new projectsConnectLocalDB();

        public ProductionSettings productionSettings    { get; set; } = new ProductionSettings();
        public ProductSettings productsettings          { get; set; } = new ProductSettings();      // Contains settings of current product
        public Productlive productlive                  { get; set; } = new Productlive();          // Contains live values of current product

        
        private void plcSendProduct()       // Send product settings to PLC
        {
            // Bla bla query naar tabellen voor gegevens en deze vervolgens opslaan voor het produceren.
        }
        private void getProductSettings()   // Get product information/settings from database
        {
            Project projectQuery = _projectConnectDB.selectSingle(productionSettings.iProjectId);       // Get project information from database
            Product productQuery = _productsConnectDB.selectProduct(productionSettings.iProductId);     // get product information from database

            productionSettings.xUseLotNumber        = projectQuery.useLotNumber;
            productionSettings.iProjectId           = projectQuery.projectId;
            productionSettings.iProductId           = productQuery.productId;
            productionSettings.iProductionMode      = projectQuery.productMode;
            productionSettings.iCounterResetMode    = projectQuery.counterReset;

            productsettings.sName = productQuery.name;
            productsettings.sCode = productQuery.code;

            productsettings.sProfileName = productQuery.profile.name;           // Set profile name
            productsettings.sProfileComment = productQuery.profile.comment;     // Set profile comment


            ProdSeamTemplate prodSeam;
            seamTemplate seamTemplateQuery;
            #region get seams
            if (productQuery.profile.seam1_id > 0)                              // If seam1 contains a value > 0, this seam is used. 
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam1_id);     // Retrieve selected seamtemplate from database
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));                               // Add to seam list
            }
            if (productQuery.profile.seam2_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam2_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam3_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam3_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam4_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam4_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam5_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam5_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam6_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam6_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam7_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam7_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam8_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam8_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam9_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam9_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam10_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam10_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam11_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam11_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam12_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam12_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam13_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam13_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam14_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam14_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam15_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam15_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            if (productQuery.profile.seam16_id > 0)
            {
                prodSeam = new ProdSeamTemplate();
                seamTemplateQuery = _seamTemplateConnectDB.selectWhere((int)productQuery.profile.seam16_id);
                productsettings.obcSeams.Add(addSeamTemplate(seamTemplateQuery));
            }
            #endregion 

            List<Object> materialIOPositionQuery;
            List<Object> materialProductQuery;

            materialIOPositionQuery = _materialIOPositionConnectDB.selectWhere(productionSettings.iProjectId);  // Get material IO positions by project ID

            ProdIOPosition newIOPosition;
            ProdMaterials newMaterial;

            // Every project has an amount of IO positions. Which actually is how many hardware devices are used. 
            // Every IO position contains and hardware device. In the project is registered which materials are added to this specific IO position. 
            foreach (Material_IOPosition materialIOposition in materialIOPositionQuery)     // For every found IO position
            {
                newIOPosition = new ProdIOPosition();

                newIOPosition.sMaterialTypeName = materialIOposition.MaterialType.name;         // Material type name        
                newIOPosition.sDeviceName = materialIOposition.DeviceType.deviceName;     // Device type name
                newIOPosition.sDeviceAddress = materialIOposition.DeviceType.deviceAdress;
                newIOPosition.sInterfaceType = materialIOposition.DeviceType.interfaceType;

                // get added material to this IO position
                materialProductQuery = _materialProductConnectDB.selectWhereProductMaterialIOPosition(productionSettings.iProductId, materialIOposition.materialIOPosition);

                foreach (MaterialProduct material in materialProductQuery)       // for every found material at this IO position
                {
                    newMaterial = new ProdMaterials();

                    newMaterial.iMaterialId = material.materialId;
                    newMaterial.sMaterialName = material.Material.name;
                    newMaterial.sMaterialCode = material.Material.code;
                    newMaterial.iMinLength = material.Material.minLength;
                    newMaterial.sCodeRange = material.Material.range;

                    newIOPosition.obcAllowedMaterials.Add(newMaterial);
                }
                productsettings.obcIOPositions.Add(newIOPosition);
            }

        }
        private ProdSeamTemplate addSeamTemplate(seamTemplate _argNewSeamTemplate)
        {
            ProdSeamTemplate prodSeam   = new ProdSeamTemplate();
            prodSeam.sName              = _argNewSeamTemplate.name;
            prodSeam.sComment           = _argNewSeamTemplate.comment;
            prodSeam.xPrintActive       = (bool)_argNewSeamTemplate.printLabelActive;
            prodSeam.iLabelPrintPos     = (int)_argNewSeamTemplate.labelPrtPos;
            prodSeam.iMinTens           = (int)_argNewSeamTemplate.minTens;
            prodSeam.iMaxTens           = (int)_argNewSeamTemplate.maxTens;
            prodSeam.iBlindTens         = (int)_argNewSeamTemplate.blindTens;
            prodSeam.iTensFilter        = (int)_argNewSeamTemplate.tensFilter;
            prodSeam.iStitchCount       = (int)_argNewSeamTemplate.stitchCount;
            prodSeam.iMinStitch         = (int)_argNewSeamTemplate.stitchCount - (int)_argNewSeamTemplate.negTol;
            prodSeam.iMaxStitch         = (int)_argNewSeamTemplate.stitchCount + (int)_argNewSeamTemplate.posTol;
            prodSeam.iBlindArea         = (int)_argNewSeamTemplate.blindArea;
            prodSeam.iSeamLength        = (int)_argNewSeamTemplate.seamLen;
            prodSeam.iStartBtFw         = (int)_argNewSeamTemplate.startBtFw;
            prodSeam.iStartBtBw         = (int)_argNewSeamTemplate.startBtBw;
            prodSeam.iEndBtFw           = (int)_argNewSeamTemplate.endBtFw;
            prodSeam.iEndBtBw           = (int)_argNewSeamTemplate.endBtBw;
            prodSeam.iSpeed             = (int)_argNewSeamTemplate.speed;
            prodSeam.iPar               = (int)_argNewSeamTemplate.par;
            prodSeam.iGeneral           = (int)_argNewSeamTemplate.general;
            prodSeam.iFunctions         = (int)_argNewSeamTemplate.functions;
            prodSeam.iMonitoring        = (int)_argNewSeamTemplate.monitoring;

            return prodSeam;
        }

        /// <summary>
        /// getProductionCounter gets production information from the database regarding counter values before start producing a product
        /// A counterValue is used to separate different products within a batch. It is possibile to reset this counterValue weekly or daily. 
        /// </summary>
        private void getProductionCounter()
        {
            ProductionCounter productionCounter; 

            productionCounter = _productionCounterConnectDB.selectSingle(productionSettings.iProjectId, productionSettings.iProductId);

            if (productionCounter != null)  // Record does exist in database. Read values
            {
                TimeSpan tsTimeDiff;
                switch (productionSettings.iCounterResetMode) // Check if counterValue is still valid
                {
                    case clConstants.RESET_DAILY:           // Daily reset counter
                        tsTimeDiff = DateTime.Now - productionCounter.firstProductInProjectCounterTime; // Current time - first produced product time
                        if (tsTimeDiff.TotalHours >= 24)    // if first produced product is last 24 hours, so reset value to zero
                        {
                            productionCounter.counterValue                      = 0;
                            productionCounter.firstProductInProjectCounterTime  = DateTime.Now;
                            productionSettings.iCounterValue              = productionCounter.counterValue;
                            productionSettings.timeProductionStart        = productionCounter.firstProductInProjectCounterTime;
                        }
                        else
                        {
                            productionSettings.iCounterValue              = productionCounter.counterValue;
                            productionSettings.timeProductionStart        = productionCounter.firstProductInProjectCounterTime;
                        }
                        break;

                    case clConstants.RESET_WEEKLY:          // Weekly reset counter
                        tsTimeDiff = DateTime.Now - productionCounter.firstProductInProjectCounterTime;
                        if (tsTimeDiff.TotalDays >= 7)      // if first produced product is last 7 days, so reset value to zero
                        {
                            productionCounter.counterValue                      = 0;
                            productionCounter.firstProductInProjectCounterTime  = DateTime.Now;
                            productionSettings.iCounterValue              = productionCounter.counterValue;
                            productionSettings.timeProductionStart        = productionCounter.firstProductInProjectCounterTime;
                        }
                        else
                        {
                            productionSettings.iCounterValue          = productionCounter.counterValue;
                            productionSettings.timeProductionStart    = productionCounter.firstProductInProjectCounterTime;
                        }
                        break;

                    case clConstants.NO_RESET:              // No reset counter 
                        productionSettings.iCounterValue          = productionCounter.counterValue;
                        productionSettings.timeProductionStart    = productionCounter.firstProductInProjectCounterTime;

                        break;
                }

                _productionCounterConnectDB.editInsertAndSubmit(productionCounter);     // save (edited) counter to database 
            }
            else
            {   
                // Record does not exist in database. Add a new one. 
                productionCounter                                   = new ProductionCounter();
                productionCounter.projectId                         = productionSettings.iProjectId;
                productionCounter.productId                         = productionSettings.iProductId;
                productionCounter.counterValue                      = 0;
                productionCounter.TotalCounterValue                 = 0;
                productionCounter.firstProductInProjectCounterTime  = DateTime.Now;

                productionSettings.iCounterValue              = 0;
                productionSettings.timeProductionStart        = productionCounter.firstProductInProjectCounterTime;

                _productionCounterConnectDB.insertAndSubmit(productionCounter);         // Insert counter record to database
            }
        }

        /// <summary>
        /// updateProductionCounter edit the counter record in database. Increase totalvalue and insert new current counterValue when is product is finished
        /// </summary>
        private void updateProductionCounter()
        {
            ProductionCounter productionCounter;
            productionCounter = _productionCounterConnectDB.selectSingle(productionSettings.iProjectId, productionSettings.iProductId);     // Select record in database

            productionCounter.counterValue      = productionSettings.iCounterValue;        // TODO: update iCounterValue somewhere
            productionCounter.TotalCounterValue += 1; 

            _productionCounterConnectDB.editInsertAndSubmit(productionCounter);
        }

        /// <summary>
        /// Update record in database after starting/finishing a product. When finishing a product, this record will be updatet. 
        /// </summary>
        private void updateBatchInformation()
        {
            Production production;

            production = _productionConnectDB.selectSingle(productionSettings.iProjectId, productionSettings.iProductId);  // get current batch info from database if exist. 
            if (production != null)
            {
                production.batchDone    = productionSettings.iBatchDone;        // After finishing a product, this record must be increased with one
                production.batchTotal   = productionSettings.iBatchSize;

                _productionConnectDB.editInsertAndSubmit(production);           // Insert possible edited record 
            }
            else
            {
                production              = new Production();
                production.projectId    = productionSettings.iProjectId;
                production.productId    = productionSettings.iProductId;
                production.batchDone    = productionSettings.iBatchDone;
                production.batchTotal   = productionSettings.iBatchSize;

                _productionConnectDB.insertAndSubmit(production);               // insert new record
            }
        }

        /// <summary>
        /// After finishing a product, this function will be called. Update database etc 
        /// </summary>
        private void endProductProduction()
        {

            productionSettings.iBatchDone       += 1;
            productionSettings.iCounterValue    += 1;

            updateProductionCounter();
            updateBatchInformation();


            // TODO bepaal wat er moet gebeuren.. Terug naar hoofdscherm, gelijk een nieuw product.. etc.
        }

        /// <summary>
        /// Call this function when material is checked and OK and operator is going to start sewing the product
        /// </summary>
        public void startProductProduction()
        {
            updateBatchInformation();       // Get batch information
            getProductionCounter();         // Get production counter information

        }

    }
}
 