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
using System.Windows.Data;
using System.Windows;
using System.ComponentModel;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Windows.Media;
using System.Globalization;
using System.Threading;
using System.IO.Ports;

namespace Sewco.Modules.ProductionModule
{
    public class ScannedMaterial : ObservableObject
    {
        private string _sMaterialTypeName   = "";
        private string _sScannedMaterial    = "";
        private bool _xWrongMaterial        = false;

        public string sDeviceAddress;
        public BackgroundWorker bgwReadMaterial                         = new BackgroundWorker();
        public ObservableCollection<ProdMaterials> obcAllowedMaterials  = new ObservableCollection<ProdMaterials>();

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
        public string sScannedMaterial
        {
            get
            {
                return _sScannedMaterial;
            }
            set
            {
                _sScannedMaterial = value;
                OnPropertyChanged("sScannedMaterial");
            }
        }
        public bool xWrongMaterial
        {
            get
            {
                return _xWrongMaterial;
            }
            set
            {
                _xWrongMaterial = value;
                OnPropertyChanged("xWrongMaterial");
            }
        }


        /*public void readMaterialFromIO(string _argsDeviceAddress, int _argiIndex, ObservableCollection<ProdMaterials> _argobcAllowedMaterials)
        {
            string _sMaterialName;
            bool _xError;
            doReadIO(_argsDeviceAddress, out _sMaterialName, out _xError);

            if (!_xError)
            {
                sScannedMaterial    = _sMaterialName;
                xWrongMaterial      = !(_argobcAllowedMaterials.Any(material => material.sMaterialName == sScannedMaterial)); // Check if material exist in obcAllowedMaterials list

                if (xWrongMaterial)                                         // Scanned material does not exist in material list
                    sScannedMaterial = clLanguages.getName("__NotAllowedMaterial: ") + sScannedMaterial;
            }
            else
            {
                sScannedMaterial    = _sMaterialName;
                xWrongMaterial      = true;
            }
        }*/
        public void readMaterialFromIO(object sender, DoWorkEventArgs e)
        {
            string _sMaterialName;
            bool _xError;
            doReadIO(out _sMaterialName, out _xError);

            if (!_xError)
            {
                sScannedMaterial = _sMaterialName;
                xWrongMaterial = !(obcAllowedMaterials.Any(material => material.sMaterialName == sScannedMaterial)); // Check if material exist in obcAllowedMaterials list

                if (xWrongMaterial)                                         // Scanned material does not exist in material list
                    sScannedMaterial = clLanguages.getName("__NotAllowedMaterial: ") + sScannedMaterial;
            }
            else
            {
                sScannedMaterial = _sMaterialName;
                xWrongMaterial = true;
            }
        }

        public void doReadIO(out string _sValue, out bool _xError, string _argsIOType = "RS232")
        {
            //string _sValue = "";
            SerialPort serialPort = new SerialPort();
            _sValue = "";
            _xError = false;
            if (_argsIOType == "RS232")
            {
                try
                {
                    serialPort.PortName     = sDeviceAddress;
                    serialPort.ReadTimeout  = 2000;    // 2000 miliseconds before time-out
                    serialPort.BaudRate     = 9600;
                    serialPort.DataBits     = 8;
                    serialPort.StopBits     = (StopBits)Enum.Parse(typeof(StopBits), "1");
                    serialPort.Parity       = (Parity)Enum.Parse(typeof(Parity), "None");
                    serialPort.Handshake    = System.IO.Ports.Handshake.None;
                    serialPort.Open();

                    _sValue = serialPort.ReadLine();
                    serialPort.DiscardInBuffer();       // Clear input buffer
                }
                catch (TimeoutException)
                {
                    _sValue = clLanguages.getName("__TimeoutReadingComPort");
                    _xError = true;
                }
                catch
                {
                    _sValue = clLanguages.getName("__ComPortIsInvalid");
                    _xError = true;
                }
                finally
                {
                    serialPort.Close();
                }
            }
        }
    }

