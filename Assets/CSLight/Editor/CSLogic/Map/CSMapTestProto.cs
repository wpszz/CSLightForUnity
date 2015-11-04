using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class CSMapTestProto : CSMap
{
    protected override IEnumerator Start()
    {
        MemoryStream ms = new MemoryStream();

        //=================================================

        CSProto01 proto = new CSProto01();
        proto.a = 12345;
        proto.b = "协议1";
        Debug.LogWarning("原数据：" + proto.desc);

        ms.Position = 0;
        CSLightMng.instance.Serialize(ms, proto);

        ms.Position = 0;
        CSProto01 proto2 = (CSProto01)CSLightMng.instance.Deserialize(ms, "CSProto01");
        Debug.LogWarning("序列化和反序列化后：" + proto2.desc);

        //=================================================

        CSProto02 proto3 = new CSProto02();
        proto3.a = proto;
        proto3.b = "协议2";
        Debug.LogWarning("原数据：" + proto3.desc);

        ms.Position = 0;
        CSLightMng.instance.Serialize(ms, proto3);

        ms.Position = 0;
        CSProto02 proto4 = (CSProto02)CSLightMng.instance.Deserialize(ms, "CSProto02");
        Debug.LogWarning("序列化和反序列化后：" + proto4.desc);

        //=================================================

        CSProto03 proto5 = new CSProto03();
        proto5.a = proto4;
        proto5.b = "协议3";
        proto5.c = new List<CSProto01>();
        proto5.c.Add(proto);
        proto5.c.Add(proto2);
        Debug.LogWarning("原数据：" + proto5.desc);

        ms.Position = 0;
        CSLightMng.instance.Serialize(ms, proto5);

        ms.Position = 0;
        CSProto03 proto6 = (CSProto03)CSLightMng.instance.Deserialize(ms, "CSProto03");
        Debug.LogWarning("序列化和反序列化后：" + proto6.desc);


        ms.Dispose();

        yield return 0;
    }
}
