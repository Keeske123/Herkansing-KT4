using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using System.Resources;
using System.Collections;
using System.IO;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Collections.Generic;
using Sewco.Resources.Helper_classes;
using Sewco.Modules.ControlPanel;


namespace Sewco.Modules.ProfilesEditor
{
    
    public class ViewModelProfilesEditor : ObservableObject
    {
        
        public ViewModelProfilesEditor()
        {
            if (ViewModelControlPanel.xValidDatabaseConnection)
            {
                xEnableScreen   = true;
                xEnableProfileMenu = false;
                // Add new relay commands if necessary
                #region RelayCommands
                newProfileCommand = new RelayCommand(
                        param => newProfile(),
                        param => (!xProfileEditMode)            // Only visible when not in New of Change mode
                    );
                saveProfileCommand = new RelayCommand(
                        param => saveProfile(),
                        param => (clCurrentProfile.name != "" && clCurrentProfile.comment != "" && (xProfileChangeMode || xProfileNewMode))
                    );
                cancelProfileCommand = new RelayCommand(
                        param => cancelProfile(),                   // Cancel profile editing.
                        param => (xProfileEditMode)                 // Is visible in profile edit mode.. (New and change mode)
                    );
                editProfileCommand = new RelayCommand(
                        param => editProfile(),             // Edit profile button  
                        param => (!xProfileEditMode)        // (!xProfileEditMode) Visible when not in edit mode.         
                    );
                deleteProfileCommand = new RelayCommand(
                        param => deleteProfile(),           // Delete profile
                        param => (xProfileChangeMode)       // Visible when not in edit mode.. TODO: Ook niet wanneer deze vaker gebruikt wordt in bovenliggende database..
                    );
                copyProfileCommand = new RelayCommand(
                        param => controlCopyProfilePopUp(true),     // Show popup screen, to copy a current profile
                        param => (xProfileEditMode)                 // Only visible when in edit mode
                    );
                cancelCopyProfileCommand = new RelayCommand(
                        param => controlCopyProfilePopUp(false),
                        param => (true)
                    );
                selectCopyProfileCommand = new RelayCommand(
                        param => copyProfile(),
                        param => (iCopyProfileSelectedValue != 0)
                    );
                firstProfileCommand = new RelayCommand(
                        param => selectFirstProfile(),
                        param => (!xProfileNewMode && !xProfileChangeMode)
                    );
                lastProfileCommand = new RelayCommand(
                        param => selectLastProfile(),
                        param => (!xProfileNewMode && !xProfileChangeMode)
                    );
                newSeamTemplateCommand = new RelayCommand(
                        param => newSeamTemplate(),
                        param => (!xSeamTemplateEditMode && !xProfileEditMode)
                    );
                saveSeamTemplateCommand = new RelayCommand(
                        param => saveSeamTemplate(),
                        param => (xSeamTemplateEditMode && !xProfileEditMode && clCurrentSeamTemplate.validateResultOK &&
                                    clCurrentSeamTemplate.name != "" && clCurrentSeamTemplate.comment != "" &&
                                    !(iSeamTemplateSelectedValue == 0 && xSeamTemplateChangeMode))
                    );
                cancelSeamTemplateCommand = new RelayCommand(
                        param => cancelSeamTemplate(),
                        param => (xSeamTemplateEditMode && !xProfileEditMode)
                    );
                editSeamTemplateCommand = new RelayCommand(
                        param => fillWhereSeamTemplateIsUsed(),  //xSeamTemplateChangeMode = true,
                        param => (!xSeamTemplateEditMode && !xProfileEditMode && iSeamTemplateSelectedValue != 0)
                    );
                deleteSeamTemplateCommand = new RelayCommand(
                        param => deleteSeamTemplate(),
                        param => (xSeamTemplateEditMode && iSeamTemplateSelectedValue != 0)
                    );
                copySeamTemplateCommand = new RelayCommand(
                        param => controlCopySeamTemplatePopUp(true),
                        param => (xSeamTemplateEditMode)
                    );
                cancelCopySeamTemplateCommand = new RelayCommand(
                        param => controlCopySeamTemplatePopUp(false),
                        param => (true)
                    );
                selectCopySeamTemplateCommand = new RelayCommand(
                        param => copySeamTemplate(),
                        param => (iCopySeamTemplateSelectedValue != 0)
                    );
                cancelPopupEditSeamTemplateCommand = new RelayCommand(
                        param => controlEditSeamTemplatePopUp(false),
                        param => (true)
                    );
                okPopupEditSeamTemplateCommand = new RelayCommand(
                        param => xSeamTemplateChangeMode = true,
                        param => (true)
                    );
                selectLabelPathFolderCommand = new RelayCommand(
                        param => openFolderDialog(),
                        param => (true)
                    );

                #endregion
                clCurrentSeamTemplate   = new clSeamTemplateModel();
                clCurrentProfile        = new clProfile();

                #region Fill comboboxes
                for (int i = 1; i <= 16; i++)
                {
                    clCurrentProfile.obcSeamOptions.Add(new clCbbFilltype1(clLanguages.getName("__Seam") + " " + i.ToString(), i));   // Add 16 seam options to the list
                }
                clCurrentSeamTemplate.clFunctionsPedalOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Block"), 0));
                clCurrentSeamTemplate.clFunctionsPedalOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Free"), 1));
                clCurrentSeamTemplate.clFunctionsPedalOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__>0"), 2));
                clCurrentSeamTemplate.clFunctionsPedalOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__>0 + FL"), 3));
                clCurrentSeamTemplate.clFunctionsPedalOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__FL + FA"), 4));

                clCurrentSeamTemplate.clFunctionsVRUOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Pass"), 0));
                clCurrentSeamTemplate.clFunctionsVRUOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Block"), 1));
                clCurrentSeamTemplate.clFunctionsVRUOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__On"), 2));
                clCurrentSeamTemplate.clFunctionsVRUOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Off"), 3));

                clCurrentSeamTemplate.clFunctionsNHTOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Pass"), 0));
                clCurrentSeamTemplate.clFunctionsNHTOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Block"), 1));
                clCurrentSeamTemplate.clFunctionsNHTOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__On"), 2));
                clCurrentSeamTemplate.clFunctionsNHTOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Off"), 3));

                clCurrentSeamTemplate.clFunctionsZVROptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Pass"), 0));
                clCurrentSeamTemplate.clFunctionsZVROptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Block"), 1));
                clCurrentSeamTemplate.clFunctionsZVROptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__On"), 2));
                clCurrentSeamTemplate.clFunctionsZVROptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Off"), 3));

                clCurrentSeamTemplate.clFunctionsSTLOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Pass"), 0));
                clCurrentSeamTemplate.clFunctionsSTLOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Block"), 1));
                clCurrentSeamTemplate.clFunctionsSTLOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__On"), 2));
                clCurrentSeamTemplate.clFunctionsSTLOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Off"), 3));

                clCurrentSeamTemplate.clFunctionsFSPROptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Pass"), 0));
                clCurrentSeamTemplate.clFunctionsFSPROptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Block"), 1));
                clCurrentSeamTemplate.clFunctionsFSPROptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__On"), 2));
                clCurrentSeamTemplate.clFunctionsFSPROptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Off"), 3));

                clCurrentSeamTemplate.clMonitoringBobbinOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Ignore"), 0));
                clCurrentSeamTemplate.clMonitoringBobbinOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Stop"), 1));
                clCurrentSeamTemplate.clMonitoringBobbinOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Reset"), 2));

                clCurrentSeamTemplate.clMonitoringCoverOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Ignore"), 0));
                clCurrentSeamTemplate.clMonitoringCoverOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Stop"), 1));
                clCurrentSeamTemplate.clMonitoringCoverOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Reset"), 2));

                clCurrentSeamTemplate.clMonitoringEltexOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Ignore"), 0));
                clCurrentSeamTemplate.clMonitoringEltexOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Stop"), 1));
                clCurrentSeamTemplate.clMonitoringEltexOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Reset"), 2));

                clCurrentSeamTemplate.clGeneralTransitOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Notch"), 0));
                clCurrentSeamTemplate.clGeneralTransitOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Count"), 1));
                clCurrentSeamTemplate.clGeneralTransitOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Pedal"), 2));
                clCurrentSeamTemplate.clGeneralTransitOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__PC"), 3));

                clCurrentSeamTemplate.clGeneralSeamOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Not recording"), 0));
                clCurrentSeamTemplate.clGeneralSeamOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Recording"), 1));

                clCurrentSeamTemplate.clGeneralScanbackOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__None"), 0));
                clCurrentSeamTemplate.clGeneralScanbackOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Pedal"), 1));
                clCurrentSeamTemplate.clGeneralScanbackOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Manual"), 2));
                clCurrentSeamTemplate.clGeneralScanbackOptions.obcItems.Add(new clCbbFilltype1(clLanguages.getName("__Scanner"), 3));
                #endregion

                initializeProfilesEditor();
            }
            else
            {
                xEnableScreen   = false;
            }                     
        }

