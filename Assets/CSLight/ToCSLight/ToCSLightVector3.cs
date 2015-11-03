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

public class ToCSLightVector3 : RegHelper_Type
{
    public ToCSLightVector3()
    {
        this.type = typeof(Vector3);
        this.sysType = this.type;
        this.keyword = "Vector3";
        this.function = new ToCSLightVector3_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((Vector3)src);
    }

    public override bool MathLogic(CLS_Content content, logictoken code, object left, CLS_Content.Value right)
    {
		if (code == logictoken.equal)
		{
			return ((Vector3)left) == ((Vector3)right.value);
		}
		if (code == logictoken.not_equal)
		{
			return ((Vector3)left) != ((Vector3)right.value);
		}

        return base.MathLogic(content, code, left, right);
    }

    public override object Math2Value(CLS_Content content, char code, object left, CLS_Content.Value right, out CLType returntype)
    {
		if (code == '+')
		{
			if (right.value is Vector3)
			{
				returntype = typeof(Vector3);
				return ((Vector3)left) + ((Vector3)right.value);
			}
		}
		if (code == '-')
		{
			returntype = typeof(Vector3);
			return ((Vector3)left) - ((Vector3)right.value);
		}
		if (code == '*')
		{
			returntype = typeof(Vector3);
			return ((Vector3)left) * Convert.ToSingle(right.value);
		}
		if (code == '/')
		{
			returntype = typeof(Vector3);
			return ((Vector3)left) / Convert.ToSingle(right.value);
		}

        return base.Math2Value(content, code, left, right, out returntype);
    }

    public class ToCSLightVector3_Fun : RegHelper_TypeFunction
    {
        public ToCSLightVector3_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			if (_params.size == 3)
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = new Vector3(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			if (_params.size == 2)
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = new Vector3(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
				return val;
			}

            return base.New(content, _params);
        }

        public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
			if (function == "Angle")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Vector3.Angle(((Vector3)_params[0].value), ((Vector3)_params[1].value));
				return val;
			}
			else if (function == "ClampMagnitude")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.ClampMagnitude(((Vector3)_params[0].value), Convert.ToSingle(_params[1].value));
				return val;
			}
			else if (function == "Cross")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.Cross(((Vector3)_params[0].value), ((Vector3)_params[1].value));
				return val;
			}
			else if (function == "Distance")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Vector3.Distance(((Vector3)_params[0].value), ((Vector3)_params[1].value));
				return val;
			}
			else if (function == "Dot")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Vector3.Dot(((Vector3)_params[0].value), ((Vector3)_params[1].value));
				return val;
			}
			else if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Vector3.Equals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "Lerp")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.Lerp(((Vector3)_params[0].value), ((Vector3)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "LerpUnclamped")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.LerpUnclamped(((Vector3)_params[0].value), ((Vector3)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "Magnitude")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Vector3.Magnitude(((Vector3)_params[0].value));
				return val;
			}
			else if (function == "Max")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.Max(((Vector3)_params[0].value), ((Vector3)_params[1].value));
				return val;
			}
			else if (function == "Min")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.Min(((Vector3)_params[0].value), ((Vector3)_params[1].value));
				return val;
			}
			else if (function == "MoveTowards")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.MoveTowards(((Vector3)_params[0].value), ((Vector3)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "Normalize")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.Normalize(((Vector3)_params[0].value));
				return val;
			}
			else if (function == "Project")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.Project(((Vector3)_params[0].value), ((Vector3)_params[1].value));
				return val;
			}
			else if (function == "ProjectOnPlane")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.ProjectOnPlane(((Vector3)_params[0].value), ((Vector3)_params[1].value));
				return val;
			}
			else if (function == "ReferenceEquals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Vector3.ReferenceEquals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "Reflect")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.Reflect(((Vector3)_params[0].value), ((Vector3)_params[1].value));
				return val;
			}
			else if (function == "RotateTowards")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.RotateTowards(((Vector3)_params[0].value), ((Vector3)_params[1].value), Convert.ToSingle(_params[2].value), Convert.ToSingle(_params[3].value));
				return val;
			}
			else if (function == "Scale")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.Scale(((Vector3)_params[0].value), ((Vector3)_params[1].value));
				return val;
			}
			else if (function == "Slerp")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.Slerp(((Vector3)_params[0].value), ((Vector3)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "SlerpUnclamped")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = Vector3.SlerpUnclamped(((Vector3)_params[0].value), ((Vector3)_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "SqrMagnitude")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Vector3.SqrMagnitude(((Vector3)_params[0].value));
				return val;
			}

            return base.StaticCall(content, function, _params);
        }

        public override CLS_Content.Value StaticValueGet(CLS_Content content, string valuename)
        {
			if (valuename == "back")
				return new CLS_Content.Value() { type = typeof(Vector3), value = Vector3.back };
			if (valuename == "down")
				return new CLS_Content.Value() { type = typeof(Vector3), value = Vector3.down };
			if (valuename == "forward")
				return new CLS_Content.Value() { type = typeof(Vector3), value = Vector3.forward };
			if (valuename == "kEpsilon")
				return new CLS_Content.Value() { type = typeof(float), value = Vector3.kEpsilon };
			if (valuename == "left")
				return new CLS_Content.Value() { type = typeof(Vector3), value = Vector3.left };
			if (valuename == "one")
				return new CLS_Content.Value() { type = typeof(Vector3), value = Vector3.one };
			if (valuename == "right")
				return new CLS_Content.Value() { type = typeof(Vector3), value = Vector3.right };
			if (valuename == "up")
				return new CLS_Content.Value() { type = typeof(Vector3), value = Vector3.up };
			if (valuename == "zero")
				return new CLS_Content.Value() { type = typeof(Vector3), value = Vector3.zero };

            return base.StaticValueGet(content, valuename);
        }

        public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
            CLS_Content.Value retVal = null;

			Vector3 newVal = (Vector3)object_this;
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
				newVal.Scale(((Vector3)_params[0].value));
				retVal = CLS_Content.Value.Void;
			}
			else if (function == "Set")
			{
				newVal.Set(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
				retVal = CLS_Content.Value.Void;
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
				return new CLS_Content.Value() { type = typeof(float), value = ((Vector3)object_this).magnitude };
			if (valuename == "normalized")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((Vector3)object_this).normalized };
			if (valuename == "sqrMagnitude")
				return new CLS_Content.Value() { type = typeof(float), value = ((Vector3)object_this).sqrMagnitude };
			if (valuename == "x")
				return new CLS_Content.Value() { type = typeof(float), value = ((Vector3)object_this).x };
			if (valuename == "y")
				return new CLS_Content.Value() { type = typeof(float), value = ((Vector3)object_this).y };
			if (valuename == "z")
				return new CLS_Content.Value() { type = typeof(float), value = ((Vector3)object_this).z };

            return base.MemberValueGet(content, object_this, valuename, isBaseCall);
        }

        public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
			Vector3 newVal = (Vector3)object_this;
			if (valuename == "x")
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
			return new CLS_Content.Value() { type = typeof(float), value = ((Vector3)object_this)[Convert.ToInt32(key)] };
        }
    }
}
