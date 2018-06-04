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
using Sewco.Modules.ProjectsEditor;
using System.IO;
using System.Data;
using System.Windows.Data;
using System.Reflection;

namespace Sewco.Modules.ProjectsEditor
{
    public class ViewModelProjects : ObservableObject
    {
        public ViewModelProjects()
        {
            if (ViewModelControlPanel.xValidDatabaseConnection)
            {
                xEnableScreen           = true;
     
                xEnableProjectMenu      = false;

                #region RelayCommands
                newProjectCommand = new RelayCommand(
                        param => newProject(),
                        param => (!xEditMode)            // Only visible when not in New of Change mode
                    );
                saveProjectCommand = new RelayCommand(
                        param => saveProject(),
                        param => (!xContentHasError) && (xChangeMode || xNewMode) &&
                        (!projectsModel.clOption1Scanback.xActive || (projectsModel.clOption1Scanback.iSelectedValue != 0 && projectsModel.clOption1Scanback.xActive)) &&
                        (!projectsModel.clOption3Bobbin.xActive || ((projectsModel.clOption3Bobbin.iSelectedValueItem1 != 0) && projectsModel.clOption3Bobbin.xActive))
                    );
                cancelProjectCommand = new RelayCommand(
                        param => cancelProject(),                   // Cancel Project editing.
                        param => (xEditMode)                 // Is visible in Project edit mode.. (New and change mode)
                    );
                editProjectCommand = new RelayCommand(
                        param => editProject(),             // Edit Project button  
                        param => (!xEditMode && projectsModel.obcProjects.Count > 0)        // (!xEditMode) Visible when not in edit mode.         
                    );
                deleteProjectCommand = new RelayCommand(
                        param => deleteProject(),           // Delete Project
                        param => (xChangeMode)       // Visible when not in edit mode.. TODO: Ook niet wanneer deze vaker gebruikt wordt in bovenliggende database..
                    );
                addIOPositionCommand = new RelayCommand(
                    param => addIOPosition(),
                    param => (true)
                );
                deleteIOPositionCommand = new RelayCommand(
                    param => deleteIOPosition(param),
                    param => (true)
                );
                firstProjectCommand = new RelayCommand(
                    param => selectFirstProject(),
                    param => (!xNewMode && !xChangeMode)
                );
                lastProjectCommand = new RelayCommand(
                        param => selectLastProject(),
                        param => (!xNewMode && !xChangeMode)
                    );
                copyProjectCommand = new RelayCommand(
                        param => controlCopyProjectPopUp(true),     // Show popup screen, to copy a current Project
                        param => (xEditMode)                 // Only visible when in edit mode
                    );
                cancelCopyProjectCommand = new RelayCommand(
                        param => controlCopyProjectPopUp(false),
                        param => (true)
                    );
                selectCopyProjectCommand = new RelayCommand(
                        param => copyProject(),
                        param => (projectsModel.obcCopyProjects.Count > 0)       
                    );
                selectImagePathFolderCommand = new RelayCommand(
                        param => openFileDialog(),
                        param => (true)
                    );

                #endregion
                projectsModel       =   new ProjectsBusinessObject();

                initializeProjectsEditor();
            }
            else
            {
                xEnableScreen = false;
            }
        }


        #region private properties declarations
        private bool    _xEnableScreen;
        private int     _iProjectSelectedValue;
        private int     _iCopyProjectSelectedValue;
        private bool    _xChangeMode;
        private bool    _xNewMode;
        private bool    _xEditMode;
        private string  _sSearchProjectList;
        private bool    _xShowPopupCopyProject;
        private bool    _xShowPopupEditProject;
        private string  _sfilterPopupCopyProject;
        private bool    _xDoubleOptionSelected;

        #endregion

        #region relayCommands declarations
        public RelayCommand newProjectCommand               { get; set; }
        public RelayCommand saveProjectCommand              { get; set; }
        public RelayCommand cancelProjectCommand            { get; set; }
        public RelayCommand editProjectCommand              { get; set; }
        public RelayCommand deleteProjectCommand            { get; set; }
        public RelayCommand copyProjectCommand              { get; set; }
        public RelayCommand cancelCopyProjectCommand        { get; set; }
        public RelayCommand selectCopyProjectCommand        { get; set; }

        public RelayCommand firstProjectCommand             { get; set; }
        public RelayCommand lastProjectCommand              { get; set; }

        public RelayCommand addIOPositionCommand            { get; set; }
        public RelayCommand deleteIOPositionCommand         { get; set; }

        public RelayCommand selectImagePathFolderCommand    { get; set; }

        public RelayCommand cbbScanbackSelectedValueChanged { get; set; }

        #endregion

        #region public properties declarations

        public static ProjectsBusinessObject projectsModel { get; set; }

        private ObservableCollection<int> _obcSelectedDeviceOptionItems { get; set; } = new ObservableCollection<int>();