    public class ViewModelProduction : ObservableObject
    {
        public ViewModelProduction()
        {
            //if (ViewModelControlPanel.xValidDatabaseConnection)
            //{
                clProductionSeamList5 = new clProductionSeams();

            //errorBlink = new blink(0, 0, 0, 0, 500);
            errorBlink.Tick     += new EventHandler(invertErrorActive);
            errorBlink.Interval = new TimeSpan(0, 0, 0, 0, 500);
            errorBlink.Start();

            updateTrend.Tick    += new EventHandler(sewing);
            updateTrend.Interval = new TimeSpan(0, 0, 0, 0, 250);

            xFunction_EBT   = true;
            xFunction_label = true;
            xFunction_HP    = true;
            //xErrorActive = errorBlink.xBlink;
            xShowErrors = true;
            //xMaterialCheck = true;

            obcStatus.Add(new clStatus(0, xStartSewing, "/Resources/Images/commonImages/motor.png"));
            obcStatus.Add(new clStatus(1, xPedalBlocked, "/Resources/Images/commonImages/pedal.png"));
            
            
            //}
            
            btnTest = new RelayCommand(
                       param => getScannerProductId(),
                       param => (true)            // Only visible when not in New of Change mode
                   );
            nextSeamCommand = new RelayCommand(
                        param => updateSeamList(),
                        param => (true)            // Only visible when not in New of Change mode
                    );
            errorCommand = new RelayCommand(
                        param => invertShowError(),
                        param => (true)            // Only visible when not in New of Change mode
                    );
            materialCommand = new RelayCommand(
                        param => invertShowMaterials(),
                        param => (true)            // Only visible when not in New of Change mode
                    );
            enterFilterProductCommand = new RelayCommand(
                        param => onEnterProductField(),
                        param => (true)          
                    );

            showProductionScreenCommand = new RelayCommand(
                                       param => enterProduction(),
                                       param => (iProductSelectedValue > 0 && iProjectSelectedValue > 0) && !productSelection.batchDone.xHasError && !productSelection.batchSize.xHasError);
            /*((((productSelection.productionSettings.iBatchDone <= productSelection.productionSettings.iBatchSize) && productSelection.productionSettings.iBatchDone >0) && productSelection.productionSettings.iProductionMode == clConstants.PRODUCTION_BATCH_MODE)
            || (productSelection.productionSettings.iProductionMode != clConstants.PRODUCTION_BATCH_MODE)));*/
            resetTrendCommand = new RelayCommand(
                        param => resetTrend(),
                        param => (true)            // Only visible when not in New of Change mode
                    );
            /*
            startTrendCommand = new RelayCommand(
                        param => updateTrend.Start(),
                        param => (true)            // Only visible when not in New of Change mode
                    );
            */

            iMinStitches = 80;
            iMaxStitches = 100;

            updateErrorList();
            updateStatusList();

            sFilterProject = "";
        }

        #region Private declarations
        private ObservableCollection<clStatus>  _obcStatus  = new ObservableCollection<clStatus>();
        private ObservableCollection<clError>   _obcErrors  = new ObservableCollection<clError>();

        private clStatusBar                     _statusBar  = new clStatusBar();
        private clTrend                         _trend      = new clTrend();

        private int _iProjectSelectedValue;
        private int _iProductSelectedValue;
        private string _sFilterProject;
        private string _sFilterProduct;
        private bool _xShowErrors;
        private bool _xShowMaterials;
        private bool _xErrorActive;
        private bool _xMaterialCheck;
        private bool _xFunction_HP;
        private bool _xFunction_STL;
        private bool _xFunction_FSPL;
        private bool _xFunction_SBT;
        private bool _xFunction_EBT;
        private bool _xFunction_label;
        private bool _xFunction_scan;
        private bool _xStartSewing;
        private bool _xPedalBlocked;

        private int _iActStitch;
        private int _iMaxStitches;
        private int _iMinStitches;
        private bool _xLabelPrinted;
        private bool _xScanbackDone;

        private void invertShowError()
        {
            xShowErrors = !xShowErrors;
            xShowMaterials = false;
        }
        private void invertShowMaterials()
        {
            xShowMaterials = !xShowMaterials;
            xShowErrors = false;
        }
        private double _dSeamNameWidth;

        public double dSeamNameWidth
        {
            get
            {
                return _dSeamNameWidth;
            }
            set
            {
                _dSeamNameWidth = value;
                OnPropertyChanged("dSeamNameWidth");
            }
        }

        System.Windows.Threading.DispatcherTimer errorBlink     = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer updateTrend    = new System.Windows.Threading.DispatcherTimer();
        #endregion

        #region Public declarations
        public clStatusBar statusBar
        {
            get
            {
                return _statusBar;
            }
            set
            {
                _statusBar = value;
                OnPropertyChanged("statusBar");
            }
        }
        public clTrend trend
        {
            get
            {
                return _trend;
            }
            set
            {
                _trend = value;
                OnPropertyChanged("trend");
            }
        }
        public RelayCommand showProductionScreenCommand { get; set; }
        public RelayCommand nextSeamCommand             { get; set; }
        public RelayCommand errorCommand                { get; set; }
        public RelayCommand materialCommand             { get; set; }
        public RelayCommand btnTest                     { get; set; }
        public RelayCommand resetTrendCommand           { get; set; }
        public RelayCommand startTrendCommand           { get; set; }
        public RelayCommand enterFilterProductCommand   { get; set; }

        
        public ObservableCollection<clStatus> obcStatus
        {
            get
            {
                return _obcStatus;
            }
            set
            {
                _obcStatus = value;
                OnPropertyChanged("obcStatus");
            }
        }
        public ObservableCollection<clError> obcErrors
        {
            get
            {
                return _obcErrors;
            }
            set
            {
                _obcErrors = value;
                OnPropertyChanged("obcErrors");
            }
        }
        public clProductionSeams clProductionSeamList5
        {
            get; set;
        }
        public ModelProductSelection productSelection { get; set; } = new ModelProductSelection();
        public ModelProduction production { get; set; }
        public ObservableCollection<ScannedMaterial> obcScannedMaterials { get; set; } = new ObservableCollection<ScannedMaterial>();

