public enum PacketID : Int32
{
    C_REQ_LOGIN=1000,
S_RES_LOGIN=2000,

}

public abstract class Packet
{
    public abstract PacketID GetID();
    public abstract Int32 GetSize();
    public abstract Serialize(Buffer buffer);
    public abstract Deserialize(Buffer buffer);
}
}