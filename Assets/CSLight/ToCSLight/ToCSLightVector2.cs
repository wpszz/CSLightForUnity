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

public class ToCSLightVector2 : RegHelper_Type
{
    public ToCSLightVector2()
    {
        this.type = typeof(Vector2);
        this.sysType = this.type;
        this.keyword = "Vector2";
        this.function = new ToCSLightVector2_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((Vector2)src);
    }

    public override bool MathLogic(CLS_Content content, logictoken code, object left, CLS_Content.Value right)
    {
		if (code == logictoken.equal)
		{
			return ((Vector2)left) == ((Vector2)right.value);
		}
		if (code == logictoken.not_equal)
		{
			return ((Vector2)left) != ((Vector2)right.value);
		}

        return base.MathLogic(content, code, left, right);
    }

    public override object Math2Value(CLS_Content content, char code, object left, CLS_Content.Value right, out CLType returntype)
    {
		if (code == '+')
		{
			if (right.value is Vector2)
			{
				returntype = typeof(Vector2);
				return ((Vector2)left) + ((Vector2)right.value);
			}
		}
		if (code == '-')
		{
			returntype = typeof(Vector2);
			return ((Vector2)left) - ((Vector2)right.value);
		}
		if (code == '*')
		{
			returntype = typeof(Vector2);
			return ((Vector2)left) * Convert.ToSingle(right.value);
		}
		if (code == '/')
		{
			returntype = typeof(Vector2);
			return ((Vector2)left) / Convert.ToSingle(right.value);
		}

        return base.Math2Value(content, code, left, right, out returntype);
    }

    public class ToCSLightVector2_Fun : RegHelper_TypeFunction
    {
        public ToCSLightVector2_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			CLS_Content.Value val = new CLS_Content.Value();
			val.type = typeof(Vector2);
			val.value = new Vector2(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
			return val;
        }

        public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
			if (function == "Angle")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Vector2.Angle(((Vector2)_params[0].value), ((Vector2)_params[1].value));
				return val;
			}
			else if (function == "ClampMagnitude")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector2);
				val.value = Vector2.ClampMagnitude(((Vector2)_params[0].value), Convert.ToSingle(_params[1].value));
				return val;
			}
			else if (function == "Distance")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Vector2.Distance(((Vector2)_params[0].value), ((Vector2)_params[1].value));
				return val;
			}
			else if (function == "Dot")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Vector2.Dot(((Vector2)_params[0].value), ((Vector2)_params[1].value));
				return val;
			}
			else if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Vector2.Equals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "Lerp")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector2);
				val.value = Vector2.Lerp(((Vector2)_params[0].value), ((Vector2)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "LerpUnclamped")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector2);
				val.value = Vector2.LerpUnclamped(((Vector2)_params[0].value), ((Vector2)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "Max")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector2);
				val.value = Vector2.Max(((Vector2)_params[0].value), ((Vector2)_params[1].value));
				return val;
			}
			else if (function == "Min")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector2);
				val.value = Vector2.Min(((Vector2)_params[0].value), ((Vector2)_params[1].value));
				return val;
			}
			else if (function == "MoveTowards")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector2);
				val.value = Vector2.MoveTowards(((Vector2)_params[0].value), ((Vector2)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "ReferenceEquals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Vector2.ReferenceEquals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "Reflect")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector2);
				val.value = Vector2.Reflect(((Vector2)_params[0].value), ((Vector2)_params[1].value));
				return val;
			}
			else if (function == "Scale")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector2);
				val.value = Vector2.Scale(((Vector2)_params[0].value), ((Vector2)_params[1].value));
				return val;
			}
			else if (function == "SqrMagnitude")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Vector2.SqrMagnitude(((Vector2)_params[0].value));
				return val;
			}

            return base.StaticCall(content, function, _params);
        }

        public override CLS_Content.Value StaticValueGet(CLS_Content content, string valuename)
        {
			if (valuename == "down")
				return new CLS_Content.Value() { type = typeof(Vector2), value = Vector2.down };
			if (valuename == "kEpsilon")
				return new CLS_Content.Value() { type = typeof(float), value = Vector2.kEpsilon };
			if (valuename == "left")
				return new CLS_Content.Value() { type = typeof(Vector2), value = Vector2.left };
			if (valuename == "one")
				return new CLS_Content.Value() { type = typeof(Vector2), value = Vector2.one };
			if (valuename == "right")
				return new CLS_Content.Value() { type = typeof(Vector2), value = Vector2.right };
			if (valuename == "up")
				return new CLS_Content.Value() { type = typeof(Vector2), value = Vector2.up };
			if (valuename == "zero")
				return new CLS_Content.Value() { type = typeof(Vector2), value = Vector2.zero };

            return base.StaticValueGet(content, valuename);
        }

        public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
            CLS_Content.Value retVal = null;

			Vector2 newVal = (Vector2)object_this;
			if (function == "Equals")
			{
				retVal = new CLS_Content.Value();
				retVal.type = typeof(bool);
				retVal.value = newVal.Equals(_params[0].value);
			}
			else if (function == "Normalize")
			{
				newVal.Normalize();
				retVal = CLS_Content.Value.Void;
			}
			else if (function == "Scale")
			{
				newVal.Scale(((Vector2)_params[0].value));
				retVal = CLS_Content.Value.Void;
			}
			else if (function == "Set")
			{
				newVal.Set(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
				retVal = CLS_Content.Value.Void;
			}
			else if (function == "SqrMagnitude")
			{
				retVal = new CLS_Content.Value();
				retVal.type = typeof(float);
				retVal.value = newVal.SqrMagnitude();
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
			if (valuename == "magnitude")
				return new CLS_Content.Value() { type = typeof(float), value = ((Vector2)object_this).magnitude };
			if (valuename == "normalized")
				return new CLS_Content.Value() { type = typeof(Vector2), value = ((Vector2)object_this).normalized };
			if (valuename == "sqrMagnitude")
				return new CLS_Content.Value() { type = typeof(float), value = ((Vector2)object_this).sqrMagnitude };
			if (valuename == "x")
				return new CLS_Content.Value() { type = typeof(float), value = ((Vector2)object_this).x };
			if (valuename == "y")
				return new CLS_Content.Value() { type = typeof(float), value = ((Vector2)object_this).y };

            return base.MemberValueGet(content, object_this, valuename, isBaseCall);
        }

        public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
			Vector2 newVal = (Vector2)object_this;
			if (valuename == "x")
				newVal.x = Convert.ToSingle(value);
			else if (valuename == "y")
				newVal.y = Convert.ToSingle(value);
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
			return new CLS_Content.Value() { type = typeof(float), value = ((Vector2)object_this)[Convert.ToInt32(key)] };
        }
    }
}