        public int iProjectSelectedValue
        {
            get
            {
                return _iProjectSelectedValue; //_iProjectSelectedValue;
            }
            set
            {
                _iProjectSelectedValue = value;
                OnPropertyChanged("iProjectSelectedValue");
                productSelection.filterProducts(value);
                setProductSelectedValue();
            }
        }
        public int iProductSelectedValue
        {
            get
            {
                return _iProductSelectedValue;
            }
            set
            {
                _iProductSelectedValue = value;
                OnPropertyChanged("iProductSelectedValue");

                productSelection.getProductionSettings(iProjectSelectedValue, _iProductSelectedValue);  // Get project settings
            }
        }
        public string sFilterProject
        {
            get
            {
                return _sFilterProject;
            }
            set
            {
                _sFilterProject = value;
                OnPropertyChanged("sFilterProject");
                productSelection.filterProjects(value);
                setProjectSelectedValue();
            }
        }
        public string sFilterProduct
        {
            get
            {
                return _sFilterProduct;
            }
            set
            {
                _sFilterProduct = value;
                OnPropertyChanged("sFilterProduct");
                productSelection.filterProducts(iProjectSelectedValue, value);
                setProductSelectedValue();
            }
        }
        public bool xShowErrors
        {
            get
            {
                return _xShowErrors;
            }
            set
            {
                _xShowErrors = value;
                OnPropertyChanged("xShowErrors");
            }
        }
        public bool xShowMaterials
        {
            get
            {
                return _xShowMaterials;
            }
            set
            {
                _xShowMaterials = value;
                OnPropertyChanged("xShowMaterials");
            }
        }
        public bool xErrorActive
        {
            get
            {
                return _xErrorActive;
            }
            set
            {
                _xErrorActive = value;
                OnPropertyChanged("xErrorActive");
            }
        }
        public bool xMaterialCheck
        {
            get
            {
                return _xMaterialCheck;
            }
            set
            {
                _xMaterialCheck = value;
                OnPropertyChanged("xMaterialCheck");
            }
        }
        public bool xFunction_HP
        {
            get
            {
                return _xFunction_HP;
            }
            set
            {
                _xFunction_HP = value;
                OnPropertyChanged("xFunction_HP");
            }
        }
        public bool xFunction_STL
        {
            get
            {
                return _xFunction_STL;
            }
            set
            {
                _xFunction_STL = value;
                OnPropertyChanged("xFunction_STL");
            }
        }
        public bool xFunction_FSPL
        {
            get
            {
                return _xFunction_FSPL;
            }
            set
            {
                _xFunction_FSPL = value;
                OnPropertyChanged("xFunction_FSPL");
            }
        }
        public bool xFunction_SBT
        {
            get
            {
                return _xFunction_SBT;
            }
            set
            {
                _xFunction_SBT = value;
                OnPropertyChanged("xFunction_SBT");
            }
        }
        public bool xFunction_EBT
        {
            get
            {
                return _xFunction_EBT;
            }
            set
            {
                _xFunction_EBT = value;
                OnPropertyChanged("xFunction_EBT");
            }
        }
        public bool xFunction_label
        {
            get
            {
                return _xFunction_label;
            }
            set
            {
                _xFunction_label = value;
                OnPropertyChanged("xFunction_label");
            }
        }
        public bool xFunction_scan
        {
            get
            {
                return _xFunction_scan;
            }
            set
            {
                _xFunction_scan = value;
                OnPropertyChanged("xFunction_scan");
            }
        }
        public bool xStartSewing
        {
            get
            {
                return _xStartSewing;
            }
            set
            {
                if (value)
                {
                    updateTrend.Start();
                } 
                else
                {
                    updateTrend.Stop();
                }

                _xStartSewing = value;
                OnPropertyChanged("xStartSewing");
                updateStatusList();
            }
        }
        public bool xPedalBlocked
        {
            get
            {
                return _xPedalBlocked;
            }
            set
            {
                _xPedalBlocked = value;
                OnPropertyChanged("xPedalBlocked");
            }
        }
        public bool xScanbackDone
        {
            get
            {
                return _xScanbackDone;
            }
            set
            {
                _xScanbackDone = value;
                OnPropertyChanged("xScanbackDone");
            }
        }
        public bool xLabelPrinted
        {
            get
            {
                return _xLabelPrinted;
            }
            set
            {
                _xLabelPrinted = value;
                OnPropertyChanged("xLabelPrinted");
            }
        }
        public int iActStitch
        {
            get
            {
                return _iActStitch;
            }
            set
            {
                _iActStitch = value;
                trend.iActStitch = value;
                statusBar.iActStitch = value;
                OnPropertyChanged("iActStitch");
            }
        }
        public int iMaxStitches
        {
            get
            {
                return _iMaxStitches;
            }
            set
            {
                _iMaxStitches = value;
                trend.iMaxStitches = value;
                statusBar.iMaxStitches = value;
                OnPropertyChanged("iMaxStitches");
            }
        }
        public int iMinStitches
        {
            get
            {
                return _iMinStitches;
            }
            set
            {
                _iMinStitches = value;
                trend.iMinStitches = value;
                statusBar.iMinStitches = value;
                OnPropertyChanged("iMinStitches");
            }
        }
        public string sTest
        {
            get
            {
                return productSelection.sTest;
            }
            set
            {
                productSelection.sTest = value;
                OnPropertyChanged("sTest");
            }
        }
        #endregion

