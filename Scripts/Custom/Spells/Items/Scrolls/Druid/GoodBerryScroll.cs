using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells.Druid; 


namespace Server.Items
{
   public class DruidGoodBerryScroll : SpellScroll
   {
      [Constructable]
      public DruidGoodBerryScroll() : this( 1 )
      {
      }

      [Constructable]
      public DruidGoodBerryScroll( int amount ) : base( 569, 0xE39, amount )
      {
         Name = "Druid Good Berry Scroll";
         Hue = 0x58B;
      }

      public DruidGoodBerryScroll( Serial serial ) : base( serial )
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
