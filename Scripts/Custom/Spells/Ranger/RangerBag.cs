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
	public class RangerBag : Bag
	{
		[Constructable]
		public RangerBag()
		{
			Hue = 2002;
			PlaceItemIn( 30, 35, new RangerFireBowScroll() );
			PlaceItemIn( 50, 35, new RangerFlightOfThePheonixScroll() );
			PlaceItemIn( 70, 35, new RangerHuntersAimScroll() );
			PlaceItemIn( 90, 35, new RangerIceBowScroll() );
			PlaceItemIn( 30, 55, new RangerLightningBowScroll() );
			PlaceItemIn( 50, 55, new RangerFamiliarScroll() );
			PlaceItemIn( 70, 55, new RangerNoxBowScroll() );
			PlaceItemIn( 90, 55, new RangerSummonMountScroll() );
			PlaceItemIn( 30, 75, new RangerNaturalHerbScroll() );
			PlaceItemIn( 50, 75, new RangerLandmarkScroll() );
			PlaceItemIn( 70, 75, new RangerTreeStrideScroll() );
			PlaceItemIn( 90, 75, new RangerWallOfAirScroll() );
			PlaceItemIn( 30, 95, new RangerWoodCarvingsScroll() );
		}

		private void PlaceItemIn( int x, int y, Item item )
		{
			AddItem( item );
			item.Location = new Point3D( x, y, 0 );
		}

		public RangerBag( Serial serial ) : base( serial )
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