        #region Functions
        private void setProjectSelectedValue()
        {
            iProjectSelectedValue = productSelection.clAvailableProjects.obcItems.First().iSelectedvaluePath;
        }
        private void setProductSelectedValue()
        {
            iProductSelectedValue = productSelection.clAvailableProducts.obcItems.First().iSelectedvaluePath;
        }
        public void getScannerProductId()
        {
            _sFilterProject = "";
            _sFilterProduct = "";
            OnPropertyChanged("sFilterProject");
            OnPropertyChanged("sFilterProduct");

            productSelection.filterProjects();

            string sProductCode = "1010101";
            Product selectedProduct = productSelection.getProduct(sProductCode);
            if(selectedProduct != null)
            {
                iProjectSelectedValue = (int)selectedProduct.projectId;
                iProductSelectedValue = selectedProduct.productId;
            }
        }
        public void onEnterProductField()
        {
            Product selectedProduct = productSelection.getFilteredProduct(sFilterProduct);

            if (selectedProduct != null)
            {
                iProjectSelectedValue = (int)selectedProduct.projectId;
                iProductSelectedValue = selectedProduct.productId;
            }
        }

        private void sewing(object sender, EventArgs e)
        {

            drawTrend();

            if (xStartSewing)
            {
                if (iActStitch == statusBar.iStitchLabel && !xLabelPrinted)
                {
                    statusBar.xStatusLabelActive = true;
                    xStartSewing = false;
                    xLabelPrinted = true;
                }
                else
                {
                    statusBar.xStatusLabelActive = false;
                }

                if (iActStitch == statusBar.iStitchScanback && !xScanbackDone)
                {
                    statusBar.xStatusScanbackActive = true;
                    xStartSewing = false;
                    xScanbackDone = true;
                }
                else
                {
                    statusBar.xStatusScanbackActive = false;
                }

                if (iActStitch >= iMaxStitches)
                {
                   production.productsettings.obcSeams[production.productlive.iActiveSeam-1].iActStitch = iActStitch;
                    resetTrend();
                    updateSeamList();
                    xStartSewing = false;
                    xPedalBlocked = true;
                    
                }
            }

            if (xStartSewing && iActStitch < iMaxStitches)
            {
                iActStitch += 1;
            }
            clProductionSeamList5.obcItems[2].iActStitches = iActStitch.ToString();
            updateErrorList();
            updateStatusList();
        }
        private void enterProduction()
        {
            production = new ModelProduction(iProjectSelectedValue, iProductSelectedValue);         // Create a new production class

            ViewControlPanel.ViewModelControlPanel.showScreen("Production");    // Open production class

            updateErrorList();
            updateStatusList();
            updateSeamList();

            checkMaterial();

            staticProductionSettings.iPCProductionStep = 0;
            updatePCStep();
        }

        private void startProductProduction()
        {
            production.startProductProduction();

        }
        
