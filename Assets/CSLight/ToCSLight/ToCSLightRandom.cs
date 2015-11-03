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

public class ToCSLightRandom : RegHelper_Type
{
    public ToCSLightRandom()
    {
        this.type = typeof(UnityEngine.Random);
        this.sysType = this.type;
        this.keyword = "UnityEngine.Random";
        this.function = new ToCSLightRandom_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((UnityEngine.Random)src);
    }

    public class ToCSLightRandom_Fun : RegHelper_TypeFunction
    {
        public ToCSLightRandom_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			CLS_Content.Value val = new CLS_Content.Value();
			val.type = typeof(UnityEngine.Random);
			val.value = new UnityEngine.Random();
			return val;
        }

        public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
			if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = UnityEngine.Random.Equals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "Range")
			{
				if ((_params[0].value is int) && (_params[1].value is int))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(int);
					val.value = UnityEngine.Random.Range(Convert.ToInt32(_params[0].value), Convert.ToInt32(_params[1].value));
					return val;
				}
				else if ((_params[0].value is IConvertible) && (_params[1].value is IConvertible))
				{
					CLS_Content.Value val = new CLS_Content.Value();
					val.type = typeof(float);
					val.value = UnityEngine.Random.Range(Convert.ToSingle(_params[0].value), Convert.ToSingle(_params[1].value));
					return val;
				}
			}
			else if (function == "ReferenceEquals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = UnityEngine.Random.ReferenceEquals(_params[0].value, _params[1].value);
				return val;
			}

            return base.StaticCall(content, function, _params);
        }

        public override CLS_Content.Value StaticValueGet(CLS_Content content, string valuename)
        {
			if (valuename == "insideUnitCircle")
				return new CLS_Content.Value() { type = typeof(Vector2), value = UnityEngine.Random.insideUnitCircle };
			if (valuename == "insideUnitSphere")
				return new CLS_Content.Value() { type = typeof(Vector3), value = UnityEngine.Random.insideUnitSphere };
			if (valuename == "onUnitSphere")
				return new CLS_Content.Value() { type = typeof(Vector3), value = UnityEngine.Random.onUnitSphere };
			if (valuename == "rotation")
				return new CLS_Content.Value() { type = typeof(Quaternion), value = UnityEngine.Random.rotation };
			if (valuename == "rotationUniform")
				return new CLS_Content.Value() { type = typeof(Quaternion), value = UnityEngine.Random.rotationUniform };
			if (valuename == "seed")
				return new CLS_Content.Value() { type = typeof(int), value = UnityEngine.Random.seed };
			if (valuename == "value")
				return new CLS_Content.Value() { type = typeof(float), value = UnityEngine.Random.value };

            return base.StaticValueGet(content, valuename);
        }

        public override void StaticValueSet(CLS_Content content, string valuename, object value)
        {
			if (valuename == "seed")
			{
				UnityEngine.Random.seed = Convert.ToInt32(value);
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
				val.value = ((UnityEngine.Random)object_this).Equals(_params[0].value);
				return val;
			}
			else if (function == "ToString")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(string);
				val.value = ((UnityEngine.Random)object_this).ToString();
				return val;
			}

            return base.MemberCall(content, object_this, function, _params, isBaseCall);
        }
    }
}
