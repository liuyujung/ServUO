using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells.Druid; 


namespace Server.Items
{
   public class DruidDelayedPoisonScroll : SpellScroll
   {
      [Constructable]
      public DruidDelayedPoisonScroll() : this( 1 )
      {
      }

      [Constructable]
      public DruidDelayedPoisonScroll( int amount ) : base( 568, 0xE39, amount )
      {
         Name = "Druid Delayed Posion Scroll";
         Hue = 0x58B;
      }

      public DruidDelayedPoisonScroll( Serial serial ) : base( serial )
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
