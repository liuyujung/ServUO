
using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.Craft;
using System.Collections.Generic;


namespace Server.Engines.BulkOrders
{
	[TypeAlias( "Scripts.Engines.BulkOrders.SmallCarpenterBOD" )]
	public class SmallCarpenterBOD : SmallBOD
	{
		public static double[] m_CarpenterMaterialChances = new double[]
			{
				0.140, // None
				0.130, // OakWood
				0.120, // AshWood
				0.110, // YewWood
				0.100, // Heartwood
				0.090, // Bloodwood
				0.080, // Frostwood
				0.070, // Ebony
				0.060, // Bamboo
				0.050, // PurpleHeart
				0.030, // Redwood
				0.020  // Petrified
			};

		public override int ComputeFame()
		{
			return CarpenterRewardCalculator.Instance.ComputeFame( this );
		}

		public override int ComputeGold()
		{
			return CarpenterRewardCalculator.Instance.ComputeGold( this );
		}

		public override List<Item> ComputeRewards( bool full )
		{
			List<Item> list = new List<Item>();

			RewardGroup rewardGroup = CarpenterRewardCalculator.Instance.LookupRewards( CarpenterRewardCalculator.Instance.ComputePoints( this ) );

			if ( rewardGroup != null )
			{
				if ( full )
				{
					for ( int i = 0; i < rewardGroup.Items.Length; ++i )
					{
						Item item = rewardGroup.Items[i].Construct();

						if ( item != null )
							list.Add( item );
					}
				}
				else
				{
					RewardItem rewardItem = rewardGroup.AcquireItem();

					if ( rewardItem != null )
					{
						Item item = rewardItem.Construct();

						if ( item != null )
							list.Add( item );
					}
				}
			}

			return list;
		}

		public static SmallCarpenterBOD CreateRandomFor( Mobile m )
		{
			SmallBulkEntry[] entries;
			bool useMaterials;

			if ( useMaterials = Utility.RandomBool() )
				entries = SmallBulkEntry.CarpenterStaff;
			else
				entries = SmallBulkEntry.CarpenterInstrument;

			if ( entries.Length > 0 )
			{
				double theirSkill = m.Skills[SkillName.Carpentry].Base;
				int amountMax;

				if ( theirSkill >= 70.1 )
					amountMax = Utility.RandomList( 10, 15, 20, 20 );
				else if ( theirSkill >= 50.1 )
					amountMax = Utility.RandomList( 10, 15, 15, 20 );
				else
					amountMax = Utility.RandomList( 10, 10, 15, 20 );

				BulkMaterialType material = BulkMaterialType.None;

				if ( useMaterials && theirSkill >= 70.1 )
				{
					for ( int i = 0; i < 20; ++i )
					{
						BulkMaterialType check = GetRandomMaterial( BulkMaterialType.OakWood, m_CarpenterMaterialChances );
						double skillReq = 0.0;

						switch ( check )
						{
							case BulkMaterialType.DullCopper:	skillReq = 52.0; break;
							case BulkMaterialType.ShadowIron:	skillReq = 56.0; break;
							case BulkMaterialType.Copper:		skillReq = 60.0; break;
							case BulkMaterialType.Bronze:		skillReq = 64.0; break;
							case BulkMaterialType.Gold:			skillReq = 68.0; break;
							case BulkMaterialType.Agapite:		skillReq = 72.0; break;
							case BulkMaterialType.Verite:		skillReq = 76.0; break;
							case BulkMaterialType.Valorite:		skillReq = 80.0; break;
							case BulkMaterialType.Blaze:		skillReq = 84.0; break;
							case BulkMaterialType.Ice:			skillReq = 88.0; break;
							case BulkMaterialType.Toxic:		skillReq = 92.0; break;
							case BulkMaterialType.Electrum:		skillReq = 96.0; break;
							case BulkMaterialType.Platinum:		skillReq = 100.0; break;
							case BulkMaterialType.Spined:		skillReq = 64.0; break;
							case BulkMaterialType.Horned:		skillReq = 68.0; break;
							case BulkMaterialType.Barbed:		skillReq = 72.0; break;
							case BulkMaterialType.Polar:		skillReq = 76.0; break;
							case BulkMaterialType.Synthetic:	skillReq = 80.0; break;
							case BulkMaterialType.BlazeL:		skillReq = 84.0; break;
							case BulkMaterialType.Daemonic:		skillReq = 88.0; break;
							case BulkMaterialType.Shadow:		skillReq = 92.0; break;
							case BulkMaterialType.Frost:		skillReq = 96.0; break;
							case BulkMaterialType.Ethereal:		skillReq = 100.0; break;
							case BulkMaterialType.OakWood:			skillReq = 60.0; break;
							case BulkMaterialType.AshWood:			skillReq = 64.0; break;
							case BulkMaterialType.YewWood:		skillReq = 68.0; break;
							case BulkMaterialType.Heartwood:			skillReq = 72.0; break;
							case BulkMaterialType.Bloodwood:			skillReq = 76.0; break;
							case BulkMaterialType.Frostwood:		skillReq = 80.0; break;
							case BulkMaterialType.Ebony:		skillReq = 84.0; break;
							case BulkMaterialType.Bamboo:		skillReq = 88.0; break;
							case BulkMaterialType.PurpleHeart:	skillReq = 92.0; break;
							case BulkMaterialType.Redwood:		skillReq = 96.0; break;
							case BulkMaterialType.Petrified:	skillReq = 100.0; break;
						}

						if ( theirSkill >= skillReq )
						{
							material = check;
							break;
						}
					}
				}

				double excChance = 0.0;

				if ( theirSkill >= 70.1 )
					excChance = (theirSkill + 80.0) / 200.0;

                bool reqExceptional = (excChance > Utility.RandomDouble());

				SmallBulkEntry entry = null;

				CraftSystem system = DefCarpentry.CraftSystem;

				for ( int i = 0; i < 150; ++i )
				{
					SmallBulkEntry check = entries[Utility.Random( entries.Length )];

					CraftItem item = system.CraftItems.SearchFor( check.Type );

					if ( item != null )
					{
						bool allRequiredSkills = true;
						double chance = item.GetSuccessChance( m, null, system, false, ref allRequiredSkills );

						if ( allRequiredSkills && chance >= 0.0 )
						{

							if ( chance > 0.0 )
							{
								entry = check;
								break;
							}
						}
					}
				}

				if ( entry != null )
                    return new SmallCarpenterBOD(entry, material, amountMax, reqExceptional);
			}

			return null;
		}

