class P_C_REQ_LOGIN : public Packet
{
public:
    PacketID GetID() override
    {
        return PI_C_REQ_LOGIN;
    }
    public int32_t GetSize()override
    {
       return sizeof(int32_t)+sizeof(float);
    }
    //member values
    int32_t value;
	float value2;
	
           
    //member function
    public void Serialize(Buffer& buffer) override
    {
	 buffer.Write(value);
		buffer.Write(value2);
		
    }

    public void Deserialize(Buffer& buffer) override
    {
	 buffer.Read(value);
		buffer.Read(value2);
		
    }
};

class P_S_RES_LOGIN : public Packet
{
public:
    PacketID GetID() override
    {
        return PI_S_RES_LOGIN;
    }
    public int32_t GetSize()override
    {
       return sizeof(float);
    }
    //member values
    float check;
	
           
    //member function
    public void Serialize(Buffer& buffer) override
    {
	 buffer.Write(check);
		
    }

    public void Deserialize(Buffer& buffer) override
    {
	 buffer.Read(check);
		
    }
};



