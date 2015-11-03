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

public class ToCSLightMathf : RegHelper_Type
{
    public ToCSLightMathf()
    {
        this.type = typeof(Mathf);
        this.sysType = this.type;
        this.keyword = "Mathf";
        this.function = new ToCSLightMathf_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((Mathf)src);
    }

    public class ToCSLightMathf_Fun : RegHelper_TypeFunction
    {
        public ToCSLightMathf_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
			if (function == "Abs")
			{
				if ((_params[0].value is int))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Mathf.Abs(Convert.ToInt32(_params[0].value));
					return val;
				}
				else if ((_params[0].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(float);
					val.value = Mathf.Abs(Convert.ToSingle(_params[0].value));
					return val;
				}
			}
			else if (function == "Acos")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Acos(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "Approximately")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Mathf.Approximately(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
				return val;
			}
			else if (function == "Asin")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Asin(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "Atan")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Atan(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "Atan2")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Atan2(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
				return val;
			}
			else if (function == "Ceil")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Ceil(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "CeilToInt")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(int);
				val.value = Mathf.CeilToInt(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "Clamp")
			{
				if ((_params[0].value is int) && (_params[1].value is int) && (_params[2].value is int))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Mathf.Clamp(Convert.ToInt32(_params[0].value), Convert.ToInt32(_params[1].value), Convert.ToInt32(_params[2].value));
					return val;
				}
				else if ((_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(float);
					val.value = Mathf.Clamp(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
					return val;
				}
			}
			else if (function == "Clamp01")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Clamp01(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "ClosestPowerOfTwo")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(int);
				val.value = Mathf.ClosestPowerOfTwo(Convert.ToInt32(_params[0].value));
				return val;
			}
			else if (function == "Cos")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Cos(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "DeltaAngle")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.DeltaAngle(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
				return val;
			}
			else if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Mathf.Equals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "Exp")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Exp(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "FloatToHalf")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(ushort);
				val.value = Mathf.FloatToHalf(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "Floor")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Floor(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "FloorToInt")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(int);
				val.value = Mathf.FloorToInt(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "Gamma")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Gamma(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "GammaToLinearSpace")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.GammaToLinearSpace(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "HalfToFloat")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.HalfToFloat(Convert.ToUInt16(_params[0].value));
				return val;
			}
			else if (function == "InverseLerp")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.InverseLerp(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "IsPowerOfTwo")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Mathf.IsPowerOfTwo(Convert.ToInt32(_params[0].value));
				return val;
			}
			else if (function == "Lerp")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Lerp(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "LerpAngle")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.LerpAngle(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "LerpUnclamped")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.LerpUnclamped(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "LinearToGammaSpace")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.LinearToGammaSpace(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "Log")
			{
				if (_params.size == 1)
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(float);
					val.value = Mathf.Log(Convert.ToSingle(_params[0].value));
					return val;
				}
				else if (_params.size == 2)
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(float);
					val.value = Mathf.Log(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
					return val;
				}
			}
			else if (function == "Log10")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Log10(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "Max")
			{
				if (_params.size == 2 && (_params[0].value is int) && (_params[1].value is int))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Mathf.Max(Convert.ToInt32(_params[0].value), Convert.ToInt32(_params[1].value));
					return val;
				}
				else if (_params.size == 2 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(float);
					val.value = Mathf.Max(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
					return val;
				}
			}
			else if (function == "Min")
			{
				if (_params.size == 2 && (_params[0].value is int) && (_params[1].value is int))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Mathf.Min(Convert.ToInt32(_params[0].value), Convert.ToInt32(_params[1].value));
					return val;
				}
				else if (_params.size == 2 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(float);
					val.value = Mathf.Min(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
					return val;
				}
			}
			else if (function == "MoveTowards")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.MoveTowards(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "MoveTowardsAngle")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.MoveTowardsAngle(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "NextPowerOfTwo")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(int);
				val.value = Mathf.NextPowerOfTwo(Convert.ToInt32(_params[0].value));
				return val;
			}
			else if (function == "PerlinNoise")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.PerlinNoise(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
				return val;
			}
			else if (function == "PingPong")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.PingPong(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
				return val;
			}
			else if (function == "Pow")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Pow(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
				return val;
			}
			else if (function == "ReferenceEquals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Mathf.ReferenceEquals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "Repeat")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Repeat(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
				return val;
			}
			else if (function == "Round")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Round(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "RoundToInt")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(int);
				val.value = Mathf.RoundToInt(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "Sign")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Sign(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "Sin")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Sin(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "SmoothStep")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.SmoothStep(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
				return val;
			}
			else if (function == "Sqrt")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Sqrt(Convert.ToSingle(_params[0].value));
				return val;
			}
			else if (function == "Tan")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(float);
				val.value = Mathf.Tan(Convert.ToSingle(_params[0].value));
				return val;
			}

            return base.StaticCall(content, function, _params);
        }

        public override CLS_Content.Value StaticValueGet(CLS_Content content, string valuename)
        {
			if (valuename == "Deg2Rad")
				return new CLS_Content.Value() { type = typeof(float), value = Mathf.Deg2Rad };
			if (valuename == "Epsilon")
				return new CLS_Content.Value() { type = typeof(float), value = Mathf.Epsilon };
			if (valuename == "Infinity")
				return new CLS_Content.Value() { type = typeof(float), value = Mathf.Infinity };
			if (valuename == "NegativeInfinity")
				return new CLS_Content.Value() { type = typeof(float), value = Mathf.NegativeInfinity };
			if (valuename == "PI")
				return new CLS_Content.Value() { type = typeof(float), value = Mathf.PI };
			if (valuename == "Rad2Deg")
				return new CLS_Content.Value() { type = typeof(float), value = Mathf.Rad2Deg };

            return base.StaticValueGet(content, valuename);
        }

        public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
            CLS_Content.Value retVal = null;

			Mathf newVal = (Mathf)object_this;
			if (function == "Equals")
			{
				retVal = new CLS_Content.Value();
				retVal.type = typeof(bool);
				retVal.value = newVal.Equals(_params[0].value);
			}
			else if (function == "ToString")
			{
				retVal = new CLS_Content.Value();
				retVal.type = typeof(string);
				retVal.value = newVal.ToString();
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
    }
}