        private SmallCarpenterBOD(SmallBulkEntry entry, BulkMaterialType material, int amountMax, bool reqExceptional)
		{
			this.Hue = 0x30;
			this.AmountMax = amountMax;
			this.Type = entry.Type;
			this.Number = entry.Number;
			this.Graphic = entry.Graphic;
            RequireExceptional = reqExceptional;
			this.Material = material;
		}

		[Constructable]
		public SmallCarpenterBOD()
		{
			SmallBulkEntry[] entries;
			bool useMaterials;

			if ( useMaterials = Utility.RandomBool() )
				entries = SmallBulkEntry.CarpenterStaff;
			else
				entries = SmallBulkEntry.CarpenterInstrument;

			if ( entries.Length > 0 )
			{
				int hue = 0x30;
				int amountMax = Utility.RandomList( 10, 15, 20 );

				BulkMaterialType material;

				if ( useMaterials )
					material = GetRandomMaterial( BulkMaterialType.OakWood, m_CarpenterMaterialChances );
				else
					material = BulkMaterialType.None;

				bool reqExceptional = Utility.RandomBool() || (material == BulkMaterialType.None);

				SmallBulkEntry entry = entries[Utility.Random( entries.Length )];

				this.Hue = hue;
				this.AmountMax = amountMax;
				this.Type = entry.Type;
				this.Number = entry.Number;
				this.Graphic = entry.Graphic;
                RequireExceptional = reqExceptional;
				this.Material = material;
			}
		}

        public SmallCarpenterBOD(int amountCur, int amountMax, Type type, int number, int graphic, bool reqExceptional, BulkMaterialType mat)
		{
			this.Hue = 0x30;
			this.AmountMax = amountMax;
			this.AmountCur = amountCur;
			this.Type = type;
			this.Number = number;
			this.Graphic = graphic;
            RequireExceptional = reqExceptional;
			this.Material = mat;
		}

		public SmallCarpenterBOD( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
