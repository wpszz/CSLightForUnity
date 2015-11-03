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

public class ToCSLightMath : RegHelper_Type
{
    public ToCSLightMath()
    {
        this.type = typeof(Math);
        this.sysType = this.type;
        this.keyword = "Math";
        this.function = new ToCSLightMath_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return src;
    }

    public class ToCSLightMath_Fun : RegHelper_TypeFunction
    {
        public ToCSLightMath_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
			if (function == "Abs")
			{
				if ((_params[0].value is sbyte))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(sbyte);
					val.value = Math.Abs(((sbyte)_params[0].value));
					return val;
				}
				else if ((_params[0].value is short))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(short);
					val.value = Math.Abs(Convert.ToInt16(_params[0].value));
					return val;
				}
				else if ((_params[0].value is int))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Math.Abs(Convert.ToInt32(_params[0].value));
					return val;
				}
				else if ((_params[0].value is long))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(long);
					val.value = Math.Abs(Convert.ToInt64(_params[0].value));
					return val;
				}
				else if ((_params[0].value is float))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(float);
					val.value = Math.Abs(Convert.ToSingle(_params[0].value));
					return val;
				}
				else if ((_params[0].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(double);
					val.value = Math.Abs(Convert.ToDouble(_params[0].value));
					return val;
				}
			}
			else if (function == "Acos")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Acos(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Asin")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Asin(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Atan")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Atan(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Atan2")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Atan2(Convert.ToDouble(_params[0].value), Convert.ToDouble(_params[1].value));
				return val;
			}
			else if (function == "Ceiling")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Ceiling(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Cos")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Cos(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Cosh")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Cosh(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Exp")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Exp(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Floor")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Floor(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "IEEERemainder")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.IEEERemainder(Convert.ToDouble(_params[0].value), Convert.ToDouble(_params[1].value));
				return val;
			}
			else if (function == "Log")
			{
				if (_params.size == 1)
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(double);
					val.value = Math.Log(Convert.ToDouble(_params[0].value));
					return val;
				}
				else if (_params.size == 2)
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(double);
					val.value = Math.Log(Convert.ToDouble(_params[0].value), Convert.ToDouble(_params[1].value));
					return val;
				}
			}
			else if (function == "Log10")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Log10(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Max")
			{
				if ((_params[0].value is sbyte) && (_params[1].value is sbyte))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(sbyte);
					val.value = Math.Max(((sbyte)_params[0].value), ((sbyte)_params[1].value));
					return val;
				}
				else if ((_params[0].value is byte) && (_params[1].value is byte))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(byte);
					val.value = Math.Max(Convert.ToByte(_params[0].value), Convert.ToByte(_params[1].value));
					return val;
				}
				else if ((_params[0].value is short) && (_params[1].value is short))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(short);
					val.value = Math.Max(Convert.ToInt16(_params[0].value), Convert.ToInt16(_params[1].value));
					return val;
				}
				else if ((_params[0].value is ushort) && (_params[1].value is ushort))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(ushort);
					val.value = Math.Max(Convert.ToUInt16(_params[0].value), Convert.ToUInt16(_params[1].value));
					return val;
				}
				else if ((_params[0].value is int) && (_params[1].value is int))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Math.Max(Convert.ToInt32(_params[0].value), Convert.ToInt32(_params[1].value));
					return val;
				}
				else if ((_params[0].value is uint) && (_params[1].value is uint))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(uint);
					val.value = Math.Max(Convert.ToUInt32(_params[0].value), Convert.ToUInt32(_params[1].value));
					return val;
				}
				else if ((_params[0].value is long) && (_params[1].value is long))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(long);
					val.value = Math.Max(Convert.ToInt64(_params[0].value), Convert.ToInt64(_params[1].value));
					return val;
				}
				else if ((_params[0].value is ulong) && (_params[1].value is ulong))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(ulong);
					val.value = Math.Max(Convert.ToUInt64(_params[0].value), Convert.ToUInt64(_params[1].value));
					return val;
				}
				else if ((_params[0].value is float) && (_params[1].value is float))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(float);
					val.value = Math.Max(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
					return val;
				}
				else if ((_params[0].value is IConvertible) && (_params[1].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(double);
					val.value = Math.Max(Convert.ToDouble(_params[0].value), Convert.ToDouble(_params[1].value));
					return val;
				}
			}
			else if (function == "Min")
			{
				if ((_params[0].value is sbyte) && (_params[1].value is sbyte))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(sbyte);
					val.value = Math.Min(((sbyte)_params[0].value), ((sbyte)_params[1].value));
					return val;
				}
				else if ((_params[0].value is byte) && (_params[1].value is byte))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(byte);
					val.value = Math.Min(Convert.ToByte(_params[0].value), Convert.ToByte(_params[1].value));
					return val;
				}
				else if ((_params[0].value is short) && (_params[1].value is short))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(short);
					val.value = Math.Min(Convert.ToInt16(_params[0].value), Convert.ToInt16(_params[1].value));
					return val;
				}
				else if ((_params[0].value is ushort) && (_params[1].value is ushort))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(ushort);
					val.value = Math.Min(Convert.ToUInt16(_params[0].value), Convert.ToUInt16(_params[1].value));
					return val;
				}
				else if ((_params[0].value is int) && (_params[1].value is int))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Math.Min(Convert.ToInt32(_params[0].value), Convert.ToInt32(_params[1].value));
					return val;
				}
				else if ((_params[0].value is uint) && (_params[1].value is uint))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(uint);
					val.value = Math.Min(Convert.ToUInt32(_params[0].value), Convert.ToUInt32(_params[1].value));
					return val;
				}
				else if ((_params[0].value is long) && (_params[1].value is long))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(long);
					val.value = Math.Min(Convert.ToInt64(_params[0].value), Convert.ToInt64(_params[1].value));
					return val;
				}
				else if ((_params[0].value is ulong) && (_params[1].value is ulong))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(ulong);
					val.value = Math.Min(Convert.ToUInt64(_params[0].value), Convert.ToUInt64(_params[1].value));
					return val;
				}
				else if ((_params[0].value is float) && (_params[1].value is float))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(float);
					val.value = Math.Min(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
					return val;
				}
				else if ((_params[0].value is IConvertible) && (_params[1].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(double);
					val.value = Math.Min(Convert.ToDouble(_params[0].value), Convert.ToDouble(_params[1].value));
					return val;
				}
			}
			else if (function == "Pow")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Pow(Convert.ToDouble(_params[0].value), Convert.ToDouble(_params[1].value));
				return val;
			}
			else if (function == "Round")
			{
				if (_params.size == 1 && (_params[0].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(double);
					val.value = Math.Round(Convert.ToDouble(_params[0].value));
					return val;
				}
				else if (_params.size == 2 && (_params[0].value is IConvertible) && (_params[1].value is MidpointRounding))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(double);
					val.value = Math.Round(Convert.ToDouble(_params[0].value), ((MidpointRounding)_params[1].value));
					return val;
				}
				else if (_params.size == 2 && (_params[0].value is IConvertible) && (_params[1].value is int))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(double);
					val.value = Math.Round(Convert.ToDouble(_params[0].value), Convert.ToInt32(_params[1].value));
					return val;
				}
				else if (_params.size == 3 && (_params[0].value is IConvertible) && (_params[1].value is int) && (_params[2].value is MidpointRounding))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(double);
					val.value = Math.Round(Convert.ToDouble(_params[0].value), Convert.ToInt32(_params[1].value), ((MidpointRounding)_params[2].value));
					return val;
				}
			}
			else if (function == "Sign")
			{
				if ((_params[0].value is sbyte))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Math.Sign(((sbyte)_params[0].value));
					return val;
				}
				else if ((_params[0].value is short))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Math.Sign(Convert.ToInt16(_params[0].value));
					return val;
				}
				else if ((_params[0].value is int))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Math.Sign(Convert.ToInt32(_params[0].value));
					return val;
				}
				else if ((_params[0].value is long))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Math.Sign(Convert.ToInt64(_params[0].value));
					return val;
				}
				else if ((_params[0].value is float))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Math.Sign(Convert.ToSingle(_params[0].value));
					return val;
				}
				else if ((_params[0].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = Math.Sign(Convert.ToDouble(_params[0].value));
					return val;
				}
			}
			else if (function == "Sin")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Sin(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Sinh")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Sinh(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Sqrt")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Sqrt(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Tan")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Tan(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Tanh")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Tanh(Convert.ToDouble(_params[0].value));
				return val;
			}
			else if (function == "Truncate")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(double);
				val.value = Math.Truncate(Convert.ToDouble(_params[0].value));
				return val;
			}

            return base.StaticCall(content, function, _params);
        }

        public override CLS_Content.Value StaticValueGet(CLS_Content content, string valuename)
        {
			if (valuename == "E")
				return new CLS_Content.Value() { type = typeof(double), value = Math.E };
			if (valuename == "PI")
				return new CLS_Content.Value() { type = typeof(double), value = Math.PI };

            return base.StaticValueGet(content, valuename);
        }
    }
}
