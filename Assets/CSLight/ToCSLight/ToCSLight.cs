using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using CSLE;

public static class ToCSLight
{
    private class ScriptLogger : ICLS_Logger
    {
        public void Log(string str)
        {
            //Debug.Log(str);
        }
        public void Log_Warn(string str)
        {
            str += "\n";
            str += CSLightMng.instance.DumpStack(null);
            Debug.LogWarning(str);
        }
        public void Log_Error(string str)
        {
            Debug.LogError(str);
        }
    }

    public static CLS_Environment CreateEnvironment()
    {
        CLS_Environment clsEnv = new CLS_Environment(new ScriptLogger());

        // 名空间注册
        clsEnv.RegType(new CLS_NameSpace("UnityEngine"));

        // 特殊类型注册
        clsEnv.RegType(new RegHelper_DeleAction<BaseEventData>(typeof(UnityAction<BaseEventData>), "UnityAction<BaseEventData>"));

        // gen start
		clsEnv.RegType(new ToCSLightBehaviour());
		clsEnv.RegType(new ToCSLightBoxCollider());
		clsEnv.RegType(new ToCSLightCollider());
		clsEnv.RegType(new ToCSLightColor());
		clsEnv.RegType(new ToCSLightComponent());
		clsEnv.RegType(new ToCSLightDebug());
		clsEnv.RegType(new ToCSLightGameObject());
		clsEnv.RegType(new ToCSLightMath());
		clsEnv.RegType(new ToCSLightMathf());
		clsEnv.RegType(new ToCSLightMonoBehaviour());
		clsEnv.RegType(new ToCSLightQuaternion());
		clsEnv.RegType(new ToCSLightSphereCollider());
		clsEnv.RegType(new ToCSLightTime());
		clsEnv.RegType(new ToCSLightTransform());
		clsEnv.RegType(new ToCSLightRandom());
		clsEnv.RegType(new ToCSLightVector2());
		clsEnv.RegType(new ToCSLightVector3());
		clsEnv.RegType(new RegHelper_DeleAction(typeof(Action), "Action"));
		clsEnv.RegType(new RegHelper_DeleAction<object>(typeof(Action<object>), "Action<object>"));
		clsEnv.RegType(new RegHelper_Type(typeof(Animator)));
		clsEnv.RegType(new RegHelper_Type(typeof(AnimatorStateInfo)));
		clsEnv.RegType(new RegHelper_Type(typeof(Application)));
		clsEnv.RegType(new RegHelper_Type(typeof(BaseEventData)));
		clsEnv.RegType(new RegHelper_Type(typeof(Button)));
		clsEnv.RegType(new RegHelper_Type(typeof(Button.ButtonClickedEvent), "Button.ButtonClickedEvent"));
		clsEnv.RegType(new RegHelper_Type(typeof(Camera)));
		clsEnv.RegType(new RegHelper_Type(typeof(Canvas)));
		clsEnv.RegType(new RegHelper_Type(typeof(CanvasGroup)));
		clsEnv.RegType(new RegHelper_Type(typeof(CanvasScaler)));
		clsEnv.RegType(new RegHelper_Type(typeof(Convert)));
		clsEnv.RegType(new RegHelper_Type(typeof(CSLightMng)));
		clsEnv.RegType(new RegHelper_Type(typeof(DateTime)));
		clsEnv.RegType(new CLS_Type_Enum(typeof(DayOfWeek)));
		clsEnv.RegType(new RegHelper_Type(typeof(Encoding)));
		clsEnv.RegType(new RegHelper_Type(typeof(EventTrigger)));
		clsEnv.RegType(new RegHelper_Type(typeof(EventTrigger.Entry), "EventTrigger.Entry"));
		clsEnv.RegType(new RegHelper_Type(typeof(EventTrigger.TriggerEvent), "EventTrigger.TriggerEvent"));
		clsEnv.RegType(new RegHelper_Type(typeof(Font)));
		clsEnv.RegType(new RegHelper_Type(typeof(Hashtable)));
		clsEnv.RegType(new RegHelper_Type(typeof(Image)));
		clsEnv.RegType(new RegHelper_Type(typeof(InputField)));
		clsEnv.RegType(new RegHelper_Type(typeof(Light)));
		clsEnv.RegType(new RegHelper_Type(typeof(MD5CryptoServiceProvider)));
		clsEnv.RegType(new RegHelper_Type(typeof(MemoryStream)));
		clsEnv.RegType(new RegHelper_Type(typeof(NavMeshAgent)));
		clsEnv.RegType(new RegHelper_Type(typeof(NavMeshHit)));
		clsEnv.RegType(new RegHelper_Type(typeof(NavMeshObstacle)));
		clsEnv.RegType(new RegHelper_Type(typeof(NavMeshPath)));
		clsEnv.RegType(new CLS_Type_Enum(typeof(ObstacleAvoidanceType)));
		clsEnv.RegType(new RegHelper_Type(typeof(ParticleSystem)));
		clsEnv.RegType(new RegHelper_Type(typeof(PointerEventData)));
		clsEnv.RegType(new CLS_Type_Enum(typeof(PrimitiveType)));
		clsEnv.RegType(new RegHelper_Type(typeof(QualitySettings)));
		clsEnv.RegType(new RegHelper_Type(typeof(Rect)));
		clsEnv.RegType(new RegHelper_Type(typeof(RectTransform)));
		clsEnv.RegType(new RegHelper_Type(typeof(Resources)));
		clsEnv.RegType(new RegHelper_Type(typeof(Rigidbody)));
		clsEnv.RegType(new CLS_Type_Enum(typeof(RuntimePlatform)));
		clsEnv.RegType(new RegHelper_Type(typeof(Screen)));
		clsEnv.RegType(new CLS_Type_Enum(typeof(SeekOrigin)));
		clsEnv.RegType(new RegHelper_Type(typeof(Slider)));
		clsEnv.RegType(new CLS_Type_Enum(typeof(Space)));
		clsEnv.RegType(new RegHelper_Type(typeof(Sprite)));
		clsEnv.RegType(new RegHelper_Type(typeof(StringBuilder)));
		clsEnv.RegType(new RegHelper_Type(typeof(Text)));
		clsEnv.RegType(new RegHelper_Type(typeof(Toggle)));
		clsEnv.RegType(new RegHelper_Type(typeof(Toggle.ToggleEvent), "Toggle.ToggleEvent"));
		clsEnv.RegType(new RegHelper_Type(typeof(Type)));
		clsEnv.RegType(new RegHelper_DeleAction(typeof(UnityAction), "UnityAction"));
		clsEnv.RegType(new RegHelper_DeleAction<bool>(typeof(UnityAction<bool>), "UnityAction<bool>"));
		clsEnv.RegType(new RegHelper_Type(typeof(UnityEngine.Object), "UnityEngine.Object"));
		clsEnv.RegType(new RegHelper_Type(typeof(Vector4)));
		clsEnv.RegType(new RegHelper_Type(typeof(WWW)));
		clsEnv.RegType(new CLS_Type_Array<Behaviour>("Behaviour[]"));
		clsEnv.RegType(new CLS_Type_List<Behaviour>("List<Behaviour>"));
		clsEnv.RegType(new CLS_Type_Array<BoxCollider>("BoxCollider[]"));
		clsEnv.RegType(new CLS_Type_List<BoxCollider>("List<BoxCollider>"));
		clsEnv.RegType(new CLS_Type_Array<Collider>("Collider[]"));
		clsEnv.RegType(new CLS_Type_List<Collider>("List<Collider>"));
		clsEnv.RegType(new CLS_Type_Array<Color>("Color[]"));
		clsEnv.RegType(new CLS_Type_List<Color>("List<Color>"));
		clsEnv.RegType(new CLS_Type_Array<Component>("Component[]"));
		clsEnv.RegType(new CLS_Type_List<Component>("List<Component>"));
		clsEnv.RegType(new CLS_Type_Array<GameObject>("GameObject[]"));
		clsEnv.RegType(new CLS_Type_List<GameObject>("List<GameObject>"));
		clsEnv.RegType(new CLS_Type_Array<MonoBehaviour>("MonoBehaviour[]"));
		clsEnv.RegType(new CLS_Type_List<MonoBehaviour>("List<MonoBehaviour>"));
		clsEnv.RegType(new CLS_Type_Array<Quaternion>("Quaternion[]"));
		clsEnv.RegType(new CLS_Type_List<Quaternion>("List<Quaternion>"));
		clsEnv.RegType(new CLS_Type_Array<SphereCollider>("SphereCollider[]"));
		clsEnv.RegType(new CLS_Type_List<SphereCollider>("List<SphereCollider>"));
		clsEnv.RegType(new CLS_Type_Array<Transform>("Transform[]"));
		clsEnv.RegType(new CLS_Type_List<Transform>("List<Transform>"));
		clsEnv.RegType(new CLS_Type_Array<Vector2>("Vector2[]"));
		clsEnv.RegType(new CLS_Type_List<Vector2>("List<Vector2>"));
		clsEnv.RegType(new CLS_Type_Array<Vector3>("Vector3[]"));
		clsEnv.RegType(new CLS_Type_List<Vector3>("List<Vector3>"));
		clsEnv.RegType(new CLS_Type_Array<Animator>("Animator[]"));
		clsEnv.RegType(new CLS_Type_List<Animator>("List<Animator>"));
		clsEnv.RegType(new CLS_Type_Array<AnimatorStateInfo>("AnimatorStateInfo[]"));
		clsEnv.RegType(new CLS_Type_List<AnimatorStateInfo>("List<AnimatorStateInfo>"));
		clsEnv.RegType(new CLS_Type_Array<Button>("Button[]"));
		clsEnv.RegType(new CLS_Type_List<Button>("List<Button>"));
		clsEnv.RegType(new CLS_Type_Array<Camera>("Camera[]"));
		clsEnv.RegType(new CLS_Type_List<Camera>("List<Camera>"));
		clsEnv.RegType(new CLS_Type_Array<EventTrigger.Entry>("EventTrigger.Entry[]"));
		clsEnv.RegType(new CLS_Type_List<EventTrigger.Entry>("List<EventTrigger.Entry>"));
		clsEnv.RegType(new CLS_Type_Array<Image>("Image[]"));
		clsEnv.RegType(new CLS_Type_List<Image>("List<Image>"));
		clsEnv.RegType(new CLS_Type_Array<InputField>("InputField[]"));
		clsEnv.RegType(new CLS_Type_List<InputField>("List<InputField>"));
		clsEnv.RegType(new CLS_Type_Array<ParticleSystem>("ParticleSystem[]"));
		clsEnv.RegType(new CLS_Type_List<ParticleSystem>("List<ParticleSystem>"));
		clsEnv.RegType(new CLS_Type_Array<Rect>("Rect[]"));
		clsEnv.RegType(new CLS_Type_List<Rect>("List<Rect>"));
		clsEnv.RegType(new CLS_Type_Array<RectTransform>("RectTransform[]"));
		clsEnv.RegType(new CLS_Type_List<RectTransform>("List<RectTransform>"));
		clsEnv.RegType(new CLS_Type_Array<Slider>("Slider[]"));
		clsEnv.RegType(new CLS_Type_List<Slider>("List<Slider>"));
		clsEnv.RegType(new CLS_Type_Array<Sprite>("Sprite[]"));
		clsEnv.RegType(new CLS_Type_List<Sprite>("List<Sprite>"));
		clsEnv.RegType(new CLS_Type_Array<Text>("Text[]"));
		clsEnv.RegType(new CLS_Type_List<Text>("List<Text>"));
		clsEnv.RegType(new CLS_Type_Array<Toggle>("Toggle[]"));
		clsEnv.RegType(new CLS_Type_List<Toggle>("List<Toggle>"));
		clsEnv.RegType(new CLS_Type_Array<Type>("Type[]"));
		clsEnv.RegType(new CLS_Type_List<Type>("List<Type>"));
		clsEnv.RegType(new CLS_Type_Array<UnityEngine.Object>("UnityEngine.Object[]"));
		clsEnv.RegType(new CLS_Type_List<UnityEngine.Object>("List<UnityEngine.Object>"));
		clsEnv.RegType(new CLS_Type_Array<Vector4>("Vector4[]"));
		clsEnv.RegType(new CLS_Type_List<Vector4>("List<Vector4>"));
		clsEnv.RegType(new CLS_Type_Array<WWW>("WWW[]"));
		clsEnv.RegType(new CLS_Type_List<WWW>("List<WWW>"));
		// gen end

        return clsEnv;
    }
}
