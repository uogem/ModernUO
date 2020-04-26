namespace Server.Items
{
  public abstract class BaseLeather : Item, ICommodity
  {
    private CraftResource m_Resource;

    public BaseLeather(CraftResource resource, int amount = 1) : base(0x1081)
    {
      Stackable = true;
      Weight = 1.0;
      Amount = amount;
      Hue = CraftResources.GetHue(resource);

      m_Resource = resource;
    }

    public BaseLeather(Serial serial) : base(serial)
    {
    }

    [CommandProperty(AccessLevel.GameMaster)]
    public CraftResource Resource
    {
      get => m_Resource;
      set
      {
        m_Resource = value;
        InvalidateProperties();
      }
    }

    public override int LabelNumber
    {
      get
      {
        if (m_Resource >= CraftResource.SpinedLeather && m_Resource <= CraftResource.BarbedLeather)
          return 1049684 + (m_Resource - CraftResource.SpinedLeather);

        return 1047022;
      }
    }

    int ICommodity.DescriptionNumber => LabelNumber;
    bool ICommodity.IsDeedable => true;

    public override void Serialize(IGenericWriter writer)
    {
      base.Serialize(writer);

      writer.Write(1); // version

      writer.Write((int)m_Resource);
    }

    public override void Deserialize(IGenericReader reader)
    {
      base.Deserialize(reader);

      int version = reader.ReadInt();

      switch (version)
      {
        case 1:
          {
            m_Resource = (CraftResource)reader.ReadInt();
            break;
          }
        case 0:
          {
            OreInfo info = new OreInfo(reader.ReadInt(), reader.ReadInt(), reader.ReadString());

            m_Resource = CraftResources.GetFromOreInfo(info);
            break;
          }
      }
    }

    public override void AddNameProperty(ObjectPropertyList list)
    {
      if (Amount > 1)
        list.Add(1050039, "{0}\t#{1}", Amount, 1024199); // ~1_NUMBER~ ~2_ITEMNAME~
      else
        list.Add(1024199); // cut leather
    }

    public override void GetProperties(ObjectPropertyList list)
    {
      base.GetProperties(list);

      if (!CraftResources.IsStandard(m_Resource))
      {
        int num = CraftResources.GetLocalizationNumber(m_Resource);

        if (num > 0)
          list.Add(num);
        else
          list.Add(CraftResources.GetName(m_Resource));
      }
    }
  }

  [Flippable(0x1081, 0x1082)]
  public class Leather : BaseLeather
  {
    [Constructible]
    public Leather(int amount = 1) : base(CraftResource.RegularLeather, amount)
    {
    }

    public Leather(Serial serial) : base(serial)
    {
    }

    public override void Serialize(IGenericWriter writer)
    {
      base.Serialize(writer);

      writer.Write(0); // version
    }

    public override void Deserialize(IGenericReader reader)
    {
      base.Deserialize(reader);

      int version = reader.ReadInt();
    }
  }

  [Flippable(0x1081, 0x1082)]
  public class SpinedLeather : BaseLeather
  {
    [Constructible]
    public SpinedLeather(int amount = 1) : base(CraftResource.SpinedLeather, amount)
    {
    }

    public SpinedLeather(Serial serial) : base(serial)
    {
    }

    public override void Serialize(IGenericWriter writer)
    {
      base.Serialize(writer);

      writer.Write(0); // version
    }

    public override void Deserialize(IGenericReader reader)
    {
      base.Deserialize(reader);

      int version = reader.ReadInt();
    }
  }

  [Flippable(0x1081, 0x1082)]
  public class HornedLeather : BaseLeather
  {
    [Constructible]
    public HornedLeather(int amount = 1) : base(CraftResource.HornedLeather, amount)
    {
    }

    public HornedLeather(Serial serial) : base(serial)
    {
    }

    public override void Serialize(IGenericWriter writer)
    {
      base.Serialize(writer);

      writer.Write(0); // version
    }

    public override void Deserialize(IGenericReader reader)
    {
      base.Deserialize(reader);

      int version = reader.ReadInt();
    }
  }

  [Flippable(0x1081, 0x1082)]
  public class BarbedLeather : BaseLeather
  {
    [Constructible]
    public BarbedLeather(int amount = 1) : base(CraftResource.BarbedLeather, amount)
    {
    }

    public BarbedLeather(Serial serial) : base(serial)
    {
    }

    public override void Serialize(IGenericWriter writer)
    {
      base.Serialize(writer);

      writer.Write(0); // version
    }

    public override void Deserialize(IGenericReader reader)
    {
      base.Deserialize(reader);

      int version = reader.ReadInt();
    }
  }
}
