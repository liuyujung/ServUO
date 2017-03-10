#region AuthorHeader
//
//	Safe Resurrection System version 2.2, by Xanthos
//
//	Some features were taken from CorpseStone by Unknown.
//
//
#endregion AuthorHeader
using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

namespace Xanthos.SafeResurrection
{
	public class ResurrectionStone : Item
	{
		public override string DefaultName
		{
			get { return "Resurrection Stone"; }
		}

		[Constructable]
		public ResurrectionStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = SafeResConfig.ResStoneHue;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !SafeResConfig.AllowCorpseRetrieval )
				return;

			if ( from.InRange( this.GetWorldLocation(), 2 ) )
			{
				from.SendGump( new GetCorpseGump( SafeResConfig.CorpseRetrievalAmount ) );
				from.CantWalk = true;
			}
			else
			{
				from.SendMessage( "You are not close enough to use this." );
			}
		}

		public override void OnDoubleClickDead( Mobile from )
		{
			if ( SafeResConfig.AllowSelfRes && SafeResConfig.UseSelfResStone )
			{
				Ankhs.Resurrect( from, this );
			}
			else
				from.SendMessage( "That is not allowed at this time." );
		}

		public ResurrectionStone( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	public class GetCorpseGump : Gump
	{
		private int m_Price;

		public GetCorpseGump( int price ) : base( 150, 50 )
		{

			m_Price = price;

			Closable = false;

			AddPage( 0 );

			AddImage( 0, 0, 3600 );

			AddImageTiled( 0, 14, 15, 200, 3603 );
			AddImageTiled( 380, 14, 14, 200, 3605 );

			AddImage( 0, 201, 3606 );

			AddImageTiled( 15, 201, 370, 16, 3607 );
			AddImageTiled( 15, 0, 370, 16, 3601 );

			AddImage( 380, 0, 3602 );

			AddImage( 380, 201, 3608 );

			AddImageTiled( 15, 15, 365, 190, 2624 );

			AddRadio( 30, 140, 9727, 9730, true, 1 );
			AddHtmlLocalized( 65, 145, 300, 25, 1060015, 0x7FFF, false, false ); // Grudgingly you pay the money

			AddRadio( 30, 175, 9727, 9730, false, 0 );
			AddHtml( 65, 178, 300, 25, "<BASEFONT COLOR=White>I'll live without my corpse!!</BASEFONT>", false, false );

			AddHtml( 30, 20, 360, 35, "<BASEFONT COLOR=Red>No way to get to your corpse? Simply pay the price, and your corpse will be brought to you...</BASEFONT>", false, false );

			AddHtmlLocalized( 30, 105, 345, 40, 1060018, 0x5B2D, false, false ); // Do you accept the fee, which will be withdrawn from your bank?

			AddImage( 65, 72, 5605 );

			AddImageTiled( 80, 90, 200, 1, 9107 );
			AddImageTiled( 95, 92, 200, 1, 9157 );

			AddLabel( 90, 70, 1645, price.ToString() );
			AddHtmlLocalized( 140, 70, 100, 25, 1023823, 0x7FFF, false, false ); // gold coins

			AddButton( 290, 175, 247, 248, 2, GumpButtonType.Reply, 0 );

			AddImageTiled( 15, 14, 365, 1, 9107 );
			AddImageTiled( 380, 14, 1, 190, 9105 );
			AddImageTiled( 15, 205, 365, 1, 9107 );
			AddImageTiled( 15, 14, 1, 190, 9105 );
			AddImageTiled( 0, 0, 395, 1, 9157 );
			AddImageTiled( 394, 0, 1, 217, 9155 );
			AddImageTiled( 0, 216, 395, 1, 9157 );
			AddImageTiled( 0, 0, 1, 217, 9155 );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			from.CloseGump( typeof( GetCorpseGump ) );

			if ( info.ButtonID == 1 || info.ButtonID == 2 )
			{
				Item corpse = from.Corpse;

				if ( corpse != null )
				{
					if ( m_Price > 0 )
					{
						if ( info.IsSwitched( 1 ) )
						{
							if ( Banker.Withdraw( from, m_Price ) )
							{
								from.SendLocalizedMessage( 1060398, m_Price.ToString() ); // Amount charged
								from.SendLocalizedMessage( 1060022, Banker.GetBalance( from ).ToString() ); // Amount left, from bank
							}
							else
							{
								from.SendMessage( "Unfortunately, you do not have enough gold in your bank to summon your corpse" );
								from.CantWalk = false;
								return;
							}
						}
						else
						{
							from.SendMessage( "You decide against paying the stone, leaving your corpse behind." );
							from.CantWalk = false;
							return;
						}

					}
					from.Corpse.Location = new Point3D( from.Location );
					Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0x3728, 10, 30, 5052 );
					Effects.PlaySound( from.Location, from.Map, 0x201 );
					from.Corpse.Map = from.Map;
					from.CantWalk = false;
				}
				else
				{
					from.SendMessage( "Your corpse could not be found." );
					from.CantWalk = false;
				}
			}
		}
	}
}