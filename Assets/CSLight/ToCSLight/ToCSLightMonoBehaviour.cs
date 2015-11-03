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

public class ToCSLightMonoBehaviour : ToCSLightBehaviour
{
    public ToCSLightMonoBehaviour()
    {
        this.type = typeof(MonoBehaviour);
        this.sysType = this.type;
        this.keyword = "MonoBehaviour";
        this.function = new ToCSLightMonoBehaviour_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((MonoBehaviour)src);
    }

    public class ToCSLightMonoBehaviour_Fun : ToCSLightBehaviour.ToCSLightBehaviour_Fun
    {
        public ToCSLightMonoBehaviour_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			CLS_Content.Value val = new CLS_Content.Value();
			val.type = typeof(MonoBehaviour);
			val.value = new MonoBehaviour();
			return val;
        }

        public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
			if (function == "print")
			{
				MonoBehaviour.print(_params[0].value);
				return CLS_Content.Value.Void;
			}

            return base.StaticCall(content, function, _params);
        }

        public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
			if (function == "CancelInvoke")
			{
				if (_params.size == 0)
				{
					((MonoBehaviour)object_this).CancelInvoke();
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 1)
				{
					((MonoBehaviour)object_this).CancelInvoke(((string)_params[0].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "Invoke")
			{
				((MonoBehaviour)object_this).Invoke(((string)_params[0].value), Convert.ToSingle(_params[1].value));
				return CLS_Content.Value.Void;
			}
			else if (function == "InvokeRepeating")
			{
				((MonoBehaviour)object_this).InvokeRepeating(((string)_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
				return CLS_Content.Value.Void;
			}
			else if (function == "IsInvoking")
			{
				if (_params.size == 0)
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(bool);
					val.value = ((MonoBehaviour)object_this).IsInvoking();
					return val;
				}
				else if (_params.size == 1)
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(bool);
					val.value = ((MonoBehaviour)object_this).IsInvoking(((string)_params[0].value));
					return val;
				}
			}
			else if (function == "StartCoroutine")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is string))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Coroutine);
					val.value = ((MonoBehaviour)object_this).StartCoroutine(((string)_params[0].value));
					return val;
				}
				else if (_params.size == 1 && (_params[0].value is IEnumerator))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Coroutine);
					val.value = ((MonoBehaviour)object_this).StartCoroutine(((IEnumerator)_params[0].value));
					return val;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Coroutine);
					val.value = ((MonoBehaviour)object_this).StartCoroutine(((string)_params[0].value), _params[1].value);
					return val;
				}
			}
			else if (function == "StartCoroutine_Auto")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Coroutine);
				val.value = ((MonoBehaviour)object_this).StartCoroutine_Auto(((IEnumerator)_params[0].value));
				return val;
			}
			else if (function == "StopAllCoroutines")
			{
				((MonoBehaviour)object_this).StopAllCoroutines();
				return CLS_Content.Value.Void;
			}
			else if (function == "StopCoroutine")
			{
				if ((_params[0].value == null || _params[0].value is string))
				{
					((MonoBehaviour)object_this).StopCoroutine(((string)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if ((_params[0].value is IEnumerator))
				{
					((MonoBehaviour)object_this).StopCoroutine(((IEnumerator)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if ((_params[0].value == null || _params[0].value is Coroutine))
				{
					((MonoBehaviour)object_this).StopCoroutine(((Coroutine)_params[0].value));
					return CLS_Content.Value.Void;
				}
			}

            return base.MemberCall(content, object_this, function, _params, isBaseCall);
        }

        public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
        {
			if (valuename == "useGUILayout")
				return new CLS_Content.Value() { type = typeof(bool), value = ((MonoBehaviour)object_this).useGUILayout };

            return base.MemberValueGet(content, object_this, valuename, isBaseCall);
        }

        public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
			if (valuename == "useGUILayout")
			{
				((MonoBehaviour)object_this).useGUILayout = Convert.ToBoolean(value);
				return;
			}

            base.MemberValueSet(content, object_this, valuename, value, isBaseCall);
        }
    }
}
