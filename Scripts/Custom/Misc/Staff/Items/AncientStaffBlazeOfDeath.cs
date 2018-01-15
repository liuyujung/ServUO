using System;

namespace Server.Items
{
    public class AncientStaffBlazeOfDeath : Halberd
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public AncientStaffBlazeOfDeath()
        {
            this.Hue = 0x501;
			this.Name = "Ancient Staff Blaze Of Death";
			this.LootType = LootType.Blessed;
			this.WeaponAttributes.SelfRepair = 5;
			this.WeaponAttributes.HitPhysicalArea = 100;
            this.WeaponAttributes.HitFireArea = 100;
			this.WeaponAttributes.HitColdArea = 100;
			this.WeaponAttributes.HitPoisonArea = 100;
			this.WeaponAttributes.HitEnergyArea = 100;
			this.WeaponAttributes.HitLightning = 100;
            this.WeaponAttributes.HitFireball = 100;
			this.WeaponAttributes.HitHarm = 100;
			this.WeaponAttributes.HitMagicArrow = 100;
			this.WeaponAttributes.HitHarm = 100;
			this.WeaponAttributes.HitLeechStam = 100;
			this.WeaponAttributes.HitLeechMana = 100;
			this.WeaponAttributes.HitLeechHits = 100;
            this.Attributes.WeaponSpeed = 25;
            this.Attributes.WeaponDamage = 100;
            this.WeaponAttributes.LowerStatReq = 100;
			this.Attributes.SpellChanneling = 1;
			this.Attributes.ReflectPhysical = 10000;
        }

        public AncientStaffBlazeOfDeath(Serial serial)
            : base(serial)
        {
        }

        /*public override int LabelNumber
        {
            get
            {
                return 1063486;
            }
        }*/
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
        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            fire = 50;
            phys = 50;

            cold = pois = nrgy = chaos = direct = 0;
        }

		public override int ArtifactRarity
		{
			get
			{
				return 1000;
			}
		}

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