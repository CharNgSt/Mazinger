/// <summary>
/// 文本操作扩展类
/// </summary>
public static class StaticTxtHelper
{
    /// <summary>
    /// 姓名脱敏
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string MaskName(this string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return string.Empty;
        }

        var maskChar = '*';
        var nameParts = name.Split(' ');
        var firstName = nameParts[0];
        var lastName = nameParts.Length > 1 ? nameParts[nameParts.Length - 1] : string.Empty;

        if (firstName.Length > 1)
        {
            firstName = firstName.Substring(0, 1) + new string(maskChar, firstName.Length - 1);
        }

        if (!string.IsNullOrEmpty(lastName) && lastName.Length > 1)
        {
            lastName = lastName.Substring(0, 1) + new string(maskChar, lastName.Length - 1);
        }

        return firstName + (string.IsNullOrEmpty(lastName) ? string.Empty : " " + lastName);
    }

    /// <summary>
    /// 随机数种子
    /// </summary>
    /// <returns></returns>
    public static string GetRandomStr(int VcodeNum = 4)
    {
        int number;
        string strCode = string.Empty;

        //随机数种子
        Random random = new Random();

        for (int i = 0; i < VcodeNum; i++) //校验码长度为4
        {
            //随机的整数
            number = random.Next();

            //字符从0-9,A-Z中随机产生,对应的ASCII码分别为
            //48-57,65-90
            number = number % 36;
            if (number < 10)
            {
                number += 48;
            }
            else
            {
                number += 55;
            }
            strCode += ((char)number).ToString();
        }

        return strCode.Replace("Z", "2").Replace("O", "0").Replace("I", "1").Replace("z", "2").Replace("o", "0").Replace("i", "1");
    }

    /// <summary>
    /// blob to string (需引用Nuget codepages)
    /// </summary>
    /// <param name="_val">blob</param>
    /// <param name="_encoding">默认GB2312</param>
    /// <returns></returns>
    public static string toByteString(this byte[] _val, string _encoding = "GB2312")
    {
        string _content = "";
        try
        {
            if (_encoding == "default") _content = Encoding.Default.GetString(_val);
            else _content = Encoding.GetEncoding(_encoding).GetString(_val);
        }
        catch { }
        return _content;
    }

    /// <summary>
    /// string to blob (需引用Nuget)
    /// </summary>
    /// <param name="_val">blob</param>
    /// <param name="_encoding">默认GB2312</param>
    /// <returns></returns>
    public static byte[] toStringByte(this string _val, string _encoding = "GB2312")
    {
        byte[] _content = null;
        try
        {
            if (_encoding == "default") _content = Encoding.Default.GetBytes(_val);
            else _content = Encoding.GetEncoding(_encoding).GetBytes(_val);
        }
        catch { }
        return _content;
    }

    /// <summary>
    /// base64 str to byte[]
    /// </summary>
    /// <param name="base64"></param>
    /// <returns></returns>
    public static byte[] toBytes(this string base64)
    {
        return Convert.FromBase64String(base64);
    }

    /// <summary>
    /// ms to base64
    /// </summary>
    /// <param name="_ms"></param>
    /// <returns></returns>
    public static string toBase64(this MemoryStream _ms)
    {
        _ms.Position = 0;
        byte[] buffer = new byte[_ms.Length];
        _ms.Read(buffer, 0, (int)_ms.Length);
        return Convert.ToBase64String(buffer);
    }


    /// <summary>
    /// 日期文本转日期型，出错返回当天日期
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static DateTime toDate(this string _val)
    {
        if (DateTime.TryParse(_val, out DateTime dt)) return dt;
        else return DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
    }

    //日期转换为大写
    public static string ToUpperDate(this DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;
        return $"{year.toUpperNum()}年{month.toUpperMonth()}月{day.toUpperDay()}日";

    }

    //月转化为大写
    public static string toUpperMonth(this int month)
    {
        if (month < 10)
        {
            return month.toUpperNum();
        }
        else
            if (month == 10) { return "十"; }

        else
        {
            return $"十{(month - 10).toUpperNum()}";
        }
    }

    //日转化为大写
    public static string toUpperDay(this int day)
    {
        if (day < 20)
        {
            return day.toUpperMonth();
        }
        else
        {
            String str = day.ToString();
            if (str[1] == '0')
            {
                return int.Parse(str[0].ToString()).toUpperNum() + "十";

            }
            else
            {
                return int.Parse(str[0].ToString()).toUpperNum() + "十"
                    + int.Parse(str[1].ToString()).toUpperNum();
            }
        }
    }


    /// <summary>
    /// 英文日期
    /// </summary>
    /// <param name="_datetxt"></param>
    /// <returns></returns>
    public static string toEnglishDate(this string _datetxt)
    {
        string _result = "";
        try
        {
            DateTime _time = DateTime.Parse(_datetxt);

            _result = EnglishMonth(_time.Month.ToString()) + " " + _time.Day.ToString() + ", " + _time.Year.ToString();
        }
        catch { }
        return _result;
    }

    public static string EnglishMonth(string _month)
    {
        string months = "";
        switch (_month)
        {
            case "1":
                months = "January";
                break;
            case "2":
                months = "February";
                break;
            case "3":
                months = "March";
                break;
            case "4":
                months = "April";
                break;
            case "5":
                months = "May";
                break;
            case "6":
                months = "June";
                break;
            case "7":
                months = "July";
                break;
            case "8":
                months = "August";
                break;
            case "9":
                months = "September";
                break;
            case "10":
                months = "October";
                break;
            case "11":
                months = "November";
                break;
            case "12":
                months = "December";
                break;
        }

        return months;

    }

    /// <summary>
    /// 获取html文件中body的内容
    /// </summary>
    /// <param name="_html"></param>
    /// <returns></returns>
    public static string getHtmlTag(string _html)
    {

        string result = _html;
        string startTag = "<body>";
        string endTag = "</body>";
        try
        {
            if (!string.IsNullOrEmpty(_html) && _html.IndexOf(startTag) > 0 && _html.IndexOf(endTag) > 0 && _html.IndexOf(endTag) > _html.IndexOf(startTag))
            {
                int a = _html.IndexOf(startTag);
                int b = _html.IndexOf(endTag);
                var len1 = a + startTag.Length;
                result = _html.Substring(len1, b - len1);
            }
        }
        catch
        {
            result = _html;
        }
        return result;

    }

    /// <summary>
    /// 是否Base64串
    /// </summary>
    /// <param name="base64Str"></param>
    /// <returns></returns>
    public static bool IsBase64(this string base64Str)
    {
        if (string.IsNullOrEmpty(base64Str))
            return false;
        else
        {
            if (base64Str.Contains(","))
                base64Str = base64Str.Split(',')[1];
            if (base64Str.Length % 4 != 0)
                return false;

            char[] base64CodeArray = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/', '=' };
            if (base64Str.Any(c => !base64CodeArray.Contains(c)))
                return false;
        }
        try
        {
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    /// <summary>
    /// 短GUID
    /// </summary>
    /// <returns></returns>
    public static string GenerateStringID()
    {
        long i = 1;
        foreach (byte b in Guid.NewGuid().ToByteArray())
        {
            i *= ((int)b + 1);
        }
        return string.Format("{0:x}", i - DateTime.Now.Ticks);
    }

    /// <summary>
    /// 获取哈希表列值
    /// </summary>
    /// <param name="_tb"></param>
    /// <param name="_colName"></param>
    /// <returns></returns>
    public static string GetHashTableValue(this Hashtable _tb, string _colName)
    {
        try
        {
            return _tb[_colName].ToString();
        }
        catch
        {
            return "";
        }

    }

    /// <summary>
    /// string to base64
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static string ToBase64(this string _val)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(_val));
    }

    /// <summary>
    /// base64 string to string
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static string FromBase64ToStr(this string _val)
    {
        if (string.IsNullOrEmpty(_val)) return "";
        else return Encoding.UTF8.GetString(Convert.FromBase64String(_val));
    }


    /// <summary>
    /// 判断输入的字符串是否是一个合法的手机号
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsMobilePhone(this string input)
    {
        Regex regex = new Regex("^13\\d{9}$");
        return regex.IsMatch(input);
    }
    /// <summary>
    /// 判断输入的字符串是否是一个合法的Email地址
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsEmail(this string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(input);
    }

    /// <summary>
    /// decimal to int str
    /// </summary>
    /// <param name="_val">传入</param>
    /// <returns></returns>
    public static string toDecimalStr(this decimal _val)
    {
        return string.Format("{0:0.#}", _val);
    }
    /// <summary>
    /// decimal to int str
    /// </summary>
    /// <param name="_val">传入</param>
    /// <returns></returns>
    public static string toDecimalStr(this decimal? _val)
    {
        return string.Format("{0:0.#}", _val);
    }

    /// <summary>
    /// decimal to int str
    /// </summary>
    /// <param name="_val">传入</param>
    /// <returns></returns>
    public static string toDecimalStr_Math2(this decimal? _val)
    {
        return ((decimal)_val).ToString("#0.00"); //string.Format("#0.00", _val);;
    }

    /// <summary>
    /// 数字大写
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static string toUpperNum(this int num)
    {
        String str = num.ToString();
        string rstr = "";
        int n;
        for (int i = 0; i < str.Length; i++)
        {
            n = Convert.ToInt16(str[i].ToString());//char转数字,转换为字符串，再转数字
            switch (n)
            {
                case 0: rstr = rstr + "〇"; break;
                case 1: rstr = rstr + "一"; break;
                case 2: rstr = rstr + "二"; break;
                case 3: rstr = rstr + "三"; break;
                case 4: rstr = rstr + "四"; break;
                case 5: rstr = rstr + "五"; break;
                case 6: rstr = rstr + "六"; break;
                case 7: rstr = rstr + "七"; break;
                case 8: rstr = rstr + "八"; break;
                default: rstr = rstr + "九"; break;
            }
        }
        return rstr;
    }

    /// <summary>
    /// string[] Obj to str
    /// </summary>
    /// <param name="_obj">传入</param>
    /// <returns></returns>
    public static string toGourpStr(this List<string> _obj, string _split_txt = ",")
    {
        var _res = "";
        foreach (var _val in _obj) _res += $"{(_res == "" ? "" : _split_txt)}{_val}";
        return _res;
    }

    /// <summary>
    /// datatable转list
    /// </summary>
    /// <param name="_db"></param>
    /// <returns></returns>
    public static List<Dictionary<string, object>> ToDicList(this DataTable _db)
    {
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        foreach (DataRow dr in _db.Rows)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (DataColumn dc in _db.Columns)
            {
                dic.Add(dc.ColumnName, dr[dc.ColumnName]);
            }
            list.Add(dic);
        }
        return list;
    }

    /// <summary>
    /// 密码符合复杂度(正则表达式)
    /// </summary>
    /// <param name="_pwd"></param>
    /// <returns></returns>
    public static bool IsPwdMatch(this string _pwd)
    {
        var regex = new Regex(
            @"
                (?=.*[0-9])                     #必须包含数字
                (?=.*[a-zA-Z])                  #必须包含小写或大写字母
                (?=([\x21-\x7e]+)[^a-zA-Z0-9])  #必须包含特殊符号
                .{8,30}                         #至少8个字符，最多30个字符
            ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
        return regex.IsMatch(_pwd);
    }

    /// <summary>
    /// 判断是否存在XSS威胁（验证字符内是否包含）
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    public static bool IsXssAtt(this string _val)
    {
        var regex = new Regex(@"<[^>]*>");
        return regex.IsMatch(_val);
    }

    /// <summary>
    /// 清除HTML格式
    /// </summary>
    /// <param name="Htmlstring"></param>
    /// <returns></returns>
    public static string RemoveHTML(this string Htmlstring)
    {
        try
        {
            Htmlstring = Htmlstring.Replace("<br />", "\n");
            Htmlstring = Htmlstring.Replace("<br/>", "\n");
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
        }
        catch { }
        return Htmlstring;
    }

}
