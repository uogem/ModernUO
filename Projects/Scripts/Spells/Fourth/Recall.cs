using Server.Factions;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Spells.Necromancy;

namespace Server.Spells.Fourth
{
  public class RecallSpell : MagerySpell, IRecallSpell
  {
    private static readonly SpellInfo m_Info = new SpellInfo(
      "Recall", "Kal Ort Por",
      239,
      9031,
      Reagent.BlackPearl,
      Reagent.Bloodmoss,
      Reagent.MandrakeRoot);

    private readonly Runebook m_Book;

    private readonly RunebookEntry m_Entry;

    public RecallSpell(Mobile caster, RunebookEntry entry = null, Runebook book = null, Item scroll = null) : base(caster, scroll, m_Info)
    {
      m_Entry = entry;
      m_Book = book;
    }

    public override SpellCircle Circle => SpellCircle.Fourth;

    public override void GetCastSkills(out double min, out double max)
    {
      if (TransformationSpellHelper.UnderTransformation(Caster, typeof(WraithFormSpell)))
        min = max = 0;
      else if (Core.SE && m_Book != null) // recall using Runebook charge
        min = max = 0;
      else
        base.GetCastSkills(out min, out max);
    }

    public override void OnCast()
    {
      if (m_Entry == null)
        Caster.Target = new RecallSpellTarget(this);
      else
        Effect(m_Entry.Location, m_Entry.Map, true);
    }

    public override bool CheckCast()
    {
      if (Sigil.ExistsOn(Caster))
      {
        Caster.SendLocalizedMessage(1061632); // You can't do that while carrying the sigil.
        return false;
      }

      if (Caster.Criminal)
      {
        Caster.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.
        return false;
      }

      if (SpellHelper.CheckCombat(Caster))
      {
        Caster.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
        return false;
      }

      if (WeightOverloading.IsOverloaded(Caster))
      {
        Caster.SendLocalizedMessage(502359, "", 0x22); // Thou art too encumbered to move.
        return false;
      }

      return SpellHelper.CheckTravel(Caster, TravelCheckType.RecallFrom);
    }

    public void Effect(Point3D loc, Map map, bool checkMulti)
    {
      if (Sigil.ExistsOn(Caster))
      {
        Caster.SendLocalizedMessage(1061632); // You can't do that while carrying the sigil.
      }
      else if (map == null || (!Core.AOS && Caster.Map != map))
      {
        Caster.SendLocalizedMessage(1005569); // You can not recall to another facet.
      }
      else if (!SpellHelper.CheckTravel(Caster, TravelCheckType.RecallFrom))
      {
      }
      else if (!SpellHelper.CheckTravel(Caster, map, loc, TravelCheckType.RecallTo))
      {
      }
      else if (map == Map.Felucca && Caster is PlayerMobile mobile && mobile.Young)
      {
        mobile.SendLocalizedMessage(1049543); // You decide against traveling to Felucca while you are still young.
      }
      else if (Caster.Kills >= 5 && map != Map.Felucca)
      {
        Caster.SendLocalizedMessage(1019004); // You are not allowed to travel there.
      }
      else if (Caster.Criminal)
      {
        Caster.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.
      }
      else if (SpellHelper.CheckCombat(Caster))
      {
        Caster.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
      }
      else if (WeightOverloading.IsOverloaded(Caster))
      {
        Caster.SendLocalizedMessage(502359, "", 0x22); // Thou art too encumbered to move.
      }
      else if (!map.CanSpawnMobile(loc.X, loc.Y, loc.Z))
      {
        Caster.SendLocalizedMessage(501942); // That location is blocked.
      }
      else if (checkMulti && SpellHelper.CheckMulti(loc, map))
      {
        Caster.SendLocalizedMessage(501942); // That location is blocked.
      }
      else if (m_Book != null && m_Book.CurCharges <= 0)
      {
        Caster.SendLocalizedMessage(502412); // There are no charges left on that item.
      }
      else if (CheckSequence())
      {
        BaseCreature.TeleportPets(Caster, loc, map, true);

        if (m_Book != null)
          --m_Book.CurCharges;

        Caster.PlaySound(0x1FC);
        Caster.MoveToWorld(loc, map);
        Caster.PlaySound(0x1FC);
      }

      FinishSequence();
    }
  }
}
