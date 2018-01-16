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
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.LucidNagual;
using Server.Spells;
using Server.ACC.CM;
using Server.Gumps;


namespace Server.Spells
{
	public class AllSpells
	{
		//--<Spell Restrictions>---------------------------------------------------<Start>
		private static ControlCenter ctrlc;
		
		public static bool CheckRestriction( Mobile caster, int sclass )
		{
			ClassREMModule mod = (ClassREMModule)CentralMemory.GetModule( caster.Serial, typeof( ClassREMModule ) );
			
			//if ( caster != null && ControlCenter.SetRestrictions == false )
			//	return true;
			
			if ( !ASSettings.EnableClassSystem )
				return true;
			
			else if ( ASSettings.EnableClassSystem && mod == null )
			{
				caster.SendGump( new ClassGump( caster ) );
				return false;
			}
						
			else if ( caster == null )
				return false;
			
			else if ( caster.AccessLevel > AccessLevel.Player || caster is BaseCreature || caster is BaseHealer || caster is BaseChampion || caster is BaseMount )
				return true;
			
			else if ( mod != null && mod.ClassREM == ClassREM.Nomad )
				return true;
			
			else if ( mod != null & mod.ClassREM == ClassREM.None )
			{
				caster.SendGump( new ClassGump( caster ) );
				return false;
			}
			
			else if ( sclass == 1 ) //Paladin
			{
				if ( mod != null && mod.ClassREM == ClassREM.Paladin )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells.", caster.NetState );
			}
			
			else if ( sclass == 2 ) //Mage
			{
				if ( mod != null && mod.ClassREM == ClassREM.Mage )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells.", caster.NetState );
			}
			
			else if ( sclass == 3 ) //Necromancer
			{
				if ( mod != null && mod.ClassREM == ClassREM.Paladin )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells.", caster.NetState );
			}
			
			else if ( sclass == 4 ) //Druid
			{
				if ( mod != null && mod.ClassREM == ClassREM.Druid )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells.", caster.NetState );
			}
			
			else if ( sclass == 4 ) //Druid Tamer
			{
				if ( mod != null && mod.ClassREM == ClassREM.Tamer )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells.", caster.NetState );
			}
			
			else if ( sclass == 5 ) //Cleric
			{
				if ( mod != null && mod.ClassREM == ClassREM.Cleric )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells.", caster.NetState );
			}
			
			else if ( sclass == 6 ) //Ranger
			{
				if ( mod != null && mod.ClassREM == ClassREM.Ranger )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells.", caster.NetState );
			}
			
			else if ( sclass == 7 ) //Rogue
			{
				if ( mod != null && mod.ClassREM == ClassREM.Rogue )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells.", caster.NetState );
			}
			
			else if ( sclass == 8 ) //Bard
			{
				if ( mod != null && mod.ClassREM == ClassREM.Bard )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells.", caster.NetState );
			}
			
			else if ( sclass == 8 ) //Druid Tamer
			{
				if ( mod != null && mod.ClassREM == ClassREM.Tamer )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells.", caster.NetState );
			}

			else if ( sclass == 9 ) //Farmer
			{
				if ( mod != null && mod.ClassREM == ClassREM.Farmer )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for farming.", caster.NetState );
			}
			
			else if ( sclass == 19 ) //Ninja
			{
				if ( mod != null && mod.ClassREM == ClassREM.Ninja )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
			}
			
			else if ( sclass == 20 ) //Samurai
			{
				if ( mod != null && mod.ClassREM == ClassREM.Samurai )
					return true;
				else
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
			}
			
			else if ( sclass == 99 && caster.AccessLevel > AccessLevel.Seer )
				return true;
			
			return false;
		}
		//--<Spell Restrictions>-----------------------------------------------------<End>
	}
	
	
	//--<Spellbook Restrictions>-----------------------------------------------<Start>
	public class SpellRestrictions
	{
		private static ControlCenter ctrlc;
		
