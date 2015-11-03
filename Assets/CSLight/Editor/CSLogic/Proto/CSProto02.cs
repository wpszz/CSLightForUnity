using ProtoBuf;

[ProtoContract]
public class CSProto02
{
    [ProtoMember(1)]
    public CSProto01 a;

    [ProtoMember(2)]
    public string b;

    public string desc
    {
        get
        {
            return string.Format("CSProto02: a=({0}) b=({1})", a.desc, b);
        }
    }
}