        public ObservableCollection<int> obcSelectedDeviceOptionItems
        {
            get
            {
                return _obcSelectedDeviceOptionItems;
            }

            set
            {
                _obcSelectedDeviceOptionItems = value;
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
        public int iProjectSelectedValue
        {
            get
            {
                return _iProjectSelectedValue;
            }
            set
            {
                _iProjectSelectedValue = value;
                projectIsChanged();

                OnPropertyChanged("iProjectSelectedValue");
            }
        }
        public int iCopyProjectSelectedValue
        {
            get
            {
                return _iCopyProjectSelectedValue;
            }
            set
            {
                _iCopyProjectSelectedValue = value;
                OnPropertyChanged("iCopyProjectSelectedValue");
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
                projectsModel.propertyValidation(value);
                OnPropertyChanged("xEditMode");
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
                fillProjectList(value);
                updateProjectDetails(iProjectSelectedValue, false);
            }
        }
        public bool xShowPopupCopyProject
        {
            get
            {
                return _xShowPopupCopyProject;
            }
            set
            {
                _xShowPopupCopyProject = value;
                xEnableScreen = !value;
                sfilterPopupCopyProject = "<DoNothing>";
                OnPropertyChanged("xShowPopupCopyProject");
            }
        }
        public bool xShowPopupEditProject
        {
            get
            {
                return _xShowPopupEditProject;
            }
            set
            {
                _xShowPopupEditProject = value;
                xEnableScreen = !value;
                OnPropertyChanged("xShowPopupEditProject");
            }
        }
        public string sfilterPopupCopyProject
        {
            get
            {
                return _sfilterPopupCopyProject;
            }
            set
            {
                if (value != "<DoNothing>")
                {
                    controlCopyProjectPopUp(xShowPopupCopyProject, value);
                    _sfilterPopupCopyProject = value;
                }
                else
                    _sfilterPopupCopyProject = "";
                OnPropertyChanged("sfilterPopupCopyProject");
            }
        }

        private bool _xEnableProjectMenu;
        public bool xEnableProjectMenu
        {
            get
            {
                return _xEnableProjectMenu;
            }
            set
            {
                _xEnableProjectMenu = value;
                OnPropertyChanged("xEnableProjectMenu");
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
                return projectsModel.validationStatus(); //productsModel.sProductName.xHasError;
            }
        }
        private static void test()
        {
            //projectsModel.reloadFilteredComboboxes();
            //projectsModel.reloadDatabase();
        }
        private void initializeProjectsEditor()
        {
            reloadProjectsEditor    ();
            //selectFirstProject      ();
            projectIsChanged        ();
        }
        private void newProject()
        {
            projectsModel.emptyProject();
            xNewMode    = true;             
        }
        private void editProject()
        {
            xChangeMode = true;         // Make input field editable. 
            projectIsChanged();
        }
        private void deleteProject()
        {
            if (!projectsModel.deleteProject(projectsModel.projectId))
            {
                MessageBox.Show(clLanguages.getName("__ErrorWritingToDatabase"));
            }

            selectFirstProject();
            cancelProject();
        }
        private void copyProject()
        {
            controlCopyProjectPopUp(false);
            updateProjectDetails(iCopyProjectSelectedValue, true);
            iCopyProjectSelectedValue = 0;
        }
        private void saveProject()
        {
            int newSelectedProjectValue = 0;
            fillUsedIOOptions();
            if (xDoubleOptionSelected)
            {
                MessageBox.Show(clLanguages.getName("__ErrorDeviceTypeAlreadyInUse"));
            }
            else
            {
                if (xNewMode)
                {
                    xNewMode = false;            // Disable new Project mode
                    newSelectedProjectValue = projectsModel.saveNewProject();
                    reloadProjectsEditor();

                    selectLastProject();        // Select last row from database, because an new Project is added, which is the last entry
                    //projectIsChanged();         // Update all values
                }
                if (xChangeMode)
                {
                    xChangeMode = false;
                    newSelectedProjectValue = projectsModel.saveEditedProject();
                    reloadProjectsEditor();
                    //projectIsChanged();
                }
                iProjectSelectedValue = newSelectedProjectValue;

            }
        }
        private void cancelProject()
        {
            xNewMode    = false;
            xChangeMode = false;

            int iTempProjectSelectedValue = iProjectSelectedValue;
            iCopyProjectSelectedValue = 0;

            reloadProjectsEditor();                     // Must be called, so reinitialize the combobox.. A possible search is used

            iProjectSelectedValue = iTempProjectSelectedValue;
            //projectIsChanged();
        }       
        private void selectFirstProject()
        {
            int iFirstItemId = projectsModel.getFirstProjectId();

            if (iFirstItemId > 0)
            {
                _iProjectSelectedValue = iFirstItemId;
                OnPropertyChanged("iProjectSelectedValue");
                fillProductOptions(_iProjectSelectedValue);
            }
        }
        private void selectLastProject()        // select last row from database
        {
            int iLastItemId = projectsModel.getLastProjectId();

            if (iLastItemId > 0)
            {
                _iProjectSelectedValue = iLastItemId;
                OnPropertyChanged("iProjectSelectedValue");
            }
        }
        private void projectIsChanged()
        {
            updateProjectDetails(iProjectSelectedValue, false);

            _sSearchProjectList = "";
            OnPropertyChanged("sSearchProjectList");
        }
        private void updateProjectDetails(int iIdNumber, bool xIsNew)   // Update profile details from database. 
        {
            projectsModel.updateProjectDetails(iIdNumber, xIsNew);

            fillIOPositions(iIdNumber);
            fillProductOptions(iIdNumber);
            fillProjectOptions(iIdNumber);
        }
        private void fillProjectOptions(int iIdNumber)
        {
            projectsModel.getProjectOptions(iIdNumber);
        }
        private void reloadProjectsEditor()
        {
            if (!projectsModel.reloadDatabase())
            {
                System.Windows.Forms.MessageBox.Show(clLanguages.getName("__NoDatabaseConnection"));
            }

            fillProjectList();
            //projectsModel.reloadFilteredComboboxes();
            fillIOPositions(_iProjectSelectedValue);
            fillProductOptions(_iProjectSelectedValue);
            fillUsedIOOptions();
            //projectsModel.getProjectOptions(_iProjectSelectedValue); // TMA
        }

        private void fillUsedIOOptions()
        {
     
            xDoubleOptionSelected = false;
            foreach (var x in obcSelectedDeviceOptionItems.ToList())
            {
                obcSelectedDeviceOptionItems.Remove(x);
            }

            obcSelectedDeviceOptionItems.Insert(0, projectsModel.clOption1Scanback.iSelectedValue);
            obcSelectedDeviceOptionItems.Insert(1, projectsModel.clOption3Bobbin.iSelectedValueItem1);
            obcSelectedDeviceOptionItems.Insert(2, projectsModel.clOption3Bobbin.iSelectedValueItem2);

            var i = 3;
            
            
            foreach (var x in projectsModel.obcIOPositions.ToList())
            {
                obcSelectedDeviceOptionItems.Insert(i, x.iSelectedvalueDeviceType);
                i++;
            }

            var dict = new Dictionary<int, int>();

            foreach (var value in obcSelectedDeviceOptionItems.ToList())
            {
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

            if (xDoubleOptionSelected)
            {
                ;
            }
            
        }

        private void fillProjectList(string sFilter = "")
        {
            projectsModel.getProjects(sFilter);
            if (projectsModel.obcProjects == null)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingDatabase"));
            } else
            {
                if (projectsModel.obcProjects.Count > 0)// && sFilter != "")
                {
                    _iProjectSelectedValue = projectsModel.obcProjects[0].iSelectedvaluePath;      // Set value to first search item, but only when something has been found during search
                    OnPropertyChanged("iProjectSelectedValue");
                }
                else
                {
                    _iProjectSelectedValue = -1;
                    OnPropertyChanged("iProjectSelectedValue");
                }
            }
        }
        private void fillIOPositions(int iProjectId)
        {
            if (projectsModel.getIOPositions(iProjectId) == null)
            {
                MessageBox.Show(clLanguages.getName("__ErrorReadingDatabase"));
            } 
        }
        private void fillProductOptions(int iProjectId)
        {
            if(projectsModel.getProducts(iProjectId) == null)
            { 
                MessageBox.Show(clLanguages.getName("__ErrorReadingDatabase"));
            } 
        }
       
        private void addIOPosition()
        {
            projectsModel.addIOPosition();
        }
       /* private void deleteIOPosition()
        {
            projectsModel.deleteIOPosition();
        }
        */

        private void deleteIOPosition(object oSelectedValue)
        {
            int iSelectedValue = Convert.ToInt32(oSelectedValue);

            projectsModel.deleteIOPosition(iSelectedValue);
        }

        private void controlCopyProjectPopUp(bool xStatus, string sFilter = "")          // Control copy Project popup
        {
            if (xStatus) // Is enabled, select first item
            {
                if (projectsModel.getFilteredCopyProject(sFilter) == null)
                {
                    MessageBox.Show(clLanguages.getName("__ErrorReadingDatabase"));
                }
                if (projectsModel.obcCopyProjects.Count() > 0)
                    iCopyProjectSelectedValue = projectsModel.obcCopyProjects[0].iSelectedvaluePath;
                else
                    iCopyProjectSelectedValue = 0;
            }
            xShowPopupCopyProject = xStatus;
        }

        private void openFileDialog()             // Open browse folder, to select folder where the label is located 
        {
            OpenFileDialog fbdImagePath     = new OpenFileDialog();
            fbdImagePath.DefaultExt         = "jpg";
            fbdImagePath.Filter             = "jpg files (*.jpg)|*.jpg";
            string sCurrentDirectory        = Directory.GetCurrentDirectory();

            fbdImagePath.InitialDirectory   = sCurrentDirectory + "\\Images\\Cars\\";   // Select initial path to folder browser 
            if (fbdImagePath.ShowDialog() == System.Windows.Forms.DialogResult.OK)      // When 'OK' buttun is pressed
            {
                projectsModel.imagePath    = fbdImagePath.FileName;                    // Save path to current template
            }
        }

        #endregion
    }

}
