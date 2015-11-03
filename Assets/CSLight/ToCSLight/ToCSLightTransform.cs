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

public class ToCSLightTransform : ToCSLightComponent
{
    public ToCSLightTransform()
    {
        this.type = typeof(Transform);
        this.sysType = this.type;
        this.keyword = "Transform";
        this.function = new ToCSLightTransform_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((Transform)src);
    }

    public class ToCSLightTransform_Fun : ToCSLightComponent.ToCSLightComponent_Fun
    {
        public ToCSLightTransform_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
			if (function == "DetachChildren")
			{
				((Transform)object_this).DetachChildren();
				return CLS_Content.Value.Void;
			}
			else if (function == "Find")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Transform);
				val.value = ((Transform)object_this).Find(((string)_params[0].value));
				return val;
			}
			else if (function == "FindChild")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Transform);
				val.value = ((Transform)object_this).FindChild(((string)_params[0].value));
				return val;
			}
			else if (function == "GetChild")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Transform);
				val.value = ((Transform)object_this).GetChild(Convert.ToInt32(_params[0].value));
				return val;
			}
			else if (function == "GetEnumerator")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(IEnumerator);
				val.value = ((Transform)object_this).GetEnumerator();
				return val;
			}
			else if (function == "GetSiblingIndex")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(int);
				val.value = ((Transform)object_this).GetSiblingIndex();
				return val;
			}
			else if (function == "InverseTransformDirection")
			{
				if (_params.size == 1 && (_params[0].value is Vector3))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Vector3);
					val.value = ((Transform)object_this).InverseTransformDirection(((Vector3)_params[0].value));
					return val;
				}
				else if (_params.size == 3 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Vector3);
					val.value = ((Transform)object_this).InverseTransformDirection(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
					return val;
				}
			}
			else if (function == "InverseTransformPoint")
			{
				if (_params.size == 1 && (_params[0].value is Vector3))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Vector3);
					val.value = ((Transform)object_this).InverseTransformPoint(((Vector3)_params[0].value));
					return val;
				}
				else if (_params.size == 3 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Vector3);
					val.value = ((Transform)object_this).InverseTransformPoint(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
					return val;
				}
			}
			else if (function == "InverseTransformVector")
			{
				if (_params.size == 1 && (_params[0].value is Vector3))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Vector3);
					val.value = ((Transform)object_this).InverseTransformVector(((Vector3)_params[0].value));
					return val;
				}
				else if (_params.size == 3 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Vector3);
					val.value = ((Transform)object_this).InverseTransformVector(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
					return val;
				}
			}
			else if (function == "IsChildOf")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = ((Transform)object_this).IsChildOf(((Transform)_params[0].value));
				return val;
			}
			else if (function == "LookAt")
			{
				if (_params.size == 1 && (_params[0].value is Vector3))
				{
					((Transform)object_this).LookAt(((Vector3)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 1 && (_params[0].value == null || _params[0].value is Transform))
				{
					((Transform)object_this).LookAt(((Transform)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value is Vector3) && (_params[1].value is Vector3))
				{
					((Transform)object_this).LookAt(((Vector3)_params[0].value), ((Vector3)_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is Transform) && (_params[1].value is Vector3))
				{
					((Transform)object_this).LookAt(((Transform)_params[0].value), ((Vector3)_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "Rotate")
			{
				if (_params.size == 1 && (_params[0].value is Vector3))
				{
					((Transform)object_this).Rotate(((Vector3)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value is Vector3) && (_params[1].value is Space))
				{
					((Transform)object_this).Rotate(((Vector3)_params[0].value), ((Space)_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value is Vector3) && (_params[1].value is IConvertible))
				{
					((Transform)object_this).Rotate(((Vector3)_params[0].value), Convert.ToSingle(_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 3 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible))
				{
					((Transform)object_this).Rotate(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 3 && (_params[0].value is Vector3) && (_params[1].value is IConvertible) && (_params[2].value is Space))
				{
					((Transform)object_this).Rotate(((Vector3)_params[0].value), Convert.ToSingle(_params[1].value), ((Space)_params[2].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 4 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible) && (_params[3].value is Space))
				{
					((Transform)object_this).Rotate(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value), ((Space)_params[3].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "RotateAround")
			{
				((Transform)object_this).RotateAround(((Vector3)_params[0].value), ((Vector3)_params[1].value), Convert.ToSingle(_params[2].value));
				return CLS_Content.Value.Void;
			}
			else if (function == "SetAsFirstSibling")
			{
				((Transform)object_this).SetAsFirstSibling();
				return CLS_Content.Value.Void;
			}
			else if (function == "SetAsLastSibling")
			{
				((Transform)object_this).SetAsLastSibling();
				return CLS_Content.Value.Void;
			}
			else if (function == "SetParent")
			{
				if (_params.size == 1 && (_params[0].value == null || _params[0].value is Transform))
				{
					((Transform)object_this).SetParent(((Transform)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value == null || _params[0].value is Transform) && (_params[1].value is IConvertible))
				{
					((Transform)object_this).SetParent(((Transform)_params[0].value), Convert.ToBoolean(_params[1].value));
					return CLS_Content.Value.Void;
				}
			}
			else if (function == "SetSiblingIndex")
			{
				((Transform)object_this).SetSiblingIndex(Convert.ToInt32(_params[0].value));
				return CLS_Content.Value.Void;
			}
			else if (function == "TransformDirection")
			{
				if (_params.size == 1 && (_params[0].value is Vector3))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Vector3);
					val.value = ((Transform)object_this).TransformDirection(((Vector3)_params[0].value));
					return val;
				}
				else if (_params.size == 3 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Vector3);
					val.value = ((Transform)object_this).TransformDirection(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
					return val;
				}
			}
			else if (function == "TransformPoint")
			{
				if (_params.size == 1 && (_params[0].value is Vector3))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Vector3);
					val.value = ((Transform)object_this).TransformPoint(((Vector3)_params[0].value));
					return val;
				}
				else if (_params.size == 3 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Vector3);
					val.value = ((Transform)object_this).TransformPoint(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
					return val;
				}
			}
			else if (function == "TransformVector")
			{
				if (_params.size == 1 && (_params[0].value is Vector3))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Vector3);
					val.value = ((Transform)object_this).TransformVector(((Vector3)_params[0].value));
					return val;
				}
				else if (_params.size == 3 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(Vector3);
					val.value = ((Transform)object_this).TransformVector(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
					return val;
				}
			}
			else if (function == "Translate")
			{
				if (_params.size == 1 && (_params[0].value is Vector3))
				{
					((Transform)object_this).Translate(((Vector3)_params[0].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value is Vector3) && (_params[1].value is Space))
				{
					((Transform)object_this).Translate(((Vector3)_params[0].value), ((Space)_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 2 && (_params[0].value is Vector3) && (_params[1].value == null || _params[1].value is Transform))
				{
					((Transform)object_this).Translate(((Vector3)_params[0].value), ((Transform)_params[1].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 3 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible))
				{
					((Transform)object_this).Translate(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 4 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible) && (_params[3].value is Space))
				{
					((Transform)object_this).Translate(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value), ((Space)_params[3].value));
					return CLS_Content.Value.Void;
				}
				else if (_params.size == 4 && (_params[0].value is IConvertible) && (_params[1].value is IConvertible) && (_params[2].value is IConvertible) && (_params[3].value == null || _params[3].value is Transform))
				{
					((Transform)object_this).Translate(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value), Convert.ToSingle(_params[2].value), ((Transform)_params[3].value));
					return CLS_Content.Value.Void;
				}
			}

            return base.MemberCall(content, object_this, function, _params, isBaseCall);
        }

        public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
        {
			if (valuename == "childCount")
				return new CLS_Content.Value() { type = typeof(int), value = ((Transform)object_this).childCount };
			if (valuename == "eulerAngles")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((Transform)object_this).eulerAngles };
			if (valuename == "forward")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((Transform)object_this).forward };
			if (valuename == "hasChanged")
				return new CLS_Content.Value() { type = typeof(bool), value = ((Transform)object_this).hasChanged };
			if (valuename == "localEulerAngles")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((Transform)object_this).localEulerAngles };
			if (valuename == "localPosition")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((Transform)object_this).localPosition };
			if (valuename == "localRotation")
				return new CLS_Content.Value() { type = typeof(Quaternion), value = ((Transform)object_this).localRotation };
			if (valuename == "localScale")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((Transform)object_this).localScale };
			if (valuename == "localToWorldMatrix")
				return new CLS_Content.Value() { type = typeof(Matrix4x4), value = ((Transform)object_this).localToWorldMatrix };
			if (valuename == "lossyScale")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((Transform)object_this).lossyScale };
			if (valuename == "parent")
				return new CLS_Content.Value() { type = typeof(Transform), value = ((Transform)object_this).parent };
			if (valuename == "position")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((Transform)object_this).position };
			if (valuename == "right")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((Transform)object_this).right };
			if (valuename == "root")
				return new CLS_Content.Value() { type = typeof(Transform), value = ((Transform)object_this).root };
			if (valuename == "rotation")
				return new CLS_Content.Value() { type = typeof(Quaternion), value = ((Transform)object_this).rotation };
			if (valuename == "up")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((Transform)object_this).up };
			if (valuename == "worldToLocalMatrix")
				return new CLS_Content.Value() { type = typeof(Matrix4x4), value = ((Transform)object_this).worldToLocalMatrix };

            return base.MemberValueGet(content, object_this, valuename, isBaseCall);
        }

        public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
			if (valuename == "eulerAngles")
			{
				((Transform)object_this).eulerAngles = ((Vector3)value);
				return;
			}
			if (valuename == "forward")
			{
				((Transform)object_this).forward = ((Vector3)value);
				return;
			}
			if (valuename == "hasChanged")
			{
				((Transform)object_this).hasChanged = Convert.ToBoolean(value);
				return;
			}
			if (valuename == "localEulerAngles")
			{
				((Transform)object_this).localEulerAngles = ((Vector3)value);
				return;
			}
			if (valuename == "localPosition")
			{
				((Transform)object_this).localPosition = ((Vector3)value);
				return;
			}
			if (valuename == "localRotation")
			{
				((Transform)object_this).localRotation = ((Quaternion)value);
				return;
			}
			if (valuename == "localScale")
			{
				((Transform)object_this).localScale = ((Vector3)value);
				return;
			}
			if (valuename == "parent")
			{
				((Transform)object_this).parent = ((Transform)value);
				return;
			}
			if (valuename == "position")
			{
				((Transform)object_this).position = ((Vector3)value);
				return;
			}
			if (valuename == "right")
			{
				((Transform)object_this).right = ((Vector3)value);
				return;
			}
			if (valuename == "rotation")
			{
				((Transform)object_this).rotation = ((Quaternion)value);
				return;
			}
			if (valuename == "up")
			{
				((Transform)object_this).up = ((Vector3)value);
				return;
			}

            base.MemberValueSet(content, object_this, valuename, value, isBaseCall);
        }
    }
}
