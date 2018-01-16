using System;
using Server.Network;
using Server;
using Server.Targets;
using Server.Targeting;
using Server.Mobiles;
using System.Collections;
using Server.Misc;
namespace Server.Items
{
	public class HerbOfLife : BaseMagicFood
	{
		[Constructable]
		public HerbOfLife() : base( 0xC85, MagicFoodEffect.Resurrect )
		{
			Weight = 1.0;
			Movable = true;
			Hue = 1288;
			Name = "herb of life";
		}

		public HerbOfLife( Serial serial ) : base( serial )
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
	  
	  	public override void Drink( Mobile m )
      	        {
         	if ( m.InRange( this.GetWorldLocation(), 1 ) ) 
         	{ 
                       // m.Target = new KillResTarget( false );
                        m.Target = new ResHerbTarget();
						m.SendMessage( "What corpse would you like to revive?" );
                        this.Delete();
         	} 
         	else 
         	{ 
            	m.LocalOverheadMessage( MessageType.Regular, 906, 1019045 ); // I can't reach that. 
         	} 
		}
	}
 public class ResHerbTarget : Target
    {
       public ResHerbTarget() : base( 12, false, TargetFlags.Beneficial )
        {
        }
        protected override void OnTarget( Mobile from, object targeted )
        {
                Corpse c = targeted as Corpse;

                        if ( c == null || c.Carved == true || c.Owner.Alive)
			{
				from.SendMessage( "Nothing happens!" );
			}
			else
			{
				Type type = null;

				if ( c.Owner != null )
					type = c.Owner.GetType();

				if (((c.Owner != null) && (c.Owner is BaseCreature)))
				{
                                   from.SendMessage( "Nothing happens!" );
				}
				else
				{
                                   c.Owner.Location = c.Location;
                                   c.Owner.Map = c.Map;
                                   c.Owner.SendMessage( "Your soul has been wrenched back into its damaged body!" );
                                   c.Owner.Resurrect();
                                   c.Open( c.Owner, true );
                                   c.Delete();
                                }
                                }
       }
    }
}
