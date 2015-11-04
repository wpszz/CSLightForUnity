using ProtoBuf;
using System.Collections.Generic;

[ProtoContract]
public class CSProto03
{
    [ProtoMember(1)]
    public CSProto02 a;

    [ProtoMember(2)]
    public string b;

    [ProtoMember(3)]
    public List<CSProto01> c;

    public string desc
    {
        get
        {
            string cDesc = "";

            if (c != null)
            {
                foreach (CSProto01 pro in c)
                {
                    cDesc += pro.desc;
                    cDesc += " ";
                }
            }

            return string.Format("CSProto03: a=({0}) b=({1}) c=({2})", a.desc, b, cDesc);
        }
    }
}