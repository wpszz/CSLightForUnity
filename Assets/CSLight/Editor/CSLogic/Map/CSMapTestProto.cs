using UnityEngine;
using System.IO;
using System.Collections;

public class CSMapTestProto : CSMap
{
    protected override IEnumerator Start()
    {
        MemoryStream ms = new MemoryStream();

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

        ms.Dispose();

        yield return 0;
    }
}
