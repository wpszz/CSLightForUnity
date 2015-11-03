using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{
    public partial class CLS_Expression_Compiler : ICLS_Expression_Compiler
    {
        int FindCodeBlock(IList<Token> tokens, int pos)
        {
            int dep = 0;
            for (int i = pos; i < tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.PUNCTUATION)
                {
                    if (tokens[i].text == "{")
                    {
                        dep++;
                    }
                    else if (tokens[i].text == "}")
                    {
                        dep--;
                        if (dep < 0)
                            return i - 1;
                    }
                }
            }

            if (dep != 0)
                return -1;
            else
                return tokens.Count - 1;
        }

        int FindCodeAny(IList<Token> tokens, ref int pos, out int depstyle)
        {
            int dep = 0;
            Token? start = null;

            depstyle = 0;
            for (int i = pos; i < tokens.Count; i++)
            {
                if (start == null)
                {
                    start = tokens[i];
                    pos = i;
                    if (start.Value.type == TokenType.PUNCTUATION)
                    {
                        if (start.Value.text == "{")
                            depstyle = 2;
                        else if (start.Value.text == "(")
                            depstyle = 1;
                        else if (start.Value.text == "[")
                            depstyle = 1;
                    }
                    else if (start.Value.type == TokenType.KEYWORD)
                    {
                        if(start.Value.text=="new")
                            return FindCodeKeyWord_New(tokens, i);

                        if (start.Value.text == "for")
                            return FindCodeKeyWord_For(tokens, i);

                        if (start.Value.text == "foreach")
                            return FindCodeKeyWord_ForEach(tokens, i);

                        if (start.Value.text == "while")
                            return FindCodeKeyWord_While(tokens, i);

                        if (start.Value.text == "do")
                            return FindCodeKeyWord_Dowhile(tokens, i);

                        if (start.Value.text == "if")
                            return FindCodeKeyWord_If(tokens, i);

                        if (start.Value.text == "return")
                            return FindCodeKeyWord_Return(tokens, i);
                    }
                }

                if (tokens[i].type == TokenType.PUNCTUATION)
                {
                    if (tokens[i].text == "{")
                    {
                        dep++;
                    }
                    else if (tokens[i].text == "}")
                    {
                        dep--;
                        if (depstyle == 2 && dep == 0)
                            return i;
                        if (dep < 0)
                            return i - 1;
                    }
                    else if (tokens[i].text == "(")
                    {
                        dep++;
                    }
                    else if (tokens[i].text == ")")
                    {
                        dep--;
                        if (depstyle == 1 && dep == 0)
                        {
                            if (start.Value.text == "(" && dep == 0)
                                return i;
                        }
                        if (dep < 0)
                            return i - 1;
                    }
                    else if (tokens[i].text == "[")
                    {
                        dep++;
                    }
                    else if (tokens[i].text == "]")
                    {
                        dep--;
                        if (depstyle == 1 && dep == 0)
                        {
                            if (start.Value.text == "[" && dep == 0)
                                return i;
                        }
                        if (dep < 0)
                            return i - 1;
                    }

                    if (depstyle == 0)
                    {
                        if (tokens[i].text == ",")//，结束的表达式
                        {
                            if (dep == 0)
                                return i - 1;
                        }
                        else if (tokens[i].text == ";")
                        {
                            if (dep == 0)
                                return i - 1;
                        }
                    }
                }
            }

            if (dep != 0)
                return -1;
            else
                return tokens.Count - 1;
        }

        int FindCodeInBlock(IList<Token> tokens, ref int pos, out int depstyle)
        {
            int dep = 0;
            Token? start = null;

            depstyle = 0;
            for (int i = pos; i < tokens.Count; i++)
            {
                if (start == null)
                {
                    start = tokens[i];
                    pos = i;
                    if (start.Value.type == TokenType.PUNCTUATION)
                    {
                        if (start.Value.text == "{")
                            depstyle = 2;
                        else if (start.Value.text == "(")
                            depstyle = 1;
                        else if (start.Value.text == "[")
                            depstyle = 1;
                    }
                    else if (start.Value.type == TokenType.KEYWORD)
                    {
                        if (start.Value.text == "for")
                            return FindCodeKeyWord_For(tokens, i);

                        if (start.Value.text == "foreach")
                            return FindCodeKeyWord_ForEach(tokens, i);

                        if (start.Value.text == "while")
                            return FindCodeKeyWord_While(tokens, i);

                        if (start.Value.text == "do")
                            return FindCodeKeyWord_Dowhile(tokens, i);

                        if (start.Value.text == "if")
                            return FindCodeKeyWord_If(tokens, i);

                        if (start.Value.text == "return")
                            return FindCodeKeyWord_Return(tokens, i);
                    }
                }

                if (tokens[i].type == TokenType.PUNCTUATION)
                {
                    if (tokens[i].text == "{")
                    {
                        dep++;
                    }
                    else if (tokens[i].text == "}")
                    {
                        dep--;
                        if (depstyle == 2 && dep == 0)
                            return i;
                        if (dep < 0)
                            return i - 1;
                    }
                    else if (tokens[i].text == "(")
                    {
                        dep++;
                    }
                    else if (tokens[i].text == ")")
                    {
                        dep--;
                        if (depstyle == 1 && dep == 0)
                        {
                            if (start.Value.text == "(" && dep == 0)
                            {
                                if(i<tokens.Count&&tokens[i+1].text==".")
                                    depstyle = 0;
                                else
                                    return i;
                            }
                        }
                        if (dep < 0)
                            return i - 1;
                    }
                    else if (tokens[i].text == "[")
                    {
                        dep++;
                    }
                    else if (tokens[i].text == "]")
                    {
                        dep--;
                        if (depstyle == 1 && dep == 0)
                        {
                            if (start.Value.text == "[" && dep == 0)
                                return i;
                        }
                        if (dep < 0)
                            return i - 1;
                    }

                    if (depstyle == 0)
                    {
                        if (tokens[i].text == ",")//，结束的表达式
                        {
                            if (dep == 0)
                                return i - 1;
                        }
                        else if (tokens[i].text == ";")
                        {
                            if (dep == 0)
                                return i - 1;
                        }
                    }
                }
            }

            if (dep != 0)
                return -1;
            else
                return tokens.Count - 1;
        }

        int FindCodeAnyInFunc(IList<Token> tokens, ref int pos, out int depstyle)
        {
            int dep = 0;
            Token? start = null;

            depstyle = 0;
            for (int i = pos; i < tokens.Count; i++)
            {
                if (start == null)
                {
                    start = tokens[i];
                    pos = i;
                    if (start.Value.type == TokenType.PUNCTUATION)
                    {
                        if (start.Value.text == "{")
                            depstyle = 2;
                        else if (start.Value.text == "(")
                            depstyle = 1;
                        else if (start.Value.text == "[")
                            depstyle = 1;
                    }
                    else if (start.Value.type == TokenType.KEYWORD)
                    {
                        if (start.Value.text == "for")
                            return FindCodeKeyWord_For(tokens, i);

                        if (start.Value.text == "foreach")
                            return FindCodeKeyWord_ForEach(tokens, i);

                        if (start.Value.text == "while")
                            return FindCodeKeyWord_While(tokens, i);

                        if (start.Value.text == "do")
                            return FindCodeKeyWord_Dowhile(tokens, i);

                        if (start.Value.text == "if")
                            return FindCodeKeyWord_If(tokens, i);

                        if (start.Value.text == "return")
                            return FindCodeKeyWord_Return(tokens, i);
                    }
                }

                if (tokens[i].type == TokenType.PUNCTUATION)
                {
                    if (tokens[i].text == "{")
                    {
                        dep++;
                    }
                    else if (tokens[i].text == "}")
                    {
                        dep--;
                        if (depstyle == 2 && dep == 0)
                            return i;

                        if (dep < 0)
                            return i - 1;
                    }
                    else if (tokens[i].text == "(")
                    {
                        dep++;
                    }
                    else if (tokens[i].text == ")")
                    {
                        dep--;
                        if (depstyle == 1 && dep == 0)
                        {
                            if (start.Value.text == "(" && dep == 0)
                                depstyle = 0;
                        }
                        if (dep < 0)
                            return i - 1;
                    }
                    else if (tokens[i].text == "[")
                    {
                        dep++;
                    }
                    else if (tokens[i].text == "]")
                    {
                        dep--;
                        if (depstyle == 1 && dep == 0)
                        {
                            if (start.Value.text == "[" && dep == 0)
                                return i;
                        }
                        if (dep < 0)
                            return i - 1;
                    }

                    if (depstyle == 0)
                    {
                        if (tokens[i].text == ",")//，结束的表达式
                        {
                            if (dep == 0)
                                return i - 1;
                        }
                        else if (tokens[i].text == ";")
                        {
                            if (dep == 0)
                                return i - 1;
                        }
                    }
                }
            }

            if (dep != 0)
                return -1;
            else
                return tokens.Count - 1;
        }

        int FindCodeAnyWithoutKeyword(IList<Token> tokens, ref int pos, out int depstyle)
        {
            int dep = 0;
            Token? start = null;
            depstyle = 0;
            for (int i = pos; i < tokens.Count; i++)
            {
                if (start == null)
                {
                    start = tokens[i];
                    pos = i;
                    if (start.Value.type == TokenType.PUNCTUATION)
                    {
                        if (start.Value.text == "{")
                            depstyle = 2;
                        else if (start.Value.text == "(")
                            depstyle = 1;
                    }
                }

                if (tokens[i].type == TokenType.PUNCTUATION)
                {
                    if (tokens[i].text == "{")
                    {
                        dep++;
                    }
                    else if (tokens[i].text == "}")
                    {
                        dep--;
                        if (depstyle == 2 && dep == 0)
                            return i;

                        if (dep < 0)
                            return i - 1;
                    }
                    else if (tokens[i].text == "(")
                    {
                        dep++;
                    }
                    else if (tokens[i].text == ")")
                    {
                        dep--;
                        if (depstyle == 1 && dep == 0)
                        {
                            if (start.Value.text == "(" && dep == 0)
                                return i;
                        }
                        if (dep < 0)
                            return i - 1;
                    }

                    if (depstyle == 0)
                    {
                        if (tokens[i].text == ",")//，结束的表达式
                        {
                            if (dep == 0)
                                return i - 1;
                        }
                        else if (tokens[i].text == ";")
                        {
                            if (dep == 0)
                                return i - 1;
                        }
                    }
                }
            }

            if (dep != 0)
                return -1;
            else
                return tokens.Count - 1;
        }

        int FindCodeKeyWord_For(IList<Token> tokens, int pos)
        {
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tokens, ref fs1, out b1);

            int b2;
            int fs2 = fe1 + 1;
            int fe2 = FindCodeAny(tokens, ref fs2, out b2);
            return fe2;
        }

        int FindCodeKeyWord_New(IList<Token> tokens, int pos)
        {
            int b1;
            int fs1 = pos + 2;
            int fe1 = FindCodeAny(tokens, ref fs1, out b1);
            if(tokens[fe1].text=="]")
            {
                fs1 = fe1 + 1;
                fe1 = FindCodeAny(tokens, ref fs1, out b1);
            }
            return fe1;
        }

        int FindCodeKeyWord_ForEach(IList<Token> tokens, int pos)
        {
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tokens, ref fs1, out b1);

            int b2;
            int fs2 = fe1 + 1;
            int fe2 = FindCodeAny(tokens, ref fs2, out b2);
            return fe2;
        }

        int FindCodeKeyWord_While(IList<Token> tokens, int pos)
        {
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tokens, ref fs1, out b1);

            int b2;
            int fs2 = fe1 + 1;
            int fe2 = FindCodeAny(tokens, ref fs2, out b2);
            return fe2;
        }

        int FindCodeKeyWord_Dowhile(IList<Token> tokens, int pos)
        {
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tokens, ref fs1, out b1);

            int b2;
            int fs2 = fe1 + 1;
            int fe2 = FindCodeAny(tokens, ref fs2, out b2);
            return fe2;
        }

        int FindCodeKeyWord_If(IList<Token> tokens, int pos)
        {
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tokens, ref fs1, out b1);

            int b2;
            int fs2 = fe1 + 1;
            int fe2 = FindCodeAny(tokens, ref fs2, out b2);

            int nelse = fe2 + 1;
            if (b2 == 0) 
                nelse++;
            FindCodeAny(tokens, ref nelse, out b2);
            if (tokens.Count > nelse)
            {
                if (tokens[nelse].type == TokenType.KEYWORD && tokens[nelse].text == "else")
                {
                    int b3;
                    int fs3 = nelse + 1;
                    int fe3 = FindCodeAny(tokens, ref fs3, out b3);
                    return fe3;
                }
            }
            return fe2;
        }

        int FindCodeKeyWord_Return(IList<Token> tokens, int pos)
        {

            int fs = pos + 1;
            if (tokens[fs].type == TokenType.PUNCTUATION && tokens[fs].text == ";")
                return pos;
            int b;
            fs = pos;
            int fe = FindCodeAnyWithoutKeyword(tokens, ref fs, out b);
            return fe;
        }

        IList<int> SplitExpressionWithOp(IList<Token> tokens, int pos, int posend)
        {
            List<int> list = new List<int>();
            List<int> listt = new List<int>();
            int dep = 0;
            int skip = 0;
            for (int i = pos; i <= posend; i++)
            {
                if (tokens[i].type == TokenType.PUNCTUATION || (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "as") || (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "is"))
                {
                    if (tokens[i].text == "(")
                    {
                        if (dep == 0 && (i == pos || tokens[i - 1].type == TokenType.PUNCTUATION) && i + 1 <= posend && tokens[i + 1].type == TokenType.TYPE)
                        {
                            list.Add(i);
                        }
                        dep++;
                        skip = i + 1;
                        continue;
                    }
                    else if (tokens[i].text == "{")
                    {
                        dep++;
                        skip = i + 1;
                        continue;
                    }
                    else if (tokens[i].text == "[")
                    {
                        if (dep == 0)
                        {
                            list.Add(i);
                        }
                        dep++;
                        skip = i + 1;
                        continue;
                    }
                    else if (tokens[i].text == ")" || tokens[i].text == "}" || tokens[i].text == "]")
                    {
                        dep--;
                        if (dep < 0) 
                            return null;
                        continue;
                    }
                }

                if (dep == 0 && i > pos && i < posend && i != skip)
                {
                    if (tokens[i].type == TokenType.PUNCTUATION || (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "as") || (tokens[i].type == TokenType.KEYWORD && tokens[i].text == "is"))
                    {
                        if (tokens[i].text == "." && tokens[i - 1].type == TokenType.TYPE)
                        {
                            listt.Add(i);
                        }
                        else
                        {
                            list.Add(i);
                        }
                        skip = i + 1;
                    }
                }
            }
            return list.Count > 0 ? list : listt;
        }

        int GetLowestMathOp(IList<Token> tokens, IList<int> list)
        {
            // 最低优先级
            int minPriority = int.MaxValue;
            // token位置
            int tokenPos = -1;
            foreach (int i in list)
            {
                int priority = 0;
                switch (tokens[i].text)
                {
                    case "=":
                    case "+=":
                    case "-=":
                    case "*=":
                    case "/=":
                    case "%=":
                    case "<<=":
                    case ">>=":
                    case "&=":
                    case "|=":
                    case "^=":
                        priority = -2;
                        break;
                    case "?":
                        priority = -1;
                        break;
                    case ":":
                        priority = 0;
                        break;
                    case "<":
                        priority = 5;
                        break;
                    case ">":
                        priority = 5;
                        break;
                    case "<=":
                        priority = 5;
                        break;
                    case ">=":
                        priority = 5;
                        break;
                    case "&&":
                        priority = 3;
                        break;
                    case "||":
                        priority = 3;
                        break;
                    case "==":
                        priority = 4;
                        break;
                    case "!=":
                        priority = 4;
                        break;
                    case "*":
                        priority = 7;
                        break;
                    case "/":
                        priority = 7;
                        break;
                    case "%":
                        priority = 7;
                        break;
                    case "+":
                        priority = 6;
                        break;
                    case "-":
                        priority = 6;
                        break;
                    case ".":
                        priority = 10;
                        break;
                    case "=>":
                        priority = 8;
                        break;
                    case "[":
                        priority = 10;
                        break;
                    case "(":
                        priority = 9;
                        break;
                    case "as":
                        priority = 9;
                        break;
                    case "is":
                        priority = 9;
                        break;
                }
                if (tokens[i].text == "(")//(int)(xxx) //这种表达式要优先处理前一个
                {
                    if (priority < minPriority)
                    {
                        minPriority = priority;
                        tokenPos = i;
                    }
                }
                else
                {
                    if (priority <= minPriority)
                    {
                        minPriority = priority;
                        tokenPos = i;
                    }
                }
            }

            return tokenPos;
        }
    }
}
