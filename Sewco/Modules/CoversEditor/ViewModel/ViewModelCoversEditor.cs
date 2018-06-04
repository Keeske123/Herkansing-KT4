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

namespace Sewco.Modules.CoversEditor
{
    class ViewModelCovers : ObservableObject, ifSewcoMenu
    {
        public ViewModelCovers()
        {
            if (ViewModelControlPanel.xValidDatabaseConnection)
            {
                xEnableScreen       = true;
                xEnableCoverMenu  = false;
                #region RelayCommands
                newObjectCommand = new RelayCommand(
                        param => newObject(),      
                        param => (!xEditObjectMode)            // Only visible when not in New of Change mode
                    );
                saveObjectCommand = new RelayCommand(
                        param => saveObject(),
                        param => ((xChangeObjectMode || xNewObjectMode) && !xContentHasError)
                    );
                cancelObjectCommand = new RelayCommand(
                        param => cancelObject(),                 
                        param => (xEditObjectMode)            // Is visible in cover edit mode.. (New and change mode)
                    );
                editObjectCommand = new RelayCommand(
                        param => editObject(),        
                        param => (!xEditObjectMode && coversModel.obcCovers.Count > 0)           // (!xEditMode) Visible when not in edit mode.         
                    );
                deleteObjectCommand = new RelayCommand(
                        param => deleteObject(), 
                        param => (xChangeObjectMode)                // Visible when not in edit mode..
                    );
                copyObjectCommand = new RelayCommand(
                        param => controlCopyPopUpObject(true),      // Show popup screen, to copy a current Project
                        param => (xEditObjectMode)         // Only visible when in edit mode, but covers editor has nothing to copy
                    );
                cancelCopyObjectCommand = new RelayCommand(
                        param => controlCopyPopUpObject(false),
                        param => (true)
                    );
                selectCopyObjectCommand = new RelayCommand(
                        param => copyObject(),
                        param => (iCopyObjectSelectedValue != 0)    // TODO
                    );
                selectImagePathFolderCommand = new RelayCommand(
                        param => openFileDialog(),
                        param => (true)
                    );

                #endregion RelayCommands
                coversModel = new CoversBusinessObject();


                initializeCoversEditor();
                xEnableCbbCoversCode = true;
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
                return coversModel.validationStatus(); 
            }
        }


        #region private properties declarations
        private bool    _xEnableScreen;
        private int     _iObjectSelectedValue;
        private bool    _xChangeObjectMode;
        private bool    _xNewObjectMode;
        private bool    _xEditObjectMode;
        private bool    _xShowPopupCopyObject;
        private int     _iCopyObjectSelectedValue;
        private string  _sfilterPopupCopyObject;
        private bool    _xEnableCbbCoversCode;
        private bool    _xEnableCoverMenu;
        private string  _sSearchObjectList;
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
        public RelayCommand selectImagePathFolderCommand { get; set; }

        #endregion relay commands

        public static CoversBusinessObject coversModel { get; set; }

        private bool _xEnableCoversMenu;
        public bool xEnableCoversMenu
        {
            get
            {
                return _xEnableCoversMenu;
            }
            set
            {
                _xEnableCoversMenu = value;
                OnPropertyChanged("xEnableCoversMenu");
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
                coversModel.propertyValidation(value);
                OnPropertyChanged("xEditObjectMode");
            }
        }
        public bool xEnableCbbCoversCode
        {
            get { return _xEnableCbbCoversCode; }
            set
            {
                _xEnableCbbCoversCode = value;
                OnPropertyChanged("xEnableCbbCoversCode");
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
        public string sSearchObjectList
        {
            get
            {
                return _sSearchObjectList;
            }
            set
            {
                _sSearchObjectList = value;
                OnPropertyChanged("sSearchObjectList");
                fillObjectList(value);
                objectIsChanged();
            }
        }

        public void newObject()
        {
            coversModel.emptyCover();

            xNewObjectMode = true;
            xEnableCbbCoversCode = false;
        }
        public void editObject()
        {
            xChangeObjectMode = true;         // Make input field editable. 
            //projectIsChanged();
            xEnableCbbCoversCode = false;
        }
        public void deleteObject()
        {
            coversModel.deleteCover(iObjectSelectedValue);
 
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
            xEnableCbbCoversCode = true;

            sSearchObjectList = "";
        }
        public void saveObject()
        {
            int newSelectedObjectValue = 0;
            if (xNewObjectMode)
            {
                xNewObjectMode = false;            // Disable new Project mode

                newSelectedObjectValue  = coversModel.saveNewCover();

                xEnableCbbCoversCode  = true;
                fillObjectList("");
            }
            if (xChangeObjectMode)
            {
                xChangeObjectMode = false;

                newSelectedObjectValue  = coversModel.saveEditedCover();

                xEnableCbbCoversCode  = true;
                fillObjectList("");
            }
            iObjectSelectedValue = newSelectedObjectValue;
        }
        private void selectFirstObject()
        {
            int iFirstItemId = coversModel.getFirstCoverId();

            if (iFirstItemId > 0)
            {
                _iObjectSelectedValue = iFirstItemId;
                OnPropertyChanged("iObjectSelectedValue");
            }
        }
        private void selectLastObject()        // select last row from database
        {
            int iLastItemId = coversModel.getLastCoverId();

            if (iLastItemId > 0)
            {
                iObjectSelectedValue = iLastItemId;
            }
        }
        public bool xEnableCoverMenu
        {
            get
            {
                return _xEnableCoverMenu;
            }
            set
            {
                _xEnableCoverMenu = value;
                OnPropertyChanged("xEnableCoverMenu");
            }
        }
        private void initializeCoversEditor()
        {
            fillObjectList("");
            objectIsChanged();
        }
        private void objectIsChanged()
        {
            updateObjectDetails(iObjectSelectedValue, false);
        }
        private void fillObjectList(string sFilter = "")
        {
            coversModel.getCovers(sFilter);

            if (coversModel.obcCovers.Count > 0)
            {
                xEnableCbbCoversCode  = true;
                _iObjectSelectedValue   = coversModel.obcCovers[0].iSelectedvaluePath;
                OnPropertyChanged("iObjectSelectedValue");
            } else
            {
                _iObjectSelectedValue = -1;
                OnPropertyChanged("iObjectSelectedValue");
            }
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
            coversModel.updateCoverDetails(iObjectId, xIsNew);

        }
        private void dummyFunction()
        {

        }
        private void controlCopyPopUpObject(bool xStatus, string sFilter = "")          // Control copy Project popup
        {
            if(xStatus) // Is enabled, select first item
            {
                coversModel.getFilteredCopyCover(sFilter);
                if (coversModel.obcCopyCovers.Count() > 0)
                    iCopyObjectSelectedValue = coversModel.obcCopyCovers[0].iSelectedvaluePath;
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

            fbdImagePath.InitialDirectory = sCurrentDirectory + "\\Images\\Covers\\";   // Select initial path to folder browser 
            if (fbdImagePath.ShowDialog() == DialogResult.OK)       // When 'OK' buttun is pressed
            {
                coversModel.sImagePath = fbdImagePath.FileName;    // Save path to current template
            }
        }
    }
}