        private void checkMaterial()
        {
            foreach (var material in obcScannedMaterials.ToList())
            {
                obcScannedMaterials.Remove(material);
            }
            ScannedMaterial scannedMaterial;
            foreach(var ioPosition in production.productsettings.obcIOPositions)                // For every IO position
            {
                scannedMaterial                     = new ScannedMaterial();
                scannedMaterial.sMaterialTypeName   = ioPosition.sMaterialTypeName;             // Material type name
                if(ioPosition.xMaterialsRequired)                                               // If material if required. Sometimes no material is added to an IO position
                {
                    scannedMaterial.sDeviceAddress                      = ioPosition.sDeviceAddress;
                    scannedMaterial.obcAllowedMaterials                 = ioPosition.obcAllowedMaterials;
                    scannedMaterial.bgwReadMaterial.DoWork              += new DoWorkEventHandler(scannedMaterial.readMaterialFromIO);      // Create event handler to read material
                    scannedMaterial.bgwReadMaterial.RunWorkerCompleted  += new RunWorkerCompletedEventHandler(eventReadMaterialDone);       // Create event handler when worker is finished
                    scannedMaterial.bgwReadMaterial.RunWorkerAsync();   // Start background worker

                    /*
                    So, create background worker (thread) to read scanner. 
                    The doReadIO waits until something is read with \n (readline).
                    readMaterialFromIO checks if this is OK. 
                    When background worker is finished, eventReadMaterialDone will be called. 
                    The background worker can finish with an error (No com-port, read timeout), or with the correct material or with a wrong material.
                    */
                }
                else    // No materials are added to this IO position
                {   
                    scannedMaterial.sScannedMaterial    = clLanguages.getName("__NoMaterialsSpecified");
                    scannedMaterial.xWrongMaterial      = false;
                }
                obcScannedMaterials.Add(scannedMaterial);
            }
        }
        /// <summary>
        /// This function is called when a material read background worker is ready
        /// </summary>
        private void eventReadMaterialDone(object sender, RunWorkerCompletedEventArgs e)
        {
            if (obcScannedMaterials.All(material => material.sScannedMaterial != ""))       // when every IO position is scanned
            {
                updatePCStep();
                xShowMaterials = obcScannedMaterials.Any(material => material.xWrongMaterial == true);
            }
        }
        /// <summary>
        /// readMaterialFromIO reads and checks of and (scanned) material is allowed at this IO position 
        /// </summary>
        /// <param name="_argsDeviceAddress">Device address of IO device. COM4 (serial), or 192.168.1.1 (Ethernet)</param>
        /// <param name="_argiIndex">Index of scannedMaterials index</param>
        /// <param name="_argobcAllowedMaterials">Allowed materials at this IO position</param>
        /*private void readMaterialFromIO(string _argsDeviceAddress, int _argiIndex, ObservableCollection<ProdMaterials> _argobcAllowedMaterials)
        {
            string _sMaterialName   = doReadIO(_argsDeviceAddress);

            if(_sMaterialName != null)
            {
                obcScannedMaterials[_argiIndex].sScannedMaterial    = _sMaterialName;
                obcScannedMaterials[_argiIndex].xWrongMaterial      = !(_argobcAllowedMaterials.Any(material => material.sMaterialName == obcScannedMaterials[_argiIndex].sScannedMaterial)); // Check if material exist in obcAllowedMaterials list

                if (obcScannedMaterials[_argiIndex].xWrongMaterial)                                         // Scanned material does not exist in material list
                    obcScannedMaterials[_argiIndex].sScannedMaterial = clLanguages.getName("__NotAllowedMaterial: ") + obcScannedMaterials[_argiIndex].sScannedMaterial;
            }
            else
            {
                obcScannedMaterials[_argiIndex].sScannedMaterial    = clLanguages.getName("__ErrorReadingMaterial");
                obcScannedMaterials[_argiIndex].xWrongMaterial      = true;
            }
        }*/
        /// <summary>
        /// Read connected IO device. COM-port, ethernet
        /// </summary>
        /// <param name="_argsDeviceAddress">Device address of IO device. COM4 (serial), or 192.168.1.1 (Ethernet) </param>
        /// <param name="_argsIOType">Serial or ethernet, or maybe CAN in the future </param>
        /// <returns></returns>
        /// 
        /*private SerialPort createSerialPort(string _argsDeviceAddress)
        {
            SerialPort serialPort = new SerialPort();

            try
            {
                // port instellingen doen..
                serialPort.PortName     = _argsDeviceAddress;
                serialPort.ReadTimeout  = 5000;    // 1000 miliseconds before time-out
                serialPort.BaudRate     = 9600;
                serialPort.DataBits     = 8;
                serialPort.StopBits     = (StopBits)Enum.Parse(typeof(StopBits), "1");
                serialPort.Parity       = (Parity)Enum.Parse(typeof(Parity), "None");
                serialPort.Handshake    = System.IO.Ports.Handshake.None;

                //serialPort.Open();   
                //serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(receive_data);
            }
            catch
            {
                System.Windows.MessageBox.Show(clLanguages.getName("__ComPortIsInvalid"));
            }
            finally
            {
                //serialPort.Close();
            }
            return serialPort; 
        }*/
        /*private void receiveSerialData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = (SerialPort)sender;

            string _sTest = serialPort.ReadExisting();
            serialPort.Close();
        }*/

        private void updatePCStep()
        {
            switch (staticProductionSettings.iPCProductionStep)
            {
                case 0:
                    if(!obcScannedMaterials.Any(material => material.xWrongMaterial == true))       // No material errors
                        staticProductionSettings.iPCProductionStep = 10;
                    break;
                case 10:
                    startProductProduction();
                    //staticProductionSettings.iPCProductionStep = 20;
                    break;
                case 20:

                    //staticProductionSettings.iPCProductionStep = 30;
                    break;
                case 30:

                    //staticProductionSettings.iPCProductionStep = 40;
                    break;
                case 40:

                    //staticProductionSettings.iPCProductionStep = 50;
                    break;
                case 50:

                    //staticProductionSettings.iPCProductionStep = 60;
                    break;
                case 60:

                    //staticProductionSettings.iPCProductionStep = 70;
                    break;
            }
        }
        private bool checkBit(int _argIntCheck, int _argIPosition)
        {
            return ((_argIntCheck >> _argIPosition) > 0);
        }
        private void doSomething(int _iTest)
        {
            
        }

