using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using CSLE;

public class ToCSLightColor : RegHelper_Type
{
    public ToCSLightColor()
    {
        this.type = typeof(Color);
        this.sysType = this.type;
        this.keyword = "Color";
        this.function = new ToCSLightColor_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((Color)src);
    }

    public override bool MathLogic(CLS_Content content, logictoken code, object left, CLS_Content.Value right)
    {
		if (code == logictoken.equal)
		{
			return ((Color)left) == ((Color)right.value);
		}
		if (code == logictoken.not_equal)
		{
			return ((Color)left) != ((Color)right.value);
		}

        return base.MathLogic(content, code, left, right);
    }

    public override object Math2Value(CLS_Content content, char code, object left, CLS_Content.Value right, out CLType returntype)
    {
		if (code == '+')
		{
			if (right.value is Color)
			{
				returntype = typeof(Color);
				return ((Color)left) + ((Color)right.value);
			}
		}
		if (code == '-')
		{
			returntype = typeof(Color);
			return ((Color)left) - ((Color)right.value);
		}
		if (code == '*')
		{
			if (right.value is Color)
			{
				returntype = typeof(Color);
				return ((Color)left) * ((Color)right.value);
			}
			if (right.value is float)
			{
				returntype = typeof(Color);
				return ((Color)left) * Convert.ToSingle(right.value);
			}
		}
		if (code == '/')
		{
			returntype = typeof(Color);
			return ((Color)left) / Convert.ToSingle(right.value);
		}

        return base.Math2Value(content, code, left, right, out returntype);
    }

