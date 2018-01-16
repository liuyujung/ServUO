/*
Special thanks to Ryan.
With RunUO we now have the ability to become our own Richard Garriott.
All Spells System created by x-SirSly-x, Admin of Land of Obsidian.
All Spells System 4.0 created & supported by Lucid Nagual, Admin of The Conjuring.
All Spells System 5.0 created by A_li_N.
All Spells Optional Restrictive System created by Alien, Daat99 and Lucid Nagual.
    _________________________________
 -=(_)_______________________________)=-
   /   .   .   . ____  . ___      _/
  /~ /    /   / /     / /   )2005 /
 (~ (____(___/ (____ / /___/     (
  \ ----------------------------- \
   \     lucidnagual@gmail.com     \
    \_     ===================      \
     \   -Admin of "The Conjuring"-  \
      \_     ===================     ~\
       )       All Spells System       )
      /~     Version [5].0 & Above   _/
    _/_______________________________/
 -=(_)_______________________________)=-
 */
using System;
using Server;

namespace Server.Items
{
	public class DruidKeyWords : BrownBook
	{
		private int i_charges;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Charges
		{
			get { return i_charges; }
			set { i_charges = value; InvalidateProperties(); }
		}

		[Constructable]
		public DruidKeyWords() : base( "Druid Spell Keywords", "Staff ", 24, false )

		{
		// NOTE: There are 8 lines per page and
		// approx 22 to 24 characters per line!
		//		0----+----1----+----2----+
		int cnt = 0;
			string[] lines;
			lines = new string[]
			{
				"This book is a quick",
				"list of the keywords",
				"used for setting spell",
				"hot keys. To use the",
				"keywords say [cs followed",
				"by the name of the spell",
				"ex: [cs SummonFirefly",
				"",
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[]
			{
				"The other keywords are:",
				"",
				"HollowReed,",
				"PackOfBeasts,",
				"SpringOfLife,",
				"GraspingRoots,",
				"BlendWithForest,",
				"SwarmOfInsects,",
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[]
			{
				"VolcanicEruption,",
				"SummonTreefellow,",
				"LureStone,",
				"NaturesPassage,",
				"MushroomGateway,",
				"RestorativeSoil,",
				"ShieldOfEarth,",
				"",
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[]
			{
				"",
				"",
				"---",
				"Warning:",
				"Book only has 5",
				"uses until its gone.",
				"",
			};
			Pages[cnt++].Lines = lines;
		Hue = Utility.RandomGreenHue();
		Charges = 5;
		}
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060658, "Uses Remaining \t{0}", this.Charges );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( this.Charges < 1 )
				{
				from.SendMessage( "The magic of the book has run out and the book is ruined." );
				from.FixedParticles( 0x36BD, 20, 10, 5044, EffectLayer.Waist );
				Delete();
				from.PlaySound( 775);
				}
			from.Send( new BookHeader( from, this ) );
			from.Send( new BookPageDetails( this ) );
			this.Charges = this.Charges - 1;

		}

		public DruidKeyWords( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
			writer.Write( (int) i_charges );

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			i_charges = reader.ReadInt();
		}

	}
}