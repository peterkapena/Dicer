using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Dicer.Core.Utl
{
    public class Fmt
    {
        public static Base64Formatter Base64 = new Base64Formatter();
        public static JsonFormatter Json = new JsonFormatter();
        public static StrFormatter Str = new StrFormatter();
        public static TriFormatter Tri = new TriFormatter();
        public static XmlFormatter Xml = new XmlFormatter(); // 0811 rose
        public static VarFormatter Var = new VarFormatter();



        public class VarFormatter
        {
            public string ToBackUrl(System.Uri Value, string DefaultValue = "")
            {
                try
                {
                    if (Value == null)
                        return Var.ToString(DefaultValue, "../../Codelib/Menu/blank.htm");
                    else
                        return Value.PathAndQuery;
                }
                catch
                {
                    return DefaultValue;
                }
            }

            public bool ToBoolean(object Value, bool DefaultValue = false)
            {
                try
                {
                    // 0805 dm  Added IsDBNull test for speed
                    if (Information.IsDBNull(Value) || Information.IsNothing(Value) || ToString(Value) == string.Empty)
                        return DefaultValue;
                    else
                    {
                        var sValue = Fmt.Var.ToString(Value);
                        switch (ToString(Value).ToUpper())
                        {
                            case "0":
                            case "FALSE":
                            case "NO":
                            case "OFF":
                                {
                                    return false;
                                }

                            default:
                                {
                                    return true;
                                }
                        }
                    }
                }
                catch
                {
                    return DefaultValue;
                }
            }

            public Color ToColour(string Value)
            {
                try
                {
                    // 0807 rose

                    if (Value == "" | Value == null)
                        return Color.WhiteSmoke;
                    else
                    {
                        Color cColor = Color.WhiteSmoke;

                        switch (Value)
                        {
                            case "LightSteelBlue":
                            case "Color [LightSteelBlue]":
                                {
                                    cColor = Color.LightSteelBlue;
                                    break;
                                }

                            default:
                                {
                                    cColor = Color.WhiteSmoke;
                                    break;
                                }
                        }

                        return cColor;
                    }
                }
                catch
                {
                    return Color.WhiteSmoke;
                }
            }



            public string ToCSVList(List<Enum> IDs, bool bIncludeZero = false)
            {
                var lst = "";
                foreach (var enm in IDs)
                {
                    int ID = Convert.ToInt32(enm);
                    if (ID > 0 | bIncludeZero)
                        lst += Interaction.IIf(lst.Length == 0, "", ",") + ID.ToString();
                }
                return lst;
            }

            /// <summary>
            ///             ''' Convert date from known string format into date object
            ///             '''     1912DM Fixed hidden bug where dates of format  "//" from Attribute throw error in daRvnGroup  (#REF:36762)
            ///             '''                 added "//" check from Attribute dates check on daRvnGroup
            ///             ''' </summary>
            public DateTime ToDate(object Value, string dteFormat = "")
            {
                DateTime dRetValue;
                try
                {
                    if (Information.IsNothing(Value) || Information.IsDBNull(Value) || string.IsNullOrEmpty(Value.ToString()) || Value.ToString() == "//")
                        dRetValue = new DateTime(0, 0, 0, 0, 0, 0);
                    else
                    {
                        switch (Value.GetType().Name)
                        {
                            case "Date":
                            case "DateTime":
                                {
                                    return (DateTime)Value;    // nm Don't convert if already converted
                                }
                        }
                        string sValue;
                        if (Value.GetType().Name == "XmlElement")
                        {
                            var xVal = (System.Xml.XmlNode)Value;
                            sValue = xVal.InnerText;
                        }
                        else
                            sValue = Value.ToString();
                        if (dteFormat.Length > 0)
                            dRetValue = DateTime.ParseExact(sValue, dteFormat, System.Globalization.CultureInfo.InvariantCulture);
                        else if (sValue.Length == 8 & Information.IsNumeric(sValue) == true)
                            // Hard coded non slashed date format is YMD
                            dRetValue = DateTime.ParseExact(sValue, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                        else
                            // NM - Changed to single digit day/month format as this handles double as well
                            dRetValue = DateTime.ParseExact(sValue, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                catch
                {
                    dRetValue = new DateTime(0, 0, 0, 0, 0, 0);
                    // Kidz.Utl.Bug.Register(ex, Usr.Current, "Object to Date converter")  'PL 20190916 - DM removed as Loads of error logged on PRODUCTION Sites
                    // DM Just show for DEFS so they can fix as needed 
                    System.Diagnostics.Debug.Assert(false, "Format.ToDate Object to Date converter exception");
                }
                return dRetValue;
            }

            public DateTime ToDateTime(object Value)
            {
                // 0906 rose

                try
                {
                    if (Information.IsDBNull(Value) || Information.IsNothing(Value))
                        return new DateTime(0, 0, 0, 0, 0, 0);
                    else
                        return System.Convert.ToDateTime(Value);
                }
                catch
                {
                    return new DateTime(0, 0, 0, 0, 0, 0);
                }
            }

            public string ToDblString(object Value, string DefaultValue = "0.00", int iDecimals = 2)
            {
                try
                {
                    if (iDecimals < 0)
                        iDecimals = 2;
                    string sDecimal = Strings.StrDup(iDecimals, "0");
                    if (Strings.Len(sDecimal) > 0)
                        sDecimal = "." + sDecimal;
                    var sFmt = "# ### ##0" + sDecimal + ";-# ### ##0" + sDecimal;

                    double dVal;
                    if (Information.IsDBNull(Value) || Information.IsNothing(Value))
                        return DefaultValue;
                    else if (Value.GetType().Name == "Double")
                        dVal = (double)Value;
                    else
                        double.TryParse(Value.ToString(), out dVal);
                    return dVal.ToString(sFmt, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    return DefaultValue;
                }
            }

            public double ToDouble(object Value, int iRoundTo = -1)
            {
                try
                {
                    string sValue = string.Empty;

                    double dDouble = 0;
                    if (Information.IsDBNull(Value) || Information.IsNothing(Value))
                        dDouble = 0;
                    else
                    {
                        if (Value.GetType().Name == "Double")
                        {
                            dDouble = (double)Value;
                            if (iRoundTo >= 0 & iRoundTo <= 4)
                                dDouble = Math.Round(dDouble, iRoundTo); // 1605 dm - added this code else the ROUND(iRoundTo) is not honoured!!!!!
                            return dDouble;
                        }
                        else if (Value.GetType().Name == "XmlElement")
                        {
                            var xVal = (System.Xml.XmlNode)Value;
                            sValue = xVal.InnerText;
                        }
                        else
                            sValue = Fmt.Var.ToString(Value);
                        sValue = Fmt.Str.SrcAndReplace(ref sValue, " ", "");
                    }
                    if (double.TryParse(sValue, out dDouble))
                    {
                        if (iRoundTo >= 0 & iRoundTo <= 4)
                            dDouble = Math.Round(dDouble, iRoundTo);
                    }
                    return dDouble;
                }
                catch
                {
                    return 0;
                }
            }

            public int ToInteger(object Value, int DftValue = 0)
            {
                // 0805 dm  Added IsDBNull test for speed
                if (Information.IsDBNull(Value) || Information.IsNothing(Value))
                    return DftValue;
                // 1411 nm  Added check for Int64 overflows likely where Acl code has not been migrated

                try
                {
                    // 0611 jk TODO - Want to add function call to remove spaces, but doesn't work for enumerated types
                    // Return CType(Kidz.Fmt.Str.SrcAndReplace(Kidz.Fmt.Var.ToString(Value), " ", ""), Integer)
                    if (Value.GetType().Name == "XmlElement")
                    {
                        var xVal = (System.Xml.XmlNode)Value;
                        Value = xVal.InnerText;
                    }
                    // 1611 jf - Handle characters and convert to integer
                    if (Value.GetType().Name == "Char")
                    {
                        if (Value.ToString().Length == 0)
                            return DftValue;
                        int rtn = 0;
                        if (int.TryParse(System.Convert.ToString(Value), out rtn))
                            return rtn;
                        else
                            return DftValue;
                    }
                    if (Value.GetType().Name == "String" || Value.GetType().Name == "JsonElement")
                    {
                        if (Value.ToString().Length == 0)
                            return DftValue;
                        int rtn = 0;
                        if (int.TryParse(Value.ToString().ToCharArray(), out rtn))
                            return rtn;
                        else
                            return DftValue;
                    }
                    return System.Convert.ToInt32(Value);
                }
                catch
                {
                    return DftValue;
                }
            }

            public string ToIntString(object Value, string DefaultValue = "0")
            {

                // 0711 jk

                try
                {
                    // 0805 dm  Added IsDBNull test for speed
                    if (Information.IsDBNull(Value) || Information.IsNothing(Value))
                        return Var.ToString(DefaultValue, "0");
                    else if (Value.ToString() == "")
                        return Var.ToString(DefaultValue, "0");
                    else
                        return Strings.LTrim(Strings.Format(Value, "# ### ##0;-# ### ##0"));
                }
                catch
                {
                    return DefaultValue;
                }
            }

            public string ToJavaParam(object Value, string DefaultValue = "", bool bAddQuotes = true, bool bUseDoubleQuotes = false)
            {
                var rtn = ToString(Value, DefaultValue);
                rtn = rtn.Replace(Constants.vbCr, @"\r");
                rtn = rtn.Replace(Constants.vbLf, @"\l");
                if (bUseDoubleQuotes)
                    rtn = rtn.Replace("\"", @"\""");
                else
                    rtn = rtn.Replace("'", @"\'");
                if (bAddQuotes)
                    rtn = string.Format("{1}{0}{1}", rtn, Interaction.IIf(bUseDoubleQuotes, "\"", "'"));
                return rtn;
            }

            public List<T> ToList<T>(string sCSV)
            {
                List<T> rtn = new List<T>();
                if (sCSV.Length > 0)
                {
                    foreach (string item in sCSV.Split(','))
                    {
                        var val = (T)Convert.ChangeType(item, typeof(T));
                        if (!rtn.Contains(val))
                            rtn.Add(val);
                    }
                }
                if (rtn.Count == 0)
                    return null;
                return rtn;
            }

            public long ToLong(object Value)
            {
                try
                {

                    // 0805 dm  Added IsDBNull test for speed
                    if (Information.IsDBNull(Value) || Information.IsNothing(Value))
                        return 0;
                    else
                        // 0611 jk todo - Want to add function call to remove spaces, but doesn't work for enumerated types
                        // Return CType(Kidz.Fmt.Str.SrcAndReplace(Kidz.Fmt.Var.ToString(Value), " ", ""), Long)
                        return System.Convert.ToInt64(Value);
                }
                catch
                {
                    return 0;
                }
            }

            public string ToLongDateString(object Value, string DefaultValue = "")
            {
                // 0606 dm added Wrapper
                try
                {
                    // 0805 dm  Added IsDBNull test for speed
                    if (Information.IsDBNull(Value) || Information.IsNothing(Value))
                        return Var.ToString(DefaultValue);
                    else
                        return System.Convert.ToDateTime(Value).ToLongDateString();
                }
                catch
                {
                    return DefaultValue;
                }
            }

            public string ToMonthString(int iMnt)
            {
                // 1212 dm added Wrapper
                string sMonth = "Undefined";
                switch (iMnt)
                {
                    case 1:
                        {
                            sMonth = "January";
                            break;
                        }

                    case 2:
                        {
                            sMonth = "February";
                            break;
                        }

                    case 3:
                        {
                            sMonth = "March";
                            break;
                        }

                    case 4:
                        {
                            sMonth = "April";
                            break;
                        }

                    case 5:
                        {
                            sMonth = "May";
                            break;
                        }

                    case 6:
                        {
                            sMonth = "June";
                            break;
                        }

                    case 7:
                        {
                            sMonth = "July";
                            break;
                        }

                    case 8:
                        {
                            sMonth = "August";
                            break;
                        }

                    case 9:
                        {
                            sMonth = "September";
                            break;
                        }

                    case 10:
                        {
                            sMonth = "October";
                            break;
                        }

                    case 11:
                        {
                            sMonth = "November";
                            break;
                        }

                    case 12:
                        {
                            sMonth = "December";
                            break;
                        }
                }

                return sMonth;
            }

            public string ToShortDateString(object oValue, string sDefault = "")
            {
                // 1405 dm - Added - OrElse return = Pub.dtSQLEmpty

                var rtn = Fmt.Var.ToDate(oValue);
                if (rtn == new DateTime(0, 0, 0, 0, 0, 0))
                    return sDefault;

                // 1404 nm - Blank out date if it's min or max
                if (rtn == DateTime.MinValue | rtn == DateTime.MaxValue)
                    return "";

                return rtn.ToShortDateString();
            }
            public string ToString(byte[] data)
            {
                var result = Encoding.UTF8.GetString(data);
                if (char.GetUnicodeCategory(result[0]) == System.Globalization.UnicodeCategory.Format)
                    return result.Substring(1);
                return result;
            }

            public string ToString(object Value, string DefaultValue = "")
            {
                // 0710 dm - Added bAddQuotes - Needed for Exports
                // 0711 dm - Added iMaxLength - Needed for Exports & combined with bAddQuotes
                // 0805 dm - added IsDBNull test for speed

                try
                {
                    string sString = "";

                    if (Information.IsDBNull(Value) || Information.IsNothing(Value))
                        sString = Var.ToString(DefaultValue, "");
                    else
                    {
                        // If Value.GetType.Name = "String" Then '1510 NM - Dont do unneeded work
                        // 'Pastel export file missing quotes around name field  (#REF:17939)
                        // 'Return Value '1511 dm - we must Continue else iMaxLength & bAddQuotes settings are skipped
                        // Else
                        if (Value.GetType().Name == "Double")
                            Value = ToDblString(Value, DefaultValue);
                        else if (Value.GetType().Name == "XmlElement")
                        {
                            var xVal = (System.Xml.XmlNode)Value;
                            Value = xVal.InnerText;
                        }
                        if (Value.ToString() == "")
                        {
                            if (DefaultValue == null)
                                sString = "";
                            else
                                sString = DefaultValue;
                        }
                        else
                            sString = Strings.Trim(Value.ToString());
                    }

                    // 1511 NM - Replace non-breaking spaces with regular spaces
                    sString = Regex.Replace(sString, @"\u00A0", " ");

                    if (sString.Trim().Length == 0)
                        sString = string.Empty;

                    return sString;
                }
                catch
                {
                    return DefaultValue;
                }
            }

            public string ToStrList(List<string> StrLst, bool WithCarriageReturn = false)
            {
                string lst = "";
                if (StrLst != null)
                {
                    foreach (var StrLsts in StrLst)
                    {
                        if (StrLsts.Length > 0)
                            lst += Fmt.Var.ToString(Interaction.IIf(lst.Length == 0, "", Interaction.IIf(WithCarriageReturn, Constants.vbCrLf, ",")) + StrLsts.ToString());
                    }
                }
                return lst;
            }

            public string ToTitleCase(object Value, string DefaultValue = "")
            {
                // 1004 NM - Use .NET TextInfo class to convert string to Title Case since it is location aware
                try
                {
                    var ci = System.Threading.Thread.CurrentThread.CurrentCulture;
                    var ti = ci.TextInfo;
                    string sString = Var.ToString(Value, DefaultValue);
                    return ti.ToTitleCase(sString);
                }
                catch
                {
                    return DefaultValue;
                }
            }

            public TriState ToTriState(object Value, TriState DefaultValue = TriState.UseDefault)     // 1303 DM
            {
                try
                {
                    // 1303 DM
                    if (Information.IsDBNull(Value) || Information.IsNothing(Value) || ToString(Value) == string.Empty)
                        return DefaultValue;
                    else
                    {
                        var sValue = Fmt.Var.ToString(Value);
                        switch (ToString(Value).ToUpper())
                        {
                            case "0":
                            case "FALSE":
                            case "NO":
                            case "OFF":
                                {
                                    return TriState.False;
                                }

                            case "1":
                            case "TRUE":
                            case "YES":
                            case "ON":
                            case "-1":
                                {
                                    return TriState.True;
                                }

                            default:
                                {
                                    return TriState.UseDefault;
                                }
                        }
                    }
                }
                catch
                {
                    return DefaultValue;
                }
            }

            public string ToTriStateString(TriState Value, string DefaultItem = "")
            {

                // 1406 DM
                switch (Value)
                {
                    case TriState.True:
                        {
                            if (DefaultItem.Length == 0)
                                DefaultItem = "Ticked";
                            break;
                        }

                    case TriState.False:
                        {
                            if (DefaultItem.Length == 0)
                                DefaultItem = "Unticked";
                            break;
                        }

                    default:
                        {
                            if (DefaultItem.Length == 0)
                                DefaultItem = "(N/A)";
                            break;
                        }
                }

                return DefaultItem;
            }

            /// <summary>
            ///             ''' Turn DataRow into XML string fragment
            ///             ''' </summary>
            /// <summary>
            ///             ''' Ensures a string is properly encoded to be stored in an XML value
            ///             ''' </summary>
            public string toXmlVal(string val)
            {
                return val.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
            }

            public string ToYearString(object Value, string DefaultValue = "")
            {

                // 0509 jk - Formats Value into 4 digit year display; if zero, returns blank string

                try
                {
                    if (Value == null)
                        return Var.ToString(DefaultValue);
                    else if (ToInteger(Value) == 0)
                        return Var.ToString(DefaultValue);
                    else
                        return Value.ToString();
                }
                catch
                {
                    return DefaultValue;
                }
            }

            public string Coalesce(params string[] values)
            {
                foreach (var value in values)
                {
                    if (!string.IsNullOrEmpty(value))
                        return value;
                }

                return null;
            }

            private string GetDigit(string sNumber)
            {

                // Used by ToNumberInWords: Converts a number from 1 to 9 into text
                // 0908 dm - Migrated from vb6 code
                switch (Conversion.Val(sNumber))
                {
                    case 1:
                        {
                            return "One";
                        }

                    case 2:
                        {
                            return "Two";
                        }

                    case 3:
                        {
                            return "Three";
                        }

                    case 4:
                        {
                            return "Four";
                        }

                    case 5:
                        {
                            return "Five";
                        }

                    case 6:
                        {
                            return "Six";
                        }

                    case 7:
                        {
                            return "Seven";
                        }

                    case 8:
                        {
                            return "Eight";
                        }

                    case 9:
                        {
                            return "Nine";
                        }

                    default:
                        {
                            return "";
                        }
                }
            }

            // End Function
            private string GetHundreds(string sNumber)
            {

                // Used by ToNumberInWords: Converts a number from 100-999 into text
                // 0908 dm - Migrated from vb6 code

                string sResult = "";

                if (Conversion.Val(sNumber) != 0)
                {
                    sNumber = Strings.Right("000" + sNumber, 3);

                    // Convert the hundreds place
                    if (Strings.Mid(sNumber, 1, 1) != "0")
                        sResult = GetDigit(Strings.Mid(sNumber, 1, 1)) + " Hundred ";

                    // Convert the tens and ones place
                    if (Strings.Mid(sNumber, 2, 1) != "0")
                        sResult = sResult + GetTens(Strings.Mid(sNumber, 2));
                    else
                        sResult = sResult + GetDigit(Strings.Mid(sNumber, 3));
                }

                return sResult;
            }
            private string GetTens(string sNumber)
            {

                // Used by ToNumberInWords: Converts a number from 10 to 99 into text
                // 0908 dm - Migrated from vb6 code
                string sResult;

                sResult = "";           // null out the temporary function value
                if (Conversion.Val(Strings.Left(sNumber, 1)) == 1)
                {
                    switch (Conversion.Val(sNumber))
                    {
                        case 10:
                            {
                                sResult = "Ten";
                                break;
                            }

                        case 11:
                            {
                                sResult = "Eleven";
                                break;
                            }

                        case 12:
                            {
                                sResult = "Twelve";
                                break;
                            }

                        case 13:
                            {
                                sResult = "Thirteen";
                                break;
                            }

                        case 14:
                            {
                                sResult = "Fourteen";
                                break;
                            }

                        case 15:
                            {
                                sResult = "Fifteen";
                                break;
                            }

                        case 16:
                            {
                                sResult = "Sixteen";
                                break;
                            }

                        case 17:
                            {
                                sResult = "Seventeen";
                                break;
                            }

                        case 18:
                            {
                                sResult = "Eighteen";
                                break;
                            }

                        case 19:
                            {
                                sResult = "Nineteen";
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
                else
                {
                    switch (Conversion.Val(Strings.Left(sNumber, 1)))
                    {
                        case 2:
                            {
                                sResult = "Twenty ";
                                break;
                            }

                        case 3:
                            {
                                sResult = "Thirty ";
                                break;
                            }

                        case 4:
                            {
                                sResult = "Forty ";
                                break;
                            }

                        case 5:
                            {
                                sResult = "Fifty ";
                                break;
                            }

                        case 6:
                            {
                                sResult = "Sixty ";
                                break;
                            }

                        case 7:
                            {
                                sResult = "Seventy ";
                                break;
                            }

                        case 8:
                            {
                                sResult = "Eighty ";
                                break;
                            }

                        case 9:
                            {
                                sResult = "Ninety ";
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                    sResult = sResult + GetDigit(Strings.Right(sNumber, 1));
                }

                return sResult;
            }
        }


        public class Base64Formatter
        {
            public string FromURL(string value)
            {
                return value.Replace('~', '=').Replace('.', '/').Replace('-', char.Parse("+"));
            }

            public string ToURL(string value)
            {
                return value.Replace('=', '~').Replace('/', '.').Replace('+', char.Parse("-"));
            }
        }

        public class JsonFormatter
        {
            public string FromObject(object obj, Newtonsoft.Json.Formatting formatting = Newtonsoft.Json.Formatting.None)
            {
                var Preferences = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                    Formatting = formatting
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Preferences);
            }


            public string FromXml(System.Xml.XmlNode node)
            {
                return Newtonsoft.Json.JsonConvert.SerializeXmlNode(node);
            }

            public T ToObject<T>(string json, bool throwErrors = false)
            {
                try
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                }
                catch (Exception ex)
                {
                    if (throwErrors)
                        throw ex;
                    return (T)Activator.CreateInstance(typeof(T));
                }
            }
            public string ToString(DBNull sValue)
            {
                return "";
            }
            public string ToString(string sValue)
            {
                var sRet = sValue.Replace("'", @"\'");
                sRet = sRet.Replace(Constants.vbCr, @"\r");
                sRet = sRet.Replace(Constants.vbLf, @"\n");
                return sRet;
            }
        }


        public class StrFormatter
        {
            public string GenerateRandomString(ref int iLength, int iMode = 0)
            {
                try
                {
                    string allowChrs = "";
                    switch (iMode)
                    {
                        case 1 // Numbers only
                       :
                            {
                                allowChrs = "0123456789";
                                break;
                            }

                        case 2 // Alfa only
                 :
                            {
                                allowChrs = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ";
                                break;
                            }

                        case 3 // Upper case only
                 :
                            {
                                allowChrs = "ABCDEFGHIJKLOMNOPQRSTUVWXYZ";
                                break;
                            }

                        default:
                            {
                                allowChrs = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789";
                                break;
                            }
                    }

                    Random r = new Random();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 1; i <= iLength; i++)
                    {
                        int idx = r.Next(0, allowChrs.Length - 1);
                        sb.Append(allowChrs.Substring(idx, 1));
                    }
                    return sb.ToString();
                }
                catch
                {
                    return "-1";
                }
            }

            public string SrcAndReplace(ref string s, string sSearch, string sReplace, bool bOnceOnly = false)
            {
                int iPos;

                iPos = Strings.InStr(s, sSearch);
                while (iPos > 0)
                {
                    s = Strings.Left(s, iPos - 1) + sReplace + Strings.Mid(s, iPos + Strings.Len(sSearch));
                    iPos = Strings.InStr(s, sSearch);
                    if (bOnceOnly)
                        break;
                }

                return s;
            }

            public string ToProperCase(string value)
            {
                return Strings.StrConv(value, VbStrConv.ProperCase);
            }
        }

        public class TriFormatter
        {
            public TriState FromInteger(int Value)
            {
                switch (Value)
                {
                    case 0:
                        {
                            return TriState.False;
                        }

                    case -1:
                        {
                            return TriState.UseDefault;
                        }
                }
                return TriState.True;
            }

            public bool ToBoolean(TriState Value, bool DefaultValue = false)
            {
                switch (Value)
                {
                    case TriState.True:
                        {
                            return true;
                        }

                    case TriState.False:
                        {
                            return false;
                        }
                }
                return DefaultValue;
            }

            public int ToInteger(TriState Value)
            {
                switch (Value)
                {
                    case TriState.False:
                        {
                            return 0;
                        }

                    case TriState.True:
                        {
                            return 1;
                        }
                }
                return -1;
            }
        }

        public class Utc
        {
            private static TimeSpan utcOffset = DateTime.Now.Subtract(DateTime.UtcNow);
            public static DateTime ToLocal(DateTime? utcDte)
            {
                if (utcDte == null)
                    return DateTime.MinValue;
                else
                    return utcDte.Value.Add(utcOffset);
            }
        }

        public class XmlFormatter
        {
            public byte[] HexToByteArray(System.Xml.XmlNode Node)
            {
                var hex = ToString(Node);
                if (hex == null)
                    return null;
                var count = (hex.Length / (double)2) - 1;
                byte[] bytes = new byte[(int)count + 1];
                for (int i = 0; i <= count; i++)
                    bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                return bytes;
            }

            public DateTime ToDate(System.Xml.XmlNode Node)
            {
                if (Node == null)
                    return default(DateTime);
                DateTime dDate;
                string sDate = ToString(Node);
                if (DateTime.TryParse(sDate, out dDate))
                    return dDate;
                else
                    return default(DateTime);
            }

            public DateTime ToDate(System.Xml.XmlAttribute Attribute)
            {
                if (Attribute == null)
                    return default(DateTime);
                DateTime dDate;
                string sDate = ToString(Attribute);
                if (DateTime.TryParse(sDate, out dDate))
                    return dDate;
                else
                    return default(DateTime);
            }

            public int ToInteger(System.Xml.XmlNode Node)
            {
                var iRes = 0;
                if ((Node != null))
                    Int32.TryParse(Node.InnerText, out iRes);
                return iRes;
            }

            public int ToInteger(System.Xml.XmlAttribute Attribute)
            {
                var iRes = 0;
                if ((Attribute != null))
                    Int32.TryParse(Attribute.Value, out iRes);
                return iRes;
            }



            public string ToString(XmlDocument xmlDoc)
            {
                var sw = new StringWriter();
                var xw = new XmlTextWriter(sw);
                xmlDoc.WriteTo(xw);
                return sw.ToString();
            }
            public string ToString(System.Xml.XmlNode Node)
            {
                if ((Node == null))
                    return string.Empty;
                return Node.InnerXml;
            }
            public string ToString(System.Xml.XmlAttribute Attribute)
            {
                if ((Attribute == null))
                    return string.Empty;
                return Attribute.Value;
            }

        }
    }

}