        public void copyListBox()
        {
            // Empty list
            foreach (var x in clProductionSeamList5.obcItems.ToList())
            {
                clProductionSeamList5.obcItems.Remove(x);
            }

            int iMaxCopyPosition = 0;
            if (production.productlive.iActiveSeam == 0)    // Add two empty rows, when active seam is 0. 
            {
                clProductionSeamList5.obcItems.Add(new clSeam("", "", "", "", "", "", "", false, 0));
                clProductionSeamList5.obcItems.Add(new clSeam("", "", "", "", "", "", "", false, 1));
                iMaxCopyPosition = 2;
            }
            if (production.productlive.iActiveSeam == 1)    // Add one empty rows, when active seam is 1. 
            {
                clProductionSeamList5.obcItems.Add(new clSeam("", "", "", "", "", "", "", false, 1));
                iMaxCopyPosition = 1;
            }

            int iStartCopyPosition = 0;
            if (production.productlive.iActiveSeam >= 2)
            {
                iStartCopyPosition = production.productlive.iActiveSeam - 2;    // Start position in array to copy from. 
            }

            int iMaxProductsToCopy = 5 - iMaxCopyPosition;                      // Calculate max products to copy. 

            // Copy the first 5 seams starting from startPos.
            for (int i = iStartCopyPosition; i < (iStartCopyPosition + iMaxProductsToCopy); i++)
            {
                if (i < production.productsettings.obcSeams.Count())
                {
                    clProductionSeamList5.obcItems.Add(new clSeam((i + 1).ToString() + ",  " + production.productsettings.obcSeams[i].sName,
                                                                    production.productsettings.obcSeams[i].iMinStitch.ToString(),
                                                                    production.productsettings.obcSeams[i].iMaxStitch.ToString(),
                                                                    production.productsettings.obcSeams[i].iMinTens.ToString(),
                                                                    production.productsettings.obcSeams[i].iMaxTens.ToString(),
                                                                    production.productsettings.obcSeams[i].iActStitch.ToString(),
                                                                    production.productsettings.obcSeams[i].iStitchLength.ToString(),
                                                                    true,
                                                                    i + 2
                                                                    ));
                }
            }
        }
        public void updateSeamList()
        {
            copyListBox();
            clProductionSeamList5.obcItems[2].iListboxItemHeight = 100;
            clProductionSeamList5.obcItems[2].sStyleName = "style2";
            clProductionSeamList5.iSelectedValue = 2;        // Selected value is always 2. 

            clProductionSeamList5.obcItems[2].sStyleName = "style2"; // Index 2, Active seam

            if (clProductionSeamList5.obcItems.Count() >= 5) // Check, not always >= 5 items
            {
                clProductionSeamList5.obcItems[4].sStyleName = "style1";  //Index 4, fade in
            }

            // Check if startPos is lower than total amount of items - 1. This way the last item will stay at index 2.
            if (production.productlive.iActiveSeam < production.productsettings.obcSeams.Count() - 1)
            {
                production.productlive.iActiveSeam += 1;        // TODO, dit moet natuurlijk in de model straks
            }
        }
        public void updateStatusList()
        {
            /// om niet steeds de lijst te hoeven verwijderen gewoon updaten....
            foreach (var i in obcStatus.ToList())
            {
                // Write new value if xStartSewing is different
                if (i.iID == 0 && i.xActive != xStartSewing)
                {
                    i.xActive = xStartSewing;
                }

                // Write new value if xPedalBlocked is different
                if (i.iID == 1 && i.xActive != xPedalBlocked)
                {
                    i.xActive = xPedalBlocked;
                }
                // obcStatus.Remove(i);
            }

            //obcStatus.Add(new clStatus(0, xStartSewing, "/Resources/Images/commonImages/motor.png"));
            //obcStatus.Add(new clStatus(1, xPedalBlocked, "/Resources/Images/commonImages/pedal.png"));
           /*
            obcStatus.Add(new clStatus(2, xStartSewing, "/Resources/Images/commonImages/motor.png"));
            obcStatus.Add(new clStatus(3, xPedalBlocked, "/Resources/Images/commonImages/pedal.png"));
            obcStatus.Add(new clStatus(4, xStartSewing, "/Resources/Images/commonImages/motor.png"));
            obcStatus.Add(new clStatus(5, xPedalBlocked, "/Resources/Images/commonImages/pedal.png"));
            obcStatus.Add(new clStatus(6, xStartSewing, "/Resources/Images/commonImages/motor.png"));
            obcStatus.Add(new clStatus(7, xPedalBlocked, "/Resources/Images/commonImages/pedal.png"));
            obcStatus.Add(new clStatus(8, xStartSewing, "/Resources/Images/commonImages/motor.png"));
            obcStatus.Add(new clStatus(9, xPedalBlocked, "/Resources/Images/commonImages/pedal.png"));
            */
        }
        public void updateErrorList()
        {
            foreach (var i in obcErrors.ToList())
            {
                obcErrors.Remove(i);
            }

            obcErrors.Add(new clError(0, true, "Bobbin", "Spoeltje leeg gedetecteerd"));
            obcErrors.Add(new clError(1, true, "Seam Active", "Naad nog actief bij starten nieuw product"));
            obcErrors.Add(new clError(2, true, "Cover", "Schuifplaat niet in positie"));
            obcErrors.Add(new clError(3, false, "ETM -", "Spanning < min"));
            obcErrors.Add(new clError(4, true, "ETM +", "Spanning > max"));
            obcErrors.Add(new clError(5, true, "Comm DB", "Kan de lokale database niet bereiken"));
            obcErrors.Add(new clError(6, true, "Comm PLC", "Geen communicatie met PLC"));
        }
        public void drawTrend()
        {
            trend.drawTrend();
        }
        public void resetTrend()
        {
            trend.resetTrend();
            updateTrend.Stop();
            iActStitch = 0;
            xPedalBlocked = false;
            xScanbackDone = false;
            xLabelPrinted = false;
        }
        public void invertErrorActive(object sender, EventArgs e)
        {
            xErrorActive = !xErrorActive;
        }
        #endregion 
    }

