using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class DataDictTemplateBLL : IDataDictTemplateBLL
    {
        public bool Validate(string name, string controlCategoryStr, string dataCategoryStr, 
                             string dataLengthStr, string controlWidthStr, string controlHeightStr,
                             string vals,string controlValue,string orderNoStr,string validateRegExpression ,FuncValidates funcValidates,out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(name))
            {
                errorMessage = "名称为空";
                return false;
            }
            var controlCategory = -1;
            if (string.IsNullOrWhiteSpace(controlCategoryStr))
            {
                errorMessage = "控件类型为空";
                return false;
            }
            if (!int.TryParse(controlCategoryStr, out controlCategory) || !controlCategory.IsEnum<ControlType>())
            {
                errorMessage = "控件类型非法";
                return false;
            }
            int dataCategory = -1;
            if (string.IsNullOrWhiteSpace(dataCategoryStr))
            {
                errorMessage = "数据类型为空";
                return false;
            }
            if (!int.TryParse(dataCategoryStr, out dataCategory) || !dataCategory.IsEnum<DataType>())
            {
                errorMessage = "数据类型非法";
                return false;
            }
            if ((DataType)System.Enum.ToObject(typeof(DataType), dataCategory) == DataType.String)
            {
                int dataLength = -1;
                if (string.IsNullOrWhiteSpace(dataLengthStr))
                {
                    errorMessage = "数据长度为空";
                    return false;
                }
                if (!int.TryParse(dataLengthStr, out dataLength) || dataLength <= 0)
                {
                    errorMessage = "数据长度非法";
                    return false;
                }
            }
            int controlWidth = -1;
            if (!string.IsNullOrWhiteSpace(controlWidthStr) && (!int.TryParse(controlWidthStr, out controlWidth) || controlWidth <= 0))
            {
                errorMessage = "控件宽度非法";
                return false;
            }
            int controlHeight = -1;
            if (!string.IsNullOrWhiteSpace(controlHeightStr) && (!int.TryParse(controlHeightStr, out controlHeight) || controlHeight <= 0))
            {
                errorMessage = "控件高度非法";
                return false;
            }
            switch ((ControlType)System.Enum.ToObject(typeof(ControlType), controlCategory))
            {
                case ControlType.RadioButtion:
                case ControlType.CheckBox:
                case ControlType.ComboxBox:
                    if (string.IsNullOrWhiteSpace(vals))
                    {
                        errorMessage = "值域为空";
                        return false;
                    }
                    break;
            }
            if (!string.IsNullOrWhiteSpace(vals) && !string.IsNullOrWhiteSpace(controlValue))
            {
                if (!vals.Split(',').Contains(controlValue))
                {
                    errorMessage = string.Format("控件默认值【{0}】不在值域【{1}】中", controlValue, vals);
                    return false;
                }
            }
            int orderNo = -1;
            if (!string.IsNullOrWhiteSpace(orderNoStr) && (!int.TryParse(orderNoStr, out orderNo) || orderNo < 0))
            {
                errorMessage = "排序号非法";
                return false;
            }
            if (!string.IsNullOrWhiteSpace(controlValue))
            {
                switch ((DataType)System.Enum.ToObject(typeof(DataType), dataCategory))
                {
                    case Model.Enum.DataType.Boolean:
                        bool b = false;
                        if (!bool.TryParse(controlValue, out b))
                        {
                            errorMessage = string.Format("控件默认值不是布尔类型");
                            return false;
                        }
                        break;
                    case Model.Enum.DataType.DateTime:
                        DateTime date = DateTime.Now;
                        if (!DateTime.TryParse(controlValue, out date))
                        {
                            errorMessage = string.Format("控件默认值不是日期类型");
                            return false;
                        }
                        break;
                    case Model.Enum.DataType.Float:
                        float f = -1f;
                        if (!float.TryParse(controlValue, out f))
                        {
                            errorMessage = string.Format("控件默认值不是浮点类型");
                            return false;
                        }
                        break;
                    case Model.Enum.DataType.Integer:
                        int i = -1;
                        if (!int.TryParse(controlValue, out i))
                        {
                            errorMessage = string.Format("控件默认值不是整数类型");
                            return false;
                        }
                        break;
                    case Model.Enum.DataType.String:
                        if (int.Parse(dataLengthStr) < controlValue.Length)
                        {
                            errorMessage = string.Format("控件默认值的长度大于【{0}】", dataLengthStr);
                            return false;
                        }
                        break;
                }
                if (!string.IsNullOrWhiteSpace(validateRegExpression) && !new System.Text.RegularExpressions.Regex(validateRegExpression).IsMatch(controlValue))
                {
                    errorMessage = string.Format("控件默认值不匹配正则表达式【{0}】", validateRegExpression);
                    return false;
                }
            }
            if (funcValidates != null) return funcValidates(out errorMessage);
            return true;
        }
        public void Init(IDataDictTemplate dataDictTemplate, string name, string controlCategoryStr, string dataCategoryStr,
                             string dataLengthStr, string controlWidthStr, string controlHeightStr,
                             string vals, string controlValue, string orderNoStr,string isRequiredStr ,
                             string controlDefaultValue,string validateRegExpression, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                dataDictTemplate.Name = name;
                dataDictTemplate.ControlCategory = int.Parse(controlCategoryStr);
                dataDictTemplate.DataCategory = int.Parse(dataCategoryStr);
                dataDictTemplate.DataLength = null;
                if (dataDictTemplate.DataCategoryEnum == DataType.String)
                    dataDictTemplate.DataLength = int.Parse(dataLengthStr);
                dataDictTemplate.ControlWidth = null;
                if (!string.IsNullOrWhiteSpace(controlWidthStr))
                    dataDictTemplate.ControlWidth = int.Parse(controlWidthStr);
                dataDictTemplate.ControlHeight = null;
                if (!string.IsNullOrWhiteSpace(controlHeightStr))
                    dataDictTemplate.ControlHeight = int.Parse(controlHeightStr);
                dataDictTemplate.Vals = null;
                if (dataDictTemplate.ControlCategoryEnum == ControlType.ComboxBox ||
                    dataDictTemplate.ControlCategoryEnum == ControlType.RadioButtion ||
                    dataDictTemplate.ControlCategoryEnum == ControlType.CheckBox)
                    dataDictTemplate.Vals = vals;
                dataDictTemplate.IsRequired = isRequiredStr == "1";
                dataDictTemplate.ControlDefaultValue = controlDefaultValue;
                dataDictTemplate.OrderNo = 0;
                if (!string.IsNullOrWhiteSpace(orderNoStr))
                    dataDictTemplate.OrderNo = int.Parse(orderNoStr);
                dataDictTemplate.ValidateRegExpression = validateRegExpression;
            }
            catch (Exception ex) 
            {
                dataDictTemplate = null;
                errorMessage = ex.Message; 
            }
        }
        public bool Validates(IDataDictTemplate dataDictTemplate, string val, out string errorMessage)
        {
            errorMessage = "";
            if (dataDictTemplate.IsRequired && string.IsNullOrWhiteSpace(val))
            {
                errorMessage = string.Format("【{0}】值为空", dataDictTemplate.Name);
                return false;
            }
            if (!string.IsNullOrWhiteSpace(val))
            {
                switch (dataDictTemplate.DataCategoryEnum)
                {
                    case Model.Enum.DataType.Boolean:
                        bool b = false;
                        if (!bool.TryParse(val, out b))
                        {
                            errorMessage = string.Format("【{0}】值不是布尔类型", dataDictTemplate.Name);
                            return false;
                        }
                        break;
                    case Model.Enum.DataType.DateTime:
                        DateTime date = DateTime.Now;
                        if (!DateTime.TryParse(val, out date))
                        {
                            errorMessage = string.Format("【{0}】值不是日期类型", dataDictTemplate.Name);
                            return false;
                        }
                        break;
                    case Model.Enum.DataType.Float:
                        float f = -1f;
                        if (!float.TryParse(val, out f))
                        {
                            errorMessage = string.Format("【{0}】值不是浮点类型", dataDictTemplate.Name);
                            return false;
                        }
                        break;
                    case Model.Enum.DataType.Integer:
                        int i = -1;
                        if (!int.TryParse(val, out i))
                        {
                            errorMessage = string.Format("【{0}】值不是整数类型", dataDictTemplate.Name);
                            return false;
                        }
                        break;
                    case Model.Enum.DataType.String:
                        if (dataDictTemplate.DataLength < val.Length)
                        {
                            errorMessage = string.Format("【{0}】值的长度大于【{1}】", dataDictTemplate.Name, dataDictTemplate.DataLength);
                            return false;
                        }
                        break;
                }
                if (!string.IsNullOrWhiteSpace(dataDictTemplate.ValidateRegExpression) && !new System.Text.RegularExpressions.Regex(dataDictTemplate.ValidateRegExpression).IsMatch(val))
                {
                    errorMessage = string.Format("【{0}】值不匹配正则表达式【{1}】", dataDictTemplate.Name, dataDictTemplate.ValidateRegExpression);
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(dataDictTemplate.Vals) && !string.IsNullOrWhiteSpace(val))
            {
                if (dataDictTemplate.ControlCategoryEnum != ControlType.CheckBox)
                {
                    if (!dataDictTemplate.Vals.Split(',').Contains(val))
                    {
                        errorMessage = string.Format("【{0}】值【{1}】不在值域【{2}】中", dataDictTemplate.Name, val, dataDictTemplate.Vals);
                        return false;
                    }
                }
                else
                {
                    var vals = val.Split(',');
                    foreach (var valTemp in vals)
                    {
                        if (!dataDictTemplate.Vals.Split(',').Contains(valTemp))
                        {
                            errorMessage = string.Format("【{0}】值【{1}】不在值域【{2}】中", dataDictTemplate.Name, valTemp, dataDictTemplate.Vals);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
