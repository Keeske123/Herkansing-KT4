using System;
using System.Collections.ObjectModel;
using Sewco.Modules.ProjectsEditor;
using Sewco.Modules.ControlPanel;
using System.Linq;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Xml;

namespace Sewco.Resources.Helper_classes
{
    interface ifSewcoMenu
    {
        RelayCommand newObjectCommand { get; set; }
        RelayCommand saveObjectCommand { get; set; }
        RelayCommand cancelObjectCommand { get; set; }
        RelayCommand editObjectCommand { get; set; }
        RelayCommand deleteObjectCommand { get; set; }
        RelayCommand copyObjectCommand { get; set; }

        bool xEnableScreen { get; set; }
        bool xChangeObjectMode { get; set; }
        bool xNewObjectMode { get; set; }
        bool xEditObjectMode { get; set; }

        void newObject();
        void editObject();
        void deleteObject();
        void copyObject();
        void saveObject();
        void cancelObject();
    }

    static class clLogin
    {
        static public bool xUserLoggedIn;
        static public string sActiveUser;
        static public string sActiveProfile;
        static public string sUserCode;
        static public int iUserId = 0;
        static public int iUserLevel = 0;
        static public string sModuleName;
    }
    public class clHeader : ObservableObject
    {
        public clHeader()
        {
            LoginCommand = new RelayCommand(
                                param => login(),
                                param => (!xUserLoggedIn)
                            );
            LogoutCommand = new RelayCommand(
                                param => logout(),
                                param => (xUserLoggedIn)
                            );
        }
        public string sActiveUser
        {
            get
            {
                return clLogin.sActiveUser;
            }
            set
            {
                clLogin.sActiveUser = value;
                OnPropertyChanged("sActiveUser");
            }
        }
        public string sActiveProfile
        {
            get
            {
                return clLogin.sActiveProfile;
            }
            set
            {
                clLogin.sActiveProfile = value;
                OnPropertyChanged("sActiveProfile");
            }
        }

        public int iUserId
        {
            get
            {
                return clLogin.iUserId;
            }
            set
            {
                clLogin.iUserId = value;
                OnPropertyChanged("iUserId");
            }
        }
        public int iUserLevel
        {
            get
            {
                return clLogin.iUserLevel;
            }
            set
            {
                clLogin.iUserLevel = value;
                OnPropertyChanged("iUserLevel");
            }
        }
        public string sUserCode
        {
            get
            {
                return clLogin.sUserCode;
            }
            set
            {
                clLogin.sUserCode = value;

                     
                OnPropertyChanged("sUserCode");
            }
        }

