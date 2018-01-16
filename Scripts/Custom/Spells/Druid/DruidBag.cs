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
using Server.Items;

namespace Server.Items
{
	public class DruidBag : Bag
	{
		[Constructable]
		public DruidBag()
		{
			Hue = 0x58B;
			PlaceItemIn( 30, 35, new BlendWithForestScroll() );
			PlaceItemIn( 50, 35, new EnchantedGroveScroll() );
			PlaceItemIn( 70, 35, new DruidFamiliarScroll() );
			PlaceItemIn( 90, 35, new GraspingRootsScroll() );
			PlaceItemIn( 30, 55, new HollowReedScroll() );
			PlaceItemIn( 50, 55, new LureStoneScroll() );
			PlaceItemIn( 70, 55, new MushroomGatewayScroll() );
			PlaceItemIn( 90, 55, new NaturesPassageScroll() );
			PlaceItemIn( 30, 75, new PackOfBeastScroll() );
			PlaceItemIn( 50, 75, new RestorativeSoilScroll() );
			PlaceItemIn( 70, 75, new ShieldOfEarthScroll() );
			PlaceItemIn( 90, 75, new SpringOfLifeScroll() );
			PlaceItemIn( 30, 95, new StoneCircleScroll() );
			PlaceItemIn( 50, 95, new SwarmOfInsectsScroll() );
			PlaceItemIn( 70, 95, new LeafWhirlwindScroll() );
			PlaceItemIn( 90, 95, new VolcanicEruptionScroll() );
			PlaceItemIn( 30, 115, new DruidBarkSkinScroll() );
			PlaceItemIn( 50, 115, new DruidDelayedPoisonScroll() );
			PlaceItemIn( 70, 115, new DruidGoodBerryScroll() );
		}

		private void PlaceItemIn( int x, int y, Item item )
		{
			AddItem( item );
			item.Location = new Point3D( x, y, 0 );
		}

		public DruidBag( Serial serial ) : base( serial )
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