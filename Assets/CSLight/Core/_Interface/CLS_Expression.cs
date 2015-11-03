using System;
using System.Collections.Generic;
using System.Text;
namespace CSLE
{
    // 值
    public interface ICLS_Value
    {
        CLType type
        {
            get;
        }

        object value
        {
            get;
        }

        int tokenBegin
        {
            get;
            set;
        }

        int tokenEnd
        {
            get;
            set;
        }

        int lineBegin
        {
            get;
            set;
        }

        int lineEnd
        {
            get;
            set;
        }
    }

    // 表达式
    public interface ICLS_Expression
    {
        List<ICLS_Expression> listParam
        {
            get;
        }

        int tokenBegin
        {
            get;
        }

        int tokenEnd
        {
            get;
        }

        int lineBegin
        {
            get;
        }

        int lineEnd
        {
            get;
        }

        CLS_Content.Value ComputeValue(CLS_Content content);
    }

    public interface ICLS_Environment
    {
        string version
        {
            get;
        }

        ICLS_Logger logger
        {
            get;
        }

        void RegType(ICLS_Type type);
        ICLS_Type GetType(CLType type);
        ICLS_Type GetTypeByKeyword(string keyword);
        ICLS_Type GetTypeByKeywordQuiet(string keyword);

        void RegFunction(ICLS_Function func);
        ICLS_Function GetFunction(string name);
    }

    public interface ICLS_Expression_Compiler
    {
        ICLS_Expression Compiler(IList<Token> tlist, ICLS_Environment content);//语句
        ICLS_Expression Compiler_NoBlock(IList<Token> tlist, ICLS_Environment content);//表达式，一条语句
        ICLS_Expression Optimize(ICLS_Expression value, ICLS_Environment content);

        IList<ICLS_Type> FileCompiler(ICLS_Environment env, string filename, IList<Token> tlist, bool embDebugToken);
        IList<ICLS_Type> FilePreCompiler(ICLS_Environment env, string filename, IList<Token> tlist);
    }
}
