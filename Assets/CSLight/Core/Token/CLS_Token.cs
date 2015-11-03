using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{
    public enum TokenType
    {
        UNKNOWN,
        KEYWORD,        //关键字
        PUNCTUATION,    //标点
        IDENTIFIER,     //标识符 变量与函数
        TYPE,           //类型
        COMMENT,        //注释
        VALUE,          //数值
        STRING,         //字符串
        ProtoIndex,     //proto成员索引
    }

    public struct Token
    {
        public string text;
        public int pos;
        public int line;
        public TokenType type;
        public override string ToString()
        {
            return string.Format("line:{0} pos:{1} type:{2} text: {3}", line, pos, type, text);
        }
    }

    public interface ICLS_TokenParser
    {
        Dictionary<string, ICLS_Type> dictTypeKeywords
        {
            get;
        }
        List<string> keywords
        {
            get;
        }
        IList<Token> Parse(string line);

        void SaveTokenList(IList<Token> tokens, System.IO.Stream stream);

        IList<Token> ReadTokenList(System.IO.Stream stream);
    }

    public class CLS_TokenParser : ICLS_TokenParser
    {
        public CLS_TokenParser(Dictionary<string, ICLS_Type> dictTypeKeywords)
        {
            this.dictTypeKeywords = dictTypeKeywords;

            keywords = new List<string>();
            keywords.Add("if");
            keywords.Add("as");
            keywords.Add("is");
            keywords.Add("else");
            keywords.Add("break");
            keywords.Add("continue");
            keywords.Add("for");
            keywords.Add("do");
            keywords.Add("while");
            keywords.Add("return");
            keywords.Add("true");
            keywords.Add("false");
            keywords.Add("null");
            keywords.Add("new");
            keywords.Add("foreach");
            keywords.Add("in");
            keywords.Add("class");
            keywords.Add("interface");
            keywords.Add("abstract");

            keywords.Add("using");
            keywords.Add("public");
            keywords.Add("protected");
            keywords.Add("private");
            keywords.Add("static");
            keywords.Add("const");
            keywords.Add("virtual");
            keywords.Add("override");

            keywords.Add("try");
            keywords.Add("catch");
            keywords.Add("throw");

            keywords.Add("yield");
        }

        public Dictionary<string, ICLS_Type> dictTypeKeywords
        {
            get;
            private set;
        }

        public List<string> keywords
        {
            get;
            private set;
        }

        int FindStart(string lines, int npos)
        {
            int n = npos;
            for (int i = n; i < lines.Length; i++)
            {
                if (lines[i] == '\n')
                    line++;
                if (!char.IsSeparator(lines, i) && lines[i] != '\n' && lines[i] != '\r' && lines[i] != '\t')
                {
                    return i;
                }
            }
            return -1;
        }

        int GetToken(string line, int nstart, List<Token> ts, out Token t)
        {
            //找到开始字符
            t.pos = nstart;
            t.line = this.line;
            t.text = " ";
            t.type = TokenType.UNKNOWN;
            if (nstart < 0) return -1;
            if (line[nstart] == '\"')
            {
                t.text = "\"";
                int pos = nstart + 1;
                bool bend = false;
                while (pos < line.Length)
                {
                    char c = line[pos];
                    if (c == '\n')
                    {
                        throw new Exception("查找字符串失败");
                    }
                    if (c == '\"')
                    {
                        t.type = TokenType.STRING;
                        bend = true;
                        //break;
                    }
                    if (c == '\\')
                    {
                        pos++;
                        c = line[pos];
                        if (c == '\\')
                        {
                            t.text += '\\';
                            pos++;
                            continue;
                        }
                        else if (c == '"')
                        {
                            t.text += '\"';
                            pos++;
                            continue;
                        }
                        else if (c == '\'')
                        {
                            t.text += '\'';
                            pos++;
                            continue;
                        }
                        else if (c == '0')
                        {
                            t.text += '\0';
                            pos++;
                            continue;
                        }
                        else if (c == 'a')
                        {
                            t.text += '\a';
                            pos++;
                            continue;
                        }
                        else if (c == 'b')
                        {
                            t.text += '\b';
                            pos++;
                            continue;
                        }
                        else if (c == 'f')
                        {
                            t.text += '\f';
                            pos++;
                            continue;
                        }
                        else if (c == 'n')
                        {
                            t.text += '\n';
                            pos++;
                            continue;
                        }
                        else if (c == 'r')
                        {
                            t.text += '\r';
                            pos++;
                            continue;
                        }
                        else if (c == 't')
                        {
                            t.text += '\t';
                            pos++;
                            continue;
                        }
                        else if (c == 'v')
                        {
                            t.text += '\v';
                            pos++;
                            continue;
                        }
                        else
                        {
                            throw new Exception("不可识别的转义序列:" + t.text);
                        }
                    }
                    t.text += line[pos];
                    pos++;
                    if (bend)
                        return pos;
                }
                throw new Exception("查找字符串失败");
            }
            else if (line[nstart] == '\'')//char
            {
                int nend = line.IndexOf('\'', nstart + 1);
                int nsub = line.IndexOf('\\', nstart + 1);
                while (nsub > 0 && nsub < nend)
                {
                    nend = line.IndexOf('\'', nsub + 2);
                    nsub = line.IndexOf('\\', nsub + 2);

                }
                if (nend - nstart + 1 < 1) throw new Exception("查找字符失败");
                t.type = TokenType.VALUE;
                int pos = nend + 1;
                t.text = line.Substring(nstart, nend - nstart + 1);
                t.text = t.text.Replace("\\\"", "\"");
                t.text = t.text.Replace("\\\'", "\'");
                t.text = t.text.Replace("\\\\", "\\");
                t.text = t.text.Replace("\\0", "\0");
                t.text = t.text.Replace("\\a", "\a");
                t.text = t.text.Replace("\\b", "\b");
                t.text = t.text.Replace("\\f", "\f");
                t.text = t.text.Replace("\\n", "\n");
                t.text = t.text.Replace("\\r", "\r");
                t.text = t.text.Replace("\\t", "\t");
                t.text = t.text.Replace("\\v", "\v");
                int sp = t.text.IndexOf('\\');
                if (sp > 0)
                {
                    throw new Exception("不可识别的转义序列:" + t.text.Substring(sp));
                }
                if (t.text.Length > 3)
                {
                    throw new Exception("char 不可超过一个字节(" + t.line + ")");
                }
                return pos;
            }
            else if (line[nstart] == '#')   // #开头的都当作注释，如#region #endregion
            {
                t.type = TokenType.COMMENT;
                int enterpos = line.IndexOf('\n', nstart + 2);
                if (enterpos < 0) t.text = line.Substring(nstart);
                else
                    t.text = line.Substring(nstart, enterpos - nstart);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '/')   // / /= 注释
            {
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                {
                    t.type = TokenType.PUNCTUATION;
                    t.text = line.Substring(nstart, 2);
                }
                else if (nstart < line.Length - 1 && line[nstart + 1] == '/')
                {
                    t.type = TokenType.COMMENT;
                    int enterpos = line.IndexOf('\n', nstart + 2);
                    if (enterpos < 0) t.text = line.Substring(nstart);
                    else
                        t.text = line.Substring(nstart, enterpos - nstart);
                }
                else if (nstart < line.Length - 1 && line[nstart + 1] == '*')
                {
                    t.type = TokenType.COMMENT;
                    int enterpos = line.IndexOf("*/", nstart + 2);
                    t.text = line.Substring(nstart, enterpos + 2 - nstart);
                }
                else
                {
                    t.type = TokenType.PUNCTUATION;
                    t.text = line.Substring(nstart, 1);
                }
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '=')//= == =>
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else if (nstart < line.Length - 1 && line[nstart + 1] == '>')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '!')//= ==
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '+')//+ += ++
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && (line[nstart + 1] == '=' || line[nstart + 1] == '+'))
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            //通用的一元二元运算符检查
            else if (line[nstart] == '-')//- -= -- 负数也先作为符号处理
            {
                // 先检测是否是负数
                if (ts.Count > 0)
                {
                    Token preToken = ts[ts.Count - 1];
                    if (preToken.type == TokenType.PUNCTUATION &&
                        preToken.text != ")" && 
                        preToken.text != "]")
                    {
                        bool isNegativeValue = false;
                        bool isHexNum = false;
                        int negativeValueEnd = nstart + 1;
                        for (; negativeValueEnd < line.Length; negativeValueEnd++)
                        {
                            char c = line[negativeValueEnd];

                            if (char.IsNumber(c))
                            {
                                isNegativeValue = true;
                                continue;
                            }
                            if (c == '.')
                            {
                                isNegativeValue = true;
                                continue;
                            }
                            if (isNegativeValue && c == 'f')
                            {
                                continue;
                            }
                            if (isNegativeValue && (negativeValueEnd == nstart + 2) && (c == 'x' || c == 'X'))
                            {
                                isHexNum = true;
                                continue;
                            }
                            if (isHexNum && (c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F'))
                            {
                                continue;
                            }

                            break;
                        }
                        if (isNegativeValue)
                        {
                            t.type = TokenType.VALUE;
                            t.text = line.Substring(nstart, negativeValueEnd - nstart);
                            return nstart + t.text.Length;
                        }
                    }
                }
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=' || line[nstart + 1] == '-')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '*')//* *=
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '/')/// /=
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '%')/// /=
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '>')//> >= >> >>=
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else if (nstart < line.Length - 1 && line[nstart + 1] == '>')
                {
                    if (nstart < line.Length - 2 && line[nstart + 2] == '=')
                        t.text = line.Substring(nstart, 3);
                    else
                        t.text = line.Substring(nstart, 2);
                }
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '<')//< <= << <<=
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else if (nstart < line.Length - 1 && line[nstart + 1] == '<')
                {
                    if (nstart < line.Length - 2 && line[nstart + 2] == '=')
                        t.text = line.Substring(nstart, 3);
                    else
                        t.text = line.Substring(nstart, 2);
                }
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '&')//& && &=
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '&')
                    t.text = line.Substring(nstart, 2);
                else if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '|')//| ||
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '|')
                    t.text = line.Substring(nstart, 2);
                else if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '~')//~
            {
                t.type = TokenType.PUNCTUATION;
                t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '^')//^ ^=
            {
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                t.type = TokenType.PUNCTUATION;
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '[' && line.Substring(nstart, line.IndexOf("]", nstart) + 1 - nstart) == "[ProtoContract]")        // proto头判断
            {
                t.text = line.Substring(nstart, line.IndexOf("]", nstart) + 1 - nstart);
                t.type = TokenType.COMMENT;
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '[' && line.Substring(nstart, line.IndexOf("]", nstart) + 1 - nstart).StartsWith("[ProtoMember"))  // proto字段判断
            {
                string tempText = line.Substring(nstart, line.IndexOf("]", nstart) + 1 - nstart);
                int pos1 = tempText.IndexOf("(");
                int pos2 = tempText.IndexOf(")");
                t.text = tempText.Substring(pos1 + 1, pos2 - pos1 - 1);
                t.type = TokenType.ProtoIndex;
                return nstart + tempText.Length;
            }
            else if (char.IsLetter(line, nstart) || line[nstart] == '_')
            {
                //字母逻辑
                //判断完整性
                int i = nstart + 1;
                while (i < line.Length && (char.IsLetterOrDigit(line, i) || line[i] == '_'))
                {
                    i++;
                }
                t.text = line.Substring(nstart, i - nstart);
                //判断字母类型： 关键字 类型 标识符
                if (keywords.Contains(t.text))
                {
                    // 将 using namespace;当作注释
                    if (t.text == "using")
                    {
                        t.type = TokenType.COMMENT;
                        return line.IndexOf(';', nstart) + 1;
                    }

                    t.type = TokenType.KEYWORD;
                    return nstart + t.text.Length;
                }
                if (dictTypeKeywords.ContainsKey(t.text))
                {
                    while (line[i] == ' ' && i < line.Length)
                    {
                        i++;
                    }
                    if (line[i] == '<')/*  || line[i] == '['*/
                    {
                        int dep = 0;
                        string text = t.text;
                        while (i < line.Length)
                        {
                            if (line[i] == '<') dep++;
                            if (line[i] == '>') dep--;
                            if (line[i] == ';' || line[i] == '(' || line[i] == '{')
                            {
                                break;
                            }
                            if (line[i] != ' ') text += line[i];
                            i++;
                            if (dep == 0)
                            {
                                t.text = text;
                                break;
                            }
                        }
                        t.type = TokenType.TYPE;
                        return i;
                    }
                    else
                    {
                        t.type = TokenType.TYPE;
                        return nstart + t.text.Length;
                    }
                }
                while (i < line.Length && line[i] == ' ')
                {
                    i++;
                }
                if (i < line.Length && (line[i] == '<'/* || line[i] == '['*/))//检查特别类型
                {
                    int dep = 0;
                    string text = t.text;
                    while (i < line.Length)
                    {
                        if (line[i] == '<')
                        {
                            dep++;
                            i++;
                            text += '<';
                            continue;
                        }
                        if (line[i] == '>')
                        {
                            dep--;
                            i++;
                            if (dep == 0)
                            {
                                t.text = text + '>';
                                break;
                            }
                            continue;
                        }
                        Token tt;
                        int nnstart = FindStart(line, i);
                        i = GetToken(line, nnstart, ts, out tt);
                        if (tt.type != TokenType.IDENTIFIER && tt.type != TokenType.TYPE && tt.text != ",")
                        {
                            break;
                        }
                        text += tt.text;
                    }
                    if (dictTypeKeywords.ContainsKey(t.text))
                    {
                        t.type = TokenType.TYPE;
                        return i;

                    }
                    else if (dep == 0)
                    {
                        t.type = TokenType.IDENTIFIER;
                        return i;
                    }
                }
                t.type = TokenType.IDENTIFIER;
                return nstart + t.text.Length;
            }
            else if (char.IsPunctuation(line, nstart))
            {
                t.type = TokenType.PUNCTUATION;
                t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (char.IsNumber(line, nstart))
            {
                // 16进制判断
                if (line[nstart] == '0' && (line[nstart + 1] == 'x' || line[nstart + 1] == 'X'))
                {
                    int iend = nstart + 2;
                    for (int i = nstart + 2; i < line.Length; i++)
                    {
                        char c = line[i];
                        if (char.IsNumber(c) || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F')
                        {
                            iend = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                    t.type = TokenType.VALUE;
                    t.text = line.Substring(nstart, iend - nstart + 1);
                }
                else
                {
                    //纯数字

                    int iend = nstart;
                    for (int i = nstart + 1; i < line.Length; i++)
                    {
                        if (char.IsNumber(line, i))
                        {
                            iend = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                    t.type = TokenType.VALUE;
                    int dend = iend + 1;
                    if (dend < line.Length && line[dend] == '.')
                    {
                        int fend = dend;
                        for (int i = dend + 1; i < line.Length; i++)
                        {
                            if (char.IsNumber(line, i))
                            {
                                fend = i;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (fend + 1 < line.Length && line[fend + 1] == 'f')
                        {
                            t.text = line.Substring(nstart, fend + 2 - nstart);
                        }
                        else
                        {
                            t.text = line.Substring(nstart, fend + 1 - nstart);
                        }
                    }
                    else
                    {
                        if (dend < line.Length && line[dend] == 'f')
                        {
                            t.text = line.Substring(nstart, dend - nstart + 1);
                        }
                        else
                        {
                            t.text = line.Substring(nstart, dend - nstart);
                        }
                    }
                }
                return nstart + t.text.Length;
            }
            else
            {
                //不可识别逻辑
                int i = nstart + 1;
                while (i < line.Length - 1 && char.IsSeparator(line, i) == false && line[i] != '\n' && line[i] != '\r' && line[i] != '\t')
                {
                    i++;
                }
                t.text = line.Substring(nstart, i - nstart);
                return nstart + t.text.Length;
            }
        }

        int line = 0;
        public IList<Token> Parse(string lines)
        {
            line = 1;
            List<Token> ts = new List<Token>();
            int n = 0;
            while (n >= 0)
            {
                Token t;
                t.line = this.line;

                int nstart = FindStart(lines, n);
                t.line = this.line;
                int nend = GetToken(lines, nstart, ts, out t);
                if (nend >= 0)
                {
                    for (int i = nstart; i < nend; i++)
                    {
                        if (lines[i] == '\n')
                            line++;
                    }
                }
                n = nend;
                if (n >= 0)
                {
                    if (ts.Count >= 2 && t.type == TokenType.IDENTIFIER && ts[ts.Count - 1].text == "." && ts[ts.Count - 2].type == TokenType.TYPE)
                    {
                        string ntype = ts[ts.Count - 2].text + ts[ts.Count - 1].text + t.text;
                        if (dictTypeKeywords.ContainsKey(ntype))
                        {//类中类，合并之
                            t.type = TokenType.TYPE;
                            t.text = ntype;
                            t.pos = ts[ts.Count - 2].pos;
                            t.line = ts[ts.Count - 2].line;
                            ts.RemoveAt(ts.Count - 1);
                            ts.RemoveAt(ts.Count - 1);

                            ts.Add(t);
                            continue;
                        }
                    }
                    if (ts.Count >= 3 && t.type == TokenType.PUNCTUATION && t.text == ">"
                        && ts[ts.Count - 1].type == TokenType.TYPE
                        && ts[ts.Count - 2].type == TokenType.PUNCTUATION && ts[ts.Count - 2].text == "<"
                        && ts[ts.Count - 3].type == TokenType.IDENTIFIER)
                    {//模板函数调用,合并之
                        string ntype = ts[ts.Count - 3].text + ts[ts.Count - 2].text + ts[ts.Count - 1].text + t.text;
                        t.type = TokenType.IDENTIFIER;
                        t.text = ntype;
                        t.pos = ts[ts.Count - 2].pos;
                        t.line = ts[ts.Count - 2].line;
                        ts.RemoveAt(ts.Count - 1);
                        ts.RemoveAt(ts.Count - 1);
                        ts.RemoveAt(ts.Count - 1);
                        ts.Add(t);
                        continue;
                    }
                    if (ts.Count >= 2 && t.type == TokenType.TYPE && ts[ts.Count - 1].text == "." && (ts[ts.Count - 2].type == TokenType.TYPE || ts[ts.Count - 2].type == TokenType.IDENTIFIER))
                    {//Type.Type IDENTIFIER.Type 均不可能，为重名
                        t.type = TokenType.IDENTIFIER;
                        ts.Add(t);
                        continue;
                    }
                    if (ts.Count >= 1 && t.type == TokenType.TYPE && ts[ts.Count - 1].type == TokenType.TYPE)
                    {//Type Type 不可能，为重名
                        t.type = TokenType.IDENTIFIER;
                        ts.Add(t);
                        continue;
                    }
                    if (ts.Count >= 2 && ts[ts.Count - 2].text == "typeof" && ts[ts.Count - 1].text == "(")
                    {// 判断是否是typeof(类型)
                        if (t.type != TokenType.STRING)
                        {
                            t.type = TokenType.STRING;
                            t.text = string.Format("\"{0}\"", t.text);
                        }
                        ts.Add(t);
                        continue;
                    }

                    // 不保存未知token
                    if (t.type == TokenType.UNKNOWN)
                        continue;

                    // 不保存注释token
                    if (t.type == TokenType.COMMENT)
                        continue;

                    // 不保存部分关键字
                    if (t.type == TokenType.KEYWORD)
                    {
                        if (t.text == "abstract")
                            continue;
                        if (t.text == "public")
                            continue;
                        if (t.text == "private")
                            continue;
                        if (t.text == "protected")
                            continue;
                        if (t.text == "virtual")
                            continue;
                        if (t.text == "override")
                            continue;

                        // 将 const 作为 static
                        if (t.text == "const")
                            t.text = "static";
                    }

                    ts.Add(t);
                }
            }
            return ts;
        }

        public void SaveTokenList(IList<Token> tokens, System.IO.Stream stream)
        {
            if (tokens.Count > 0xffff)
                throw new Exception("不支持这么复杂的token保存");

            // 写入token数量
            byte[] tokenCountBytes = BitConverter.GetBytes((UInt16)tokens.Count);
            stream.Write(tokenCountBytes, 0, tokenCountBytes.Length);

            // 收集字符串
            List<string> listText = new List<string>();
            foreach (var t in tokens)
            {
                if (!listText.Contains(t.text))
                    listText.Add(t.text);
            }

            // 写入字符串数量
            byte[] textCountBytes = BitConverter.GetBytes((UInt16)listText.Count);
            stream.Write(textCountBytes, 0, textCountBytes.Length);

            // 写入字符串
            foreach (var text in listText)
            {
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text);
                WriteUInt16Variant(textBytes.Length, stream);
                stream.Write(textBytes, 0, textBytes.Length);
            }

            // 创建所有token指向listText字符串列表的索引
            if (listText.Count > 0xff)
            {
                foreach (var t in tokens)
                {
                    byte[] bs = new byte[3];
                    bs[0] = (byte)t.type;
                    byte[] tmpBs = BitConverter.GetBytes((UInt16)listText.IndexOf(t.text));
                    bs[1] = tmpBs[0];
                    bs[2] = tmpBs[1];
                    stream.Write(bs, 0, 3);
                }
            }
            else
            {
                foreach (var t in tokens)
                {
                    byte[] bs = new byte[2];
                    bs[0] = (byte)t.type;
                    bs[1] = (byte)listText.IndexOf(t.text);
                    stream.Write(bs, 0, 2);
                }
            }
        }

        public IList<Token> ReadTokenList(System.IO.Stream stream)
        {
            byte[] bs = new byte[0xffff];
            // 读取token数量
            stream.Read(bs, 0, 2);
            UInt16 len = BitConverter.ToUInt16(bs, 0);
            // 读取字符串数量
            stream.Read(bs, 0, 2);
            UInt16 lenstr = BitConverter.ToUInt16(bs, 0);

            // 读取字符串
            List<string> listText = new List<string>();
            for (int i = 0; i < lenstr; i++)
            {
                int bsLen;
                ReadUInt16Variant(stream, out bsLen);
                stream.Read(bs, 0, bsLen);
                listText.Add(System.Text.Encoding.UTF8.GetString(bs, 0, bsLen));
            }

            // 创建token
            List<Token> tokens = new List<Token>();
            if (listText.Count > 0xff)
            {
                for (int i = 0; i < len; i++)
                {
                    stream.Read(bs, 0, 3);
                    Token t = new Token();
                    t.type = (TokenType)bs[0];
                    t.text = listText[BitConverter.ToUInt16(bs, 1)];
                    tokens.Add(t);
                }
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    stream.Read(bs, 0, 2);
                    Token t = new Token();
                    t.type = (TokenType)bs[0];
                    t.text = listText[bs[1]];
                    tokens.Add(t);
                }
            }

            return tokens;
        }

        private void WriteUInt16Variant(int value, System.IO.Stream stream)
        {
            if ((value >> 7) == 0)
            {
                stream.WriteByte((byte)(value & 0x7F));
            }
            else
            {
                stream.WriteByte((byte)((value & 0x7F) | 0x80));
                stream.WriteByte((byte)((value >> 7) & 0x7F));
            }
        }

        private int ReadUInt16Variant(System.IO.Stream stream, out int value)
        {
            value = stream.ReadByte();
            if ((value & 0x80) == 0)
                return 1;
            value &= 0x7F;     
            value |= (stream.ReadByte() & 0x7F) << 7;
            return 2;
        }
    }
}