        public string sModuleName
        {
            get
            {
                return clLogin.sModuleName;
            }
            set
            {
                clLogin.sModuleName = value;
                OnPropertyChanged("sModuleName");
            }
        }
        public bool xUserLoggedIn
        {
            get
            {
                return clLogin.xUserLoggedIn;
            }
            set
            {
                clLogin.xUserLoggedIn = value;
                OnPropertyChanged("xUserLoggedIn");
            }
        }
        public void login()
        {
            xUserLoggedIn = true;


            //if (clLogin.sUserCode == "123456")
            //{
            //    sActiveUser = "Grote baas";
            //    sActiveProfile = "Adminstrator";
            //    iUserId = 1;

            //}
            //else
            //{
            //    sActiveUser = "Angelina Fox";
            //    sActiveProfile = "Operator";
            //    iUserId = 0;
            //}
        }
        public void logout()
        {
            xUserLoggedIn = false;
        }
        public RelayCommand LoginCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }
    }

    public class clImage
    {
        public clImage()
        {
        }

        public const string sDefaultImage = "/Resources/Images/DefaultImage/No_image_selected.png";
      
        public string checkImagePath(string p_sImagepath)
        {
            string _sImagepath;
            if (!(p_sImagepath == "") && !(p_sImagepath == null))
            {
                if (!File.Exists(p_sImagepath))
                {
                    _sImagepath = sDefaultImage;
                }
                else
                {
                    _sImagepath = p_sImagepath;
                }
            }
            else
            {
                _sImagepath = sDefaultImage;
            }
            return _sImagepath;
        }

    }

    public class clcbbOptions : ObservableObject
    {
        public clcbbOptions()
        {
        }

        public ObservableCollection<clCbbFilltype1> obcItems { get; set; } = new ObservableCollection<clCbbFilltype1>();

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

    public class clcbbProjectOptions : clCbbFilter //, ObservableObject
    {
        private int _iCbbId;
        //private ObservableCollection<clCbbFilltype1> _obcAvailableDeviceTypes;
        private bool _xFilterActive;
        public clcbbProjectOptions(int p_iCbbId, bool p_xFilterActive) //ObservableCollection<clCbbFilltype1> p_obcAvailableDeviceTypes,
        {
            _iCbbId = p_iCbbId;
            // _obcAvailableDeviceTypes    = p_obcAvailableDeviceTypes;
            _xFilterActive = p_xFilterActive;
        }

        public ObservableCollection<clCbbFilltype1> obcItems { get; set; } = new ObservableCollection<clCbbFilltype1>();

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
                if (_xFilterActive && (_iSelectedValue != value))
                {
                    addSelectedValueToList(_iCbbId, _iSelectedValue);
                    ProjectsBusinessObject.usedDeviceTypelist[_iCbbId] = _iSelectedValue;
                    //addSelectedValueToList(_iCbbId, _iSelectedValue);
                    //xSelectedItemChanged = true;
                    //cbbFilter(_iCbbId, _obcAvailableDeviceTypes);
                    //obcItems = getObcFilteredDevices();
                }
                if (_iSelectedValue == 0 && xActive)
                {
                    iBorderThickness = 2;
                }
                else
                {
                    iBorderThickness = 0;
                }
                OnPropertyChanged("iSelectedValue");
            }

        }

        private bool _xSelectedItemChanged;
        public bool xSelectedItemChanged
        {
            get
            {
                return _xSelectedItemChanged;
            }
            set
            {
                _xSelectedItemChanged = value;
                //ProjectsBusinessObject.selectedValueChanged = value;
                if (_xSelectedItemChanged)
                {

                    //_xSelectedItemChanged = false;
                    //ProjectsBusinessObject.selectedValueChanged = false;
                }
                //obcItems = cbbFilter(_iCbbId, _obcAvailableDeviceTypes);
                OnPropertyChanged("xSelectedItemChanged");
            }
        }

        private bool _xActive;
        public bool xActive
        {
            get
            {
                return _xActive;
            }
            set
            {
                _xActive = value;

                if (_xActive)
                {
                    if (iSelectedValue == 0)
                    {
                        iBorderThickness = 2;
                    }
                    else
                    {
                        iBorderThickness = 0;
                    }
                }
                else
                {
                    iSelectedValue = 0;
                    iBorderThickness = 0;
                }

                OnPropertyChanged("xActive");
            }
        }

        private int _iBorderThickness;
        public int iBorderThickness
        {
            get
            {
                return _iBorderThickness;
            }
            set
            {
                _iBorderThickness = value;
                OnPropertyChanged("iBorderThickness");
            }
        }
    }
    public class clcbbProjectOptions2 : clCbbFilter
    {
        private int[] _ariUsedDeviceTypes = new int[10];

        private int _iCbbId1, _iCbbId2;
        //private ObservableCollection<clCbbFilltype1> _obcAvailableDeviceTypes;
        private bool _xFilterActive;
        public clcbbProjectOptions2(int p_iCbbId1, int p_iCbbId2, bool p_xFilterActive) //ObservableCollection<clCbbFilltype1> p_obcAvailableDeviceTypes,
        {
            _iCbbId1 = p_iCbbId1;
            _iCbbId2 = p_iCbbId2;
            // _obcAvailableDeviceTypes = p_obcAvailableDeviceTypes;
            _xFilterActive = p_xFilterActive;
        }

        public ObservableCollection<clCbbFilltype1> obcItems1 { get; set; } = new ObservableCollection<clCbbFilltype1>();
        public ObservableCollection<clCbbFilltype1> obcItems2 { get; set; } = new ObservableCollection<clCbbFilltype1>();

        private bool _xSelectedItemChanged;
        public bool xSelectedItemChanged
        {
            get
            {
                return _xSelectedItemChanged;
            }
            set
            {
                _xSelectedItemChanged = value;
                //ProjectsBusinessObject.selectedValueChanged = value;
                if (_xSelectedItemChanged)
                {
                    _xSelectedItemChanged = false;
                    //ProjectsBusinessObject.selectedValueChanged = false;
                }
                ///obcItems = cbbFilter(_iCbbId, _obcAvailableDeviceTypes);
                OnPropertyChanged("xSelectedItemChanged");
            }
        }

        private int _iSelectedValueItem1;
        public int iSelectedValueItem1
        {
            get
            {
                return _iSelectedValueItem1;
            }
            set
            {
                _iSelectedValueItem1 = value;
                if (_xFilterActive && (_iSelectedValueItem1 != value))
                {
                    addSelectedValueToList(_iCbbId1, _iSelectedValueItem1);
                    //addSelectedValueToList(_iCbbId1, _iSelectedValueItem1);
                    // cbbFilter(_iCbbId1, _obcAvailableDeviceTypes);
                    // obcItems1 = getObcFilteredDevices();
                    // xSelectedItemChanged = true;
                }
                if (_iSelectedValueItem1 == 0 && xActive)
                {
                    iBorderThicknessItem1 = 2;
                }
                else
                {
                    iBorderThicknessItem1 = 0;
                }
                OnPropertyChanged("iSelectedValueItem1");
            }
        }



        private int _iSelectedValueItem2;
        public int iSelectedValueItem2
        {
            get
            {
                return _iSelectedValueItem2;
            }
            set
            {
                _iSelectedValueItem2 = value;
                if (_xFilterActive && (_iSelectedValueItem2 != value))
                {
                    addSelectedValueToList(_iCbbId2, _iSelectedValueItem2);
                    ProjectsBusinessObject.usedDeviceTypelist[_iCbbId2] = _iSelectedValueItem2;

                    //cbbFilter(_iCbbId2, _obcAvailableDeviceTypes);
                    //obcItems2 = getObcFilteredDevices();
                    //xSelectedItemChanged = true;
                }

                if (_iSelectedValueItem2 == 0 && xActive)
                {
                    iBorderThicknessItem2 = 2;
                }
                else
                {
                    iBorderThicknessItem2 = 0;
                }
                OnPropertyChanged("iSelectedValueItem2");
            }
        }

        private bool _xActive;
        public bool xActive
        {
            get
            {
                return _xActive;
            }
            set
            {
                _xActive = value;

                if (_xActive)
                {
                    if (_iSelectedValueItem1 == 0)
                    {
                        iBorderThicknessItem1 = 2;
                    }
                    else
                    {
                        iBorderThicknessItem1 = 0;
                    }
                }
                else
                {
                    iSelectedValueItem1 = 0;
                    iSelectedValueItem2 = 0;
                    iBorderThicknessItem1 = 0;
                }

                OnPropertyChanged("xActive");
            }
        }

        private int _iBorderThicknessItem1;
        public int iBorderThicknessItem1
        {
            get
            {
                return _iBorderThicknessItem1;
            }
            set
            {
                _iBorderThicknessItem1 = value;
                OnPropertyChanged("iBorderThicknessItem1");
            }
        }

        private int _iBorderThicknessItem2;
        public int iBorderThicknessItem2
        {
            get
            {
                return _iBorderThicknessItem2;
            }
            set
            {
                _iBorderThicknessItem2 = value;
                OnPropertyChanged("iBorderThicknessItem2");
            }
        }
        /*
        public ObservableCollection<clCbbFilltype1> cbbFilter(int cbbNumber)
        {
            ObservableCollection<clCbbFilltype1> obcFilteredDeviceTypeOptions = new ObservableCollection<clCbbFilltype1>();

            // Fill combobox
            foreach (var q in getAvailableDeviceTypes())
            //foreach (var q in deviceTypeQuery)
            {
                //obcFilteredDeviceTypeOptions.Add(new clCbbFilltype1(q.deviceName, q.deviceTypeId));
                obcFilteredDeviceTypeOptions.Add(new clCbbFilltype1(q.sDisplayName, q.iSelectedvaluePath));
            }

            // Filter combobox with unused devicetypes and selected.
            foreach (var selectedValue in obcFilteredDeviceTypeOptions.ToList())
            {
                foreach (var usedSelectedValue in ariDeviceTypeUsed)
                {
                    // Remove all in array except selected value of this combobox.
                    if (((int)usedSelectedValue != ariDeviceTypeUsed[cbbNumber]) &&     // if selected value not selected by this combobox
                        (selectedValue.iSelectedvaluePath != 0) &&                      // if selected value not equal to '0' (not used), always visible
                        (selectedValue.iSelectedvaluePath == (int)usedSelectedValue))   // if selected value is equal to value in used values list
                    {
                        // Remove devicetype because it's selected in another combobox.
                        obcFilteredDeviceTypeOptions.Remove(selectedValue);
                    }
                }
            }
            return obcFilteredDeviceTypeOptions;
        }*/
    }

    /*
    public class clcbbDeviceTypes : ObservableObject
    {
        public clcbbDeviceTypes()
        {
        }

        public ObservableCollection<clCbbFilltype1> obcItems1 { get; set; } = new ObservableCollection<clCbbFilltype1>();
        public ObservableCollection<clCbbFilltype1> obcItems2 { get; set; } = new ObservableCollection<clCbbFilltype1>();

        public int[] ariDeviceTypeUsed = new int[10];
        public void fillSelectedValuesCombobox(int _selectedValue_scanback, int _selectedValue_bobbin1, int _selectedValue_bobbin2, Array _selectedValues_IOPos)
        {

            if (_selectedValue_scanback != 0)
            {
                ariDeviceTypeUsed[0] = _selectedValue_scanback;
            }
            if (_selectedValue_bobbin1 != 0)
            {
                ariDeviceTypeUsed[1] = _selectedValue_bobbin1;
            }
            if (_selectedValue_bobbin2 != 0)
            {
                ariDeviceTypeUsed[2] = _selectedValue_bobbin2;
            }

            int i = 0;
            foreach (int IOPos in _selectedValues_IOPos)
            {
                // Max array size
                if (i < ariDeviceTypeUsed.Length)
                {
                    if (IOPos != 0)
                    {
                        ariDeviceTypeUsed[3 + i] = IOPos;
                    }
                    i++;
                }
            }
        }

        public ObservableCollection<clCbbFilltype1> cbbFilter(int cbbNumber, int _iSelectedValue)
        {
            // fillSelectedValuesCombobox();

            ObservableCollection<clCbbFilltype1> obcFilteredDeviceTypeOptions = new ObservableCollection<clCbbFilltype1>();

            // Fill combobox
            foreach (var q in availableDeviceTypes)
            {
                //obcFilteredDeviceTypeOptions.Add(new clCbbFilltype1(q.deviceName, q.deviceTypeId));
                obcFilteredDeviceTypeOptions.Add(new clCbbFilltype1(q.sDisplayName, q.iSelectedvaluePath));
            }

            // Filter combobox with unused devicetypes and selected.
            foreach (var selectedValue in obcFilteredDeviceTypeOptions)
            {
                foreach (var usedSelectedValue in ariDeviceTypeUsed)
                {
                    // Remove all in array except selected value of this combobox.
                    if (((int)usedSelectedValue != ariDeviceTypeUsed[cbbNumber]) &&     // if selected value not selected by this combobox
                        (selectedValue.iSelectedvaluePath != 0) &&                      // if selected value not equal to '0' (not used), always visible
                        (selectedValue.iSelectedvaluePath == (int)usedSelectedValue))   // if selected value is equal to value in used values list
                    {
                        // Remove devicetype because it's selected in another combobox.
                        obcFilteredDeviceTypeOptions.Remove(selectedValue);
                    }
                }
            }
            return 
        }


        private int _iSelectedValueItem1;
        public int iSelectedValueItem1
        {
            get
            {
                return _iSelectedValueItem1;
            }
            set
            {
                _iSelectedValueItem1 = value;

                fillSelectedValuesCombobox();
                cbbFilter();

                if (_iSelectedValueItem1 == 0 && xActive)
                {
                    iBorderThicknessItem1 = 2;
                }
                else
                {
                    iBorderThicknessItem1 = 0;
                }
                OnPropertyChanged("iSelectedValueItem1");
            }
        }

        private int _iSelectedValueItem2;
        public int iSelectedValueItem2
        {
            get
            {
                return _iSelectedValueItem2;
            }
            set
            {
                _iSelectedValueItem2 = value;
                if (_iSelectedValueItem2 == 0 && xActive)
                {
                    iBorderThicknessItem2 = 2;
                }
                else
                {
                    iBorderThicknessItem2 = 0;
                }
                OnPropertyChanged("iSelectedValueItem2");
            }
        }

        private bool _xActive;
        public bool xActive
        {
            get
            {
                return _xActive;
            }
            set
            {
                _xActive = value;

                if (_xActive)
                {
                    if (_iSelectedValueItem1 == 0)
                    {
                        iBorderThicknessItem1 = 2;
                    }
                    else
                    {
                        iBorderThicknessItem1 = 0;
                    }
                }
                else
                {
                    iSelectedValueItem1 = 0;
                    iSelectedValueItem2 = 0;
                    iBorderThicknessItem1 = 0;
                }

                OnPropertyChanged("xActive");
            }
        }

        private int _iBorderThicknessItem1;
        public int iBorderThicknessItem1
        {
            get
            {
                return _iBorderThicknessItem1;
            }
            set
            {
                _iBorderThicknessItem1 = value;
                OnPropertyChanged("iBorderThicknessItem1");
            }
        }

        private int _iBorderThicknessItem2;
        public int iBorderThicknessItem2
        {
            get
            {
                return _iBorderThicknessItem2;
            }
            set
            {
                _iBorderThicknessItem2 = value;
                OnPropertyChanged("iBorderThicknessItem2");
            }
        }
    }
    */
    public class clCbbFilltype1 : ObservableObject
    {
        public clCbbFilltype1(string _argsName, int _argiId)
        {
            this.sDisplayName = _argsName;
            this.iSelectedvaluePath = _argiId;
        }
        private string _sDisplayName;
        private int _iSelectedvaluePath;
        private bool _xSeamNotEmpty;

        public string sDisplayName
        {
            get
            {
                return _sDisplayName;
            }
            set
            {
                _sDisplayName = value;
                OnPropertyChanged("sDisplayName");
            }
        }
        public int iSelectedvaluePath
        {
            get
            {
                return _iSelectedvaluePath;
            }
            set
            {
                _iSelectedvaluePath = value;
                OnPropertyChanged("iSelectedvaluePath");
            }
        }

        private bool _xSelectionChanged;
        public bool xSelectionChanged
        {
            get
            {
                return _xSelectionChanged;
            }
            set
            {
                _xSelectionChanged = value;
                OnPropertyChanged("xSelectionChanged");
            }
        }
        public bool xSeamNotEmpty
        {
            get
            {
                return _xSeamNotEmpty;
            }
            set
            {
                _xSeamNotEmpty = value;
                OnPropertyChanged("xSeamNotEmpty");
            }
        }
    }


    /*
     * 
     * 
     * 
        public interface ITest1 { }
        public interface ITest2 { }

        public class Test1 : ITest1
        { }

        public class Test2 : ITest2
        { }

        public class clCbbFilter : ITest1, ITest2
        {

        }


    */

    public class clCbbFilter : ObservableObject
    {

        public clCbbFilter() { }

        private static int[] ariDeviceTypeUsed = new int[10];

        public void addSelectedValueToList(int iCbbId, int selectedValue)
        {
            ariDeviceTypeUsed[iCbbId] = selectedValue;
        }
        /*
        public void fillSelectedValuesCombobox(int _selectedValue_scanback, int _selectedValue_bobbin1, int _selectedValue_bobbin2) // , Array  _selectedValues_IOPos
        {
            if (_selectedValue_scanback != 0)
            {
                ariDeviceTypeUsed[0] = _selectedValue_scanback;
            }
            if (_selectedValue_bobbin1 != 0)
            {
                ariDeviceTypeUsed[1] = _selectedValue_bobbin1;
            }
            if (_selectedValue_bobbin2 != 0)
            {
                ariDeviceTypeUsed[2] = _selectedValue_bobbin2;
            }
            
            int i = 0;
            foreach (int IOPos in _selectedValues_IOPos)
            {
                // Max array size
                if (i < ariDeviceTypeUsed.Length)
                {
                    if (IOPos != 0)
                    {
                        ariDeviceTypeUsed[3 + i] = IOPos;
                    }
                    i++;
                }
            }
        }*/

        public ObservableCollection<clCbbFilltype1> getObcFilteredDevices()
        {
            return this.obcFilteredDeviceTypeOptions;

        }

        public ObservableCollection<clCbbFilltype1> obcFilteredDeviceTypeOptions { get; set; } = new ObservableCollection<clCbbFilltype1>();
        public void cbbFilter(int p_iCbbId, ObservableCollection<clCbbFilltype1> p_availableDeviceTypes)
        {
            foreach (var f in obcFilteredDeviceTypeOptions.ToList())
            {
                obcFilteredDeviceTypeOptions.Remove(f);
            }

            // Fill combobox
            foreach (var q in p_availableDeviceTypes)
            {
                //obcFilteredDeviceTypeOptions.Add(new clCbbFilltype1(q.deviceName, q.deviceTypeId));
                obcFilteredDeviceTypeOptions.Add(new clCbbFilltype1(q.sDisplayName, q.iSelectedvaluePath));
            }

            if (obcFilteredDeviceTypeOptions != null)
            {
                // Filter combobox with unused devicetypes and selected.
                foreach (var selectedValue in obcFilteredDeviceTypeOptions.ToList())
                {
                    foreach (var usedSelectedValue in ariDeviceTypeUsed)
                    {
                        // Remove all in array except selected value of this combobox.
                        if (((int)usedSelectedValue != ariDeviceTypeUsed[p_iCbbId]) &&     // if selected value not selected by this combobox
                            (selectedValue.iSelectedvaluePath != 0) &&                      // if selected value not equal to '0' (not used), always visible
                            (selectedValue.iSelectedvaluePath == (int)usedSelectedValue))   // if selected value is equal to value in used values list
                        {
                            // Remove devicetype because it's selected in another combobox.
                            obcFilteredDeviceTypeOptions.Remove(selectedValue);
                        }
                    }
                }
            }

        }
    }

    public static class clConstants
    {
        public const int EMPTYCHECK     = 0;
        public const int INTCHECK       = 1;
        public const int STRINGCHECK    = 2;
        public const int MINLENGTHCHECK = 3;
        public const int MAXLENGTHCHECK = 4;
        public const int MINVALCHECK    = 5;
        public const int MAXVALCHECK    = 6;

        public static readonly string[] ProductionMode = { "__Single", "__Batch", "__Unlimited"};
        public const int PRODUCTION_SINGLE_MODE     = 0;
        public const int PRODUCTION_BATCH_MODE      = 1;
        public const int PRODUCTION_UNLIMITED_MODE  = 2;

        public static readonly string[] ResetOptions = { "__Daily", "__Weekly", "__NoReset" };
        public const int RESET_DAILY    = 0;
        public const int RESET_WEEKLY   = 1;
        public const int NO_RESET       = 2;

        public static readonly string[] ProductionCode = { "__Ford", "__Volvo"};
        public const int FORD   = 0;
        public const int VOLVO  = 1;

        public const int ERROR_ETMPlus          = 0;
        public const int ERROR_ETMMin           = 1;
        public const int ERROR_SEAMActive       = 2;
        public const int ERROR_STITCHESPlus     = 3;
        public const int ERROR_STITCHESMin      = 4;
        public const int ERROR_COMMPc           = 5;
        public const int ERROR_COMMPlc          = 6;
        public const int ERROR_COMMRs232        = 7;
        public const int ERROR_COMMDb           = 8;

    }

    public static class Def
    {
        public static bool getBit(int _argiValue, int _argiBitNumber)
        {
            return (((_argiValue >> _argiBitNumber) & 1) != 0);
        }
        public static int getBit(int _argiValue, int _argiStartBitNumber, int _argiMaxValue = 3)
        {
            return ((_argiValue >> _argiStartBitNumber) & _argiMaxValue);
        }

        /// <summary>
        /// Set/resets one bit within an int
        /// </summary>
        /// <param name="_argiValue">The int where the bit must be set</param>
        /// <param name="_argiBitNumber">Position of bit within int. First position = 0</param>
        /// <param name="_argxSetValue">The new value</param>
        /// <returns></returns>
        public static int setBit(int _argiValue, int _argiBitNumber, bool _argxSetValue)
        {
            int iTemp = 0;
            if (_argxSetValue)
                iTemp = 1;
            return _argiValue ^ (-iTemp ^ _argiValue) & (1 << _argiBitNumber);
        }

        /// <summary>
        /// Set/Resets specific bits within an int. 
        /// </summary>
        /// <param name="_argiValue">The int where the bit must be set </param>
        /// <param name="_argiStartBitNumber">Start position of bit within int. First position = 0</param>
        /// <param name="_argiSetValue">The new value</param>
        /// <param name="_argiMaxValue">Maximum value when all bits are one. (2 bits = 3, 3 bits = 7, 4 bits = 15)</param>
        /// <returns></returns>
        public static int setBit(int _argiValue, int _argiStartBitNumber, int _argiSetValue, int _argiMaxValue = 3)     // TODO: bit 15. Deze kan negatief zijn, checken of het dan nog steeds werkt. 
        {
            int iZeroMask   = _argiMaxValue << _argiStartBitNumber;     // Example: 3 << 1 = 6 => 0b0000 0110
            iZeroMask       =  ~iZeroMask;                              // ~6 = 249 => 0b1111.1001

            int iValueTemp  = _argiValue & iZeroMask;                   // Example: 58 & 249 => 0b0011.1010 & 1111.1001 = 0b0011.1000. (Current bits are reset)

            int iTemp2      = (_argiSetValue << _argiStartBitNumber);   // Example: 3 << 1 = 6 => 0b0000 0110

            return iValueTemp | iTemp2;                                 // 0b0011.1000 | 0b0000.0110 = 0b0011.1110 
        }



    }

    /*
    public class clCbbDeviceTypes 
    {
        //private ObservableCollection<clCbbFilltype1> _availableDeviceTypes;
        public clCbbDeviceTypes()
        {
           // _availableDeviceTypes = availableDeviceTypes;
        }

        private int[] ariDeviceTypeUsed = new int[10];
        public clcbbProjectOptions     clOption1Scanback          { get; set; } = new clcbbProjectOptions();
        public clcbbProjectOptions     clOption2VWcode            { get; set; } = new clcbbProjectOptions();
        public clcbbProjectOptions2    clOption3Bobbin            { get; set; } = new clcbbProjectOptions2();
        public ObservableCollection<clIOPosition> obcIOPositions  { get; set; } = new ObservableCollection<clIOPosition>();
       
        public void fillSelectedValuesCombobox(int _selectedValue_scanback, int _selectedValue_bobbin1, int _selectedValue_bobbin2) // , Array  _selectedValues_IOPos
        {
            if (_selectedValue_scanback != 0)
            {
                ariDeviceTypeUsed[0] = _selectedValue_scanback;
            }
            if (_selectedValue_bobbin1 != 0)
            {
                ariDeviceTypeUsed[1] = _selectedValue_bobbin1;
            }
            if (_selectedValue_bobbin2 != 0)
            {
                ariDeviceTypeUsed[2] = _selectedValue_bobbin2;
            }
            /*
            int i = 0;
            foreach (int IOPos in _selectedValues_IOPos)
            {
                // Max array size
                if (i < ariDeviceTypeUsed.Length)
                {
                    if (IOPos != 0)
                    {
                        ariDeviceTypeUsed[3 + i] = IOPos;
                    }
                    i++;
                }
            }
        }

        public ObservableCollection<clCbbFilltype1> cbbFilter(int cbbNumber, int _selectedValue_scanback, int _selectedValue_bobbin1, int _selectedValue_bobbin2, ObservableCollection<clCbbFilltype1> availableDeviceTypes)
        {
            fillSelectedValuesCombobox( _selectedValue_scanback, _selectedValue_bobbin1, _selectedValue_bobbin2);
            ObservableCollection<clCbbFilltype1> obcFilteredDeviceTypeOptions = new ObservableCollection<clCbbFilltype1>();

            // Fill combobox
            foreach (var q in availableDeviceTypes)
            {
                //obcFilteredDeviceTypeOptions.Add(new clCbbFilltype1(q.deviceName, q.deviceTypeId));
                obcFilteredDeviceTypeOptions.Add(new clCbbFilltype1(q.sDisplayName, q.iSelectedvaluePath));
            }

            if (obcFilteredDeviceTypeOptions != null)
            {
                // Filter combobox with unused devicetypes and selected.
                foreach (var selectedValue in obcFilteredDeviceTypeOptions.ToList())
                {
                    foreach (var usedSelectedValue in ariDeviceTypeUsed)
                    {
                        // Remove all in array except selected value of this combobox.
                        if (((int)usedSelectedValue != ariDeviceTypeUsed[cbbNumber]) &&     // if selected value not selected by this combobox
                            (selectedValue.iSelectedvaluePath != 0) &&                      // if selected value not equal to '0' (not used), always visible
                            (selectedValue.iSelectedvaluePath == (int)usedSelectedValue))   // if selected value is equal to value in used values list
                        {
                            // Remove devicetype because it's selected in another combobox.
                            obcFilteredDeviceTypeOptions.Remove(selectedValue);
                        }
                    }
                }
            }
            return obcFilteredDeviceTypeOptions;
        }
    }*/


    public class projectsConnectLocalDB
    {
        List<Object> liReturnObject;
        Project obProject;
        public List<Object> selectWhere(string sTableName, int iProjectId, string sFilter)
        {
            
            try
            {
                liReturnObject = new List<Object>();

                var query = from x in ViewModelControlPanel.DBDataClass.Projects
                            where x.projectName.Contains(sFilter) || x.comment.Contains(sFilter)
                            orderby x.projectId ascending
                            select x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());

                liReturnObject = null;
            }
            return liReturnObject;
        }
        public List<Object> select(string sFilter)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from x in ViewModelControlPanel.DBDataClass.Projects
                            where (x.comment.Contains(sFilter) || x.projectName.Contains(sFilter))
                            orderby x.projectId ascending
                            select x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
        public Project selectSingle(int iProjectId)
        {
            try
            {
                obProject = new Project();
                obProject = ViewModelControlPanel.DBDataClass.Projects.SingleOrDefault(c => c.projectId == iProjectId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                obProject = null;
            }
            return obProject;
        }
    }
    public class productsConnectLocalDB
    {
        List<Object> liReturnObject;
        public List<Object> selectWhere(string sTableName, int iProjectId, string sFilter)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from x in ViewModelControlPanel.DBDataClass.Products
                            where x.projectId == iProjectId && (x.code.Contains(sFilter) || x.name.Contains(sFilter))
                            orderby x.projectId ascending
                            select x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
        public List<Object> select(string sFilter)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from    x in ViewModelControlPanel.DBDataClass.Products
                            where   (x.code.Contains(sFilter) || x.name.Contains(sFilter))
                            orderby x.projectId ascending
                            select  x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
        public Product selectProduct(int iProductId)
        {
            //Product returnProduct;
            try
            {
                return ViewModelControlPanel.DBDataClass.Products.SingleOrDefault(c => c.productId == iProductId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                return null;
            }
        }
        public Product selectProduct(string sCode)
        {
            try
            {
                return ViewModelControlPanel.DBDataClass.Products.SingleOrDefault(c => c.code == sCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                return null;
            }
        }

        public int insertAndSubmit(Product newProduct)
        {
            try
            {
                ViewModelControlPanel.DBDataClass.Products.InsertOnSubmit(newProduct);
                ViewModelControlPanel.DBDataClass.SubmitChanges();
                return newProduct.productId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                return -1;
            }

            
        }
        public int editInsertAndSubmit(Product editedProduct)
        {
            try
            {
                Product newEditedProduct = ViewModelControlPanel.DBDataClass.Products.Single(c => c.productId == editedProduct.productId);

                newEditedProduct = editedProduct;
                ViewModelControlPanel.DBDataClass.SubmitChanges();
                return newEditedProduct.productId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
                return -1;
            }
        }
        public void deleteWhereProductId(int iProductId)
        {
            try
            {
                Product deleteProduct = ViewModelControlPanel.DBDataClass.Products.Single(c => c.productId == iProductId);
                ViewModelControlPanel.DBDataClass.Products.DeleteOnSubmit(deleteProduct);

                ViewModelControlPanel.DBDataClass.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
            }
        }

    }
    public class profilesConnectLocalDB
    {
        List<Object> liReturnObject;
        profile Profile;
        public profile selectWhere(int iProfileId)
        {
            try
            {
                Profile = new profile();
                Profile = ViewModelControlPanel.DBDataClass.profiles.SingleOrDefault(c => c.profileId == iProfileId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                Profile = null;
            }
            return Profile;
        }
        public List<Object> select(string sFilter)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from x in ViewModelControlPanel.DBDataClass.profiles
                            where (x.comment.Contains(sFilter) || x.name.Contains(sFilter))
                            orderby x.profileId ascending
                            select x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
    }
    public class seamTemplateConnectLocalDB
    {
        seamTemplate SeamTemplate;
        public seamTemplate selectWhere(int iSeamTemplateId)
        {
            try
            {
                SeamTemplate = new seamTemplate();
                SeamTemplate = ViewModelControlPanel.DBDataClass.seamTemplates.SingleOrDefault(c => c.Id == iSeamTemplateId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                SeamTemplate = null;
            }
            return SeamTemplate;
        }
    }
    public class materialIOPositionConnectLocalDB
    {
        List<Object> liReturnObject;
        Material_IOPosition obMaterial_IOPosition;
        public List<Object> selectWhere(int iProjectId)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from    x in ViewModelControlPanel.DBDataClass.Material_IOPositions
                            where   x.projectId == iProjectId
                            orderby x.projectId ascending, x.materialIOPosition ascending
                            select  x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
        public List<Object> select(string sFilter)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from    x in ViewModelControlPanel.DBDataClass.profiles
                            where   (x.comment.Contains(sFilter) || x.name.Contains(sFilter))
                            orderby x.profileId ascending
                            select  x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
        public Material_IOPosition selectSingle(int iProjectId, int iMaterialIOPosition)
        {
            try
            {
                obMaterial_IOPosition = new Material_IOPosition();
                obMaterial_IOPosition = ViewModelControlPanel.DBDataClass.Material_IOPositions.SingleOrDefault(c => c.projectId == iProjectId && c.materialIOPosition == iMaterialIOPosition);
            }
            catch (Exception ex)
            {
				MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                obMaterial_IOPosition = null;
            }
            return obMaterial_IOPosition;
        }
    }
    public class materialProductConnectLocalDB
    {
        List<Object> liReturnObject;
        public List<Object> selectWhere(int iProductId)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from x in ViewModelControlPanel.DBDataClass.MaterialProducts
                            where x.productId == iProductId
                            orderby x.productId ascending, x.materialIOPosition ascending
                            select x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
        public List<Object> selectWhereProductMaterialIOPosition(int iProductId, int iMaterialIOPosition)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from x in ViewModelControlPanel.DBDataClass.MaterialProducts
                            where x.productId == iProductId && x.materialIOPosition == iMaterialIOPosition
                            orderby x.productId ascending, x.materialIOPosition ascending
                            select x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
        public List<Object> select(string sFilter)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from    x in ViewModelControlPanel.DBDataClass.profiles
                            where   (x.comment.Contains(sFilter) || x.name.Contains(sFilter))
                            orderby x.profileId ascending
                            select  x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
        public void insertAndSubmit(MaterialProduct newMaterialProduct)
        {
            try
            {
                ViewModelControlPanel.DBDataClass.MaterialProducts.InsertOnSubmit(newMaterialProduct);
                submit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
            }
        }
        public void insert(MaterialProduct newMaterialProduct)
        {
            try
            {
                ViewModelControlPanel.DBDataClass.MaterialProducts.InsertOnSubmit(newMaterialProduct);
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
            }
        }
        public void submit()
        {
            try
            {
                ViewModelControlPanel.DBDataClass.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
            }
        }
        public void deleteWhereProductId(int iProductId)        // Delete product. It will automaticly delete foreign keys (delete on casade in database)
        {
            try
            {
                List<Object> MaterialProductsQuery = selectWhere(iProductId);

                foreach (MaterialProduct material in MaterialProductsQuery)
                {
                    ViewModelControlPanel.DBDataClass.MaterialProducts.DeleteOnSubmit(material);
                }
                submit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
            }
        }
    }
    public class materialTypeConnectLocalDB
    {
        List<Object>    liReturnObject;
        MaterialType    obMaterialType;
        public MaterialType selectWhere(int iMaterialTypeId)
        {
            try
            {
                obMaterialType = new MaterialType();
                obMaterialType = ViewModelControlPanel.DBDataClass.MaterialTypes.SingleOrDefault(c => c.materialTypeId == iMaterialTypeId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                obMaterialType = null;
            }
            return obMaterialType;
        }
        public List<Object> select()
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from    x in ViewModelControlPanel.DBDataClass.MaterialTypes
                            orderby x.materialTypeId ascending
                            select  x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
    }
    public class materialTypeMaterialConnectLocalDB
    {
        List<Object> liReturnObject;
        public List<Object> selectWhereMaterialId(int iMaterialId)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from    x in ViewModelControlPanel.DBDataClass.MaterialTypeMaterials
                            where   x.materialId == iMaterialId
                            orderby x.materialId ascending
                            select  x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
        public List<Object> selectWhereMaterialTypeId(int iMaterialTypeId)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from    x in ViewModelControlPanel.DBDataClass.MaterialTypeMaterials
                            where   x.materialTypeId == iMaterialTypeId
                            orderby x.materialId ascending
                            select  x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
        public List<Object> select()
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from    x in ViewModelControlPanel.DBDataClass.MaterialTypeMaterials
                            orderby x.materialId ascending
                            select  x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
    }
    public class materialConnectLocalDB
    {
        List<Object> liReturnObject;
        Material    obMaterial;
        public Material selectWhere(string sTableName, int iMaterialId, string sFilter)
        {
            try
            {
                obMaterial = new Material();
                obMaterial = ViewModelControlPanel.DBDataClass.Materials.SingleOrDefault(c => c.materialId == iMaterialId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                obMaterial = null;
            }
            return obMaterial;
        }
        public List<Object> select(string sFilter)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from x in ViewModelControlPanel.DBDataClass.Materials
                            where (x.code.Contains(sFilter) || x.name.Contains(sFilter))
                            orderby x.materialId ascending
                            select x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                liReturnObject = null;
            }
            return liReturnObject;
        }
    }
    public class typeConnectLocalDB
    {
        List<Object> liReturnObject;
        Types obType;
        public Types selectWhere(int iTypeId)
        {
            try
            {
                obType = new Types();
                obType = ViewModelControlPanel.DBDataClass.Types.SingleOrDefault(c => c.typeId == iTypeId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                obType = null;
            }
            return obType;
        }
        public List<Object> select(string sFilter)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from    x in ViewModelControlPanel.DBDataClass.Types
                            where   (x.code.Contains(sFilter) || x.name.Contains(sFilter))
                            orderby x.typeId ascending
                            select  x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                //liReturnObject = null;
            }
            return liReturnObject;
        }

        public int insertAndSubmit(Types newType)
        {
            try
            {
                ViewModelControlPanel.DBDataClass.Types.InsertOnSubmit(newType);
                ViewModelControlPanel.DBDataClass.SubmitChanges();
                return newType.typeId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                return -1;
            }


        }
        public int editInsertAndSubmit(Types editedType)
        {
            try
            {
                Types newEditedType = ViewModelControlPanel.DBDataClass.Types.Single(c => c.typeId == editedType.typeId);

                newEditedType = editedType;
                ViewModelControlPanel.DBDataClass.SubmitChanges();
                return newEditedType.typeId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
                return -1;
            }
        }
        public void deleteWhereTypeId(int iTypeId)
        {
            try
            {
                Types deleteType = ViewModelControlPanel.DBDataClass.Types.Single(c => c.typeId == iTypeId);
                ViewModelControlPanel.DBDataClass.Types.DeleteOnSubmit(deleteType);

                ViewModelControlPanel.DBDataClass.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
            }
        }
    }
    public class coverConnectLocalDB
    {
        List<Object> liReturnObject;
        Covers obCover;
        public Covers selectWhere(int iCoverId)
        {
            try
            {
                obCover = new Covers();
                obCover = ViewModelControlPanel.DBDataClass.Covers.SingleOrDefault(c => c.coverId == iCoverId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                obCover = null;
            }
            return obCover;
        }
        public List<Object> select(string sFilter)
        {
            try
            {
                liReturnObject = new List<Object>();

                var query = from x in ViewModelControlPanel.DBDataClass.Covers
                            where (x.code.Contains(sFilter) || x.customer.Contains(sFilter))
                            orderby x.coverId ascending
                            select x;

                foreach (Object item in query)
                {
                    liReturnObject.Add(item);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                //liReturnObject = null;
            }
            return liReturnObject;
        }

        public int insertAndSubmit(Covers newType)
        {
            try
            {
                ViewModelControlPanel.DBDataClass.Covers.InsertOnSubmit(newType);
                ViewModelControlPanel.DBDataClass.SubmitChanges();
                return newType.coverId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
                return -1;
            }


        }
        public int editInsertAndSubmit(Covers editedType)
        {
            try
            {
                Covers newEditedType = ViewModelControlPanel.DBDataClass.Covers.Single(c => c.coverId == editedType.coverId);

                newEditedType = editedType;
                ViewModelControlPanel.DBDataClass.SubmitChanges();
                return newEditedType.coverId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
                return -1;
            }
        }
        public void deleteWhereCoverId(int iCoverId)
        {
            try
            {
                Covers deleteCover = ViewModelControlPanel.DBDataClass.Covers.Single(c => c.coverId == iCoverId);
                ViewModelControlPanel.DBDataClass.Covers.DeleteOnSubmit(deleteCover);

                ViewModelControlPanel.DBDataClass.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
            }
        }
    }
    public class productionConnectLocalDB
    {
        Production production;
        public Production selectSingle(int _argiProjectId, int _argiProductId)
        {
            try
            {
                production = new Production();
                production = ViewModelControlPanel.DBDataClass.Productions.SingleOrDefault(c => c.projectId == _argiProjectId && c.productId == _argiProductId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                production = null;
            }
            return production;
        }
        public void insertAndSubmit(Production newProduction)
        {
            try
            {
                ViewModelControlPanel.DBDataClass.Productions.InsertOnSubmit(newProduction);
                ViewModelControlPanel.DBDataClass.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
            }
        }
        public void editInsertAndSubmit(Production editedProduction)
        {
            try
            {
                Production neweditedProduction = ViewModelControlPanel.DBDataClass.Productions.Single(c => (c.projectId == editedProduction.projectId) && (c.productId == editedProduction.productId));

                neweditedProduction = editedProduction;
                ViewModelControlPanel.DBDataClass.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
            }
        }
    }
    public class productionCounterConnectLocalDB
    {
        ProductionCounter productionCounter;

        public ProductionCounter selectSingle(int _argiProjectId, int _argiProductId)
        {
            try
            {
                productionCounter = new ProductionCounter();
                productionCounter = ViewModelControlPanel.DBDataClass.ProductionCounters.SingleOrDefault(c => c.projectId == _argiProjectId && c.productId == _argiProductId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingFromDatabase") + "\n" + ex.ToString());
                productionCounter = null;
            }
            return productionCounter;
        }

        public void insertAndSubmit(ProductionCounter newProductionCounter)
        {
            try
            {
                ViewModelControlPanel.DBDataClass.ProductionCounters.InsertOnSubmit(newProductionCounter);
                ViewModelControlPanel.DBDataClass.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
            }
        }
        public void editInsertAndSubmit(ProductionCounter editedProductionCounter)
        {
            try
            {
                ProductionCounter neweditedProductionCounter = ViewModelControlPanel.DBDataClass.ProductionCounters.Single(c => (c.projectId == editedProductionCounter.projectId) && (c.productId == editedProductionCounter.productId));

                neweditedProductionCounter = editedProductionCounter;
                ViewModelControlPanel.DBDataClass.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase") + "\n" + ex.ToString());
            }
        }
    }

    public class clErrors
    {
        public clErrors(int _iErrorType, string _sPropertyName, string _sErrorReport = "", string _sValidCharacters = ""/*, int _iLength = 0*/)
        {
            iErrorType          = _iErrorType;
            sValidCharacters    = _sValidCharacters;
            //iLength             = _iLength;

            if (_sErrorReport == "")     // Add default text to error report when no one is given. 
            {
                switch (_iErrorType)    
                {
                    case clConstants.EMPTYCHECK:
                        sErrorReport = _sPropertyName + "\n  -" + clLanguages.getName("__IsEmpty");
                        break;
                    case clConstants.INTCHECK:
                        sErrorReport = _sPropertyName + "\n  -" + clLanguages.getName("__IsNotNumeric");
                        break;
                    case clConstants.STRINGCHECK:
                        sErrorReport = _sPropertyName + "\n  -" + clLanguages.getName("__IsNotValidCharacter");
                        break;
                    case clConstants.MINLENGTHCHECK:
                        sErrorReport = _sPropertyName + "\n  -" + clLanguages.getName("__MinimumLength");
                        int.TryParse(_sValidCharacters, out iLength);
                        break;
                    case clConstants.MAXLENGTHCHECK:
                        sErrorReport = _sPropertyName + "\n  -" + clLanguages.getName("__MaximumLength");
                        int.TryParse(_sValidCharacters, out iLength);
                        break;
                    case clConstants.MINVALCHECK:
                        sErrorReport = _sPropertyName + "\n  -" + clLanguages.getName("__MinimumValue");
                        int.TryParse(_sValidCharacters, out iValue);
                        break;
                    case clConstants.MAXVALCHECK:
                        sErrorReport = _sPropertyName + "\n  -" + clLanguages.getName("__MaximumValue");
                        int.TryParse(_sValidCharacters, out iValue);
                        break;
                }
            } else
            {
                sErrorReport = _sPropertyName + " " + _sErrorReport;
            }
        }
        public int      iErrorType;              // TODO, hier een type van maken zodat alleen bepaalde types worden toegevoegd 
        public string   sErrorReport;         
        public string   sValidCharacters;
        public int      iLength;
        public int      iValue;
    }
    

    public class clTextBoxValidation : ObservableObject
    {
        public clTextBoxValidation(ObservableCollection<clErrors> _obcErrorChecks, bool _xValidateActive = false)
        {
            obcErrorChecks = _obcErrorChecks;
            xValidateActive = _xValidateActive;
            sInput = "";
        }

        private bool _xValidateActive;
        private bool _xHasError;
        private string _sInput;
       // private int _iInput;
        private string _sErrorReport;
        private int _iBorderThickness;

        public ObservableCollection<clErrors> obcErrorChecks = new ObservableCollection<clErrors>();

        public bool xHasError
        {
            get
            {
                return _xHasError;
            }
            set
            {
                _xHasError = value;
                if (value)
                {
                    iBorderThickness = 2;
                }
                else
                {
                    iBorderThickness = 1;
                }
                OnPropertyChanged("xHasError");
            }
        }
        public string sInput
        {
            get
            {
                return _sInput;
            }
            set
            {
                
                _sInput = value;
                validateInput();
                OnPropertyChanged("sInput");
            }
        }
 
        public string sErrorReport
        {
            get
            {
                return _sErrorReport;
            }
            set
            {
                _sErrorReport = value;
                OnPropertyChanged("sErrorReport");
            }
        }
        public int iBorderThickness
        {
            get
            {
                return _iBorderThickness;
            }
            set
            {
                _iBorderThickness = value;
                OnPropertyChanged("iBorderThickness");
            }
        }
        public void validateInput()
        {
            bool xReturnHasError    = false;
            sErrorReport            = null;
            xReturnHasError         = false;
            if (xValidateActive)
            {
                foreach (clErrors errorCheck in obcErrorChecks)
                {
                    int iValue = 0;
                    switch (errorCheck.iErrorType)
                    {
                        case clConstants.EMPTYCHECK:
                            if (sInput == "" || sInput.Substring(0, 1) == " ")
                            {
                                if (sErrorReport != null)
                                {
                                    sErrorReport += "\n";
                                }
                                sErrorReport = errorCheck.sErrorReport;
                                xReturnHasError = true;
                            }
                            break;

                        case clConstants.INTCHECK:
                            int n;
                            if (!int.TryParse(sInput, out n))
                            {
                                if (sErrorReport != null)
                                {
                                    sErrorReport += "\n";
                                }
                                sErrorReport += errorCheck.sErrorReport;
                                xReturnHasError = true;
                            }
                            break;

                        case clConstants.STRINGCHECK:
                            if (!Regex.IsMatch(sInput, errorCheck.sValidCharacters))
                            {
                                if (sErrorReport != null)
                                {
                                    sErrorReport += "\n";
                                }
                                sErrorReport += errorCheck.sErrorReport;
                                xReturnHasError = true;
                            }
                            break;

                        case clConstants.MINLENGTHCHECK:
                            if (sInput.Length < errorCheck.iLength)
                            {
                                if (sErrorReport != null)
                                {
                                    sErrorReport += "\n";
                                }
                                sErrorReport += errorCheck.sErrorReport + ": " + errorCheck.iLength;
                                xReturnHasError = true;
                            }
                            break;

                        case clConstants.MAXLENGTHCHECK:
                            if (sInput.Length > errorCheck.iLength)
                            {
                                if (sErrorReport != null)
                                {
                                    sErrorReport += "\n";
                                }
                                sErrorReport += errorCheck.sErrorReport + ": " + errorCheck.iLength;
                                xReturnHasError = true;
                            }
                            break;

                        case clConstants.MINVALCHECK:
                            int.TryParse(sInput, out iValue);
                            if (iValue < errorCheck.iValue)
                            {
                                if (sErrorReport != null)
                                {
                                    sErrorReport += "\n";
                                }
                                sErrorReport += errorCheck.sErrorReport + ": " + errorCheck.iValue; 
                                xReturnHasError = true;
                            }
                            break;

                        case clConstants.MAXVALCHECK:
                            int.TryParse(sInput, out iValue);
                            if (iValue > errorCheck.iValue)
                            {
                                if (sErrorReport != null)
                                {
                                    sErrorReport += "\n";
                                }
                                sErrorReport += errorCheck.sErrorReport + ": " + errorCheck.iValue;
                                xReturnHasError = true;
                            }
                            break;
                    }
                }
            }
            xHasError = xReturnHasError;
        }
        public bool xValidateActive
        {
            get
            {
                return _xValidateActive;
            }
            set
            {
                _xValidateActive = value;
                if (_xValidateActive)
                {
                    //validateInput();
                }
            }
        }
    }

    public class blink : ObservableObject
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        

        private bool _xBlink;
        public  bool xBlink
        {
            get
            {
                return _xBlink;
            }
            set
            {
                _xBlink = value;
                OnPropertyChanged("xBlink");
            }
        }

        public blink(int days, int hours, int minutes, int seconds, int milliseconds, EventHandler _target)
        {
            dispatcherTimer.Tick += new EventHandler(_target);
            dispatcherTimer.Interval = new TimeSpan(days, hours, minutes, seconds, milliseconds);
            dispatcherTimer.Start();
        }
        public void invertBlink(object sender, EventArgs e)
        {
            xBlink = !xBlink;
        }
    }

    public class DataPiping
    {
        #region DataPipes (Attached DependencyProperty)

        public static readonly DependencyProperty DataPipesProperty =
            DependencyProperty.RegisterAttached("DataPipes",
            typeof(DataPipeCollection),
            typeof(DataPiping),
            new UIPropertyMetadata(null));

        public static void SetDataPipes(DependencyObject o, DataPipeCollection value)
        {
            o.SetValue(DataPipesProperty, value);
        }

        public static DataPipeCollection GetDataPipes(DependencyObject o)
        {
            return (DataPipeCollection)o.GetValue(DataPipesProperty);
        }

        #endregion
    }

    public class DataPipeCollection : FreezableCollection<DataPipe>
    {

    }

    public class DataPipe : Freezable
    {
        #region Source (DependencyProperty)

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(object), typeof(DataPipe),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataPipe)d).OnSourceChanged(e);
        }

        protected virtual void OnSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            Target = e.NewValue;
        }

        #endregion

        #region Target (DependencyProperty)

        public object Target
        {
            get { return (object)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(object), typeof(DataPipe),
            new FrameworkPropertyMetadata(null));

        #endregion

        protected override Freezable CreateInstanceCore()
        {
            return new DataPipe();
        }
    }

    public class SerialComm : ObservableObject// INotifyPropertyChanged
    {
        private string _data1 = string.Empty;
        private string _data2 = string.Empty;

        public string Material1 { get { return _data1; } set { _data1 = value; OnPropertyChanged("Material1"); } }
        public string Material2 { get { return _data2; } set { _data2 = value; OnPropertyChanged("Material2"); } }


        public SerialPort CreatePort(string PortName, int index)
        {
            string idx = Convert.ToString(index);
            String idxBaudRate = "BaudRatePort" + idx;
            String idxDataBits = "DataBitsPort" + idx;
            String idxStopBits = "StopBitsPort" + idx;
            String idxParity = "ParityPort" + idx;
            String idxReadTimeout = "ReadTimeoutPort" + idx;
            String idxWriteTimeout = "WriteTimeoutPort" + idx;

            try
            {
                string pth = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                pth += @"\CommsData\CommsData.xml";
                XmlDocument xml = new XmlDocument();
                xml.Load(pth);

                SerialPort Port = new SerialPort();

                Port.PortName = PortName;

                XmlElement baud = xml.GetElementById(idxBaudRate);
                string strBaudRate = baud.InnerText;
                Port.BaudRate = Convert.ToInt16(strBaudRate);

                XmlElement DataBits = xml.GetElementById(idxDataBits);
                string strDataBits = DataBits.InnerText;
                Port.DataBits = Convert.ToInt16(strDataBits);

                XmlElement StopBits = xml.GetElementById(idxStopBits);
                string strStopBits = StopBits.InnerText;
                Port.StopBits = (StopBits)Enum.Parse(typeof(StopBits), strStopBits);

                XmlElement Parity = xml.GetElementById(idxParity);
                string strParity = Parity.InnerText;
                Port.Parity = (Parity)Enum.Parse(typeof(Parity), strParity);

                XmlElement ReadTimeout = xml.GetElementById(idxReadTimeout);
                string strReadTimeout = ReadTimeout.InnerText;
                Port.ReadTimeout = Convert.ToInt16(strReadTimeout);

                XmlElement WriteTimeout = xml.GetElementById(idxWriteTimeout);
                string strWriteTimeout = WriteTimeout.InnerText;
                Port.WriteTimeout = Convert.ToInt16(strWriteTimeout);
                Port.Handshake = System.IO.Ports.Handshake.None;

                Port.Open();

                Port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(receive_data);
                return (Port);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                // MessageBox.Show("Port : " + PortName + "set on index : " + index);
            }
        }

        private delegate void UpdateUiTextDelegate(string text);

        private void receive_data(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SerialPort prt = (SerialPort)sender;
            switch (prt.PortName)
            {
                case "COM4": Material1 = prt.ReadExisting(); break;
                case "COM5": Material2 = prt.ReadExisting(); break;
                default: Material1 = Material2 = "Error Input"; break;
            }

            //string received_data = prt.ReadExisting();
            //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(this.Display_Data), received_data);
        }

        private static byte[] HexStringToByteArray(string hex)
        {
            //hex = hex.ToUpper();
            //hex = hex.Replace(" ", string.Empty);

            //byte[] arr = new byte[hex.Length >> 1];
            //for (int i = 0; i < hex.Length >> 1; i++)
            //    arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            //return (arr);
            return null;
        }
        private static int GetHexVal(char Hex)
        {
            int val = (int)Hex;
            return (val - (val < 58 ? 48 : 55));
        }

        private void Display_Data(string d)
        {
            try
            {
                //ReceivedString = d;
                //MessageBox.Show(this.ReceivedString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void SerialCmdSend(SerialPort port, string Data, Boolean Hex)
        {
            if (true) //port.IsOpen)
                try
                {
                    if (Hex)
                    {
                        byte[] ByteArray = HexStringToByteArray(Data);

                        port.Write(ByteArray, 0, ByteArray.Length);
                    }
                    else
                    {

                    }
                }
                catch (Exception Ex)
                { }
        }
    }


}
