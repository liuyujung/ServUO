using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells.Druid; 


namespace Server.Items
{
   public class LeafWhirlwindScroll : SpellScroll
   {
      [Constructable]
      public LeafWhirlwindScroll() : this( 1 )
      {
      }

      [Constructable]
      public LeafWhirlwindScroll( int amount ) : base( 551, 0xE39 )
      {
         Name = "Leaf Whirlwind Scroll";
         Hue = 0x58B;
      }

      public LeafWhirlwindScroll( Serial serial ) : base( serial )
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