    public class ToCSLightColor_Fun : RegHelper_TypeFunction
    {
        public ToCSLightColor_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			if (_params.size == 4)
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Color);
				val.value = new Color(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value), Convert.ToSingle(_params[3].value));
				return val;
			}
			if (_params.size == 3)
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Color);
				val.value = new Color(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}

            return base.New(content, _params);
        }

        public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
			if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Color.Equals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "Lerp")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Color);
				val.value = Color.Lerp(((Color)_params[0].value), ((Color)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "LerpUnclamped")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Color);
				val.value = Color.LerpUnclamped(((Color)_params[0].value), ((Color)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "ReferenceEquals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Color.ReferenceEquals(_params[0].value, _params[1].value);
				return val;
			}

            return base.StaticCall(content, function, _params);
        }

        public override CLS_Content.Value StaticValueGet(CLS_Content content, string valuename)
        {
			if (valuename == "black")
				return new CLS_Content.Value() { type = typeof(Color), value = Color.black };
			if (valuename == "blue")
				return new CLS_Content.Value() { type = typeof(Color), value = Color.blue };
			if (valuename == "clear")
				return new CLS_Content.Value() { type = typeof(Color), value = Color.clear };
			if (valuename == "cyan")
				return new CLS_Content.Value() { type = typeof(Color), value = Color.cyan };
			if (valuename == "gray")
				return new CLS_Content.Value() { type = typeof(Color), value = Color.gray };
			if (valuename == "green")
				return new CLS_Content.Value() { type = typeof(Color), value = Color.green };
			if (valuename == "grey")
				return new CLS_Content.Value() { type = typeof(Color), value = Color.grey };
			if (valuename == "magenta")
				return new CLS_Content.Value() { type = typeof(Color), value = Color.magenta };
			if (valuename == "red")
				return new CLS_Content.Value() { type = typeof(Color), value = Color.red };
			if (valuename == "white")
				return new CLS_Content.Value() { type = typeof(Color), value = Color.white };
			if (valuename == "yellow")
				return new CLS_Content.Value() { type = typeof(Color), value = Color.yellow };

            return base.StaticValueGet(content, valuename);
        }

        public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
            CLS_Content.Value retVal = null;

			Color newVal = (Color)object_this;
			if (function == "Equals")
			{
				retVal = new CLS_Content.Value();
				retVal.type = typeof(bool);
				retVal.value = newVal.Equals(_params[0].value);
			}
			else if (function == "ToString")
			{
				if (_params.size == 0)
				{
					retVal = new CLS_Content.Value();
					retVal.type = typeof(string);
					retVal.value = newVal.ToString();
				}
				else if (_params.size == 1)
				{
					retVal = new CLS_Content.Value();
					retVal.type = typeof(string);
					retVal.value = newVal.ToString(((string)_params[0].value));
				}
			}
            else
                return base.MemberCall(content, object_this, function, _params, isBaseCall);
            ICLS_Expression expLeft = content.CallExpression.listParam[0];
            CLS_Expression_GetValue expGetValue = expLeft as CLS_Expression_GetValue;
            if (expGetValue != null)
            {
                content.Set(expGetValue.value_name, newVal);
                return retVal;
            }
            CLS_Expression_MemberFind expMemberFind = expLeft as CLS_Expression_MemberFind;
            if (expMemberFind != null)
            {
                expGetValue = expMemberFind.listParam[0] as CLS_Expression_GetValue;
                CLS_Content.Value val = content.Get(expGetValue.value_name);
                content.environment.GetType(val.type).function.MemberValueSet(content, val.value, expMemberFind.membername, newVal);
                return retVal;
            }

            return base.MemberCall(content, object_this, function, _params, isBaseCall);
        }

        public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
        {
			if (valuename == "a")
				return new CLS_Content.Value() { type = typeof(float), value = ((Color)object_this).a };
			if (valuename == "b")
				return new CLS_Content.Value() { type = typeof(float), value = ((Color)object_this).b };
			if (valuename == "g")
				return new CLS_Content.Value() { type = typeof(float), value = ((Color)object_this).g };
			if (valuename == "gamma")
				return new CLS_Content.Value() { type = typeof(Color), value = ((Color)object_this).gamma };
			if (valuename == "grayscale")
				return new CLS_Content.Value() { type = typeof(float), value = ((Color)object_this).grayscale };
			if (valuename == "linear")
				return new CLS_Content.Value() { type = typeof(Color), value = ((Color)object_this).linear };
			if (valuename == "maxColorComponent")
				return new CLS_Content.Value() { type = typeof(float), value = ((Color)object_this).maxColorComponent };
			if (valuename == "r")
				return new CLS_Content.Value() { type = typeof(float), value = ((Color)object_this).r };

            return base.MemberValueGet(content, object_this, valuename, isBaseCall);
        }

        public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
			Color newVal = (Color)object_this;
			if (valuename == "a")
				newVal.a = Convert.ToSingle(value);
			else if (valuename == "b")
				newVal.b = Convert.ToSingle(value);
			else if (valuename == "g")
				newVal.g = Convert.ToSingle(value);
			else if (valuename == "r")
				newVal.r = Convert.ToSingle(value);
            else
                base.MemberValueSet(content, object_this, valuename, value, isBaseCall);
            ICLS_Expression expLeft = content.CallExpression.listParam[0];
            CLS_Expression_GetValue expGetValue = expLeft as CLS_Expression_GetValue;
            if (expGetValue != null)
            {
                content.Set(expGetValue.value_name, newVal);
                return;
            }
            CLS_Expression_MemberFind expMemberFind = expLeft as CLS_Expression_MemberFind;
            if (expMemberFind != null)
            {
                expGetValue = expMemberFind.listParam[0] as CLS_Expression_GetValue;
                if (content.CallExpression is CLS_Expression_SelfOpWithValue)
                {
                    content.Set(expGetValue.value_name, newVal);
                    return;
                }
                CLS_Content.Value val = content.Get(expGetValue.value_name);
                content.environment.GetType(val.type).function.MemberValueSet(content, val.value, expMemberFind.membername, newVal);
                return;
            }

            base.MemberValueSet(content, object_this, valuename, value, isBaseCall);
        }

        public override CLS_Content.Value IndexGet(CLS_Content content, object object_this, object key)
        {
			return new CLS_Content.Value() { type = typeof(float), value = ((Color)object_this)[Convert.ToInt32(key)] };
        }
    }
}
