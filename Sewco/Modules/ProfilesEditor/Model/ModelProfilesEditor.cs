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
using Sewco.Resources.Helper_classes;

namespace Sewco.Modules.ProfilesEditor
{
    /*public class clCbbFilltype1 : ObservableObject
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

    }*/
    public class clCbbFilltype2 : ObservableObject
    {
        public clCbbFilltype2(string _argsDisplayName, string _argsRealName, string _argsId)
        {
            this.sDisplayName = _argsDisplayName;
            this.sRealName = _argsRealName;
            this.sSelectedvaluePath = _argsId;
        }
        private string _sDisplayName;
        private string _sRealName;
        private string _sSelectedvaluePath;

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
        public string sRealName
        {
            get
            {
                return _sRealName;
            }
            set
            {
                _sRealName = value;
                OnPropertyChanged("sRealName");
            }
        }
        public string sSelectedvaluePath
        {
            get
            {
                return _sSelectedvaluePath;
            }
            set
            {
                _sSelectedvaluePath = value;
                OnPropertyChanged("sSelectedvaluePath");
            }
        }
    }

    /*public class clMirrorProfile : ObservableObject
    {
        public clMirrorProfile(string displayName, int selectedvaluePath)
        {
            this.sDisplayName = displayName;
            this.iSelectedvaluePath = selectedvaluePath;
        }

        private string _sDisplayName;
        private int _iSelectedvaluePath;
        private static int _iArrayIndex;

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
        public static int iArrayIndex
        {
            get
            {
                return _iArrayIndex;
            }
            set
            {
                if (value < 0)
                    value = 0;
                _iArrayIndex = value;
            }
        }
    }*/

    /*public class CheckBoxDatabase : ObservableObject
    {
        public CheckBoxDatabase(string name, bool isChecked)
        {
            this.Name = name;
            this.IsChecked = isChecked;
        }

        private string name;
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
    }*/