		public static bool CheckRestrictions( Mobile caster, Spellbook book )
		{
			ClassREMModule mod = (ClassREMModule)CentralMemory.GetModule( caster.Serial, typeof( ClassREMModule ) );
			
			if ( ControlCenter.SetRestrictions == false )
				return true;
			
			if ( book == null )
				return false;
			
			if ( caster == null )
				return false;
			
			if ( book.SpellbookType == SpellbookType.Invalid )
				return false;
			
			if ( caster.AccessLevel > AccessLevel.Player )
				return true;
			
			if ( mod == null )
			{
				caster.SendGump( new ClassGump( caster ) );
				return false;
			}
			
			if ( mod != null & mod.ClassREM == ClassREM.None )
			{
				caster.SendGump( new ClassGump( caster ) );
				return false;
			}
			
			if ( book.SpellbookType == SpellbookType.Regular )
			{
				if ( mod != null && mod.ClassREM == ClassREM.Mage )
					return true;
				else
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
					return false;
				}
			}
			else if ( book.SpellbookType == SpellbookType.Necromancer )
			{
				if ( mod != null && mod.ClassREM == ClassREM.Necromancer )
					return true;
				else
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
					return false;
				}
			}
			else if ( book.SpellbookType == SpellbookType.Paladin )
			{
				if ( mod != null && mod.ClassREM == ClassREM.Paladin )
					return true;
				else
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
					return false;
				}
			}
			else if ( book.SpellbookType == SpellbookType.Avatar )
			{
				if ( mod != null && mod.ClassREM == ClassREM.Paladin )
					return true;
				else
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
					return false;
				}
			}
			else if ( book.SpellbookType == SpellbookType.Ninja )
			{
				if ( mod != null && mod.ClassREM == ClassREM.Ninja )
					return true;
				else
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
					return false;
				}
			}
			else if ( book.SpellbookType == SpellbookType.Samurai )
			{
				if ( mod != null && mod.ClassREM == ClassREM.Samurai )
					return true;
				else
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
					return false;
				}
			}
			else if ( book.SpellbookType == SpellbookType.Druid )
			{
				if ( mod != null && mod.ClassREM == ClassREM.Tamer )
					return true;
				else
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
					return false;
				}
			}
			else if ( book.SpellbookType == SpellbookType.Ancient )
			{
				if( caster.AccessLevel <= AccessLevel.Seer )
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You are allowed to use this.", caster.NetState );
					return false;
				}
				else if( caster.AccessLevel >= AccessLevel.Administrator )
					return true;
			}
			else if ( book.SpellbookType == SpellbookType.Cleric )
			{
				if ( mod != null && mod.ClassREM == ClassREM.Cleric )
					return true;
				else
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
					return false;
				}
			}
			else if ( book.SpellbookType == SpellbookType.Song )
			{
				if ( mod != null && mod.ClassREM == ClassREM.Tamer )
					return true;
				else
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
					return false;
				}
			}
			else if ( book.SpellbookType == SpellbookType.Undead )
			{
				if ( mod != null && mod.ClassREM == ClassREM.Necromancer )
					return true;
				else
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
					return false;
				}
			}
			else if ( book.SpellbookType == SpellbookType.Rogue )
			{
				if ( mod != null && mod.ClassREM == ClassREM.Rogue )
					return true;
				else
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
					return false;
				}
			}
			else if ( book.SpellbookType == SpellbookType.Ranger )
			{
				if ( mod != null && mod.ClassREM == ClassREM.Ranger )
					return true;
				else
				{
					caster.PrivateOverheadMessage( MessageType.Regular, 0x22, false, "You do not posses the training required for these spells. You must visit a Magics and Skill Trainer.", caster.NetState );
					return false;
				}
			}
			return true;
		}
	}
	//--<Spellbook Restrictions>-------------------------------------------------<End>
}
