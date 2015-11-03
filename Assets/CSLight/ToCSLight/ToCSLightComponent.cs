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

public class ToCSLightComponent : RegHelper_Type
{
    public ToCSLightComponent()
    {
        this.type = typeof(Component);
        this.sysType = this.type;
        this.keyword = "Component";
        this.function = new ToCSLightComponent_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((Component)src);
    }

    public override bool MathLogic(CLS_Content content, logictoken code, object left, CLS_Content.Value right)
    {
		if (code == logictoken.equal)
		{
			return ((UnityEngine.Object)left) == ((UnityEngine.Object)right.value);
		}
		if (code == logictoken.not_equal)
		{
			return ((UnityEngine.Object)left) != ((UnityEngine.Object)right.value);
		}

        return base.MathLogic(content, code, left, right);
    }

    public class ToCSLightComponent_Fun : RegHelper_TypeFunction
    {
        public ToCSLightComponent_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			CLS_Content.Value val = new CLS_Content.Value();
			val.type = typeof(Component);
			val.value = new Component();
			return val;
        }

        public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
			if (function == "Destroy")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is UnityEngine.Object))
				{
					Component.Destroy(((UnityEngine.Object)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is UnityEngine.Object) && (_params[1].value is IConvertible))
				{
					Component.Destroy(((UnityEngine.Object)_params[0].value), Convert.ToSingle(_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "DestroyImmediate")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is UnityEngine.Object))
				{
					Component.DestroyImmediate(((UnityEngine.Object)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is UnityEngine.Object) && (_params[1].value is IConvertible))
				{
					Component.DestroyImmediate(((UnityEngine.Object)_params[0].value), Convert.ToBoolean(_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "DestroyObject")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is UnityEngine.Object))
				{
					Component.DestroyObject(((UnityEngine.Object)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is UnityEngine.Object) && (_params[1].value is IConvertible))
				{
					Component.DestroyObject(((UnityEngine.Object)_params[0].value), Convert.ToSingle(_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "DontDestroyOnLoad")
			{
				Component.DontDestroyOnLoad(((UnityEngine.Object)_params[0].value));
				return CLS_Content.Value.Void;
			}
			else if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Component.Equals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "FindObjectOfType")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(UnityEngine.Object);
					val.value = Component.FindObjectOfType(((Type)_params[0].value));
					return val;
				}
			}
			else if (function == "FindObjectsOfType")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(UnityEngine.Object[]);
					val.value = Component.FindObjectsOfType(((Type)_params[0].value));
					return val;
				}
			}
			else if (function == "Instantiate")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is UnityEngine.Object))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(UnityEngine.Object);
					val.value = Component.Instantiate(((UnityEngine.Object)_params[0].value));
					return val;
				}
				else if (_params.size == 3 && (_params[0].value == null || _params[0].value is UnityEngine.Object) && (_params[1].value is Vector3) && (_params[2].value is Quaternion))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(UnityEngine.Object);
					val.value = Component.Instantiate(((UnityEngine.Object)_params[0].value), ((Vector3)_params[1].value), ((Quaternion)_params[2].value));
					return val;
				}
			}
			else if (function == "ReferenceEquals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Component.ReferenceEquals(_params[0].value, _params[1].value);
				return val;
			}

            return base.StaticCall(content, function, _params);
        }

        public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
			if (function == "BroadcastMessage")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is string))
				{
					((Component)object_this).BroadcastMessage(((string)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string) && (_params[1].value is SendMessageOptions))
				{
					((Component)object_this).BroadcastMessage(((string)_params[0].value), ((SendMessageOptions)_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string))
				{
					((Component)object_this).BroadcastMessage(((string)_params[0].value), _params[1].value);
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 3 && (_params[0].value == null || _params[0].value is string) && (_params[2].value is SendMessageOptions))
				{
					((Component)object_this).BroadcastMessage(((string)_params[0].value), _params[1].value, ((SendMessageOptions)_params[2].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "CompareTag")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = ((Component)object_this).CompareTag(((string)_params[0].value));
				return val;
			}
			else if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = ((Component)object_this).Equals(_params[0].value);
				return val;
			}
			else if (function == "GetComponent")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is string))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component);
					val.value = ((Component)object_this).GetComponent(((string)_params[0].value));
					return val;
				}
				else if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component);
					val.value = ((Component)object_this).GetComponent(((Type)_params[0].value));
					return val;
				}
			}
			else if (function == "GetComponentInChildren")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component);
					val.value = ((Component)object_this).GetComponentInChildren(((Type)_params[0].value));
					return val;
				}
			}
			else if (function == "GetComponentInParent")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component);
					val.value = ((Component)object_this).GetComponentInParent(((Type)_params[0].value));
					return val;
				}
			}
			else if (function == "GetComponents")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component[]);
					val.value = ((Component)object_this).GetComponents(((Type)_params[0].value));
					return val;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is Type) && (_params[1].value == null || _params[1].value is List<Component>))
				{
					((Component)object_this).GetComponents(((Type)_params[0].value), ((List<Component>)_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "GetComponentsInChildren")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component[]);
					val.value = ((Component)object_this).GetComponentsInChildren(((Type)_params[0].value));
					return val;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is Type) && (_params[1].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component[]);
					val.value = ((Component)object_this).GetComponentsInChildren(((Type)_params[0].value), Convert.ToBoolean(_params[1].value));
					return val;
				}
			}
			else if (function == "GetComponentsInParent")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component[]);
					val.value = ((Component)object_this).GetComponentsInParent(((Type)_params[0].value));
					return val;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is Type) && (_params[1].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component[]);
					val.value = ((Component)object_this).GetComponentsInParent(((Type)_params[0].value), Convert.ToBoolean(_params[1].value));
					return val;
				}
			}
			else if (function == "GetInstanceID")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(int);
				val.value = ((Component)object_this).GetInstanceID();
				return val;
			}
			else if (function == "SendMessage")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is string))
				{
					((Component)object_this).SendMessage(((string)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string) && (_params[1].value is SendMessageOptions))
				{
					((Component)object_this).SendMessage(((string)_params[0].value), ((SendMessageOptions)_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string))
				{
					((Component)object_this).SendMessage(((string)_params[0].value), _params[1].value);
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 3 && (_params[0].value == null || _params[0].value is string) && (_params[2].value is SendMessageOptions))
				{
					((Component)object_this).SendMessage(((string)_params[0].value), _params[1].value, ((SendMessageOptions)_params[2].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "SendMessageUpwards")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is string))
				{
					((Component)object_this).SendMessageUpwards(((string)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string) && (_params[1].value is SendMessageOptions))
				{
					((Component)object_this).SendMessageUpwards(((string)_params[0].value), ((SendMessageOptions)_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string))
				{
					((Component)object_this).SendMessageUpwards(((string)_params[0].value), _params[1].value);
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 3 && (_params[0].value == null || _params[0].value is string) && (_params[2].value is SendMessageOptions))
				{
					((Component)object_this).SendMessageUpwards(((string)_params[0].value), _params[1].value, ((SendMessageOptions)_params[2].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "ToString")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(string);
				val.value = ((Component)object_this).ToString();
				return val;
			}

            return base.MemberCall(content, object_this, function, _params, isBaseCall);
        }

        public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
        {
			if (valuename == "gameObject")
				return new CLS_Content.Value() { type = typeof(GameObject), value = ((Component)object_this).gameObject };
			if (valuename == "tag")
				return new CLS_Content.Value() { type = typeof(string), value = ((Component)object_this).tag };
			if (valuename == "transform")
				return new CLS_Content.Value() { type = typeof(Transform), value = ((Component)object_this).transform };
			if (valuename == "hideFlags")
				return new CLS_Content.Value() { type = typeof(HideFlags), value = ((Component)object_this).hideFlags };
			if (valuename == "name")
				return new CLS_Content.Value() { type = typeof(string), value = ((Component)object_this).name };

            return base.MemberValueGet(content, object_this, valuename, isBaseCall);
        }

        public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
			if (valuename == "tag")
			{
				((Component)object_this).tag = ((string)value);
				return;
			}
			if (valuename == "hideFlags")
			{
				((Component)object_this).hideFlags = ((HideFlags)value);
				return;
			}
			if (valuename == "name")
			{
				((Component)object_this).name = ((string)value);
				return;
			}

            base.MemberValueSet(content, object_this, valuename, value, isBaseCall);
        }
    }
}
