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

public class ToCSLightGameObject : RegHelper_Type
{
    public ToCSLightGameObject()
    {
        this.type = typeof(GameObject);
        this.sysType = this.type;
        this.keyword = "GameObject";
        this.function = new ToCSLightGameObject_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((GameObject)src);
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

    public class ToCSLightGameObject_Fun : RegHelper_TypeFunction
    {
        public ToCSLightGameObject_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			if (_params.size == 1 && (_params[0].value == null || _params[0].value is string))
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(GameObject);
				val.value = new GameObject(((string)_params[0].value));
				return val;
			}
			if (_params.size == 0)
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(GameObject);
				val.value = new GameObject();
				return val;
			}
			if (_params.size == 2 && (_params[0].value == null || _params[0].value is string) && (_params[1].value == null || _params[1].value is Type[]))
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(GameObject);
				val.value = new GameObject(((string)_params[0].value), ((Type[])_params[1].value));
				return val;
			}

            return base.New(content, _params);
        }

        public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
			if (function == "CreatePrimitive")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(GameObject);
				val.value = GameObject.CreatePrimitive(((PrimitiveType)_params[0].value));
				return val;
			}
			else if (function == "Destroy")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is UnityEngine.Object))
				{
					GameObject.Destroy(((UnityEngine.Object)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is UnityEngine.Object) && (_params[1].value is IConvertible))
				{
					GameObject.Destroy(((UnityEngine.Object)_params[0].value), Convert.ToSingle(_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "DestroyImmediate")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is UnityEngine.Object))
				{
					GameObject.DestroyImmediate(((UnityEngine.Object)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is UnityEngine.Object) && (_params[1].value is IConvertible))
				{
					GameObject.DestroyImmediate(((UnityEngine.Object)_params[0].value), Convert.ToBoolean(_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "DestroyObject")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is UnityEngine.Object))
				{
					GameObject.DestroyObject(((UnityEngine.Object)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is UnityEngine.Object) && (_params[1].value is IConvertible))
				{
					GameObject.DestroyObject(((UnityEngine.Object)_params[0].value), Convert.ToSingle(_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "DontDestroyOnLoad")
			{
				GameObject.DontDestroyOnLoad(((UnityEngine.Object)_params[0].value));
				return CLS_Content.Value.Void;
			}
			else if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = GameObject.Equals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "Find")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(GameObject);
				val.value = GameObject.Find(((string)_params[0].value));
				return val;
			}
			else if (function == "FindGameObjectsWithTag")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(GameObject[]);
				val.value = GameObject.FindGameObjectsWithTag(((string)_params[0].value));
				return val;
			}
			else if (function == "FindGameObjectWithTag")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(GameObject);
				val.value = GameObject.FindGameObjectWithTag(((string)_params[0].value));
				return val;
			}
			else if (function == "FindObjectOfType")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(UnityEngine.Object);
					val.value = GameObject.FindObjectOfType(((Type)_params[0].value));
					return val;
				}
			}
			else if (function == "FindObjectsOfType")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(UnityEngine.Object[]);
					val.value = GameObject.FindObjectsOfType(((Type)_params[0].value));
					return val;
				}
			}
			else if (function == "FindWithTag")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(GameObject);
				val.value = GameObject.FindWithTag(((string)_params[0].value));
				return val;
			}
			else if (function == "Instantiate")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is UnityEngine.Object))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(UnityEngine.Object);
					val.value = GameObject.Instantiate(((UnityEngine.Object)_params[0].value));
					return val;
				}
				else if (_params.size == 3 && (_params[0].value == null || _params[0].value is UnityEngine.Object) && (_params[1].value is Vector3) && (_params[2].value is Quaternion))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(UnityEngine.Object);
					val.value = GameObject.Instantiate(((UnityEngine.Object)_params[0].value), ((Vector3)_params[1].value), ((Quaternion)_params[2].value));
					return val;
				}
			}
			else if (function == "ReferenceEquals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = GameObject.ReferenceEquals(_params[0].value, _params[1].value);
				return val;
			}

            return base.StaticCall(content, function, _params);
        }

        public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
			if (function == "AddComponent")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component);
					val.value = ((GameObject)object_this).AddComponent(((Type)_params[0].value));
					return val;
				}
			}
			else if (function == "BroadcastMessage")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is string))
				{
					((GameObject)object_this).BroadcastMessage(((string)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string) && (_params[1].value is SendMessageOptions))
				{
					((GameObject)object_this).BroadcastMessage(((string)_params[0].value), ((SendMessageOptions)_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string))
				{
					((GameObject)object_this).BroadcastMessage(((string)_params[0].value), _params[1].value);
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 3 && (_params[0].value == null || _params[0].value is string) && (_params[2].value is SendMessageOptions))
				{
					((GameObject)object_this).BroadcastMessage(((string)_params[0].value), _params[1].value, ((SendMessageOptions)_params[2].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "CompareTag")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = ((GameObject)object_this).CompareTag(((string)_params[0].value));
				return val;
			}
			else if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = ((GameObject)object_this).Equals(_params[0].value);
				return val;
			}
			else if (function == "GetComponent")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is string))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component);
					val.value = ((GameObject)object_this).GetComponent(((string)_params[0].value));
					return val;
				}
				else if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component);
					val.value = ((GameObject)object_this).GetComponent(((Type)_params[0].value));
					return val;
				}
			}
			else if (function == "GetComponentInChildren")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component);
					val.value = ((GameObject)object_this).GetComponentInChildren(((Type)_params[0].value));
					return val;
				}
			}
			else if (function == "GetComponentInParent")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component);
					val.value = ((GameObject)object_this).GetComponentInParent(((Type)_params[0].value));
					return val;
				}
			}
			else if (function == "GetComponents")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component[]);
					val.value = ((GameObject)object_this).GetComponents(((Type)_params[0].value));
					return val;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is Type) && (_params[1].value == null || _params[1].value is List<Component>))
				{
					((GameObject)object_this).GetComponents(((Type)_params[0].value), ((List<Component>)_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "GetComponentsInChildren")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component[]);
					val.value = ((GameObject)object_this).GetComponentsInChildren(((Type)_params[0].value));
					return val;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is Type) && (_params[1].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component[]);
					val.value = ((GameObject)object_this).GetComponentsInChildren(((Type)_params[0].value), Convert.ToBoolean(_params[1].value));
					return val;
				}
			}
			else if (function == "GetComponentsInParent")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Type))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component[]);
					val.value = ((GameObject)object_this).GetComponentsInParent(((Type)_params[0].value));
					return val;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is Type) && (_params[1].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Component[]);
					val.value = ((GameObject)object_this).GetComponentsInParent(((Type)_params[0].value), Convert.ToBoolean(_params[1].value));
					return val;
				}
			}
			else if (function == "GetInstanceID")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(int);
				val.value = ((GameObject)object_this).GetInstanceID();
				return val;
			}
			else if (function == "SendMessage")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is string))
				{
					((GameObject)object_this).SendMessage(((string)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string) && (_params[1].value is SendMessageOptions))
				{
					((GameObject)object_this).SendMessage(((string)_params[0].value), ((SendMessageOptions)_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string))
				{
					((GameObject)object_this).SendMessage(((string)_params[0].value), _params[1].value);
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 3 && (_params[0].value == null || _params[0].value is string) && (_params[2].value is SendMessageOptions))
				{
					((GameObject)object_this).SendMessage(((string)_params[0].value), _params[1].value, ((SendMessageOptions)_params[2].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "SendMessageUpwards")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is string))
				{
					((GameObject)object_this).SendMessageUpwards(((string)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string) && (_params[1].value is SendMessageOptions))
				{
					((GameObject)object_this).SendMessageUpwards(((string)_params[0].value), ((SendMessageOptions)_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is string))
				{
					((GameObject)object_this).SendMessageUpwards(((string)_params[0].value), _params[1].value);
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 3 && (_params[0].value == null || _params[0].value is string) && (_params[2].value is SendMessageOptions))
				{
					((GameObject)object_this).SendMessageUpwards(((string)_params[0].value), _params[1].value, ((SendMessageOptions)_params[2].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "SetActive")
			{
				((GameObject)object_this).SetActive(Convert.ToBoolean(_params[0].value));
				return CLS_Content.Value.Void;
			}
			else if (function == "ToString")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(string);
				val.value = ((GameObject)object_this).ToString();
				return val;
			}

            return base.MemberCall(content, object_this, function, _params, isBaseCall);
        }

        public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
        {
			if (valuename == "activeInHierarchy")
				return new CLS_Content.Value() { type = typeof(bool), value = ((GameObject)object_this).activeInHierarchy };
			if (valuename == "activeSelf")
				return new CLS_Content.Value() { type = typeof(bool), value = ((GameObject)object_this).activeSelf };
			if (valuename == "gameObject")
				return new CLS_Content.Value() { type = typeof(GameObject), value = ((GameObject)object_this).gameObject };
			if (valuename == "isStatic")
				return new CLS_Content.Value() { type = typeof(bool), value = ((GameObject)object_this).isStatic };
			if (valuename == "layer")
				return new CLS_Content.Value() { type = typeof(int), value = ((GameObject)object_this).layer };
			if (valuename == "tag")
				return new CLS_Content.Value() { type = typeof(string), value = ((GameObject)object_this).tag };
			if (valuename == "transform")
				return new CLS_Content.Value() { type = typeof(Transform), value = ((GameObject)object_this).transform };
			if (valuename == "hideFlags")
				return new CLS_Content.Value() { type = typeof(HideFlags), value = ((GameObject)object_this).hideFlags };
			if (valuename == "name")
				return new CLS_Content.Value() { type = typeof(string), value = ((GameObject)object_this).name };

            return base.MemberValueGet(content, object_this, valuename, isBaseCall);
        }

        public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
			if (valuename == "isStatic")
			{
				((GameObject)object_this).isStatic = Convert.ToBoolean(value);
				return;
			}
			if (valuename == "layer")
			{
				((GameObject)object_this).layer = Convert.ToInt32(value);
				return;
			}
			if (valuename == "tag")
			{
				((GameObject)object_this).tag = ((string)value);
				return;
			}
			if (valuename == "hideFlags")
			{
				((GameObject)object_this).hideFlags = ((HideFlags)value);
				return;
			}
			if (valuename == "name")
			{
				((GameObject)object_this).name = ((string)value);
				return;
			}

            base.MemberValueSet(content, object_this, valuename, value, isBaseCall);
        }
    }
}
