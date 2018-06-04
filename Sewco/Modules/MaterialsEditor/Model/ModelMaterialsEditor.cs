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
using Sewco.Modules.MaterialsEditor;
using Sewco.Modules.ControlPanel;
using Sewco.Resources.Helper_classes;

namespace Sewco.Modules.MaterialsEditor
{
    public class clMaterialType : MaterialTypeMaterial
    {
        
        public clMaterialType(int _argPosition, int _argiMaterialId, int _argiMaterialTypeId, ObservableCollection<clCbbFilltype1> _argobcMaterialTypeOptions)
        {
            position                        = _argPosition;
            this.materialId                 = _argiMaterialId;
            this.iSelectedvalueMaterialType = _argiMaterialTypeId;
            this.obcMaterialTypeOptions     = _argobcMaterialTypeOptions;
            this.xIsLastItem                = false;
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

        private int _position;
        public int position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                SendPropertyChanged("position");
            }
        }

        public ObservableCollection<clCbbFilltype1> obcMaterialTypeOptions { get; set; } = new ObservableCollection<clCbbFilltype1>();

    }
 
    public class MaterialsBusinessObject : Material
    {
        public MaterialsBusinessObject()
        {
            getLanguages();

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__MaterialName")));
            sName = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__MaterialCode")));
            sCode = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__MaterialRange")));
            sRange = new clTextBoxValidation(clInputErrors);

            clInputErrors = new ObservableCollection<clErrors>();
            clInputErrors.Add(new clErrors(clConstants.EMPTYCHECK, clLanguages.getName("__MaterialLength")));
            clInputErrors.Add(new clErrors(clConstants.INTCHECK, clLanguages.getName("__MaterialLength")));
            iMinLength = new clTextBoxValidation(clInputErrors);
        }

        #region private properties declarations
        private ObservableCollection<clMaterialType> _obcMaterialType { get; set; } = new ObservableCollection<clMaterialType>();
        private ObservableCollection<clCbbFilltype1> _obcMaterials { get; set; } = new ObservableCollection<clCbbFilltype1>();
        private ObservableCollection<clCbbFilltype1> _obcCopyMaterials { get; set; } = new ObservableCollection<clCbbFilltype1>();
        private ObservableCollection<clCbbFilltype1> _obcAvailableMaterialTypes { get; set; } = new ObservableCollection<clCbbFilltype1>();
        private ObservableCollection<clErrors> clInputErrors = new ObservableCollection<clErrors>();


        private clcbbOptions _clLanguageList { get; set; } = new clcbbOptions();

        private string  _sImagePath;


        public static ObservableCollection<int> usedDeviceTypelist;

        #endregion

        #region public properties declararions
        clImage newImage = new clImage();
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

        public ObservableCollection<clMaterialType> obcMaterialType
        {
            get
            {
                return this._obcMaterialType;
            }
            set
            {
                this._obcMaterialType = value;
                SendPropertyChanged("obcIOPositions");
            }
        }
       

        public ObservableCollection<clCbbFilltype1> obcMaterials
        {
            get
            {
                return this._obcMaterials;
            }
            set
            {
                this._obcMaterials = value;
                SendPropertyChanged("obcMaterials");
            }
        }
        public ObservableCollection<clCbbFilltype1> obcCopyMaterials
        {
            get
            {
                return this._obcCopyMaterials;
            }
            set
            {
                this._obcCopyMaterials = value;
                SendPropertyChanged("obcCopyMaterials");
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

        public clTextBoxValidation sName
        {
            get; set;
        }
        /*public bool active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                SendPropertyChanged("active");
            }
        }*/
        public clTextBoxValidation sCode
        {
            get; set;
        }
        public clTextBoxValidation iMinLength
        {
            get; set;
        }
        public clTextBoxValidation sRange
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
                SendPropertyChanged("sImagePath");
            }
        }


        private int _iHighestMaterialType;
        #endregion

        #region public functions
        public bool saveNewMaterial()
        {
            try
            {
                Material saveNewMaterial = new Material();                          // Create a new instance of Material

                saveNewMaterial.name        = sName.sInput;
                saveNewMaterial.code        = sCode.sInput;
                saveNewMaterial.active      = active;
                saveNewMaterial.minLength   = Int32.Parse(iMinLength.sInput);
                saveNewMaterial.range       = sRange.sInput;
                saveNewMaterial.imagePath   = imagePath;

                ViewModelControlPanel.DBDataClass.Materials.InsertOnSubmit(saveNewMaterial);     // Insert new Material   
                ViewModelControlPanel.DBDataClass.SubmitChanges();

                // Upload MaterialIOPositions to database
                foreach (var obcMaterialType in this.obcMaterialType.ToList())
                {
                    MaterialTypeMaterial newMaterial_MaterialType = new MaterialTypeMaterial();

                    newMaterial_MaterialType.materialId = saveNewMaterial.materialId;
                    newMaterial_MaterialType.materialTypeId = obcMaterialType.iSelectedvalueMaterialType;//materialTypeId;
                    
                    ViewModelControlPanel.DBDataClass.MaterialTypeMaterials.InsertOnSubmit(newMaterial_MaterialType);
                }
                ViewModelControlPanel.DBDataClass.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
        public bool saveEditedMaterial()
        {
           try
           {
                Material editedMaterial = ViewModelControlPanel.DBDataClass.Materials.Single(c => c.materialId == materialId);  // Get the correct material

                editedMaterial.name         = sName.sInput;
                editedMaterial.code         = sCode.sInput;
                editedMaterial.active       = active;
                editedMaterial.minLength    = Int32.Parse(iMinLength.sInput); 
                editedMaterial.range        = sRange.sInput;
                editedMaterial.imagePath    = imagePath;
                

                // First delete all current items 
                var deleteMaterialTypeQuery = from x in ViewModelControlPanel.DBDataClass.MaterialTypeMaterials
                                             where x.materialId == editedMaterial.materialId
                                              select x;

                foreach (var q in deleteMaterialTypeQuery)
                {
                    ViewModelControlPanel.DBDataClass.MaterialTypeMaterials.DeleteOnSubmit(q);
                }
                ViewModelControlPanel.DBDataClass.SubmitChanges();
               

                // Upload new values to database
                foreach (var obcMaterialType in this.obcMaterialType.ToList())
                {
                    MaterialTypeMaterial newMaterial_MaterialType = new MaterialTypeMaterial();

                    newMaterial_MaterialType.materialId = editedMaterial.materialId;
                    newMaterial_MaterialType.materialTypeId = obcMaterialType.iSelectedvalueMaterialType;

                    ViewModelControlPanel.DBDataClass.MaterialTypeMaterials.InsertOnSubmit(newMaterial_MaterialType);
                }
               
                ViewModelControlPanel.DBDataClass.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool deleteMaterial(int iMaterialId)
        {
            try
            {
                // Delete I/O positions from material
                var deleteMaterialTypeQuery = from x in ViewModelControlPanel.DBDataClass.MaterialTypeMaterials
                                             where x.materialId == iMaterialId
                                             select x;
                foreach (var q in deleteMaterialTypeQuery)
                {
                    ViewModelControlPanel.DBDataClass.MaterialTypeMaterials.DeleteOnSubmit(q);
                }

                ViewModelControlPanel.DBDataClass.SubmitChanges();

                // Delete Material
                var deleteMaterialQuery = from x in ViewModelControlPanel.DBDataClass.Materials
                                         where x.materialId == iMaterialId
                                         select x;

                foreach (var q in deleteMaterialQuery)
                {
                    ViewModelControlPanel.DBDataClass.Materials.DeleteOnSubmit(q);
                }

                ViewModelControlPanel.DBDataClass.SubmitChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }
        public int getFirstMaterialId()
        {
            if (this.obcMaterials.Count > 0)
            {
                return this.obcMaterials[0].iSelectedvaluePath;
            }
            else
            {
                return -1;
            }
        }
        public int getLastMaterialId()
        {
            if (this.obcMaterials.Count > 0)
            {
                return this.obcMaterials[this.obcMaterials.Count - 1].iSelectedvaluePath;
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
                this.getMaterials();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Material getMaterial(int iMaterialId)
        {
            Material getMaterial = new Material();
            try
            {
                getMaterial = ViewModelControlPanel.DBDataClass.Materials.Single(c => c.materialId == iMaterialId);
                return getMaterial;
            }
            catch
            {
                return null;
            }
        }
        public void getMaterials(string sFilter = "")
        {
            foreach (var materials in this.obcMaterials.ToList())          // Delete all items in this mirrow database list
            {
                this.obcMaterials.Remove(materials);                       // Remove all database data from observableobject. 
            }

            try
            {
                var MaterialQuery = from x in ViewModelControlPanel.DBDataClass.Materials
                                   where x.name.Contains(sFilter) || x.code.Contains(sFilter)
                                   orderby x.materialId ascending
                                   select x;
                foreach (var materials in MaterialQuery)
                {
                    this.obcMaterials.Add(new clCbbFilltype1(materials.name + ",  " + materials.code, materials.materialId));      // Add database data to observableobject. 
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
        public ObservableCollection<clMaterialType> getMaterialTypes(int iMaterialId)
        {
            // This is only possible when an observable list doesn't have to be a property
            this.obcAvailableMaterialTypes = new ObservableCollection<clCbbFilltype1>();

            try
            {
                var materialTypesQuery = from x in ViewModelControlPanel.DBDataClass.MaterialTypes
                                         orderby x.materialTypeId ascending
                                         select x;

                foreach (var m in materialTypesQuery)
                {
                    this.obcAvailableMaterialTypes.Add(new clCbbFilltype1(m.name, m.materialTypeId));
                }
                
                var MaterialTypeQuery = from x in ViewModelControlPanel.DBDataClass.MaterialTypeMaterials
                                 where x.materialId == iMaterialId
                                 select x;

                foreach (var q in this.obcMaterialType.ToList())
                {
                    this.obcMaterialType.Remove(q);
                }
                int i = 0;
                foreach (var q in MaterialTypeQuery)
                {
                    // Get all available IO position from database. 
                    this.obcMaterialType.Add(new clMaterialType(i, q.materialId, q.materialTypeId, this.obcAvailableMaterialTypes));
                    i++;
                }

                _iHighestMaterialType = this.obcMaterialType.Count();      // Highest positions equals count(), starts at 1
                if (_iHighestMaterialType > 0)
                {
                    this.obcMaterialType[_iHighestMaterialType - 1].xIsLastItem = true;
                }
                return this.obcMaterialType;
            }
            catch
            {
                return null;
            }
        }
       
        public ObservableCollection<clCbbFilltype1> getFilteredCopyMaterial(string sFilter = "")
        {
            try
            {
                foreach (var material in this.obcCopyMaterials.ToList())          // Delete all items in this mirrow database list
                {
                    this.obcCopyMaterials.Remove(material);                       // Remove all database data from observableobject. 
                }
                var MaterialQuery = from x in ViewModelControlPanel.DBDataClass.Materials
                                   where x.name.Contains(sFilter) || x.code.Contains(sFilter)
                                   orderby x.materialId ascending
                                   select x;
                foreach (var material in MaterialQuery)
                {
                    this.obcCopyMaterials.Add(new clCbbFilltype1(clLanguages.getName("__Name") + ":\t\t" + material.name + "\n" + clLanguages.getName("__Comment") + ":\t" + material.code, material.materialId));
                }
                return this.obcCopyMaterials;
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
    
       
        public void addMaterialType()
        {
            _iHighestMaterialType = this.obcMaterialType.Count();
            if (_iHighestMaterialType > 0)
            {
                 this.obcMaterialType[_iHighestMaterialType - 1].xIsLastItem = false;
            }
            int i = 0;
            if (obcMaterialType.Count() < this.obcAvailableMaterialTypes.Count()) {

                this.obcMaterialType.Add(new clMaterialType(_iHighestMaterialType, materialId, 0, getAvailableMaterialTypes()));
                i++;
                _iHighestMaterialType += 1;

            }
           this.obcMaterialType[_iHighestMaterialType - 1].xIsLastItem = true;
            
            
        }
        public void deleteMaterialType(int iSelectedItem)
        {

            //this.obcMaterialType[0].position
            // Delete selected IO position
            this.obcMaterialType.RemoveAt(iSelectedItem);
            
            // re-position
            for (int i=0; i< this.obcMaterialType.Count(); i++)
            {
                this.obcMaterialType[i].position = i;
            }
        }

        /*
        public void deleteMaterialType()
        {
            // Get highest IO position
            _iHighestMaterialType = this.obcMaterialType.Count();
            if (_iHighestMaterialType > 0)
            {
                // Delete last IO position
                this.obcMaterialType.RemoveAt(_iHighestMaterialType - 1);

                // Get highest IO position
                _iHighestMaterialType = this.obcMaterialType.Count();
                // Set last IO position to IsLastItem
                if (_iHighestMaterialType > 0)
                {
                    this.obcMaterialType[_iHighestMaterialType - 1].xIsLastItem = true;
                }
            }
        }
         */

        public void updateMaterialDetails(int iIdNumber, bool xIsNew)
        {
            Material material = getMaterial(iIdNumber);
            
            if (material != null)
            {
                if (!xIsNew) {                                            // Do not copy name and comment to current class when a class is copied. 
         
                    materialId      = material.materialId;
                    sName.sInput    = material.name.ToString();
                    sCode.sInput    = material.code.ToString();
                }

                active              = material.active;
                sRange.sInput       = material.range;
                imagePath           = newImage.checkImagePath(material.imagePath);
                iMinLength.sInput   = material.minLength.ToString();
              
                string sSewcoPath = Directory.GetCurrentDirectory();
                
                reloadFilteredComboboxes();
            } else
            {
                emptyMaterial();
            }
        }
        public void emptyMaterial()
        {
            sName.sInput        = "";
            sCode.sInput        = "";
            active              = true;
            sRange.sInput       = "";
            iMinLength.sInput   = "";
            
            imagePath = clImage.sDefaultImage;
            
            foreach (var v in this.obcMaterialType.ToList())
            {
                this.obcMaterialType.Remove(v);
            }
            _iHighestMaterialType = this.obcMaterialType.Count();            // Set new highest position, which should be zero. 
        }
        public void reloadFilteredComboboxes()
        {
        }

        public void propertyValidation(bool xEnable)
        {
            sName.xValidateActive       = xEnable;
            sCode.xValidateActive       = xEnable;
            sRange.xValidateActive      = xEnable;
            iMinLength.xValidateActive  = xEnable;
        }
        public bool validationStatus()
        {
            return (sName.xHasError     ||
                    sCode.xHasError     ||
                    sRange.xHasError    ||
                    iMinLength.xHasError);
        }

        #endregion
    } // End class MaterialsBusinessObject
}