        #region Properties 
        private bool _xSeamTemplateChangeMode;
        public bool xSeamTemplateChangeMode
        {
            get { return _xSeamTemplateChangeMode; }
            set
            {
                _xSeamTemplateChangeMode = value;
                xSeamTemplateEditMode = value;
                controlEditSeamTemplatePopUp(false);
                OnPropertyChanged("xSeamTemplateChangeMode");
            }
        }

        private bool _xSeamTemplateNewMode;
        public bool xSeamTemplateNewMode
        {
            get { return _xSeamTemplateNewMode; }
            set
            {
                _xSeamTemplateNewMode = value;
                xSeamTemplateEditMode = value;
                OnPropertyChanged("xSeamTemplateNewMode");
            }
        }

        private bool _xSeamTemplateEditMode;
        public bool xSeamTemplateEditMode
        {
            get { return _xSeamTemplateEditMode; }
            set
            {
                _xSeamTemplateEditMode = value;
                OnPropertyChanged("xSeamTemplateEditMode");
            }
        }

        private bool _xProfileChangeMode;
        public bool xProfileChangeMode
        {
            get { return _xProfileChangeMode; }
            set
            {
                _xProfileChangeMode = value;
                xProfileEditMode = value;
                OnPropertyChanged("xProfileChangeMode");
            }
        }

        private bool _xProfileEditMode;
        public bool xProfileEditMode
        {
            get
            {
                return _xProfileEditMode;
            }
            set
            {
                _xProfileEditMode = value;
                //xcbbSeamTemplateSelectEditMode = value;
                if (!value)
                {
                    //sfilterProfileOptions = "<DoNothing>";    // TODO weggehaald, is lelijk
                }
                OnPropertyChanged("xProfileEditMode");
            }
        }

        private bool _xProfileNewMode;
        public bool xProfileNewMode
        {
            get
            {
                return _xProfileNewMode;
            }
            set
            {
                _xProfileNewMode = value;
                xProfileEditMode = value;
                OnPropertyChanged("xProfileNewMode");
            }
        }

        private int _iSeamTemplateSelectedValue;
        public int iSeamTemplateSelectedValue
        {
            get
            {
                return _iSeamTemplateSelectedValue;
            }
            set
            {
                _iSeamTemplateSelectedValue = value;
                OnPropertyChanged("iSeamTemplateSelectedValue");
                updateSeamTemplateDetails(value, false);
                if (iSeamSelectedValue != 0 && xProfileEditMode)
                    clCurrentProfile.obcSeamOptions[iSeamSelectedValue - 1].xSeamNotEmpty = clCurrentProfile.ocSeamList[iSeamSelectedValue] > 0;
            }
        }

        private int _iSeamSelectedValue;
        public int iSeamSelectedValue
        {
            get
            {
                return _iSeamSelectedValue;
            }
            set
            {
                _iSeamSelectedValue = value;
                iSeamTemplateSelectedValue = getCurrentSeamTemplateID(_iSeamSelectedValue);
                OnPropertyChanged("iSeamSelectedValue");
            }
        }

        private int _iprofileSelectedValue;
        public int iprofileSelectedValue
        {
            get
            {
                return _iprofileSelectedValue;
            }
            set
            {
                _iprofileSelectedValue = value;
                profileIsChanged();

                OnPropertyChanged("iprofileSelectedValue");
            }
        }

        private bool _xShowPopupCopyProfile;
        public bool xShowPopupCopyProfile
        {
            get
            {
                return _xShowPopupCopyProfile;
            }
            set
            {
                _xShowPopupCopyProfile = value;
                xEnableScreen = !value;
                sfilterPopupCopyProfile = "<DoNothing>";
                OnPropertyChanged("xShowPopupCopyProfile");
            }
        }

        private bool _xShowPopupCopySeamTemplate;
        public bool xShowPopupCopySeamTemplate
        {
            get
            {
                return _xShowPopupCopySeamTemplate;
            }
            set
            {
                _xShowPopupCopySeamTemplate = value;
                xEnableScreen = !value;
                sfilterPopupCopySeamTemplate = "<DoNothing>";
                OnPropertyChanged("xShowPopupCopySeamTemplate");
            }
        }

        private bool _xShowPopupEditSeamTemplate;
        public bool xShowPopupEditSeamTemplate
        {
            get
            {
                return _xShowPopupEditSeamTemplate;
            }
            set
            {
                _xShowPopupEditSeamTemplate = value;
                xEnableScreen = !value;
                OnPropertyChanged("xShowPopupEditSeamTemplate");
            }
        }

        private bool _xEnableScreen;
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

        private string _sfilterProfileOptions;
        public string sfilterProfileOptions
        {
            get
            {
                return _sfilterProfileOptions;
            }
            set
            {
                _sfilterProfileOptions = value;
                OnPropertyChanged("sfilterProfileOptions");
                fillProfileOptions(value);
                updateProfileDetails(iprofileSelectedValue, false);
                updateSeamTemplateDetails(iSeamTemplateSelectedValue, false);
            }
        }

        private string _sfilterSeamTemplateOptions;
        public string sfilterSeamTemplateOptions
        {
            get
            {
                return _sfilterSeamTemplateOptions;
            }
            set
            {
                _sfilterSeamTemplateOptions = value;
                OnPropertyChanged("sfilterSeamTemplateOptions");
                fillSeamTemplateOptions(value);
                updateSeamTemplateDetails(iSeamTemplateSelectedValue, false);
            }
        }

