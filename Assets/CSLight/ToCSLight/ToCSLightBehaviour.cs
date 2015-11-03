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

public class ToCSLightBehaviour : ToCSLightComponent
{
    public ToCSLightBehaviour()
    {
        this.type = typeof(Behaviour);
        this.sysType = this.type;
        this.keyword = "Behaviour";
        this.function = new ToCSLightBehaviour_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((Behaviour)src);
    }

    public class ToCSLightBehaviour_Fun : ToCSLightComponent.ToCSLightComponent_Fun
    {
        public ToCSLightBehaviour_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			CLS_Content.Value val = new CLS_Content.Value();
			val.type = typeof(Behaviour);
			val.value = new Behaviour();
			return val;
        }


        public override CLS_Content.Value MemberValueGet(CLS_Content content, object object_this, string valuename, bool isBaseCall = false)
        {
			if (valuename == "enabled")
				return new CLS_Content.Value() { type = typeof(bool), value = ((Behaviour)object_this).enabled };
			if (valuename == "isActiveAndEnabled")
				return new CLS_Content.Value() { type = typeof(bool), value = ((Behaviour)object_this).isActiveAndEnabled };

            return base.MemberValueGet(content, object_this, valuename, isBaseCall);
        }

        public override void MemberValueSet(CLS_Content content, object object_this, string valuename, object value, bool isBaseCall = false)
        {
			if (valuename == "enabled")
			{
				((Behaviour)object_this).enabled = Convert.ToBoolean(value);
				return;
			}

            base.MemberValueSet(content, object_this, valuename, value, isBaseCall);
        }
    }
}
