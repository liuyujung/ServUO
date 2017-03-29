using System; 
using Server; 
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Guilds;
using Server.Gumps;
using Server.Mobiles; 
using Server.Targeting;

namespace Server.Items
{ 
	public class ItemRelayerDeed : Item
	{
		[Constructable]
		public ItemRelayerDeed() : base( 0x14F0 )
		{
			Weight = 1.0;
                  	Hue = 1163;
			Movable = true;
			Name = "Item Relayer Deed";
			LootType = LootType.Blessed;
		}

        public ItemRelayerDeed(Serial serial)
            : base(serial)
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

		public override bool DisplayLootType{ get{ return false; } }

		public override void OnDoubleClick( Mobile from ) 
		{
			if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack
			{
				 from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		    }
            else
                from.SendGump(new ItemRelayerDeed2(from, this));

		}	
	}
   public class ItemRelayerDeed2 : Gump 
   {
       private ItemRelayerDeed m_Deed;

       public ItemRelayerDeed2(Mobile from, ItemRelayerDeed deed)
           : base(300, 100)
       {
           this.Closable = true;
           this.Disposable = true;
           this.Dragable = true;
           this.Resizable = false;

           m_Deed = deed;

           AddPage(0);
           AddBackground(17, 27, 461, 445, 9380);
           AddImage(273, 219, 50960, 1159);
           AddImage(273, 220, 121);
           AddImage(273, 220, 60987);
           AddLabel(331, 328, 1259, @"Created By:");
           AddLabel(363, 356, 1259, @"Tyman900");
           AddLabel(86, 389, 1259, @"Talisman");
           AddLabel(86, 115, 1259, @"Pants");
           AddLabel(86, 170, 1259, @"Shirt");
           AddLabel(86, 225, 1259, @"Helm");
           AddLabel(86, 279, 1259, @"Gloves");
           AddLabel(86, 333, 1259, @"Ring");
           AddLabel(86, 60, 1259, @"Shoes");
           AddButton(60, 60, 11400, 11400, 1, GumpButtonType.Reply, 0);
           AddButton(60, 170, 11400, 11400, 3, GumpButtonType.Reply, 0);
           AddButton(60, 114, 11400, 11400, 2, GumpButtonType.Reply, 0);
           AddButton(60, 224, 11400, 11400, 4, GumpButtonType.Reply, 0);
           AddButton(60, 334, 11400, 11400, 6, GumpButtonType.Reply, 0);
           AddButton(60, 280, 11400, 11400, 5, GumpButtonType.Reply, 0);
           AddButton(60, 390, 11400, 11400, 7, GumpButtonType.Reply, 0);
           AddLabel(198, 58, 1259, @"Neck");
           AddLabel(198, 113, 1259, @"Waist");
           AddLabel(198, 168, 1259, @"InnerTorso");
           AddLabel(198, 223, 1259, @"Bracelet");
           AddLabel(198, 279, 1259, @"MiddleTorso");
           AddLabel(198, 389, 1259, @"Arms");
           AddLabel(198, 333, 1259, @"Earrings");
           AddButton(174, 60, 11400, 11400, 8, GumpButtonType.Reply, 0);
           AddButton(174, 170, 11400, 11400, 10, GumpButtonType.Reply, 0);
           AddButton(174, 114, 11400, 11400, 9, GumpButtonType.Reply, 0);
           AddButton(174, 224, 11400, 11400, 11, GumpButtonType.Reply, 0);
           AddButton(174, 334, 11400, 11400, 13, GumpButtonType.Reply, 0);
           AddButton(174, 280, 11400, 11400, 12, GumpButtonType.Reply, 0);
           AddButton(174, 390, 11400, 11400, 14, GumpButtonType.Reply, 0);
           AddLabel(325, 59, 1259, @"Cloak");
           AddLabel(325, 113, 1259, @"OuterTorso");
           AddLabel(325, 166, 1259, @"OuterLegs");
           AddLabel(325, 220, 1259, @"InnerLegs");
           AddButton(302, 60, 11400, 11400, 15, GumpButtonType.Reply, 0);
           AddButton(302, 169, 11400, 11400, 17, GumpButtonType.Reply, 0);
           AddButton(302, 114, 11400, 11400, 16, GumpButtonType.Reply, 0);
           AddButton(302, 223, 11400, 11400, 18, GumpButtonType.Reply, 0);



       }

public override void OnResponse( NetState state, RelayInfo info )  
      { 
        Mobile from = state.Mobile;
         switch ( info.ButtonID ) 
         { 
            /*case 10: 
            { 
		   from.AddToBackpack(new BoardDeed());
               from.SendMessage( "" );
			   from.PlaySound( 521 ); 
               break;
            }*/
            case 1:
            {
               from.AddToBackpack(new RelayerShoesDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
            } 
            case 2:
            {
               from.AddToBackpack(new RelayerPantsDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
            }
            case 3:  
            {
               from.AddToBackpack(new RelayerShirtDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
            }
            case 4:  
           {
              from.AddToBackpack(new RelayerHelmDeed());
              from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			  from.PlaySound( 521 );
              m_Deed.Delete();

		  break;
            }
            case 5:  
            {
                from.AddToBackpack(new RelayerGlovesDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
             }
            case 6:
            {
                from.AddToBackpack(new RelayerRingDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
            } 
            case 7:
            {
               from.AddToBackpack(new RelayerTalismanDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
            }
            case 8:  
            {
                from.AddToBackpack(new RelayerNeckDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
            }
            case 9:  
           {
               from.AddToBackpack(new RelayerWaistDeed());
              from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			  from.PlaySound( 521 );
              m_Deed.Delete();

		  break;
            }
            case 10:  
            {
                from.AddToBackpack(new RelayerInnerTorsoDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		break;
              }
            case 11:
            {
                from.AddToBackpack(new RelayerBraceletDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
            } 
            case 12:
            {
                from.AddToBackpack(new RelayerMiddleTorsoDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
            }
            case 13:  
            {
                from.AddToBackpack(new RelayerEarringsDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
            }
            case 14:  
           {
               from.AddToBackpack(new RelayerArmsDeed());
              from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			  from.PlaySound( 521 );
              m_Deed.Delete();

		  break;
            }
            case 15:  
            {
                from.AddToBackpack(new RelayerCloakDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		break;
              }
            case 16:
            {
                from.AddToBackpack(new RelayerOuterTorsoDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
            } 
            case 17:
            {
                from.AddToBackpack(new RelayerOuterLegsDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
            }
            case 18:  
            {
                from.AddToBackpack(new RelayerInnerLegsDeed());
               from.SendMessage( "The Relayer Deed Has Been Placed In Your Pack!" );
			   from.PlaySound( 521 );
               m_Deed.Delete();

		   break;
            } 
         }
      }
   }
}