    public class clTrend : ObservableObject
    {
        public clTrend()
        {
            obsTrendData = new ObservableDataSource<Point>();
            obsTrendAvg = new ObservableDataSource<Point>();

            dTrendMin = -1.0;
            dTrendMax = 1.0;
            
        }

        #region Private declarations
        private double _dTrendMin;
        private double _dTrendMax;
        private double _dTrendAvg;
        private double _dTrendAct;
        
        private int _iActStitch;
        private int _iMaxStitches;
        private int _iMinStitches;

        private ObservableCollection<double> _obcDouble = new ObservableCollection<double>();

        #endregion
        #region Public declarations
       
        public ObservableDataSource<Point> obsTrendData { get; set; }
        public ObservableDataSource<Point> obsTrendAvg { get; set; }

        public ObservableCollection<double> obcDouble
        {
            get
            {
                return _obcDouble;
            }
            set
            {
                _obcDouble = value;
                OnPropertyChanged("obcDouble");
            }
        }

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
        public int iMaxStitches
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
        public int iMinStitches
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
        public double dTrendMin
        {
            get
            {
                return _dTrendMin;
            }
            set
            {
                _dTrendMin = value;
                OnPropertyChanged("dTrendMin");
            }
        }
        public double dTrendMax
        {
            get
            {
                return _dTrendMax;
            }
            set
            {
                _dTrendMax = value;
                OnPropertyChanged("dTrendMax");
            }
        }
        public double dTrendAvg
        {
            get
            {
                return _dTrendAvg;
            }
            set
            {
                _dTrendAvg = value;
                OnPropertyChanged("dTrendAvg");
            }
        }
        public double dTrendAct
        {
            get
            {
                return _dTrendAct;
            }
            set
            {
                _dTrendAct = value;
                OnPropertyChanged("dTrendAct");
            }
        }
        #endregion
        #region Functions
        public void drawTrend()
        {
            if (iActStitch < iMaxStitches)
            {
                Random rnd = new Random();
                obcDouble.Add(rnd.NextDouble());
                obsTrendData.Collection.Add(new Point(iActStitch, obcDouble[iActStitch]));
                dTrendAvg = Math.Round(obcDouble.Average(), 4);
                dTrendAct = Math.Round(obcDouble[iActStitch], 4);
            }
        }

        public void resetTrend()
        {
            
            //_iTrendIndex = 0;
            dTrendAvg = 0.0;
           
            foreach (var i in obsTrendData.Collection.ToList())
            {
                obsTrendData.Collection.Remove(i);
            }
            foreach (var i in obsTrendAvg.Collection.ToList())
            {
                obsTrendAvg.Collection.Remove(i);
            }

            foreach (var i in obcDouble.ToList())
            {
                obcDouble.Remove(i);
            }

        }
        #endregion

    }

    public class clStatusBar : ObservableObject
    {
        public clStatusBar()
        {
            dHeightStatusbar = dStatusSliderHeight; // px


            sImagePathScanback = "/Resources/Images/commonImages/scanner.png";
            sImagePathLabel = "/Resources/Images/commonImages/Print.png";


            iStitchLabel = 20;
            iStitchScanback = 70;

            
            dStatusSliderHeight = 400;
        }

        #region Private declarations

        private ObservableCollection<double> _obcDouble = new ObservableCollection<double>();
        private ObservableCollection<double> _obcSliderTicks = new ObservableCollection<double>();
        private DoubleCollection _docSliderTicks = new DoubleCollection();
        
        private bool _xStatusLabelActive;
        private bool _xStatusScanbackActive;

        // vervangen voor data uit db
        private int _iActStitch;
        private int _iMaxStitches;
        private int _iMinStitches;
        private int _iBlindArea = 20;
        private int _iStitchLabel;
        private int _iStitchScanback;

        private double _dHeightStatusSeamBlindArea;
        private double _dHeightStatusSeamMinMax;
        private double _dHeightStatusbar;
        private double _dHeightSlider;
        private double _dHeightStatusbarScanback;
        private double _dHeightStatusbarLabel;
        private double _dStatusSliderHeight;
        private double _dStatusSliderWidth;

        private string _sImagePathLabel;
        private string _sImagePathScanback;

        private decimal _decSliderMaximum;
        #endregion