        private string _sfilterPopupCopySeamTemplate;
        public string sfilterPopupCopySeamTemplate
        {
            get
            {
                return _sfilterPopupCopySeamTemplate;
            }
            set
            {
                if (value != "<DoNothing>")
                {
                    controlCopySeamTemplatePopUp(xShowPopupCopySeamTemplate, value);
                    _sfilterPopupCopySeamTemplate = value;
                }
                else
                    _sfilterPopupCopySeamTemplate = "";
                OnPropertyChanged("sfilterPopupCopySeamTemplate");
            }
        }
        private string _sfilterPopupCopyProfile;
        public string sfilterPopupCopyProfile
        {
            get
            {
                return _sfilterPopupCopyProfile;
            }
            set
            {

                if (value != "<DoNothing>")
                {
                    controlCopyProfilePopUp(xShowPopupCopyProfile, value);
                    _sfilterPopupCopyProfile = value;
                }
                else
                    _sfilterPopupCopyProfile = "";

                OnPropertyChanged("sfilterPopupCopyProfile");
            }
        }

        private string _sNameStitches;
        public string sNameStitches
        {
            get
            {
                return _sNameStitches;
            }
            set
            {
                _sNameStitches = value;
                OnPropertyChanged("sNameStitches");
            }
        }

        private bool _xEnableProfileMenu;
        public bool xEnableProfileMenu
        {
            get
            {
                return _xEnableProfileMenu;
            }
            set
            {
                _xEnableProfileMenu = value;
                OnPropertyChanged("xEnableProfileMenu");
            }
        }

        public ObservableCollection<string> obcNames { get; set; } = new ObservableCollection<string>();

        public clProfile clCurrentProfile { get; set; }
        public clSeamTemplateModel clCurrentSeamTemplate { get; set; }
        //public clLanguage clMultiLanguage { get; set; }

        public ObservableCollection<clCbbFilltype1> obcCopyProfileOption { get; set; } = new ObservableCollection<clCbbFilltype1>();
        public ObservableCollection<clCbbFilltype1> obcCopySeamTemplateOption { get; set; } = new ObservableCollection<clCbbFilltype1>();
        public ObservableCollection<clCbbFilltype1> obcEditSeamTemplateInProfiles { get; set; } = new ObservableCollection<clCbbFilltype1>();



        private int _iCopyProfileSelectedValue;
        public int iCopyProfileSelectedValue
        {
            get
            {
                return _iCopyProfileSelectedValue;
            }
            set
            {
                _iCopyProfileSelectedValue = value;
                OnPropertyChanged("iCopyProfileSelectedValue");
            }
        }
        private int _iCopySeamTemplateSelectedValue;
        public int iCopySeamTemplateSelectedValue
        {
            get
            {
                return _iCopySeamTemplateSelectedValue;
            }
            set
            {
                _iCopySeamTemplateSelectedValue = value;
                OnPropertyChanged("iCopySeamTemplateSelectedValue");
            }
        }
        #endregion

        #region RelayCommands
        public RelayCommand newProfileCommand                   { get; set; }
        public RelayCommand saveProfileCommand                  { get; set; }
        public RelayCommand cancelProfileCommand                { get; set; }
        public RelayCommand editProfileCommand                  { get; set; }
        public RelayCommand deleteProfileCommand                { get; set; }
        public RelayCommand copyProfileCommand                  { get; set; }
        public RelayCommand cancelCopyProfileCommand            { get; set; }
        public RelayCommand selectCopyProfileCommand            { get; set; }
        public RelayCommand firstProfileCommand                 { get; set; }
        public RelayCommand lastProfileCommand                  { get; set; }
        public RelayCommand newSeamTemplateCommand              { get; set; }
        public RelayCommand saveSeamTemplateCommand             { get; set; }
        public RelayCommand cancelSeamTemplateCommand           { get; set; }
        public RelayCommand editSeamTemplateCommand             { get; set; }
        public RelayCommand deleteSeamTemplateCommand           { get; set; }
        public RelayCommand copySeamTemplateCommand             { get; set; }
        public RelayCommand cancelCopySeamTemplateCommand       { get; set; }
        public RelayCommand selectCopySeamTemplateCommand       { get; set; }
        public RelayCommand cancelPopupEditSeamTemplateCommand  { get; set; }
        public RelayCommand okPopupEditSeamTemplateCommand      { get; set; }
        public RelayCommand selectLabelPathFolderCommand        { get; set; }

        #endregion

