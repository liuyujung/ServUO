using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells.Druid; 


namespace Server.Items
{
   public class DruidBarkSkinScroll : SpellScroll
   {
      [Constructable]
      public DruidBarkSkinScroll() : this( 1 )
      {
      }

      [Constructable]
      public DruidBarkSkinScroll( int amount ) : base( 567, 0xE39, amount )
      {
         Name = "Druid Bark Skin Scroll";
         Hue = 0x58B;
      }

      public DruidBarkSkinScroll( Serial serial ) : base( serial )
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
