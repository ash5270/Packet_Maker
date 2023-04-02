public class P_C_REQ_LOGIN : Packet
{
    public override PacketID GetID()
    {
        return PI_C_REQ_LOGIN;
    }
    public override Int32 GetSize()
    {
       return sizeof(Int32)+sizeof(float);
    }
    //member values
    public Int32 value;
	public float value2;
	
           
    //member function
    public override void Serialize(Buffer buffer)
    {
	 buffer.Write(value);
		buffer.Write(value2);
		
    }

    public override void Deserialize(Buffer buffer)
    {
	 buffer.Read(value);
		buffer.Read(value2);
		
    }
}

public class P_S_RES_LOGIN : Packet
{
    public override PacketID GetID()
    {
        return PI_S_RES_LOGIN;
    }
    public override Int32 GetSize()
    {
       return sizeof(float);
    }
    //member values
    public float check;
	
           
    //member function
    public override void Serialize(Buffer buffer)
    {
	 buffer.Write(check);
		
    }

    public override void Deserialize(Buffer buffer)
    {
	 buffer.Read(check);
		
    }
}



