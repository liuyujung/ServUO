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
	public class ClericBag : Bag
	{
		[Constructable]
		public ClericBag()
		{
			Hue = 0x1F0;
			PlaceItemIn( 30, 35, new ClericAngelicFaithScroll() );
			PlaceItemIn( 50, 35, new ClericBanishEvilScroll() );
			PlaceItemIn( 70, 35, new ClericDampenSpiritScroll() );
			PlaceItemIn( 90, 35, new ClericDivineFocusScroll() );
			PlaceItemIn( 30, 55, new ClericHammerOfFaithScroll() );
			PlaceItemIn( 50, 55, new ClericPurgeScroll() );
			PlaceItemIn( 70, 55, new ClericRestorationScroll() );
			PlaceItemIn( 90, 55, new ClericSacredBoonScroll() );
			PlaceItemIn( 30, 75, new ClericSacrificeScroll() );
			PlaceItemIn( 50, 75, new ClericSmiteScroll() );
			PlaceItemIn( 70, 75, new ClericTouchOfLifeScroll() );
			PlaceItemIn( 90, 75, new ClericTrialByFireScroll() );
		}
		
		private void PlaceItemIn( int x, int y, Item item )
		{
			AddItem( item );
			item.Location = new Point3D( x, y, 0 );
		}
		
		public ClericBag( Serial serial ) : base( serial )
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
