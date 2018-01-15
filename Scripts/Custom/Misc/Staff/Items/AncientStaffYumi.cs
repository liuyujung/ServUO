using System;
using Server.Items;
namespace Server.Custom.Misc.Staff.Items
{
    public class AncientStaffYumi : Yumi
	{
		public override bool IsArtifact { get { return true; } }
		[Constructable]
		public AncientStaffYumi()
            : base()
        {
			this.Name = "Ancient Staff Yumi";
			this.LootType = LootType.Blessed;
			this.WeaponAttributes.SelfRepair = 5;
			this.WeaponAttributes.HitLightning = 100;
			this.WeaponAttributes.HitFireball = 100;
			this.WeaponAttributes.HitHarm = 100;
			this.WeaponAttributes.HitMagicArrow = 100;
			this.WeaponAttributes.HitHarm = 100;
			this.WeaponAttributes.HitLeechStam = 100;
			this.WeaponAttributes.HitLeechMana = 100;
			this.WeaponAttributes.HitLeechHits = 100;
			this.Attributes.WeaponDamage = 100;
			this.WeaponAttributes.LowerStatReq = 100;
			this.Attributes.SpellChanneling = 1;
			this.Attributes.ReflectPhysical = 10000;
			this.Attributes.BonusStr = 50;
		}

		public AncientStaffYumi(Serial serial)
            : base(serial)
        {
		}

		public override int InitMinHits
		{
			get
			{
				return 255;
			}
		}
		public override int InitMaxHits
		{
			get
			{
				return 255;
			}
		}
		/*public override int LabelNumber
		{
			get
			{
				return 1070967;
			}
		}// The Horselord*/
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
