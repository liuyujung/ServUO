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
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells;
using Server.Commands;


namespace Server.Scripts.Commands
{
	public class AllSpellCommands
	{
		public static void Initialize()
		{
			CommandSystem.Register( "CS", AccessLevel.Player, new CommandEventHandler( CS_OnCommand ) );
		}

		[Usage( "CS" )]
		[Description( "Casts the specified spell." )]
		public static void CS_OnCommand( CommandEventArgs e )
		{
			if( e.Length == 1 )
			{
				if( !Multis.DesignContext.Check( e.Mobile ) )
					return; // They are customizing

				string spellType = e.GetString( 0 ) + "Spell";
				Spell spell = null;
				for( int i = 0; i < SpellRegistry.Types.Length; i++ )
				{
					Type type = SpellRegistry.Types[i];
					if( type == null )
						continue;

					string currentName = type.Name;
					if( currentName == null )
						continue;

					if( Insensitive.Equals( spellType, currentName ) )
					{
						if( HasSpell( e.Mobile, i ) )
						{
							spell = SpellRegistry.NewSpell( i, e.Mobile, null );
							break;
						}
						else
						{
							e.Mobile.SendLocalizedMessage( 500015 ); // You do not have that spell!
							return;
						}
					}
				}

				if( spell != null )
					spell.Cast();
				else
					e.Mobile.SendMessage( "That spell was not found." );

			}
			else
			{
				e.Mobile.SendMessage( "Format: CS <name>" );
			}
		}
		public static bool HasSpell( Mobile from, int spellID )
		{
			Spellbook book = Spellbook.Find( from, spellID );

			if( book != null && book.UseRestrictions && !SpellRestrictions.CheckRestrictions( from, book ) )
			{
				return false;
			}

			return ( book != null && book.HasSpell( spellID ) );
		}
	}
}
