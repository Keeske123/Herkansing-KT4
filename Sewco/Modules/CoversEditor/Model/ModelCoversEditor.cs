using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
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
using Sewco.Modules.ControlPanel;
using Sewco.Resources.Helper_classes;




namespace Sewco.Modules.CoversEditor
{
    public class CoversBusinessObject : ObservableObject
    {
        public CoversBusinessObject()
        {
            _projectsConnectDB              = new projectsConnectLocalDB();
            _productsConnectDB              = new productsConnectLocalDB();
            _profilesConnectDB              = new profilesConnectLocalDB();
            _materialProductConnectDB       = new materialProductConnectLocalDB();
            _materialConnectDB              = new materialConnectLocalDB();
            _materialIOPositionConnectDB    = new materialIOPositionConnectLocalDB();
            _CoversConnectDB                 = new coverConnectLocalDB();

            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__CoverCode")));
            sCode = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__customer")));
            sCustomer    = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__program")));
            sProgram = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__style")));
            sStyle = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__colour")));
            sColour = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.INTCHECK, clLanguages.getName("__manyear")));
            clInputErrors.Add(new clErrors(clConstants.MINLENGTHCHECK, "", "", "4"));
            clInputErrors.Add(new clErrors(clConstants.MAXLENGTHCHECK, "", "", "4"));
            clInputErrors.Add(new clErrors(clConstants.MINVALCHECK, "", "", "1990"));
            clInputErrors.Add(new clErrors(clConstants.MAXVALCHECK, "", "", "2017"));
            sManYear = new clTextBoxValidation(clInputErrors);
        }
        private int         iCoverId;
        //private int         iManYear;
        private string      _sImagePath;
        private bool        _xActive;
        private ObservableCollection<clErrors> clInputErrors = new ObservableCollection<clErrors>();

        public clTextBoxValidation sCustomer
        {
            get; set;
        }
        public clTextBoxValidation sCode
        {
            get; set;
        }
        public clTextBoxValidation sManYear
        {
            get; set;
        }
        public clTextBoxValidation sProgram
        {
            get; set;
        }
        public clTextBoxValidation sStyle
        {
            get; set;
        }

        public clTextBoxValidation sColour
        {
            get; set;
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
        


        private ObservableCollection<clCbbFilltype1>    _obcCovers        { get; set; } = new ObservableCollection<clCbbFilltype1>();
        private ObservableCollection<clCbbFilltype1>    _obcCopyCovers    { get; set; } = new ObservableCollection<clCbbFilltype1>();

        
        private projectsConnectLocalDB              _projectsConnectDB;
        private productsConnectLocalDB              _productsConnectDB;
        private profilesConnectLocalDB              _profilesConnectDB;
        private materialProductConnectLocalDB       _materialProductConnectDB;
        private materialConnectLocalDB              _materialConnectDB;
        private materialIOPositionConnectLocalDB    _materialIOPositionConnectDB;
        private coverConnectLocalDB                  _CoversConnectDB;

        #region public properties declararions

        public ObservableCollection<clCbbFilltype1> obcCovers
        {
            get
            {
                return _obcCovers;
            }
            set
            {
                _obcCovers = value;
                OnPropertyChanged("obcCovers");
            }
        }
        public ObservableCollection<clCbbFilltype1> obcCopyCovers
        {
            get
            {
                return _obcCopyCovers;
            }
            set
            {
                _obcCopyCovers = value;
                OnPropertyChanged("obcCopyCovers");
            }
        }

        #endregion
        clImage  coverImage = new clImage();
        #region public functions
        public int saveNewCover()
        {
            Covers saveNewCover    = new Covers();
            
            saveNewCover.code           = sCode.sInput;
            saveNewCover.customer       = sCustomer.sInput;
            saveNewCover.program        = sProgram.sInput;
            saveNewCover.manyear        = sManYear.sInput;
            saveNewCover.style          = sStyle.sInput;
            saveNewCover.colour         = sColour.sInput;
            saveNewCover.active         = xActive;
            saveNewCover.imagePath      = sImagePath;

            int iNewCoverId          = _CoversConnectDB.insertAndSubmit(saveNewCover);

            return iNewCoverId;
        }
        public int saveEditedCover()
        {
            Covers editedCover        = new Covers();            // get current cover from database
            editedCover              = _CoversConnectDB.selectWhere(iCoverId);

            editedCover.code           = sCode.sInput;
            editedCover.customer       = sCustomer.sInput;
            editedCover.program        = sProgram.sInput;
            editedCover.manyear        = sManYear.sInput;
            editedCover.style          = sStyle.sInput;
            editedCover.colour         = sColour.sInput;
            editedCover.active         = xActive;
            editedCover.imagePath      = sImagePath;

            int iEditedCoverId       = _CoversConnectDB.editInsertAndSubmit(editedCover);   // insert edited cover to database

            return iEditedCoverId;
        }
        public void deleteCover(int iDeleteCoverId)
        {
            _CoversConnectDB.deleteWhereCoverId(iDeleteCoverId);
        }
        public int getFirstCoverId()
        {
            if (this.obcCovers.Count > 0)
            {
                return this.obcCovers[0].iSelectedvaluePath;
            }
            else
            {
                return -1;
            }
        }
        public int getLastCoverId()
        {
            if (this.obcCovers.Count > 0)
            {
                return this.obcCovers[this.obcCovers.Count - 1].iSelectedvaluePath;
            }
            else
            {
                return -1;
            }
        }
        public Covers getCover(int iNewCoverId)
        {
            return _CoversConnectDB.selectWhere(iNewCoverId);
        }
        public void getCovers(string sFilter = "", int iProfileId = 0)
        {
            foreach (var Covers in this.obcCovers.ToList())          // Delete all items in this mirrow database list
            {
                this.obcCovers.Remove(Covers);                       // Remove all database data from observableobject. 
            }

            List<Object> CoverQuery = new List<Object>(_CoversConnectDB.select(sFilter));


            foreach (Covers Cover in CoverQuery)
            {
                this.obcCovers.Add(new clCbbFilltype1(Cover.code, Cover.coverId));      // Add database data to observableobject. 
            }
        }
        public void getFilteredCopyCover(string sFilter = "")
        {
            foreach (var Covers in obcCopyCovers.ToList())          // Delete all items in this mirrow database list
            {
                obcCopyCovers.Remove(Covers);                       // Remove all database data from observableobject. 
            }
            List<Object> CoverQuery = new List<Object>(_CoversConnectDB.select(sFilter));

            foreach (Covers Covers in CoverQuery)
            {
                obcCopyCovers.Add(new clCbbFilltype1(clLanguages.getName("__Code") + ":\t\t" + Covers.code + "\n" + clLanguages.getName("__Name") + ":\t", Covers.coverId));
            }
        }
        public void updateCoverDetails(int iNewCoverId, bool xIsNew)
        {
            Covers cover = getCover(iNewCoverId);

            if(cover != null)
            {
                if (!xIsNew)                                             // Do not copy name, comment and code to current class when a class is copied. 
                {
                    iCoverId        = cover.coverId;
                    sCode.sInput    = cover.code;
                }
               
                sCustomer.sInput    = cover.customer;
                sProgram.sInput     = cover.program;
                sManYear.sInput     = cover.manyear;
                sStyle.sInput       = cover.style;
                sColour.sInput      = cover.colour;
                xActive             = cover.active;
                sImagePath          = coverImage.checkImagePath(cover.imagePath);   
            }
            else
            {
                emptyCover();
            }
        }
        public void emptyCover()
        {
            iCoverId            = -1;
            sCode.sInput        = "";
            sCustomer.sInput    = "";
            sProgram.sInput     = "";
            sStyle.sInput       = "";
            sColour.sInput      = "";
            sManYear.sInput     = DateTime.Now.Year.ToString();
            sImagePath          = clImage.sDefaultImage;
            xActive             = true;
        }
        public void propertyValidation(bool xEnable)    
        {
      
            sCode.xValidateActive       = xEnable;
            sCustomer.xValidateActive   = xEnable;
            sProgram.xValidateActive    = xEnable;
            sStyle.xValidateActive      = xEnable;
            sColour.xValidateActive     = xEnable;
            sManYear.xValidateActive    = xEnable;

        }
        public bool validationStatus()
        {
            return ( sCode.xHasError || sStyle.xHasError || sProgram.xHasError || sCustomer.xHasError || sColour.xHasError || sManYear.xHasError );
        }

        #endregion
    }
}
