using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells.Druid; 


namespace Server.Items
{
   public class DruidFamiliarScroll : SpellScroll
   {
      [Constructable]
      public DruidFamiliarScroll() : this( 1 )
      {
      }

      [Constructable]
      public DruidFamiliarScroll( int amount ) : base( 559, 0xE39, amount )
      {
         Name = "Druid Familiar Scroll";
         Hue = 0x58B;
      }

      public DruidFamiliarScroll( Serial serial ) : base( serial )
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
