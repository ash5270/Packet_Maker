
enum PacketID : int32_t
{
    C_REQ_LOGIN=1000,
S_RES_LOGIN=2000,

};

class Packet
{
public:
    virtual PacketID GetID()=0;
    virtual int32_t GetSize()=0;
    virtual void Serialize(Buffer& buffer)=0;
    virtual void Deserialize(Buffer& buffer)=0;
};
};