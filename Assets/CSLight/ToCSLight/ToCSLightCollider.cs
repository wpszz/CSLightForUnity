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

public class ToCSLightCollider : ToCSLightComponent
{
    public ToCSLightCollider()
    {
        this.type = typeof(Collider);
        this.sysType = this.type;
        this.keyword = "Collider";
        this.function = new ToCSLightCollider_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((Collider)src);
    }

    public class ToCSLightCollider_Fun : ToCSLightComponent.ToCSLightComponent_Fun
    {
        public ToCSLightCollider_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			CLS_Content.Value val = new CLS_Content.Value();
			val.type = typeof(Collider);
			val.value = new Collider();
			return val;
        }

        public override CLS_Content.Value MemberCall(CLS_Content content, object object_this, string function, BetterList<CLS_Content.Value> _params, bool isBaseCall = false)
        {
			if (function == "ClosestPointOnBounds")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(Vector3);
				val.value = ((Collider)object_this).ClosestPointOnBounds(((Vector3)_params[0].value));
				return val;
			}

            return base.MemberCall(content, object_this, function, _params, isBaseCall);
        }

        public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
        {
			if (valuename == "attachedRigidbody")
				return new CLS_Content.Value() { type = typeof(Rigidbody), value = ((Collider)object_this).attachedRigidbody };
			if (valuename == "bounds")
				return new CLS_Content.Value() { type = typeof(Bounds), value = ((Collider)object_this).bounds };
			if (valuename == "contactOffset")
				return new CLS_Content.Value() { type = typeof(float), value = ((Collider)object_this).contactOffset };
			if (valuename == "enabled")
				return new CLS_Content.Value() { type = typeof(bool), value = ((Collider)object_this).enabled };
			if (valuename == "isTrigger")
				return new CLS_Content.Value() { type = typeof(bool), value = ((Collider)object_this).isTrigger };
			if (valuename == "material")
				return new CLS_Content.Value() { type = typeof(PhysicMaterial), value = ((Collider)object_this).material };
			if (valuename == "sharedMaterial")
				return new CLS_Content.Value() { type = typeof(PhysicMaterial), value = ((Collider)object_this).sharedMaterial };

            return base.MemberValueGet(content, object_this, valuename, isBaseCall);
        }

        public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
			if (valuename == "contactOffset")
			{
				((Collider)object_this).contactOffset = Convert.ToSingle(value);
				return;
			}
			if (valuename == "enabled")
			{
				((Collider)object_this).enabled = Convert.ToBoolean(value);
				return;
			}
			if (valuename == "isTrigger")
			{
				((Collider)object_this).isTrigger = Convert.ToBoolean(value);
				return;
			}
			if (valuename == "material")
			{
				((Collider)object_this).material = ((PhysicMaterial)value);
				return;
			}
			if (valuename == "sharedMaterial")
			{
				((Collider)object_this).sharedMaterial = ((PhysicMaterial)value);
				return;
			}

            base.MemberValueSet(content, object_this, valuename, value, isBaseCall);
        }
    }
}
