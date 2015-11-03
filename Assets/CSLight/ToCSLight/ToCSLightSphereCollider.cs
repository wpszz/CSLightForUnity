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

public class ToCSLightSphereCollider : ToCSLightCollider
{
    public ToCSLightSphereCollider()
    {
        this.type = typeof(SphereCollider);
        this.sysType = this.type;
        this.keyword = "SphereCollider";
        this.function = new ToCSLightSphereCollider_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((SphereCollider)src);
    }

    public class ToCSLightSphereCollider_Fun : ToCSLightCollider.ToCSLightCollider_Fun
    {
        public ToCSLightSphereCollider_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			CLS_Content.Value val = new CLS_Content.Value();
			val.type = typeof(SphereCollider);
			val.value = new SphereCollider();
			return val;
        }


        public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
        {
			if (valuename == "center")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((SphereCollider)object_this).center };
			if (valuename == "radius")
				return new CLS_Content.Value() { type = typeof(float), value = ((SphereCollider)object_this).radius };

            return base.MemberValueGet(content, object_this, valuename, isBaseCall);
        }

        public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
			if (valuename == "center")
			{
				((SphereCollider)object_this).center = ((Vector3)value);
				return;
			}
			if (valuename == "radius")
			{
				((SphereCollider)object_this).radius = Convert.ToSingle(value);
				return;
			}

            base.MemberValueSet(content, object_this, valuename, value, isBaseCall);
        }
    }
}
