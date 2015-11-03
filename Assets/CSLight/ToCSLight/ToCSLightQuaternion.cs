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

public class ToCSLightQuaternion : RegHelper_Type
{
    public ToCSLightQuaternion()
    {
        this.type = typeof(Quaternion);
        this.sysType = this.type;
        this.keyword = "Quaternion";
        this.function = new ToCSLightQuaternion_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((Quaternion)src);
    }

    public override bool MathLogic(CLS_Content content, logictoken code, object left, CLS_Content.Value right)
    {
		if (code == logictoken.equal)
		{
			return ((Quaternion)left) == ((Quaternion)right.value);
		}
		if (code == logictoken.not_equal)
		{
			return ((Quaternion)left) != ((Quaternion)right.value);
		}

        return base.MathLogic(content, code, left, right);
    }

    public override object Math2Value(CLS_Content content, char code, object left, CLS_Content.Value right, out CLType returntype)
    {
		if (code == '*')
		{
			if (right.value is Quaternion)
			{
				returntype = typeof(Quaternion);
				return ((Quaternion)left) * ((Quaternion)right.value);
			}
			if (right.value is Vector3)
			{
				returntype = typeof(Vector3);
				return ((Quaternion)left) * ((Vector3)right.value);
			}
		}

        return base.Math2Value(content, code, left, right, out returntype);
    }

    public class ToCSLightQuaternion_Fun : RegHelper_TypeFunction
    {
        public ToCSLightQuaternion_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			CLS_Content.Value val = new CLS_Content.Value();
			val.type = typeof(Quaternion);
			val.value = new Quaternion(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value), Convert.ToSingle(_params[3].value));
			return val;
        }

        public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
			if (function == "Angle")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Quaternion.Angle(((Quaternion)_params[0].value), ((Quaternion)_params[1].value));
				return val;
			}
			else if (function == "AngleAxis")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Quaternion);
				val.value = Quaternion.AngleAxis(Convert.ToSingle(_params[0].value), ((Vector3)_params[1].value));
				return val;
			}
			else if (function == "Dot")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Quaternion.Dot(((Quaternion)_params[0].value), ((Quaternion)_params[1].value));
				return val;
			}
			else if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Quaternion.Equals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "Euler")
			{
				if (_params.size == 1 && (_params[0].value is Vector3))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Quaternion);
					val.value = Quaternion.Euler(((Vector3)_params[0].value));
					return val;
				}
				else if (_params.size == 3 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Quaternion);
					val.value = Quaternion.Euler(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
					return val;
				}
			}
			else if (function == "FromToRotation")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Quaternion);
				val.value = Quaternion.FromToRotation(((Vector3)_params[0].value), ((Vector3)_params[1].value));
				return val;
			}
			else if (function == "Inverse")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Quaternion);
				val.value = Quaternion.Inverse(((Quaternion)_params[0].value));
				return val;
			}
			else if (function == "Lerp")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Quaternion);
				val.value = Quaternion.Lerp(((Quaternion)_params[0].value), ((Quaternion)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "LerpUnclamped")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Quaternion);
				val.value = Quaternion.LerpUnclamped(((Quaternion)_params[0].value), ((Quaternion)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "LookRotation")
			{
				if (_params.size == 1)
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Quaternion);
					val.value = Quaternion.LookRotation(((Vector3)_params[0].value));
					return val;
				}
				else if (_params.size == 2)
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Quaternion);
					val.value = Quaternion.LookRotation(((Vector3)_params[0].value), ((Vector3)_params[1].value));
					return val;
				}
			}
			else if (function == "ReferenceEquals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Quaternion.ReferenceEquals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "RotateTowards")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Quaternion);
				val.value = Quaternion.RotateTowards(((Quaternion)_params[0].value), ((Quaternion)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "Slerp")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Quaternion);
				val.value = Quaternion.Slerp(((Quaternion)_params[0].value), ((Quaternion)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "SlerpUnclamped")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Quaternion);
				val.value = Quaternion.SlerpUnclamped(((Quaternion)_params[0].value), ((Quaternion)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}

            return base.StaticCall(content, function, _params);
        }

        public override CLS_Content.Value StaticValueGet(CLS_Content content, string valuename)
        {
			if (valuename == "identity")
				return new CLS_Content.Value() { type = typeof(Quaternion), value = Quaternion.identity };
			if (valuename == "kEpsilon")
				return new CLS_Content.Value() { type = typeof(float), value = Quaternion.kEpsilon };

            return base.StaticValueGet(content, valuename);
        }

        public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
            CLS_Content.Value retVal = null;

			Quaternion newVal = (Quaternion)object_this;
			if (function == "Equals")
			{
				retVal = new CLS_Content.Value();
				retVal.type = typeof(bool);
				retVal.value = newVal.Equals(_params[0].value);
			}
			else if (function == "Set")
			{
				newVal.Set(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value), Convert.ToSingle(_params[3].value));
				retVal = CLS_Content.Value.Void;
			}
			else if (function == "SetFromToRotation")
			{
				newVal.SetFromToRotation(((Vector3)_params[0].value), ((Vector3)_params[1].value));
				retVal = CLS_Content.Value.Void;
			}
			else if (function == "SetLookRotation")
			{
				if (_params.size == 1)
				{
					newVal.SetLookRotation(((Vector3)_params[0].value));
					retVal = CLS_Content.Value.Void;
				}
				else if (_params.size == 2)
				{
					newVal.SetLookRotation(((Vector3)_params[0].value), ((Vector3)_params[1].value));
					retVal = CLS_Content.Value.Void;
				}
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
			if (valuename == "eulerAngles")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((Quaternion)object_this).eulerAngles };
			if (valuename == "w")
				return new CLS_Content.Value() { type = typeof(float), value = ((Quaternion)object_this).w };
			if (valuename == "x")
				return new CLS_Content.Value() { type = typeof(float), value = ((Quaternion)object_this).x };
			if (valuename == "y")
				return new CLS_Content.Value() { type = typeof(float), value = ((Quaternion)object_this).y };
			if (valuename == "z")
				return new CLS_Content.Value() { type = typeof(float), value = ((Quaternion)object_this).z };

            return base.MemberValueGet(content, object_this, valuename, isBaseCall);
        }

        public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
			Quaternion newVal = (Quaternion)object_this;
			if (valuename == "eulerAngles")
				newVal.eulerAngles = ((Vector3)value);
			else if (valuename == "w")
				newVal.w = Convert.ToSingle(value);
			else if (valuename == "x")
				newVal.x = Convert.ToSingle(value);
			else if (valuename == "y")
				newVal.y = Convert.ToSingle(value);
			else if (valuename == "z")
				newVal.z = Convert.ToSingle(value);
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
			return new CLS_Content.Value() { type = typeof(float), value = ((Quaternion)object_this)[Convert.ToInt32(key)] };
        }
    }
}
