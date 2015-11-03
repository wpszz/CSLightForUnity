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

public class ToCSLightTime : RegHelper_Type
{
    public ToCSLightTime()
    {
        this.type = typeof(Time);
        this.sysType = this.type;
        this.keyword = "Time";
        this.function = new ToCSLightTime_Fun(this.type);
    }

    public override object ConvertTo(CLS_Content content, object src, CLType targetType)
    {
		return ((Time)src);
    }

    public class ToCSLightTime_Fun : RegHelper_TypeFunction
    {
        public ToCSLightTime_Fun(Type type)
            : base(type)
        {
        }

        public override CLS_Content.Value New(CLS_Content content, BetterList<CLS_Content.Value> _params)
        {
			CLS_Content.Value val = new CLS_Content.Value();
			val.type = typeof(Time);
			val.value = new Time();
			return val;
        }

        public override CLS_Content.Value StaticCall(CLS_Content content, string function, BetterList<CLS_Content.Value> _params)
        {
			if (function == "Equals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Time.Equals(_params[0].value, _params[1].value);
				return val;
			}
			else if (function == "ReferenceEquals")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(bool);
				val.value = Time.ReferenceEquals(_params[0].value, _params[1].value);
				return val;
			}

            return base.StaticCall(content, function, _params);
        }

        public override CLS_Content.Value StaticValueGet(CLS_Content content, string valuename)
        {
			if (valuename == "captureFramerate")
				return new CLS_Content.Value() { type = typeof(int), value = Time.captureFramerate };
			if (valuename == "deltaTime")
				return new CLS_Content.Value() { type = typeof(float), value = Time.deltaTime };
			if (valuename == "fixedDeltaTime")
				return new CLS_Content.Value() { type = typeof(float), value = Time.fixedDeltaTime };
			if (valuename == "fixedTime")
				return new CLS_Content.Value() { type = typeof(float), value = Time.fixedTime };
			if (valuename == "frameCount")
				return new CLS_Content.Value() { type = typeof(int), value = Time.frameCount };
			if (valuename == "maximumDeltaTime")
				return new CLS_Content.Value() { type = typeof(float), value = Time.maximumDeltaTime };
			if (valuename == "realtimeSinceStartup")
				return new CLS_Content.Value() { type = typeof(float), value = Time.realtimeSinceStartup };
			if (valuename == "renderedFrameCount")
				return new CLS_Content.Value() { type = typeof(int), value = Time.renderedFrameCount };
			if (valuename == "smoothDeltaTime")
				return new CLS_Content.Value() { type = typeof(float), value = Time.smoothDeltaTime };
			if (valuename == "time")
				return new CLS_Content.Value() { type = typeof(float), value = Time.time };
			if (valuename == "timeScale")
				return new CLS_Content.Value() { type = typeof(float), value = Time.timeScale };
			if (valuename == "timeSinceLevelLoad")
				return new CLS_Content.Value() { type = typeof(float), value = Time.timeSinceLevelLoad };
			if (valuename == "unscaledDeltaTime")
				return new CLS_Content.Value() { type = typeof(float), value = Time.unscaledDeltaTime };
			if (valuename == "unscaledTime")
				return new CLS_Content.Value() { type = typeof(float), value = Time.unscaledTime };

            return base.StaticValueGet(content, valuename);
        }

        public override void StaticValueSet(CLS_Content content, string valuename, object value)
        {
			if (valuename == "captureFramerate")
			{
				Time.captureFramerate = Convert.ToInt32(value);
				return;
			}
			if (valuename == "fixedDeltaTime")
			{
				Time.fixedDeltaTime = Convert.ToSingle(value);
				return;
			}
			if (valuename == "maximumDeltaTime")
			{
				Time.maximumDeltaTime = Convert.ToSingle(value);
				return;
			}
			if (valuename == "timeScale")
			{
				Time.timeScale = Convert.ToSingle(value);
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
				val.value = ((Time)object_this).Equals(_params[0].value);
				return val;
			}
			else if (function == "ToString")
			{
				CLS_Content.Value val = new CLS_Content.Value();
				val.type = typeof(string);
				val.value = ((Time)object_this).ToString();
				return val;
			}

            return base.MemberCall(content, object_this, function, _params, isBaseCall);
        }
    }
}
