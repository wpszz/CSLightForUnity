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

public class ToCSLightDebug : RegHelper_Type
{
    public ToCSLightDebug()
    {
        this.type = typeof(Debug);
        this.sysType = this.type;
        this.keyword = "Debug";
        this.function = new ToCSLightDebug_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((Debug)src);
    }

    public class ToCSLightDebug_Fun : RegHelper_TypeFunction
    {
        public ToCSLightDebug_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			CLS_Content.Value val = new CLS_Content.Value();
			val.type = typeof(Debug);
			val.value = new Debug();
			return val;
        }

        public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
			if (function == "Assert")
			{
				if (_params.size == 1 && (_params[0].value is bool))
				{
					Debug.Assert(Convert.ToBoolean(_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value is bool) && (_params[1].value == null || _params[1].value is string))
				{
					Debug.Assert(Convert.ToBoolean(_params[0].value), ((string)_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "Break")
			{
				Debug.Break();
				return CLS_Content.Value.Void;
			}
			else if (function == "ClearDeveloperConsole")
			{
				Debug.ClearDeveloperConsole();
				return CLS_Content.Value.Void;
			}
			else if (function == "DebugBreak")
			{
				Debug.DebugBreak();
				return CLS_Content.Value.Void;
			}
			else if (function == "DrawLine")
			{
				if (_params.size == 2 && (_params[0].value is Vector3) && (_params[1].value is Vector3))
				{
					Debug.DrawLine(((Vector3)_params[0].value), ((Vector3)_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 3 && (_params[0].value is Vector3) && (_params[1].value is Vector3) && (_params[2].value is Color))
				{
					Debug.DrawLine(((Vector3)_params[0].value), ((Vector3)_params[1].value), ((Color)_params[2].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 4 && (_params[0].value is Vector3) && (_params[1].value is Vector3) && (_params[2].value is Color) && (_params[3].value is IConvertible))
				{
					Debug.DrawLine(((Vector3)_params[0].value), ((Vector3)_params[1].value), ((Color)_params[2].value), Convert.ToSingle(_params[3].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 5 && (_params[0].value is Vector3) && (_params[1].value is Vector3) && (_params[2].value is Color) && (_params[3].value is IConvertible) && (_params[4].value is IConvertible))
				{
					Debug.DrawLine(((Vector3)_params[0].value), ((Vector3)_params[1].value), ((Color)_params[2].value), Convert.ToSingle(_params[3].value), Convert.ToBoolean(_params[4].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "DrawRay")
			{
				if (_params.size == 2 && (_params[0].value is Vector3) && (_params[1].value is Vector3))
				{
					Debug.DrawRay(((Vector3)_params[0].value), ((Vector3)_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 3 && (_params[0].value is Vector3) && (_params[1].value is Vector3) && (_params[2].value is Color))
				{
					Debug.DrawRay(((Vector3)_params[0].value), ((Vector3)_params[1].value), ((Color)_params[2].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 4 && (_params[0].value is Vector3) && (_params[1].value is Vector3) && (_params[2].value is Color) && (_params[3].value is IConvertible))
				{
					Debug.DrawRay(((Vector3)_params[0].value), ((Vector3)_params[1].value), ((Color)_params[2].value), Convert.ToSingle(_params[3].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 5 && (_params[0].value is Vector3) && (_params[1].value is Vector3) && (_params[2].value is Color) && (_params[3].value is IConvertible) && (_params[4].value is IConvertible))
				{
					Debug.DrawRay(((Vector3)_params[0].value), ((Vector3)_params[1].value), ((Color)_params[2].value), Convert.ToSingle(_params[3].value), Convert.ToBoolean(_params[4].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Debug.Equals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "Log")
			{
				if (_params.size == 1)
				{
					Debug.Log(_params[0].value);
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[1].value == null || _params[1].value is UnityEngine.Object))
				{
					Debug.Log(_params[0].value, ((UnityEngine.Object)_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "LogError")
			{
				if (_params.size == 1)
				{
					Debug.LogError(_params[0].value);
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[1].value == null || _params[1].value is UnityEngine.Object))
				{
					Debug.LogError(_params[0].value, ((UnityEngine.Object)_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "LogException")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Exception))
				{
					Debug.LogException(((Exception)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is Exception) && (_params[1].value == null || _params[1].value is UnityEngine.Object))
				{
					Debug.LogException(((Exception)_params[0].value), ((UnityEngine.Object)_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "LogWarning")
			{
				if (_params.size == 1)
				{
					Debug.LogWarning(_params[0].value);
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[1].value == null || _params[1].value is UnityEngine.Object))
				{
					Debug.LogWarning(_params[0].value, ((UnityEngine.Object)_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "ReferenceEquals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Debug.ReferenceEquals(_params[0].value, _params[1].value);
				return val;
			}

            return base.StaticCall(content, function, _params);
        }

        public override CLS_Content.Value StaticValueGet(CLS_Content content, string valuename)
        {
			if (valuename == "developerConsoleVisible")
				return new CLS_Content.Value() { type = typeof(bool), value = Debug.developerConsoleVisible };
			if (valuename == "isDebugBuild")
				return new CLS_Content.Value() { type = typeof(bool), value = Debug.isDebugBuild };

            return base.StaticValueGet(content, valuename);
        }

        public override void StaticValueSet(CLS_Content content, string valuename, object value)
        {
			if (valuename == "developerConsoleVisible")
			{
				Debug.developerConsoleVisible = Convert.ToBoolean(value);
				return;
			}

            base.StaticValueSet(content, valuename, value);
        }

        public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
			if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = ((Debug)object_this).Equals(_params[0].value);
				return val;
			}
			else if (function == "ToString")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(string);
				val.value = ((Debug)object_this).ToString();
				return val;
			}

            return base.MemberCall(content, object_this, function, _params, isBaseCall);
        }
    }
}
