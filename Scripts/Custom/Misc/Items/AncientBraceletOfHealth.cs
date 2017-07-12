using System;

namespace Server.Items
{
    public class AncientBraceletOfHealth : GoldBracelet
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public AncientBraceletOfHealth()
        {
            this.Hue = 0x21;
			this.Name = "Ancient Bracelet of Health";
			this.LootType = LootType.Blessed;
			this.Attributes.BonusStr = 500;
			this.Attributes.BonusDex = 300;
			this.Attributes.BonusInt = 300;
            this.Attributes.BonusHits = 500;
			this.Attributes.BonusStam = 200;
			this.Attributes.BonusMana = 200;
			this.Attributes.SpellDamage = 600;
            this.Attributes.RegenHits = 55;
			this.Attributes.RegenMana = 35;
			this.Attributes.RegenStam = 45;
			this.Attributes.CastSpeed = 4;
			this.Attributes.CastRecovery = 6;
			this.Attributes.LowerRegCost = 100;
			this.Attributes.LowerManaCost = 40;
			this.Attributes.Luck = 6000;
			this.Resistances.Physical = 70;
			this.Resistances.Fire = 70;
			this.Resistances.Cold = 70;
			this.Resistances.Poison = 70;
			this.Resistances.Energy = 70;
        }

        public AncientBraceletOfHealth(Serial serial)
            : base(serial)
        {
        }

        /*public override int LabelNumber
        {
            get
            {
                return 1061103;
            }
        }// Bracelet of Health*/
		
        public override int ArtifactRarity
        {
            get
            {
                return 99;
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