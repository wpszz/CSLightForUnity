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

public class ToCSLightBoxCollider : ToCSLightCollider
{
    public ToCSLightBoxCollider()
    {
        this.type = typeof(BoxCollider);
        this.sysType = this.type;
        this.keyword = "BoxCollider";
        this.function = new ToCSLightBoxCollider_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((BoxCollider)src);
    }

    public class ToCSLightBoxCollider_Fun : ToCSLightCollider.ToCSLightCollider_Fun
    {
        public ToCSLightBoxCollider_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			CLS_Content.Value val = new CLS_Content.Value();
			val.type = typeof(BoxCollider);
			val.value = new BoxCollider();
			return val;
        }


        public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
        {
			if (valuename == "center")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((BoxCollider)object_this).center };
			if (valuename == "size")
				return new CLS_Content.Value() { type = typeof(Vector3), value = ((BoxCollider)object_this).size };

            return base.MemberValueGet(content, object_this, valuename, isBaseCall);
        }

        public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
			if (valuename == "center")
			{
				((BoxCollider)object_this).center = ((Vector3)value);
				return;
			}
			if (valuename == "size")
			{
				((BoxCollider)object_this).size = ((Vector3)value);
				return;
			}

            base.MemberValueSet(content, object_this, valuename, value, isBaseCall);
        }
    }
}
