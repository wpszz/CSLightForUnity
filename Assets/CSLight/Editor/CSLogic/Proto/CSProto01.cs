using ProtoBuf;

[ProtoContract]
public class CSProto01
{
    [ProtoMember(1)]
    public int a;

    [ProtoMember(2)]
    public string b;

    public string desc
    {
        get
        {
            return string.Format("CSProto01: a={0} b={1}", a, b);
        }
    }
}