        #region Public declarations
        public ObservableCollection<double> obcSliderTicks
        {
            get
            {
                return _obcSliderTicks;
            }
            set
            {
                _obcSliderTicks = value;
                OnPropertyChanged("obcSliderTicks");
            }
        }
        public DoubleCollection docSliderTicks
        {
            get
            {
                return _docSliderTicks;
            }
            set
            {
                _docSliderTicks = value;
                OnPropertyChanged("docSliderTicks");
            }
        }
        public double dHeightStatusbar
        {
            get
            {
                return _dHeightStatusbar;
            }
            set
            {
                _dHeightStatusbar = value;
                OnPropertyChanged("dHeightStatusbar");
            }
        }
        public double dHeightStatusSeamBlindArea
        {
            get
            {
                return _dHeightStatusSeamBlindArea;
            }
            set
            {
                _dHeightStatusSeamBlindArea = value;
                OnPropertyChanged("dHeightStatusSeamBlindArea");
            }
        }
        public double dHeightStatusSeamMinMax
        {
            get
            {
                return _dHeightStatusSeamMinMax;
            }
            set
            {
                _dHeightStatusSeamMinMax = value;
                OnPropertyChanged("dHeightStatusSeamMinMax");
            }
        }
        public int iMaxStitches
        {
            get
            {
                return _iMaxStitches;
            }
            set
            {
                _iMaxStitches = value;
                decSliderMaximum = (decimal)value;
                fillSliderTicks(value, 10);
                OnPropertyChanged("iMaxStitches");
            }
        }
        public int iMinStitches
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
        public decimal decSliderMaximum
        {
            get
            {
                return _decSliderMaximum;
            }
            set
            {
                _decSliderMaximum = value;
                OnPropertyChanged("decSliderMaximum");
            }
        }
        public int iStitchLabel
        {
            get
            {
                return _iStitchLabel;
            }
            set
            {
                _iStitchLabel = value;
                OnPropertyChanged("iStitchLabel");

            }
        }
        public int iStitchScanback
        {
            get
            {
                return _iStitchScanback;
            }
            set
            {
                _iStitchScanback = value;
                OnPropertyChanged("iStitchScanback");

            }
        }
        public double dStatusSliderHeight
        {
            get { return _dStatusSliderHeight; }
            set
            {

                _dStatusSliderHeight        = value;
                dHeightStatusbar            = value - 60;
                dHeightSlider               = value - 20;
                dHeightStatusSeamBlindArea  = calcRowHeightStatusbar(iBlindArea);
                dHeightStatusSeamMinMax     = calcRowHeightStatusbar(iMaxStitches - iMinStitches);
                dHeightStatusbarLabel       = calcRowHeightStatusbar(iStitchLabel) + 25;
                dHeightStatusbarScanback    = calcRowHeightStatusbar(iStitchScanback) - calcRowHeightStatusbar(iStitchLabel);


                OnPropertyChanged("dStatusSliderHeight");
            }
        }
        public double dStatusSliderWidth
        {
            get { return _dStatusSliderWidth; }
            set
            {
                _dStatusSliderWidth = value;
                OnPropertyChanged("dStatusSliderWidth");
            }
        }
        public double dHeightStatusbarScanback
        {
            get
            {
                return _dHeightStatusbarScanback;
            }
            set
            {
                _dHeightStatusbarScanback = value;
                OnPropertyChanged("dHeightStatusbarScanback");
            }
        }
        public string sImagePathScanback
        {
            get
            {
                return _sImagePathScanback;
            }
            set
            {
                _sImagePathScanback = value;
                OnPropertyChanged("sImagePathScanback");
            }
        }
        public bool xStatusScanbackActive
        {
            get
            {
                return _xStatusScanbackActive;
            }
            set
            {
                _xStatusScanbackActive = value;
                OnPropertyChanged("xStatusScanbackActive");
            }
        }
        public double dHeightStatusbarLabel
        {
            get
            {
                return _dHeightStatusbarLabel;
            }
            set
            {
                _dHeightStatusbarLabel = value;
                OnPropertyChanged("dHeightStatusbarLabel");
            }
        }
        public double dHeightSlider
        {
            get
            {
                return _dHeightSlider;
            }
            set
            {
                _dHeightSlider = value;
            }
        }
        public string sImagePathLabel
        {
            get
            {
                return _sImagePathLabel;
            }
            set
            {
                _sImagePathLabel = value;
                OnPropertyChanged("sImagePathLabel");
            }
        }
        public bool xStatusLabelActive
        {
            get
            {
                return _xStatusLabelActive;
            }
            set
            {
                _xStatusLabelActive = value;
                OnPropertyChanged("xStatusLabelActive");
            }
        }
        #endregion

        #region Functions
        public double calcRowHeightStatusbar(int _iAmountOfStitches)
        {
            return ((dHeightStatusbar / iMaxStitches) * _iAmountOfStitches); // return number of pixels (int)statusSliderHeight
        }
        public void fillSliderTicks(int arg_iMaxTicks, int arg_iStepFrequency)
        {
            int iStepCounter = 0;

            foreach (var x in docSliderTicks.ToList())
            {
                docSliderTicks.Remove(x);
            }
            docSliderTicks.Add(0);
            for (double i = 1; i <= arg_iMaxTicks; i++)
            {
                iStepCounter++;
                if (iStepCounter == arg_iStepFrequency)
                {
                    docSliderTicks.Add(i);
                    iStepCounter = 0;
                }


            }
        }
        #endregion
    }
}