    public partial class clProfile : profile, IDataErrorInfo
    {
        public clProfile()
        {
            for (short i = 0; i < 17; i++)
            {
                ocSeamList.Add(1);
            }
        }
        public bool validateResultOK { get; set; }
        public ObservableCollection<short> ocSeamList { get; set; } = new ObservableCollection<short>();
        public ObservableCollection<clCbbFilltype1> obcProfileOptions { get; set; } = new ObservableCollection<clCbbFilltype1>();
        public ObservableCollection<clCbbFilltype1> obcSeamOptions { get; set; } = new ObservableCollection<clCbbFilltype1>();

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        List<string> list = new List<string>();

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
                    // Result error tekst.
                    return sResult;
                }
                else
                {
                    if (list.Contains(sSaveName))
                    {
                        list.Remove(sSaveName);
                    }
                    // Validate result is good. Set new value to database property
                    vSaveProperty.SetValue(this, sPropertyValue);
                }
                return null;
            }

        }

        private string validateInput(string value)
        {
            string sResult = "";
            int iNumberOfErrors = 0;

            if (value == "")
            {
                iNumberOfErrors++;
                if (iNumberOfErrors > 1)
                    sResult += "\n";
                sResult += "Input is empty";
            }

            this.validateResultOK = (sResult == "");
            return sResult;
        }

        private string _sName;
        public string sName
        {
            get
            {
                return _sName;
            }
            set
            {
                _sName = value;
                SendPropertyChanged("sName");
            }
        }

        private string _sComment;
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
        /* private short _Id;
         private string _name;
         private string _comment;
         private bool _active;

         private System.Nullable<short> _seam1_id;
         private System.Nullable<short> _seam2_id;
         private System.Nullable<short> _seam3_id;
         private System.Nullable<short> _seam4_id;
         private System.Nullable<short> _seam5_id;
         private System.Nullable<short> _seam6_id;
         private System.Nullable<short> _seam7_id;
         private System.Nullable<short> _seam8_id;
         private System.Nullable<short> _seam9_id;
         private System.Nullable<short> _seam10_id;
         private System.Nullable<short> _seam11_id;
         private System.Nullable<short> _seam12_id;
         private System.Nullable<short> _seam13_id;
         private System.Nullable<short> _seam14_id;
         private System.Nullable<short> _seam15_id;
         private System.Nullable<short> _seam16_id;

         public short Id
         {
             get
             {
                 return _Id;
             }
             set
             {
                 _Id = value;
                 OnPropertyChanged("Id");
             }
         }
         public string name
         {
             get
             {
                 return _name;
             }
             set
             {
                 _name = value;
                 OnPropertyChanged("name");
             }
         }
         public string comment
         {
             get
             {
                 return _comment;
             }
             set
             {
                 _comment = value;
                 OnPropertyChanged("comment");
             }
         }
         public bool active
         {
             get
             {
                 return _active;
             }
             set
             {
                 _active = value;
                 OnPropertyChanged("active");
             }
         }

         public System.Nullable<short> seam1_id
         {
             get
             {
                 return _seam1_id;
             }
             set
             {
                 _seam1_id = value;
                 OnPropertyChanged("seam1_id");
             }
         }
         public System.Nullable<short> seam2_id
         {
             get
             {
                 return _seam2_id;
             }
             set
             {
                 _seam2_id = value;
                 OnPropertyChanged("seam2_id");
             }
         }
         public System.Nullable<short> seam3_id
         {
             get
             {
                 return _seam3_id;
             }
             set
             {
                 _seam3_id = value;
                 OnPropertyChanged("seam3_id");
             }
         }
         public System.Nullable<short> seam4_id
         {
             get
             {
                 return _seam4_id;
             }
             set
             {
                 _seam4_id = value;
                 OnPropertyChanged("seam4_id");
             }
         }
         public System.Nullable<short> seam5_id
         {
             get
             {
                 return _seam5_id;
             }
             set
             {
                 _seam5_id = value;
                 OnPropertyChanged("seam5_id");
             }
         }
         public System.Nullable<short> seam6_id
         {
             get
             {
                 return _seam6_id;
             }
             set
             {
                 _seam6_id = value;
                 OnPropertyChanged("seam6_id");
             }
         }
         public System.Nullable<short> seam7_id
         {
             get
             {
                 return _seam7_id;
             }
             set
             {
                 _seam7_id = value;
                 OnPropertyChanged("seam7_id");
             }
         }
         public System.Nullable<short> seam8_id
         {
             get
             {
                 return _seam8_id;
             }
             set
             {
                 _seam8_id = value;
                 OnPropertyChanged("seam8_id");
             }
         }
         public System.Nullable<short> seam9_id
         {
             get
             {
                 return _seam9_id;
             }
             set
             {
                 _seam9_id = value;
                 OnPropertyChanged("seam9_id");
             }
         }
         public System.Nullable<short> seam10_id
         {
             get
             {
                 return _seam10_id;
             }
             set
             {
                 _seam10_id = value;
                 OnPropertyChanged("seam10_id");
             }
         }
         public System.Nullable<short> seam11_id
         {
             get
             {
                 return _seam11_id;
             }
             set
             {
                 _seam11_id = value;
                 OnPropertyChanged("seam11_id");
             }
         }
         public System.Nullable<short> seam12_id
         {
             get
             {
                 return _seam12_id;
             }
             set
             {
                 _seam12_id = value;
                 OnPropertyChanged("seam12_id");
             }
         }
         public System.Nullable<short> seam13_id
         {
             get
             {
                 return _seam13_id;
             }
             set
             {
                 _seam13_id = value;
                 OnPropertyChanged("seam13_id");
             }
         }
         public System.Nullable<short> seam14_id
         {
             get
             {
                 return _seam14_id;
             }
             set
             {
                 _seam14_id = value;
                 OnPropertyChanged("seam14_id");
             }
         }
         public System.Nullable<short> seam15_id
         {
             get
             {
                 return _seam15_id;
             }
             set
             {
                 _seam15_id = value;
                 OnPropertyChanged("seam15_id");
             }
         }
         public System.Nullable<short> seam16_id
         {
             get
             {
                 return _seam16_id;
             }
             set
             {
                 _seam16_id = value;
                 OnPropertyChanged("seam16_id");
             }
         }*/
    }

    /*public class clcbbOptions : ObservableObject
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
    }*/

    public class clSeamTemplateModel : seamTemplate, IDataErrorInfo
    {
        public bool validateResultOK { get; set; }
        public ObservableCollection<clCbbFilltype1> obcSeamTemplateOptions { get; set; } = new ObservableCollection<clCbbFilltype1>();
        string IDataErrorInfo.Error
        {
            get { return null; }
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
                    vSaveProperty.SetValue(this, (short)0);
                    // Result error tekst.
                    return sResult;
                }
                else
                {
                    // Validate result is good. Set new value to database property
                    vSaveProperty.SetValue(this, short.Parse(sPropertyValue));
                }
                return null;
            }
        }
        /*
        When adding a new variable to the database which need to be checked, add a new property below. 
        Also, extend database with new column. 
        Both variables must correspondent to each other. 
        Step 1: Extend/update database with column, with format:                newVariable
        Step 2: Delete database from ProfileDataClass.dbml and add new one. 
        Step 3: class seamTemplate is updated. Add new property below:          sNewVariable
        Step 4: Add new textbox to the view and add the new variable in ViewModel.     
        */

        
        private string _sMinTens;
        public string sMinTens
        {
            get
            {
                return _sMinTens;
            }
            set
            {
                _sMinTens = value;
                SendPropertyChanged("sMinTens");
            }
        }

        private string _sMaxTens;
        public string sMaxTens
        {
            get
            {
                return _sMaxTens;
            }
            set
            {
                _sMaxTens = value;
                SendPropertyChanged("sMaxTens");
            }
        }

        private string _sTensFilter;
        public string sTensFilter
        {
            get
            {
                return _sTensFilter;
            }
            set
            {
                _sTensFilter = value;
                SendPropertyChanged("sTensFilter");
            }
        }

        private string _sBlindTens;
        public string sBlindTens
        {
            get
            {
                return _sBlindTens;
            }
            set
            {
                _sBlindTens = value;
                SendPropertyChanged("sBlindTens");
            }
        }

        private string _sStitchCount;
        public string sStitchCount
        {
            get
            {
                return _sStitchCount;
            }
            set
            {
                _sStitchCount = value;
                SendPropertyChanged("sStitchCount");
            }
        }

        private string _sPosTol;
        public string sPosTol
        {
            get
            {
                return _sPosTol;
            }
            set
            {
                _sPosTol = value;
                SendPropertyChanged("sPosTol");
            }
        }

        private string _sNegTol;
        public string sNegTol
        {
            get
            {
                return _sNegTol;
            }
            set
            {
                _sNegTol = value;
                SendPropertyChanged("sNegTol");
            }
        }

        private string _sBlindArea;
        public string sBlindArea
        {
            get
            {
                return _sBlindArea;
            }
            set
            {
                _sBlindArea = value;
                SendPropertyChanged("sBlindArea");
            }
        }

        private string _sSeamLen;
        public string sSeamLen
        {
            get
            {
                return _sSeamLen;
            }
            set
            {
                _sSeamLen = value;
                SendPropertyChanged("sSeamLen");
            }
        }

        private string _sStitchLen;
        public string sStitchLen
        {
            get
            {
                return _sStitchLen;
            }
            set
            {
                _sStitchLen = value;
                SendPropertyChanged("sStitchLen");
            }
        }

        private string _sSpeed;
        public string sSpeed
        {
            get
            {
                return _sSpeed;
            }
            set
            {
                _sSpeed = value;
                SendPropertyChanged("sSpeed");
            }
        }

        private string _sStartBtFw;
        public string sStartBtFw
        {
            get
            {
                return _sStartBtFw;
            }
            set
            {
                _sStartBtFw = value;
                SendPropertyChanged("sStartBtFw");
            }
        }

        private string _sStartBtBw;
        public string sStartBtBw
        {
            get
            {
                return _sStartBtBw;
            }
            set
            {
                _sStartBtBw = value;
                SendPropertyChanged("sStartBtBw");
            }
        }

        private string _sEndBtFw;
        public string sEndBtFw
        {
            get
            {
                return _sEndBtFw;
            }
            set
            {
                _sEndBtFw = value;
                SendPropertyChanged("sEndBtFw");
            }
        }

        private string _sEndBtBw;
        public string sEndBtBw
        {
            get
            {
                return _sEndBtBw;
            }
            set
            {
                _sEndBtBw = value;
                SendPropertyChanged("sEndBtBw");
            }
        }

        private string validateInput(string value)
        {
            string sResult = "";
            int iNumberOfErrors = 0;
            int iNumber = 0;

            if (!Int32.TryParse(value, out iNumber))
            {
                iNumberOfErrors++;
                if (iNumberOfErrors > 1)
                    sResult += "\n";
                sResult += "Input is not a number";
            }
            if (value == "")
            {
                iNumberOfErrors++;
                if (iNumberOfErrors > 1)
                    sResult += "\n";
                sResult += "Input is empty";
            }
            if (Int32.TryParse(value, out iNumber))
            {
                iNumberOfErrors++;
                if (iNumber > 32000)
                {
                    if (iNumberOfErrors > 1)
                        sResult += "\n";
                    sResult += "Value is to high";
                }
                else if (iNumber < 0)
                {
                    if (iNumberOfErrors > 1)
                        sResult += "\n";
                    sResult += "Value is below 0";
                }
            }

            this.validateResultOK = (sResult == "");
            return sResult;
        }

        public clcbbOptions clFunctionsPedalOptions          { get; set; } = new clcbbOptions();
        public clcbbOptions clFunctionsVRUOptions       { get; set; } = new clcbbOptions();
        public clcbbOptions clFunctionsNHTOptions { get; set; } = new clcbbOptions();
        public clcbbOptions clFunctionsZVROptions { get; set; } = new clcbbOptions();
        public clcbbOptions clFunctionsSTLOptions { get; set; } = new clcbbOptions();
        public clcbbOptions clFunctionsFSPROptions { get; set; } = new clcbbOptions();
        public clcbbOptions clMonitoringBobbinOptions { get; set; } = new clcbbOptions();
        public clcbbOptions clMonitoringCoverOptions { get; set; } = new clcbbOptions();
        public clcbbOptions clMonitoringEltexOptions { get; set; } = new clcbbOptions();
        public clcbbOptions clGeneralTransitOptions { get; set; } = new clcbbOptions();
        public clcbbOptions clGeneralSeamOptions { get; set; } = new clcbbOptions();
        public clcbbOptions clGeneralScanbackOptions { get; set; } = new clcbbOptions();
        public clcbbOptions clCopyProfileOption { get; set; } = new clcbbOptions();
        public clcbbOptions clCopySeamTemplateOption { get; set; } = new clcbbOptions();
        public clcbbOptions clEditSeamTemplateInProfiles { get; set; } = new clcbbOptions();
    }

    /*public class clLanguage : ObservableObject
    {
        // This class is based on:     http://www.wpf-tutorial.com/wpf-application/resources/
        private string sModuleName;         // Contains the name of the module. Example: ProfileEditor
        private string _sSelectedLanguage;  // Contains the current selected language, example: Dutch
        public string sSelectedLanguage   // Contains the current selected language, example: Dutch
        {
            get
            {
                return this._sSelectedLanguage;
            }
            set
            {
                this._sSelectedLanguage = value;
                OnPropertyChanged("sSelectedLanguage");
                updateLanguage(this._sSelectedLanguage);
            }
        }

        // An observablecollection, which collects all names available languages 
        public ObservableCollection<clCbbFilltype2> obcLanguages { get; set; } = new ObservableCollection<clCbbFilltype2>();

        void getAvailableLanguagesFromFolder()
        {
            string sCurrentDirectory = Directory.GetCurrentDirectory();
            string[] sFiles = Directory.GetFiles(sCurrentDirectory + "\\Languages\\", "*.xaml");
            short iCounter = 0;
            foreach (string sFile in sFiles)
            {
                if (sFile.Contains(sModuleName))
                {
                    string sFileName = sFile.Split('\\').Last();         // sFile contains whole file path. sFileName contains for example ProfilesEditor_Duits.xaml 
                    string sDisplayName = sFileName.Split('.').First();     // sDisplayName contains ProfilesEditor_Duits
                    sDisplayName = sDisplayName.Split('_').Last();   // sDisplayName contains Duits
                    obcLanguages.Add(new clCbbFilltype2(sDisplayName, sFileName, sDisplayName));
                    iCounter++;
                }
            }
            // When no languages available, it will select its own defined in MainWindows.xaml (DefaultLanguage.xaml)
        }

        public void updateLanguage(string _argsNewLanguage)
        {
            int iId = 0;
            // Get index of the selected new language from the obcLanguages array
            iId = obcLanguages.IndexOf((from x in obcLanguages
                                        where x.sDisplayName == _argsNewLanguage
                                        select x).FirstOrDefault());
            if (iId >= 0)    // If < 0, index is not found
            {
                try
                {
                    string sSelectedLanguageFile = obcLanguages[iId].sRealName;      // Get selected language name

                    ResourceDictionary dict = new ResourceDictionary();         // Create new resource dictionary
                    string sSelectedLanguageUrl = "pack://siteoforigin:,,,/Languages/" + sSelectedLanguageFile;     // Create url to file

                    dict.Source = new Uri(sSelectedLanguageUrl, UriKind.RelativeOrAbsolute);
                    App.Current.MainWindow.Resources.MergedDictionaries.Add(dict);      // Add new Language file to resources. This file will be automaticly selected 
                    
                }
                catch
                {
                    // Do nothing
                }
            }
        }
        public clLanguage(string _argsName)
        {
            sModuleName = _argsName;        // Select modulename.. .xaml file contains the name of the module, like: ProfilesEditor_Dutch.xaml
            getAvailableLanguagesFromFolder();
        }
    }*/

    /*public class clTestSource : ObservableObject
    {
        private static readonly clTestSource instance = new clTestSource();

        public static Dictionary<string, string> obcNames = new Dictionary<string, string>();

        public static clTestSource Instance
        {
            get { return instance; }
        }

        public string this[string key]
        {
            get { return obcNames[key]; }
        }
    }*/
    /*public class clTest : System.Windows.Data.Binding, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
  
        public clTest(string name) : base("[" + name + "]")
        {
            this.initialize();
            this.Mode       = BindingMode.OneWay;
            this.Source     = clTestSource.Instance;
        }
        public void add(string xKey, string sValue)
        {
            if(!clTestSource.obcNames.ContainsKey(xKey))
                clTestSource.obcNames.Add(xKey, sValue);
        }
        public void initialize()
        {
            this.add("New", "Nieuw");
            this.add("Old", "Oud");
        }
        public void initializeSecond()
        {
            this.add("New", "Neu");
            this.add("Old", "Oud");
        }

        public void reload()
        {
            clTestSource.obcNames = new Dictionary<string, string>();
            initializeSecond();
            NotifyPropertyChanged("obcNames");
            NotifyPropertyChanged("clTest");
            NotifyPropertyChanged("New");
        }


    }*/

}