        #region Functions

        
        private bool fillWhereSeamTemplateIsUsed()
        {
            bool xReturn = false;
            //if (databaseConnectionIsValid())
            //{
                try
                {
                    foreach (var q in obcEditSeamTemplateInProfiles.ToList())      // Remove all current options from this list 
                    {
                        obcEditSeamTemplateInProfiles.Remove(q);
                    }

                    var EditSeamTemplateInProfilesQuery = from x in ViewModelControlPanel.DBDataClass.profiles         // Select all templates from profiles database
                                                          where x.seam1_id == iSeamTemplateSelectedValue ||
                                                                  x.seam2_id == iSeamTemplateSelectedValue ||
                                                                  x.seam3_id == iSeamTemplateSelectedValue ||
                                                                  x.seam4_id == iSeamTemplateSelectedValue ||
                                                                  x.seam5_id == iSeamTemplateSelectedValue ||
                                                                  x.seam6_id == iSeamTemplateSelectedValue ||
                                                                  x.seam7_id == iSeamTemplateSelectedValue ||
                                                                  x.seam8_id == iSeamTemplateSelectedValue ||
                                                                  x.seam9_id == iSeamTemplateSelectedValue ||
                                                                  x.seam10_id == iSeamTemplateSelectedValue ||
                                                                  x.seam11_id == iSeamTemplateSelectedValue ||
                                                                  x.seam12_id == iSeamTemplateSelectedValue ||
                                                                  x.seam13_id == iSeamTemplateSelectedValue ||
                                                                  x.seam14_id == iSeamTemplateSelectedValue ||
                                                                  x.seam15_id == iSeamTemplateSelectedValue ||
                                                                  x.seam16_id == iSeamTemplateSelectedValue
                                                          select x;

                    foreach (var q in EditSeamTemplateInProfilesQuery)
                    {
                        int iNumberOfSeams = 0;
                        string sVisibleText = "";

                        if (iSeamTemplateSelectedValue == q.seam1_id)
                        {
                            sVisibleText += "1,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam2_id)
                        {
                            sVisibleText += "2,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam3_id)
                        {
                            sVisibleText += "3,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam4_id)
                        {
                            sVisibleText += "4,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam5_id)
                        {
                            sVisibleText += "5,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam6_id)
                        {
                            sVisibleText += "6,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam7_id)
                        {
                            sVisibleText += "7,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam8_id)
                        {
                            sVisibleText += "8,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam9_id)
                        {
                            sVisibleText += "9,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam10_id)
                        {
                            sVisibleText += "10,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam11_id)
                        {
                            sVisibleText += "11,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam12_id)
                        {
                            sVisibleText += "12,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam13_id)
                        {
                            sVisibleText += "13,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam14_id)
                        {
                            sVisibleText += "14,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam15_id)
                        {
                            sVisibleText += "15,"; iNumberOfSeams++;
                        }
                        if (iSeamTemplateSelectedValue == q.seam16_id)
                        {
                            sVisibleText += "16,"; iNumberOfSeams++;
                        }

                        sVisibleText = sVisibleText.TrimEnd(',');

                        sVisibleText = sVisibleText.Insert(0, clLanguages.getName("__ProfileName") + "\t" + q.name + "\n" + clLanguages.getName("__Seam") + " " + clLanguages.getName("__Numbers") + ": \t");
                        obcEditSeamTemplateInProfiles.Add(new clCbbFilltype1(sVisibleText, q.profileId));

                        xReturn = true;
                    }
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorReadingDatabase"));
                }
            //}
            if (xReturn)
                controlEditSeamTemplatePopUp(xReturn);       // Show PopUpScreen, always true
            else
                xSeamTemplateChangeMode = true;

            return xReturn;
        }

        private void newProfile()       // Used to create a new profile
        {
            emptyCurrentProfile();              // Empty properties
            iSeamSelectedValue = 1;     // Set current SeamSelectedValue to 1, because this is seam1
            _iSeamTemplateSelectedValue = 0;     // Set iSeamTemplateSelectedValue to 0. Zero is an empty row from the database. 
            OnPropertyChanged("iSeamTemplateSelectedValue");
            // Todo updateSeamTemplateDetails(_iSeamTemplateSelectedValue, false);

            xProfileNewMode = true;             // Set xProfileNewMode to true
        }
        private void saveProfile()
        {
            if (xProfileNewMode)
            {
                saveNewProfile();
                reloadProfilesEditor();         // Reload new data from database
                selectLastProfile();                    // Select last row from database, because an new profile is added, which is the last entry
                profileIsChanged();   // Update all values
            }
            if (xProfileChangeMode)
            {
                saveEditedProfile();
                reloadProfilesEditor();      
                profileIsChanged();
            }
            _sfilterSeamTemplateOptions = "";
            OnPropertyChanged("sfilterSeamTemplateOptions");
        }
        private void cancelProfile()
        {
            xProfileNewMode = false;
            xProfileChangeMode = false;

            iCopyProfileSelectedValue = 0;

            _sfilterSeamTemplateOptions = "";
            OnPropertyChanged("sfilterSeamTemplateOptions");

            reloadProfilesEditor();                     // Must be called, so reinitialize the combobox.. A possible search is used
            profileIsChanged();
        }
        private void editProfile()
        {
            xProfileChangeMode = true;         // Make input field editable. 
            profileIsChanged();
            fillSeamTemplateOptions();
            OnPropertyChanged("iSeamTemplateSelectedValue");        // Combobox is changed, always regenerate a OnPropertyChanged for iSeamTemplateSelectedValue
        }
        private void deleteProfile()    // Delete profile from database. TODO, beveiliging. Controleren of profiel gebruikt wordt in bovenliggende DB
        {
            //if (databaseConnectionIsValid())
            //{
                try
                {
                    var deleteProfileQuery = from x in ViewModelControlPanel.DBDataClass.profiles
                                             where x.profileId == clCurrentProfile.profileId
                                             select x;

                    foreach (var q in deleteProfileQuery)
                    {
                        ViewModelControlPanel.DBDataClass.profiles.DeleteOnSubmit(q);
                    }
                    ViewModelControlPanel.DBDataClass.SubmitChanges();
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase"));
                }
                selectFirstProfile();
                cancelProfile();
            //}
        }
        private void copyProfile()      // 
        {
            controlCopyProfilePopUp(false);
            updateProfileDetails(iCopyProfileSelectedValue, true);
            iCopyProfileSelectedValue = 0;
        }

        private void newSeamTemplate()
        {
            xSeamTemplateNewMode = true;
            _iSeamTemplateSelectedValue = 0;
            OnPropertyChanged("iSeamTemplateSelectedValue");

            updateSeamTemplateDetails(iSeamTemplateSelectedValue, true);
        }
        private void saveSeamTemplate()
        {
            if (xSeamTemplateNewMode)
            {
                saveNewSeamTemplate();
            }
            if (xSeamTemplateChangeMode)
            {
                saveEditedSeamTemplate();
            }

            reloadProfilesEditor();
            profileIsChanged();
            _sfilterSeamTemplateOptions = "";
            OnPropertyChanged("sfilterSeamTemplateOptions");
        }
        private void cancelSeamTemplate()
        {
            xSeamTemplateChangeMode = false;
            xSeamTemplateNewMode = false;

            seamTemplateHasChanged();
        }
        private void editSeamTemplate()
        {
            xSeamTemplateChangeMode = true;
        }
        private void deleteSeamTemplate()       // Delete an existing seam template
        {
            // TODO try catch
            // Check if this template isn't used in one or more profiles 
            var getSeamTemplateQuery = from x in ViewModelControlPanel.DBDataClass.profiles
                                       where x.seam1_id == iSeamTemplateSelectedValue ||
                                             x.seam2_id == iSeamTemplateSelectedValue ||
                                             x.seam3_id == iSeamTemplateSelectedValue ||
                                             x.seam4_id == iSeamTemplateSelectedValue ||
                                             x.seam5_id == iSeamTemplateSelectedValue ||
                                             x.seam6_id == iSeamTemplateSelectedValue ||
                                             x.seam7_id == iSeamTemplateSelectedValue ||
                                             x.seam8_id == iSeamTemplateSelectedValue ||
                                             x.seam9_id == iSeamTemplateSelectedValue ||
                                             x.seam10_id == iSeamTemplateSelectedValue ||
                                             x.seam11_id == iSeamTemplateSelectedValue ||
                                             x.seam12_id == iSeamTemplateSelectedValue ||
                                             x.seam13_id == iSeamTemplateSelectedValue ||
                                             x.seam14_id == iSeamTemplateSelectedValue ||
                                             x.seam15_id == iSeamTemplateSelectedValue ||
                                             x.seam16_id == iSeamTemplateSelectedValue
                                       select x;
            if (getSeamTemplateQuery.Any()) // if used, collect names from profiles where this seamtemplate is used
            {
                string sMessageBoxText = clLanguages.getName("__puCanNotDelete") + "\n";
                sMessageBoxText += clLanguages.getName("__puIsUsedIn") + "\n\n";
                foreach (var q in getSeamTemplateQuery)
                {
                    sMessageBoxText += q.name + ", " + q.comment + "\n";
                }
                sMessageBoxText += "\n\n" + clLanguages.getName("__puSelectOtherSeamTemplate");
                System.Windows.MessageBox.Show(sMessageBoxText);
            }
            else
            {
                var deleteSeamTemplateQuery = from x in ViewModelControlPanel.DBDataClass.seamTemplates
                                              where x.Id == clCurrentSeamTemplate.Id
                                              select x;
                foreach (var q in deleteSeamTemplateQuery)
                {
                    ViewModelControlPanel.DBDataClass.seamTemplates.DeleteOnSubmit(q);
                }
                ViewModelControlPanel.DBDataClass.SubmitChanges();
                reloadProfilesEditor();
            }
            cancelSeamTemplate();
        }
        private void copySeamTemplate()
        {
            controlCopySeamTemplatePopUp(false);        // Hide the popup screen

            updateSeamTemplateDetails(iCopySeamTemplateSelectedValue, true);       // Update seamTemplate properties with copied template
            iCopySeamTemplateSelectedValue = 0;
        }

        private void initializeProfilesEditor()
        {
            reloadProfilesEditor();
            _iSeamSelectedValue = 1;     // On startup, select first seam, which is seam1
            OnPropertyChanged("iSeamSelectedValue");

            selectFirstProfile();
            profileIsChanged();
        }

        private void selectFirstProfile()       // select first row from database
        {
            if (clCurrentProfile.obcProfileOptions.Count > 0)
            {
                _iprofileSelectedValue = clCurrentProfile.obcProfileOptions[0].iSelectedvaluePath;
                OnPropertyChanged("iprofileSelectedValue");
            }
        }
        private void selectLastProfile()        // select last row from database
        {
            if (clCurrentProfile.obcProfileOptions.Count > 0)
            {
                _iprofileSelectedValue = clCurrentProfile.obcProfileOptions[clCurrentProfile.obcProfileOptions.Count - 1].iSelectedvaluePath;
                OnPropertyChanged("iprofileSelectedValue");
            }
        }

        private void saveEditedProfile()            // Save an edited profile. 
        {
            //if (databaseConnectionIsValid())
            //{
                try
                {
                    profile editedProfile = ViewModelControlPanel.DBDataClass.profiles.Single(c => c.profileId == clCurrentProfile.profileId);  // Select the right row in the database

                    editedProfile.name = clCurrentProfile.name;
                    editedProfile.comment = clCurrentProfile.comment;
                    editedProfile.active = clCurrentProfile.active;
                    editedProfile.seam1_id = clCurrentProfile.ocSeamList[1];
                    editedProfile.seam2_id = clCurrentProfile.ocSeamList[2];
                    editedProfile.seam3_id = clCurrentProfile.ocSeamList[3];
                    editedProfile.seam4_id = clCurrentProfile.ocSeamList[4];
                    editedProfile.seam5_id = clCurrentProfile.ocSeamList[5];
                    editedProfile.seam6_id = clCurrentProfile.ocSeamList[6];
                    editedProfile.seam7_id = clCurrentProfile.ocSeamList[7];
                    editedProfile.seam8_id = clCurrentProfile.ocSeamList[8];
                    editedProfile.seam9_id = clCurrentProfile.ocSeamList[9];
                    editedProfile.seam10_id = clCurrentProfile.ocSeamList[10];
                    editedProfile.seam11_id = clCurrentProfile.ocSeamList[11];
                    editedProfile.seam12_id = clCurrentProfile.ocSeamList[12];
                    editedProfile.seam13_id = clCurrentProfile.ocSeamList[13];
                    editedProfile.seam14_id = clCurrentProfile.ocSeamList[14];
                    editedProfile.seam15_id = clCurrentProfile.ocSeamList[15];
                    editedProfile.seam16_id = clCurrentProfile.ocSeamList[16];

                    ViewModelControlPanel.DBDataClass.SubmitChanges();
                    xProfileChangeMode = false;
                }
                catch
                {
                    xProfileChangeMode = false;
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase"));
                }
            //}
        }
        private void saveNewProfile()               // Save a new profile to the database
        {
            //if (databaseConnectionIsValid())
            //{
                try
                {
                    profile newProfile = new profile();     // Create a new instance of profile

                    newProfile.name = clCurrentProfile.name;        // Copy current properties to the new profile
                    newProfile.comment = clCurrentProfile.comment;
                    newProfile.active = clCurrentProfile.active;
                    newProfile.seam1_id = clCurrentProfile.ocSeamList[1];
                    newProfile.seam2_id = clCurrentProfile.ocSeamList[2];
                    newProfile.seam3_id = clCurrentProfile.ocSeamList[3];
                    newProfile.seam4_id = clCurrentProfile.ocSeamList[4];
                    newProfile.seam5_id = clCurrentProfile.ocSeamList[5];
                    newProfile.seam6_id = clCurrentProfile.ocSeamList[6];
                    newProfile.seam7_id = clCurrentProfile.ocSeamList[7];
                    newProfile.seam8_id = clCurrentProfile.ocSeamList[8];
                    newProfile.seam9_id = clCurrentProfile.ocSeamList[9];
                    newProfile.seam10_id = clCurrentProfile.ocSeamList[10];
                    newProfile.seam11_id = clCurrentProfile.ocSeamList[11];
                    newProfile.seam12_id = clCurrentProfile.ocSeamList[12];
                    newProfile.seam13_id = clCurrentProfile.ocSeamList[13];
                    newProfile.seam14_id = clCurrentProfile.ocSeamList[14];
                    newProfile.seam15_id = clCurrentProfile.ocSeamList[15];
                    newProfile.seam16_id = clCurrentProfile.ocSeamList[16];

                    ViewModelControlPanel.DBDataClass.profiles.InsertOnSubmit(newProfile);        // Insert new profile   
                    ViewModelControlPanel.DBDataClass.SubmitChanges();

                    xProfileNewMode = false;            // Disable new profile mode
                }
                catch
                {
                    xProfileNewMode = false;            // Disable new profile mode
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase"));
                }
           // }
        }
        private void saveEditedSeamTemplate()       // Save an edited Seam template
        {
            //if (databaseConnectionIsValid())
            //{
                try
                {
                    seamTemplate editedSeamTemplate = ViewModelControlPanel.DBDataClass.seamTemplates.Single(c => c.Id == iSeamTemplateSelectedValue);    // Select the right row

                    editedSeamTemplate.name = clCurrentSeamTemplate.name;
                    editedSeamTemplate.comment = clCurrentSeamTemplate.comment;
                    editedSeamTemplate.active = clCurrentSeamTemplate.active;
                    editedSeamTemplate.minTens = clCurrentSeamTemplate.minTens;
                    editedSeamTemplate.maxTens = clCurrentSeamTemplate.maxTens;
                    editedSeamTemplate.tensFilter = clCurrentSeamTemplate.tensFilter;
                    editedSeamTemplate.blindTens = clCurrentSeamTemplate.blindTens;
                    editedSeamTemplate.stitchCount = clCurrentSeamTemplate.stitchCount;
                    editedSeamTemplate.posTol = clCurrentSeamTemplate.posTol;
                    editedSeamTemplate.negTol = clCurrentSeamTemplate.negTol;
                    editedSeamTemplate.blindArea = clCurrentSeamTemplate.blindArea;
                    editedSeamTemplate.seamLen = clCurrentSeamTemplate.seamLen;
                    editedSeamTemplate.stitchLen = clCurrentSeamTemplate.stitchLen;
                    editedSeamTemplate.speed = clCurrentSeamTemplate.speed;
                    editedSeamTemplate.startBtFw = clCurrentSeamTemplate.startBtFw;
                    editedSeamTemplate.startBtBw = clCurrentSeamTemplate.startBtBw;
                    editedSeamTemplate.endBtFw = clCurrentSeamTemplate.endBtFw;
                    editedSeamTemplate.endBtBw = clCurrentSeamTemplate.endBtBw;

                    // Each option is stored in one word. Select the right bits.
                    editedSeamTemplate.functions = (short)
                                                        (
                                                                clCurrentSeamTemplate.clFunctionsPedalOptions.iSelectedValue |
                                                                (clCurrentSeamTemplate.clFunctionsVRUOptions.iSelectedValue << 3) |
                                                                (clCurrentSeamTemplate.clFunctionsNHTOptions.iSelectedValue << 5) |
                                                                (clCurrentSeamTemplate.clFunctionsZVROptions.iSelectedValue << 7) |
                                                                (clCurrentSeamTemplate.clFunctionsSTLOptions.iSelectedValue << 9) |
                                                                (clCurrentSeamTemplate.clFunctionsFSPROptions.iSelectedValue << 11)
                                                        );
                    // Each option is stored in one word. Select the right bits.
                    editedSeamTemplate.monitoring = (short)
                                                        (
                                                                clCurrentSeamTemplate.clMonitoringBobbinOptions.iSelectedValue |
                                                                (clCurrentSeamTemplate.clMonitoringCoverOptions.iSelectedValue << 2) |
                                                                (clCurrentSeamTemplate.clMonitoringEltexOptions.iSelectedValue << 4)
                                                        );
                    // Each option is stored in one word. Select the right bits.
                    editedSeamTemplate.general = (short)
                                                        (
                                                                clCurrentSeamTemplate.clGeneralSeamOptions.iSelectedValue |
                                                                (clCurrentSeamTemplate.clGeneralTransitOptions.iSelectedValue << 2) |
                                                                (clCurrentSeamTemplate.clGeneralScanbackOptions.iSelectedValue << 4)
                                                        );

                    editedSeamTemplate.printLabelActive = clCurrentSeamTemplate.printLabelActive;
                    editedSeamTemplate.labelPrtPos = clCurrentSeamTemplate.labelPrtPos;
                    editedSeamTemplate.labelFile = clCurrentSeamTemplate.labelFile;

                    ViewModelControlPanel.DBDataClass.SubmitChanges();

                    //profileIsChanged(iprofileSelectedValue);
                    //iSeamTemplateSelectedValue = editedSeamTemplate.Id;     // Get the ID from the new inserted row, and select it..
                    xSeamTemplateChangeMode = false;                        // Disable template change mode.
                }
                catch
                {
                    xSeamTemplateChangeMode = false;                        // Disable template change mode.
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase"));
                }
            //}
        }
        private void saveNewSeamTemplate()          // Save new template to database
        {
            //if (databaseConnectionIsValid())
            //{
                try
                {
                    seamTemplate newSeamTemplate = new seamTemplate();

                    newSeamTemplate.name = clCurrentSeamTemplate.name;
                    newSeamTemplate.comment = clCurrentSeamTemplate.comment;
                    newSeamTemplate.active = clCurrentSeamTemplate.active;
                    newSeamTemplate.minTens = clCurrentSeamTemplate.minTens;
                    newSeamTemplate.maxTens = clCurrentSeamTemplate.maxTens;
                    newSeamTemplate.tensFilter = clCurrentSeamTemplate.tensFilter;
                    newSeamTemplate.blindTens = clCurrentSeamTemplate.blindTens;
                    newSeamTemplate.stitchCount = clCurrentSeamTemplate.stitchCount;
                    newSeamTemplate.posTol = clCurrentSeamTemplate.posTol;
                    newSeamTemplate.negTol = clCurrentSeamTemplate.negTol;
                    newSeamTemplate.blindArea = clCurrentSeamTemplate.blindArea;
                    newSeamTemplate.seamLen = clCurrentSeamTemplate.seamLen;
                    newSeamTemplate.stitchLen = clCurrentSeamTemplate.stitchLen;
                    newSeamTemplate.speed = clCurrentSeamTemplate.speed;
                    newSeamTemplate.startBtFw = clCurrentSeamTemplate.startBtFw;
                    newSeamTemplate.startBtBw = clCurrentSeamTemplate.startBtBw;
                    newSeamTemplate.endBtFw = clCurrentSeamTemplate.endBtFw;
                    newSeamTemplate.endBtBw = clCurrentSeamTemplate.endBtBw;

                    newSeamTemplate.functions = clCurrentSeamTemplate.functions;
                    newSeamTemplate.monitoring = clCurrentSeamTemplate.monitoring;
                    newSeamTemplate.general = clCurrentSeamTemplate.general;

                    newSeamTemplate.printLabelActive = clCurrentSeamTemplate.printLabelActive;
                    newSeamTemplate.labelPrtPos = clCurrentSeamTemplate.labelPrtPos;
                    newSeamTemplate.labelFile = clCurrentSeamTemplate.labelFile;

                    ViewModelControlPanel.DBDataClass.seamTemplates.InsertOnSubmit(newSeamTemplate);
                    ViewModelControlPanel.DBDataClass.SubmitChanges();

                    xSeamTemplateNewMode    = false;            // Deactivate new mode, 
                    xSeamTemplateChangeMode = true;             // And go to edit mode..

                    //profileIsChanged(iprofileSelectedValue);
                    _iSeamTemplateSelectedValue = newSeamTemplate.Id;    // Get (inserted) new ID from database
                    OnPropertyChanged("iSeamTemplateSelectedValue");
                }
                catch
                {
                    xSeamTemplateNewMode    = false;            
                    xSeamTemplateChangeMode = false;             
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase"));
                }
            //}
        }

        private void reloadProfilesEditor()                   // Reload data from database everytime when something is changed. 
        {
            try
            {
                ViewModelControlPanel.reloadDatabase();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(clLanguages.getName("__NoDatabaseConnection"));
            }

            fillProfileOptions();                                   // Fill profile combobox with items
            fillSeamTemplateOptions();                              // Fill Seamoptions combobox with items
        }

        private void profileIsChanged()
        {
            updateProfileDetails(iprofileSelectedValue, false);
            updateSeamTemplateDetails(iSeamTemplateSelectedValue, false);

            _sfilterSeamTemplateOptions = "";
            OnPropertyChanged("sfilterSeamTemplateOptions");

            _sfilterProfileOptions = "";
            OnPropertyChanged("sfilterProfileOptions");
        }

        private void seamTemplateHasChanged()
        {
            updateSeamTemplateDetails(iSeamTemplateSelectedValue, false);

            _sfilterSeamTemplateOptions = "";
            OnPropertyChanged("sfilterSeamTemplateOptions");
        }
        private void updateProfileDetails(int iIdNumber, bool xIsNew)   // Update profile details from database. 
        {
            //if (databaseConnectionIsValid())
            //{
                try
                {
                    var profileQuery = from x in ViewModelControlPanel.DBDataClass.profiles
                                       where x.profileId == iIdNumber      // select Id number of the current index in query
                                       select x;

                    foreach (var q in profileQuery)
                    {
                        if (!xIsNew)                                             // Do not copy name and comment to current class when a class is copied. 
                        {
                            clCurrentProfile.profileId = q.profileId;
                            clCurrentProfile.sName = q.name.ToString();
                            clCurrentProfile.sComment = q.comment.ToString();
                        }

                        clCurrentProfile.active = (bool)q.active;
                        clCurrentProfile.ocSeamList[1] = (short)q.seam1_id;     // Start with 1, this is easyer to read. 
                        clCurrentProfile.obcSeamOptions[0].xSeamNotEmpty = clCurrentProfile.ocSeamList[1] > 0;
                        clCurrentProfile.ocSeamList[2] = (short)q.seam2_id;
                        clCurrentProfile.obcSeamOptions[1].xSeamNotEmpty = clCurrentProfile.ocSeamList[2] > 0;
                        clCurrentProfile.ocSeamList[3] = (short)q.seam3_id;
                        clCurrentProfile.obcSeamOptions[2].xSeamNotEmpty = clCurrentProfile.ocSeamList[3] > 0;
                        clCurrentProfile.ocSeamList[4] = (short)q.seam4_id;
                        clCurrentProfile.obcSeamOptions[3].xSeamNotEmpty = clCurrentProfile.ocSeamList[4] > 0;
                        clCurrentProfile.ocSeamList[5] = (short)q.seam5_id;
                        clCurrentProfile.obcSeamOptions[4].xSeamNotEmpty = clCurrentProfile.ocSeamList[5] > 0;
                        clCurrentProfile.ocSeamList[6] = (short)q.seam6_id;
                        clCurrentProfile.obcSeamOptions[5].xSeamNotEmpty = clCurrentProfile.ocSeamList[6] > 0;
                        clCurrentProfile.ocSeamList[7] = (short)q.seam7_id;
                        clCurrentProfile.obcSeamOptions[6].xSeamNotEmpty = clCurrentProfile.ocSeamList[7] > 0;
                        clCurrentProfile.ocSeamList[8] = (short)q.seam8_id;
                        clCurrentProfile.obcSeamOptions[7].xSeamNotEmpty = clCurrentProfile.ocSeamList[8] > 0;
                        clCurrentProfile.ocSeamList[9] = (short)q.seam9_id;
                        clCurrentProfile.obcSeamOptions[8].xSeamNotEmpty = clCurrentProfile.ocSeamList[9] > 0;
                        clCurrentProfile.ocSeamList[10] = (short)q.seam10_id;
                        clCurrentProfile.obcSeamOptions[9].xSeamNotEmpty = clCurrentProfile.ocSeamList[10] > 0;
                        clCurrentProfile.ocSeamList[11] = (short)q.seam11_id;
                        clCurrentProfile.obcSeamOptions[10].xSeamNotEmpty = clCurrentProfile.ocSeamList[11] > 0;
                        clCurrentProfile.ocSeamList[12] = (short)q.seam12_id;
                        clCurrentProfile.obcSeamOptions[11].xSeamNotEmpty = clCurrentProfile.ocSeamList[12] > 0;
                        clCurrentProfile.ocSeamList[13] = (short)q.seam13_id;
                        clCurrentProfile.obcSeamOptions[12].xSeamNotEmpty = clCurrentProfile.ocSeamList[13] > 0;
                        clCurrentProfile.ocSeamList[14] = (short)q.seam14_id;
                        clCurrentProfile.obcSeamOptions[13].xSeamNotEmpty = clCurrentProfile.ocSeamList[14] > 0;
                        clCurrentProfile.ocSeamList[15] = (short)q.seam15_id;
                        clCurrentProfile.obcSeamOptions[14].xSeamNotEmpty = clCurrentProfile.ocSeamList[15] > 0;
                        clCurrentProfile.ocSeamList[16] = (short)q.seam16_id;
                        clCurrentProfile.obcSeamOptions[15].xSeamNotEmpty = clCurrentProfile.ocSeamList[16] > 0;

                        _iSeamTemplateSelectedValue = clCurrentProfile.ocSeamList[iSeamSelectedValue];   // Update iSeamTemplateSelectedValue, because the profile is changed
                        OnPropertyChanged("iSeamTemplateSelectedValue");
                    }
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase"));
                }
           // }
        }
        private void updateSeamTemplateDetails(int iSelectedValue, bool xIsCopy) // Update current SeamDetails
        {
            //if (databaseConnectionIsValid())
            //{
                try
                {
                    var seamTemplateQuery = from x in ViewModelControlPanel.DBDataClass.seamTemplates
                                            where x.Id == iSelectedValue
                                            select x;

                    foreach (var q in seamTemplateQuery)
                    {
                        if (iSelectedValue == 0)        // When a new template is choosen, name and comment are ""
                        {
                            clCurrentSeamTemplate.Id        = q.Id;
                            clCurrentSeamTemplate.name      = "";
                            clCurrentSeamTemplate.comment   = "";
                        }
                        else
                        {
                            if (!xIsCopy)
                            {
                                clCurrentSeamTemplate.Id        = q.Id;
                                clCurrentSeamTemplate.name      = q.name;
                                clCurrentSeamTemplate.comment   = q.comment;
                            }
                        }
                        clCurrentSeamTemplate.active            = q.active;
                        clCurrentSeamTemplate.sMinTens          = q.minTens.ToString();
                        clCurrentSeamTemplate.sMaxTens          = q.maxTens.ToString();
                        clCurrentSeamTemplate.sTensFilter       = q.tensFilter.ToString();
                        clCurrentSeamTemplate.sBlindTens        = q.blindTens.ToString();
                        clCurrentSeamTemplate.sStitchCount      = q.stitchCount.ToString();
                        clCurrentSeamTemplate.sPosTol           = q.posTol.ToString();
                        clCurrentSeamTemplate.sNegTol           = q.negTol.ToString();
                        clCurrentSeamTemplate.sBlindArea        = q.blindArea.ToString();
                        clCurrentSeamTemplate.sSeamLen          = q.seamLen.ToString();
                        clCurrentSeamTemplate.sStitchLen        = q.stitchLen.ToString();
                        clCurrentSeamTemplate.sSpeed            = q.speed.ToString();
                        clCurrentSeamTemplate.sStartBtFw        = q.startBtFw.ToString();
                        clCurrentSeamTemplate.sStartBtBw        = q.startBtBw.ToString();
                        clCurrentSeamTemplate.sEndBtFw          = q.endBtFw.ToString();
                        clCurrentSeamTemplate.sEndBtBw          = q.endBtBw.ToString();

                        clCurrentSeamTemplate.functions         = q.functions;
                        clCurrentSeamTemplate.monitoring        = q.monitoring;
                        clCurrentSeamTemplate.general           = q.general;

                        clCurrentSeamTemplate.clFunctionsPedalOptions.iSelectedValue = ((int)clCurrentSeamTemplate.functions & 7) >> 0;
                        clCurrentSeamTemplate.clFunctionsVRUOptions.iSelectedValue = ((int)clCurrentSeamTemplate.functions & 24) >> 3;
                        clCurrentSeamTemplate.clFunctionsNHTOptions.iSelectedValue = ((int)clCurrentSeamTemplate.functions & 96) >> 5;
                        clCurrentSeamTemplate.clFunctionsZVROptions.iSelectedValue = ((int)clCurrentSeamTemplate.functions & 384) >> 7;
                        clCurrentSeamTemplate.clFunctionsSTLOptions.iSelectedValue = ((int)clCurrentSeamTemplate.functions & 1536) >> 9;
                        clCurrentSeamTemplate.clFunctionsFSPROptions.iSelectedValue = ((int)clCurrentSeamTemplate.functions & 6144) >> 11;

                        clCurrentSeamTemplate.clMonitoringBobbinOptions.iSelectedValue = ((int)clCurrentSeamTemplate.monitoring & 3) >> 0;
                        clCurrentSeamTemplate.clMonitoringCoverOptions.iSelectedValue = ((int)clCurrentSeamTemplate.monitoring & 12) >> 2;
                        clCurrentSeamTemplate.clMonitoringEltexOptions.iSelectedValue = ((int)clCurrentSeamTemplate.monitoring & 48) >> 4;

                        clCurrentSeamTemplate.clGeneralSeamOptions.iSelectedValue = ((int)clCurrentSeamTemplate.general & 3) >> 0;
                        clCurrentSeamTemplate.clGeneralTransitOptions.iSelectedValue = ((int)clCurrentSeamTemplate.general & 12) >> 2;
                        clCurrentSeamTemplate.clGeneralScanbackOptions.iSelectedValue = ((int)clCurrentSeamTemplate.general & 48) >> 4;

                        clCurrentSeamTemplate.printLabelActive = q.printLabelActive;
                        clCurrentSeamTemplate.labelPrtPos = q.labelPrtPos;
                        clCurrentSeamTemplate.labelFile = q.labelFile;

                        // Each option is stored in one word. Select the right bits. 
                    }
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorReadingDatabase"));
                }
                if (xProfileEditMode)        // Only update this value when in edit mode..
                    clCurrentProfile.ocSeamList[iSeamSelectedValue] = (short)iSelectedValue;
            //}

        }
        private int getCurrentSeamTemplateID(int iSelectedValue)        // Returns the current ID from database
        {
            int iID = 0;
            //if (databaseConnectionIsValid())
            //{
                try
                {
                    var getCurrentIDQuery = from x in ViewModelControlPanel.DBDataClass.seamTemplates
                                            where x.Id == clCurrentProfile.ocSeamList[iSelectedValue]
                                            select x;

                    foreach (var q in getCurrentIDQuery)
                    {
                        iID = q.Id;
                    }
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorReadingDatabase"));
                }
            //}
            return iID;
        }

        private void fillProfileOptions(string sFilter = "")        // This function fills the combobox with data, regarding the given filter. 
        {
                try
                {
                    foreach (var q in clCurrentProfile.obcProfileOptions.ToList())          // Delete all items in this mirrow database list
                    {
                        clCurrentProfile.obcProfileOptions.Remove(q);                       // Remove all database data from observableobject. 
                    }

                    var profileQuery = from x in ViewModelControlPanel.DBDataClass.profiles
                                       where x.name.Contains(sFilter) || x.comment.Contains(sFilter)
                                       orderby x.profileId ascending
                                       select x;

                    foreach (var q in profileQuery)
                    {
                        clCurrentProfile.obcProfileOptions.Add(new clCbbFilltype1(q.name + ",  " + q.comment, q.profileId));      // Add database data to observableobject. 
                    }
                    if (clCurrentProfile.obcProfileOptions.Count > 0 && sFilter != "")
                    {
                        _iprofileSelectedValue = clCurrentProfile.obcProfileOptions[0].iSelectedvaluePath;      // Set value to first search item, but only when something has been found during search
                        OnPropertyChanged("iprofileSelectedValue");
                    }
                    else
                    {
                        OnPropertyChanged("iprofileSelectedValue");
                    }
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorReadingDatabase"));
                }
        }
        private void fillSeamTemplateOptions(string sFilter = "")       // Fill seamtemplate options to the list, with optional filter 
        {
            //if (databaseConnectionIsValid())
            //{
                try
                {
                    // This function is neccesary when a new seam template is added to the database. 
                    foreach (var q in clCurrentSeamTemplate.obcSeamTemplateOptions.ToList())           // Delete all items in this seam template list
                    {
                        clCurrentSeamTemplate.obcSeamTemplateOptions.Remove(q);
                    }

                    if (xProfileEditMode)   // When in edit mode, only get result where template is active..
                    {
                        var Query = from x in ViewModelControlPanel.DBDataClass.seamTemplates             // Query to find all seamTemplates, with filter
                                    where ((x.name.Contains(sFilter) || x.comment.Contains(sFilter)) && x.active == true)
                                    orderby x.Id ascending
                                    select x;
                        foreach (var q in Query)
                        {
                            clCurrentSeamTemplate.obcSeamTemplateOptions.Add(new clCbbFilltype1(q.name, q.Id));    // iSeamTemplateSelectedValue (q.Id) is the ID number, so it can be used in a query 
                        }
                    }
                    else
                    {
                        var Query1 = from x in ViewModelControlPanel.DBDataClass.seamTemplates             // Query to find all seamTemplates, with filter
                                     where (x.name.Contains(sFilter) || x.comment.Contains(sFilter))
                                     orderby x.Id ascending
                                     select x;
                        foreach (var q in Query1)
                        {
                            clCurrentSeamTemplate.obcSeamTemplateOptions.Add(new clCbbFilltype1(q.name, q.Id));    // iSeamTemplateSelectedValue (q.Id) is the ID number, so it can be used in a query 
                        }
                    }

                    if (clCurrentSeamTemplate.obcSeamTemplateOptions.Count > 0 && sFilter != "")
                    {
                        _iSeamTemplateSelectedValue = clCurrentSeamTemplate.obcSeamTemplateOptions[0].iSelectedvaluePath;      // Set value to first search item, but only when something has been found during search
                        OnPropertyChanged("iSeamTemplateSelectedValue");
                    }
                    else
                    {
                        OnPropertyChanged("iSeamTemplateSelectedValue");
                    }
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorReadingDatabase"));
                }
            //}
        }

        private void openFolderDialog()             // Open browse folder, to select folder where the label is located 
        {
            FolderBrowserDialog fbdLabelPath = new FolderBrowserDialog();
            fbdLabelPath.SelectedPath = clCurrentSeamTemplate.labelFile;      // Select initial path to folder browser 
            if (fbdLabelPath.ShowDialog() == System.Windows.Forms.DialogResult.OK)      // When 'OK' buttun is pressed
            {
                clCurrentSeamTemplate.labelFile = fbdLabelPath.SelectedPath;            // Save path to current template
            }
        }

        private void controlCopyProfilePopUp(bool xStatus, string sFilter = "")          // Control copy profile popup
        {
            foreach (var q in obcCopyProfileOption.ToList()) // Delete all current options from list
            {
                obcCopyProfileOption.Remove(q);
            }
            if (xStatus) // Is enabled, select first item
            {
                try
                {
                    var profileQuery = from x in ViewModelControlPanel.DBDataClass.profiles
                                        where x.name.Contains(sFilter) || x.comment.Contains(sFilter)
                                        orderby x.profileId ascending
                                        select x;
                    foreach (var q in profileQuery)
                    {
                        obcCopyProfileOption.Add(new clCbbFilltype1(clLanguages.getName("__Name") + ":\t\t" + q.name + "\n" + clLanguages.getName("__Comment") + ":\t" + q.comment, q.profileId));
                    }
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase"));
                }
                if (obcCopyProfileOption.Count() > 0)
                    iCopyProfileSelectedValue = obcCopyProfileOption[0].iSelectedvaluePath;
                else
                    iCopyProfileSelectedValue = 0;
            }

            xShowPopupCopyProfile = xStatus;
        }

        private void controlCopySeamTemplatePopUp(bool xStatus, string sFilter = "")     // Control the copy seamTemplate popup
        {
            foreach (var q in obcCopySeamTemplateOption.ToList())   // Delete all options from the list
            {
                obcCopySeamTemplateOption.Remove(q);
            }

            if (xStatus) // Is enabled, select first item
            {
                try
                {
                    var seamTemplateQuery = from x in ViewModelControlPanel.DBDataClass.seamTemplates
                                            orderby x.Id ascending
                                            where x.name.Contains(sFilter) || x.comment.Contains(sFilter)
                                            select x;
                    foreach (var q in seamTemplateQuery)
                    {
                        obcCopySeamTemplateOption.Add(new clCbbFilltype1(clLanguages.getName("__Name") + ":\t\t" + q.name + "\n" + clLanguages.getName("__Comment") + ":\t" + q.comment, q.Id));
                    }
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(clLanguages.getName("__ErrorReadingDatabase"));
                }
                if (obcCopySeamTemplateOption.Count() > 0)
                    iCopySeamTemplateSelectedValue = obcCopySeamTemplateOption[0].iSelectedvaluePath;
                else
                    iCopySeamTemplateSelectedValue = 0;
            }
            xShowPopupCopySeamTemplate = xStatus;
        }
        private void controlEditSeamTemplatePopUp(bool xStatus)
        {
            xShowPopupEditSeamTemplate = xStatus;                  // Show popup screen
        }

        private void emptyCurrentProfile()
        {
            clCurrentProfile.sName = "";
            clCurrentProfile.sComment = "";
            clCurrentProfile.active = true;

            for (int i = 1; i <= 16; i++)
            {
                clCurrentProfile.ocSeamList[i] = 0;
            }
            for (int i = 0; i <= 15; i++)
            {
                clCurrentProfile.obcSeamOptions[i].xSeamNotEmpty = false;
            }

        }
        #endregion
    }
}
