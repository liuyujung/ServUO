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
using Server.Spells;
using Server.Spells.Cleric;

namespace Server.Items
{
	public class ClericSpellbook : Spellbook
	{
		public override SpellbookType SpellbookType{ get{ return SpellbookType.Cleric; } }
		public override int BookOffset{ get{ return 801; } }
		public override int BookCount{ get{ return 12; } }
		
		[Constructable]
		public ClericSpellbook() : this( (ulong)0 )
		{
		}
		
		[Constructable]
		public ClericSpellbook( ulong content ) : base( content, 0xEFA )
		{
			Hue = 0x1F0;
			Name = "Cleric Spell Book";
			Layer = Layer.FirstValid;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel == AccessLevel.Player )
			{
				Container pack = from.Backpack;
				if( !(Parent == from || (pack != null && Parent == pack)) )
				{
					from.SendMessage( "The spellbook must be in your backpack [and not in a container within] to open." );
					return;
				}
				else if( UseRestrictions && !SpellRestrictions.CheckRestrictions( from, this ) )
				{
					return;
				}
			}
			
			from.CloseGump( typeof( ClericSpellbookGump ) );
			from.SendGump( new ClericSpellbookGump( from, this ) );
		}
		
		public ClericSpellbook( Serial serial ) : base( serial )
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
