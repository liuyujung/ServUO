/*
Special thanks to Ryan.
With RunUO we now have the ability to become our own Richard Garriott.
Special thanks to ArteGordon for technical support.
Lucid's Characterization System created by Lucid Nagual <<_Admin of the Conjuring_>>.
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
       )   Characterization System     )
      /~    Version [1].0 & Above    _/
    _/_______________________________/
 -=(_)_______________________________)=-
 */
using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using Server.ACC.CM;


namespace Server.LucidNagual
{
	public class ClassGump : Gump
	{
		private Mobile m_Mobile;
		
		string c1 = "Nomad";
		string c2 = "Cleric";
		string c3 = "Druid";
		string c4 = "Bard";
		string c5 = "Ranger";
		string c6 = "Rogue";
		string c7 = "Paladin";
		string c8 = "Mage";
		string c9 = "Necromancer";
		string c10 = "Tamer";
		string c11 = "Farmer";
		string c12 = "Crafter";
		string c13 = "Ninja";
		string c14 = "Samurai";
		string c15 = "Arcanist";
		
		string cmsg = "You are now a";
		int bp = 5000;    //Change spellbook prices here!
		ushort sc = 100;  //Skillcap setting.
		ushort rc = 29;   //Restricted cap.
		ushort ms = 75;   //Main skill setting.
		ushort ss = 50;   //Secondary skill setting.
		
		public ClassGump( Mobile from ) : base( 0, 0 )
		{
			from.CloseGump( typeof ( ClassGump ) );
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			
			//<<Page One>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
			AddPage( 1 );
			AddWindow();
			//this.AddPage(0);
			
			//--<<Title>>----------------------------------------------------------------------<>
			this.AddLabel(180, 145, 1485, @"Choose Your Class");
			//--<<Title>>----------------------------------------------------------------------<>
			
			//--<<Class Menu>>-<<Page 1>>-------------------------------------------------<Start>
			this.AddLabel(180, 175, 0, c1);
			this.AddButton(145, 170, 2152, 2154, 1/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 205, 0, c2);
			this.AddButton(145, 200, 2152, 2154, 2/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 235, 0, c3);
			this.AddButton(145, 230, 2152, 2154, 3/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 264, 0, c4);
			this.AddButton(145, 260, 2152, 2154, 4/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 295, 0, c5);
			this.AddButton(145, 290, 2152, 2154, 5/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 325, 0, c6);
			this.AddButton(145, 320, 2152, 2154, 6/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 355, 0, c7);
			this.AddButton(145, 350, 2152, 2154, 7/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 385, 0, c8);
			this.AddButton(145, 380, 2152, 2154, 8/*Button Number*/, GumpButtonType.Reply, 0);
			//--<<Class Menu>>-<<Page 1>>---------------------------------------------------<End>
			
			//--<<Turn Page Buttons>>-<<Page 1>>------------------------------------------<Start>
			//Turn pages with these buttons:
			this.AddButton(290, 415, 59, 2154, 0/*Page Number*/,   GumpButtonType.Page, 2);
			//this.AddButton(145, 415, 57, 2154, 0/*Page Number*/,  GumpButtonType.Reply, 0);
			//--<<Turn Page Buttons>>-<<Page 1>>--------------------------------------------<End>
			
			//<<Page One>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
			
			
			//<<Page Two>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
			AddPage( 2 );
			AddWindow();
			//this.AddPage(0);
			
			//--<<Title>>----------------------------------------------------------------------<>
			this.AddLabel(180, 145, 1485, @"Choose Your Class");
			//--<<Title>>----------------------------------------------------------------------<>
			
			//--<<Class Menu>>-<<Page 2>>-------------------------------------------------<Start>
			this.AddLabel(180, 175, 0, c9);
			this.AddButton(145, 170, 2152, 2154, 9/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 205, 0, c10);
			this.AddButton(145, 200, 2152, 2154, 10/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 235, 0, c11);
			this.AddButton(145, 230, 2152, 2154, 11/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 265, 0, c12);
			this.AddButton(145, 260, 2152, 2154, 12/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 295, 0, c13);
			this.AddButton(145, 290, 2152, 2154, 13/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 325, 0, c14);
			this.AddButton(145, 320, 2152, 2154, 14/*Button Number*/, GumpButtonType.Reply, 0);
			
			this.AddLabel(180, 355, 0, c15);
			this.AddButton(145, 350, 2152, 2154, 15/*Button Number*/, GumpButtonType.Reply, 0);
			
			//this.AddLabel(180, 385, 0, c16);
			//this.AddButton(145, 380, 2152, 2154, 16/*Button Number*/, GumpButtonType.Reply, 0);
			//--<<Class Menu>>-<<Page 2>>---------------------------------------------------<End>
			
			//--<<Turn Page Buttons>>-<<Page 2>>------------------------------------------<Start>
			//Turn pages with these buttons:
			//this.AddButton(290, 415, 59, 2154, 0/*Page Number*/,   GumpButtonType.Page, 3);
			this.AddButton(145, 415, 57, 2154, 0/*Page Number*/,  GumpButtonType.Page, 1);
			//--<<Turn Page Buttons>>-<<Page 2>>--------------------------------------------<End>
			//<<Page Two>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
			
			
			//<<Page Three>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
			//AddPage( 3 );
			//AddWindow();
			//this.AddPage(0);
			
			//--<<Title>>----------------------------------------------------------------------<>
			//this.AddLabel(180, 145, 1485, @"Choose Your Class");
			//--<<Title>>----------------------------------------------------------------------<>
			
			//--<<Class Menu>>-<<Page 3>>-------------------------------------------------<Start>
			//this.AddLabel(180, 175, 0, c17);
			//this.AddButton(145, 170, 2152, 2154, 17/*Button Number*/, GumpButtonType.Reply, 0);
			
			//this.AddLabel(180, 205, 0, c18);
			//this.AddButton(145, 200, 2152, 2154, 18/*Button Number*/, GumpButtonType.Reply, 0);
			
			//this.AddLabel(180, 235, 0, c19);
			//this.AddButton(145, 230, 2152, 2154, 19/*Button Number*/, GumpButtonType.Reply, 0);
			
			//this.AddLabel(180, 265, 0, @"");
			//this.AddButton(145, 260, 2152, 2154, 20/*Button Number*/, GumpButtonType.Reply, 0);
			
			//this.AddLabel(180, 295, 0, @"");
			//this.AddButton(145, 290, 2152, 2154, 21/*Button Number*/, GumpButtonType.Reply, 0);
			
			//this.AddLabel(180, 325, 0, @"");
			//this.AddButton(145, 320, 2152, 2154, 22/*Button Number*/, GumpButtonType.Reply, 0);
			
			//this.AddLabel(180, 355, 0, @"");
			//this.AddButton(145, 350, 2152, 2154, 23/*Button Number*/, GumpButtonType.Reply, 0);
			
			//this.AddLabel(180, 385, 0, @"");
			//this.AddButton(145, 380, 2152, 2154, 24/*Button Number*/, GumpButtonType.Reply, 0);
			//--<<Class Menu>>-<<Page 3>>---------------------------------------------------<End>
			
			//--<<Turn Page Buttons>>-<<Page 3>>------------------------------------------<Start>
			//Turn pages with these buttons:
			//this.AddButton(290, 415, 59, 2154, 0/*Page Number*/,   GumpButtonType.Page, 2);
			//this.AddButton(145, 415, 57, 2154, 0/*Page Number*/,  GumpButtonType.Page, 2);
			//--<<Turn Page Buttons>>-<<Page 3>>--------------------------------------------<End>
			//<<Page Three>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
		}
		
		private void AddWindow()
		{
			this.AddBackground(124, 128, 219, 318, 9200);
			this.AddImage(132, 138, 48);
			this.AddImage(331, 406, 10464);
			this.AddImage(141, 147, 1124);
			this.AddImage(103, 65, 10463);
			this.AddImage(233, 65, 10463);
			this.AddImage(103, 136, 10464);
			this.AddImage(103, 190, 10464);
			this.AddImage(103, 244, 10464);
			this.AddImage(103, 298, 10464);
			this.AddImage(103, 352, 10464);
			this.AddImage(103, 406, 10464);
			this.AddImage(103, 440, 10463);
			this.AddImage(331, 136, 10464);
			this.AddImage(233, 440, 10463);
			this.AddImage(331, 190, 10464);
			this.AddImage(331, 244, 10464);
			this.AddImage(331, 298, 10464);
			this.AddImage(331, 352, 10464);
			this.AddImage(199, 57, 9804);
			this.AddImage(198, 425, 9000);
			this.AddImage(295, 447, 5569);
			this.AddImage(109, 447, 5567);
			this.AddImage(295, 70, 5553);
			this.AddImage(109, 70, 5549);
			this.AddImage(201, 447, 112);
		}
		
		private void CSpellCaps( Mobile from )
		{
			from.Skills[SkillName.Magery].Cap = rc;
			from.Skills[SkillName.Chivalry].Cap = rc;
			from.Skills[SkillName.Necromancy].Cap = rc;
			from.Skills[SkillName.Ninjitsu].Cap = rc;
			from.Skills[SkillName.Bushido].Cap = rc;
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			Container pack = from.Backpack;
			BankBox Banker = from.BankBox;
			
			switch ( info.ButtonID )
			{
				case 0: //Close Gump
					{
						from.CloseGump( typeof ( ClassGump ) );
						break;
					}
					
				case 1: //Nomad
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Nomad ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c1 );
						for ( int i = 0; i < PowerScroll.Skills.Count; ++i )
							from.Skills[PowerScroll.Skills[ i ]].Cap = sc;
						//from.SendMessage( "The Nomad has no boundaries, but cannot use powerscrolls. A Nomad has a limited skill cap of 100 in all skills." );
						break;
					}
					
				case 2: //Cleric
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Cleric ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c2 );
						CSpellCaps( from );
						if ( ControlCenter.SetFreeSkills == true )
						{
							if ( from.Skills[SkillName.Chivalry].Base <= ms - 1 )
							{
								from.Skills[SkillName.Chivalry].Cap = sc;
								from.Skills[SkillName.Chivalry].Base = ms;
							}
							if ( from.Skills[SkillName.Focus].Base <= ss - 1 )
							{
								from.Skills[SkillName.Focus].Cap = sc;
								from.Skills[SkillName.Focus].Base = ss;
							}
						}
						else if ( ControlCenter.SetFreeSkills == false )
						{
							if ( Banker.ConsumeTotal(typeof(Gold), bp) )
							{
								if ( from.Skills[SkillName.Chivalry].Base <= ms - 1 )
								{
									from.Skills[SkillName.Chivalry].Cap = sc;
									from.Skills[SkillName.Chivalry].Base = ms;
								}
								if ( from.Skills[SkillName.Focus].Base <= ss - 1 )
								{
									from.Skills[SkillName.Focus].Cap = sc;
									from.Skills[SkillName.Focus].Base = ss;
								}
								from.SendMessage( "{0} gold has been withdrawn from your bank.", bp );
							}
							else
								from.PrivateOverheadMessage( MessageType.Whisper, 0, true,"You do not have enough funds to purchase this training.", from.NetState );
						}
						else if ( ControlCenter.SetSkinHues == true )
						{
							from.Hue = 1150;
						}
						else if ( ControlCenter.SetGiveBooks == true )
						{
							pack.DropItem( new BookOfChivalry() );
							from.SendMessage( "The spellbook has been placed in your backpack." );
						}
						else if ( ControlCenter.SetGiveBooks == false )
						{
							from.SendMessage( "You can find spellbooks at the Scribe for {0} gold.", bp );
						}
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c2 );
						from.SendMessage( "Paladins are good fighters that look out for their fellow man." );
						break;
					}
					
				case 3: //Druid
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Druid ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c3 );
						CSpellCaps( from );
						if ( ControlCenter.SetFreeSkills == true )
						{
							if ( from.Skills[SkillName.AnimalTaming].Base <= ms -1 )
							{
								from.Skills[SkillName.AnimalTaming].Cap = sc;
								from.Skills[SkillName.AnimalTaming].Base = ms;
							}
							if ( from.Skills[SkillName.AnimalLore].Base <= ss -1 )
							{
								from.Skills[SkillName.AnimalLore].Cap = sc;
								from.Skills[SkillName.AnimalLore].Base = ss;
							}
						}
						else if ( ControlCenter.SetFreeSkills == false )
						{
							if ( Banker.ConsumeTotal(typeof(Gold), bp) )
							{
								if ( from.Skills[SkillName.AnimalTaming].Base <= ms -1 )
								{
									from.Skills[SkillName.AnimalTaming].Cap = sc;
									from.Skills[SkillName.AnimalTaming].Base = ms;
								}
								if ( from.Skills[SkillName.AnimalLore].Base <= ss -1 )
								{
									from.Skills[SkillName.AnimalLore].Cap = sc;
									from.Skills[SkillName.AnimalLore].Base = ss;
								}
								from.SendMessage( "{0} gold has been withdrawn from your bank.", bp );
							}
							else
								from.PrivateOverheadMessage( MessageType.Whisper, 0, true,"You do not have enough funds to purchase this training.", from.NetState );
						}
						else if ( ControlCenter.SetSkinHues == true )
						{
							from.Hue = 0x58B;
						}
						else if ( ControlCenter.SetGiveBooks == true )
						{
							pack.DropItem( new DruidSpellbook() );
							from.SendMessage( "The spellbook has been placed in your backpack." );
						}
						else if ( ControlCenter.SetGiveBooks == false )
						{
							from.SendMessage( "You can find spellbooks at the Scribe for {0} gold.", bp );
						}
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c3 );
						from.SendMessage( "Druids are good tamers and are at one with nature." );
						break;
					}
					
				case 4: //Bard
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Bard ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c4 );
						CSpellCaps( from );
						if ( ControlCenter.SetFreeSkills == true )
						{
							if ( from.Skills[SkillName.Musicianship].Base <= ms -1 )
							{
								from.Skills[SkillName.Musicianship].Cap = sc;
								from.Skills[SkillName.Musicianship].Base = ms;
							}
						}
						else if ( ControlCenter.SetFreeSkills == false )
						{
							if ( Banker.ConsumeTotal(typeof(Gold), bp) )
							{
								if ( from.Skills[SkillName.Musicianship].Base <= ms - 1 )
								{
									from.Skills[SkillName.Musicianship].Cap = sc;
									from.Skills[SkillName.Musicianship].Base = ms;
								}
								from.SendMessage( "{0} gold has been withdrawn from your bank.", bp );
							}
							else
								from.PrivateOverheadMessage( MessageType.Whisper, 0, true,"You do not have enough funds to purchase this training.", from.NetState );
						}
						else if ( ControlCenter.SetSkinHues == true )
						{
							from.Hue = 0x96;
						}
						else if ( ControlCenter.SetGiveBooks == true )
						{
							//pack.DropItem( new SongBook() );
							//from.SendMessage( "The spellbook has been placed in your backpack." );
						}
						else if ( ControlCenter.SetGiveBooks == false )
						{
							from.SendMessage( "You can find spellbooks at the Scribe for 5,000 gold." );
						}
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c4 );
						from.SendMessage( "Bards make songs of power that are good for the soul." );
						break;
					}
					
				case 5: //Ranger
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Ranger ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c5 );
						CSpellCaps( from );
						if ( ControlCenter.SetFreeSkills == true )
						{
							if ( from.Skills[SkillName.Archery].Base <= ms - 1 )
							{
								from.Skills[SkillName.Archery].Cap = sc;
								from.Skills[SkillName.Archery].Base = ms;
							}
							if ( from.Skills[SkillName.Veterinary].Base <= ss - 1 )
							{
								from.Skills[SkillName.Veterinary].Cap = sc;
								from.Skills[SkillName.Veterinary].Base = ss;
							}
						}
						else if ( ControlCenter.SetFreeSkills == false )
						{
							if ( Banker.ConsumeTotal(typeof(Gold), bp) )
							{
								if ( from.Skills[SkillName.Archery].Base <= ms - 1 )
								{
									from.Skills[SkillName.Archery].Cap = sc;
									from.Skills[SkillName.Archery].Base = ms;
								}
								if ( from.Skills[SkillName.Veterinary].Base <= ss - 1 )
								{
									from.Skills[SkillName.Veterinary].Cap = sc;
									from.Skills[SkillName.Veterinary].Base = ss;
								}
								from.SendMessage( "{0} gold has been withdrawn from your bank.", bp );
							}
							else
								from.PrivateOverheadMessage( MessageType.Whisper, 0, true,"You do not have enough funds to purchase this training.", from.NetState );
						}
						else if ( ControlCenter.SetSkinHues == true )
						{
							from.Hue = 2001;
						}
						else if ( ControlCenter.SetGiveBooks == true )
						{
							pack.DropItem( new RangerSpellbook() );
							from.SendMessage( "The spellbook has been placed in your backpack." );
						}
						else if ( ControlCenter.SetGiveBooks == false )
						{
							from.SendMessage( "You can find spellbooks at the Scribe for {0} gold.", bp );
						}
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c5 );
						from.SendMessage( "Rangers are good with animals and know how to use a bow." );
						break;
					}
					
				case 6: //Rogue
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Rogue ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c6 );
						CSpellCaps( from );
						if ( ControlCenter.SetFreeSkills == true )
						{
							if ( from.Skills[SkillName.Stealing].Base <= ms - 1 )
							{
								from.Skills[SkillName.Stealing].Cap = sc;
								from.Skills[SkillName.Stealing].Base = ms;
							}
							if ( from.Skills[SkillName.Hiding].Base <= ss - 1 )
							{
								from.Skills[SkillName.Hiding].Cap = sc;
								from.Skills[SkillName.Hiding].Base = ss;
							}
						}
						else if ( ControlCenter.SetFreeSkills == false )
						{
							if ( Banker.ConsumeTotal(typeof(Gold), bp) )
							{
								if ( from.Skills[SkillName.Stealing].Base <= ms - 1 )
								{
									from.Skills[SkillName.Stealing].Cap = sc;
									from.Skills[SkillName.Stealing].Base = ms;
								}
								if ( from.Skills[SkillName.Hiding].Base <= ss - 1 )
								{
									from.Skills[SkillName.Hiding].Cap = sc;
									from.Skills[SkillName.Hiding].Base = ss;
								}
								from.SendMessage( "{0} gold has been withdrawn from your bank.", bp );
							}
							else
								from.PrivateOverheadMessage( MessageType.Whisper, 0, true,"You do not have enough funds to purchase this training.", from.NetState );
						}
						else if ( ControlCenter.SetSkinHues == true )
						{
							from.Hue = 0x20;
						}
						else if ( ControlCenter.SetGiveBooks == true )
						{
							//pack.DropItem( new RogueSpellbook() );
							//from.SendMessage( "The spellbook has been placed in your backpack." );
						}
						else if ( ControlCenter.SetGiveBooks == false )
						{
							from.SendMessage( "You can find spellbooks at the Scribe for {0} gold.", bp );
						}
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c6 );
						from.SendMessage( "Rogues are dastardly beings who are good at stealing." );
						break;
					}
					
				case 7: //Paladin
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Paladin ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c7 );
						CSpellCaps( from );
						if ( ControlCenter.SetFreeSkills == true )
						{
							if ( from.Skills[SkillName.Chivalry].Base <= ms - 1 )
							{
								from.Skills[SkillName.Chivalry].Cap = sc;
								from.Skills[SkillName.Chivalry].Base = ms;
							}
							if ( from.Skills[SkillName.Focus].Base <= ss - 1 )
							{
								from.Skills[SkillName.Focus].Cap = sc;
								from.Skills[SkillName.Focus].Base = ss;
							}
						}
						else if ( ControlCenter.SetFreeSkills == false )
						{
							if ( Banker.ConsumeTotal(typeof(Gold), bp) )
							{
								if ( from.Skills[SkillName.Chivalry].Base <= ms - 1 )
								{
									from.Skills[SkillName.Chivalry].Cap = sc;
									from.Skills[SkillName.Chivalry].Base = ms;
								}
								if ( from.Skills[SkillName.Focus].Base <= ss - 1 )
								{
									from.Skills[SkillName.Focus].Cap = sc;
									from.Skills[SkillName.Focus].Base = ss;
								}
								from.SendMessage( "{0} gold has been withdrawn from your bank.", bp );
							}
							else
								from.PrivateOverheadMessage( MessageType.Whisper, 0, true,"You do not have enough funds to purchase this training.", from.NetState );
						}
						else if ( ControlCenter.SetSkinHues == true )
						{
							from.Hue = 1150;
						}
						else if ( ControlCenter.SetGiveBooks == true )
						{
							pack.DropItem( new BookOfChivalry() );
							from.SendMessage( "The spellbook has been placed in your backpack." );
						}
						else if ( ControlCenter.SetGiveBooks == false )
						{
							from.SendMessage( "You can find spellbooks at the Scribe for {0} gold.", bp );
						}
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c7 );
						break;
					}
					
				case 8: //Mage
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Mage ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c8 );
						CSpellCaps( from );
						if ( ControlCenter.SetFreeSkills == true )
						{
							if ( from.Skills[SkillName.Magery].Base <= ms - 1 )
							{
								from.Skills[SkillName.Magery].Cap = sc;
								from.Skills[SkillName.Magery].Base = ms;
							}
							if ( from.Skills[SkillName.Meditation].Base <= ss - 1 )
							{
								from.Skills[SkillName.Meditation].Cap = sc;
								from.Skills[SkillName.Meditation].Base = ss;
							}
						}
						else if ( ControlCenter.SetFreeSkills == false )
						{
							if ( Banker.ConsumeTotal(typeof(Gold), bp) )
							{
								if ( from.Skills[SkillName.Magery].Base <= ms - 1 )
								{
									from.Skills[SkillName.Magery].Cap = sc;
									from.Skills[SkillName.Magery].Base = ms;
								}
								if ( from.Skills[SkillName.Meditation].Base <= ss - 1 )
								{
									from.Skills[SkillName.Meditation].Cap = sc;
									from.Skills[SkillName.Meditation].Base = ss;
								}
								from.SendMessage( "{0} gold has been withdrawn from your bank.", bp );
							}
							else
								from.PrivateOverheadMessage( MessageType.Whisper, 0, true,"You do not have enough funds to purchase this training.", from.NetState );
						}
						else if ( ControlCenter.SetSkinHues == true )
						{
							from.Hue = 0x481;
						}
						else if ( ControlCenter.SetGiveBooks == true )
						{
							pack.DropItem( new Spellbook() );
							from.SendMessage( "The spellbook has been placed in your backpack." );
						}
						else if ( ControlCenter.SetGiveBooks == false )
						{
							from.SendMessage( "You can find spellbooks at the Scribe for {0} gold.", bp );
						}
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c8 );
						break;
					}
					
				case 9: //Necromancer
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Necromancer ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c9 );
						CSpellCaps( from );
						if ( ControlCenter.SetFreeSkills == true )
						{
							if ( from.Skills[SkillName.Necromancy].Base <= ms - 1 )
							{
								from.Skills[SkillName.Necromancy].Cap = sc;
								from.Skills[SkillName.Necromancy].Base = ms;
							}
							if ( from.Skills[SkillName.SpiritSpeak].Base <= ss - 1 )
							{
								from.Skills[SkillName.SpiritSpeak].Cap = sc;
								from.Skills[SkillName.SpiritSpeak].Base = ss;
							}
						}
						else if ( ControlCenter.SetFreeSkills == false )
						{
							if ( Banker.ConsumeTotal(typeof(Gold), bp) )
							{
								if ( from.Skills[SkillName.Necromancy].Base <= ms - 1 )
								{
									from.Skills[SkillName.Necromancy].Cap = sc;
									from.Skills[SkillName.Necromancy].Base = ms;
								}
								if ( from.Skills[SkillName.SpiritSpeak].Base <= ss - 1 )
								{
									from.Skills[SkillName.SpiritSpeak].Cap = sc;
									from.Skills[SkillName.SpiritSpeak].Base = ss;
								}
								from.SendMessage( "{0} gold has been withdrawn from your bank.",bp );
							}
							else
								from.PrivateOverheadMessage( MessageType.Whisper, 0, true,"You do not have enough funds to purchase this training.", from.NetState );
						}
						else if ( ControlCenter.SetSkinHues == true )
						{
							from.Hue = 0x44;
						}
						else if ( ControlCenter.SetGiveBooks == true )
						{
							pack.DropItem( new NecromancerSpellbook() );
							//pack.DropItem( new UndeadSpellbook() );
							from.SendMessage( "The spellbooks have been placed in your backpack." );
						}
						else if ( ControlCenter.SetGiveBooks == false )
						{
							from.SendMessage( "You can find spellbooks at the Scribe for {0} gold.", bp );
						}
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c9 );						break;
					}
					
				case 10: //Tamer
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Tamer ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c10 );
						CSpellCaps( from );
						if ( ControlCenter.SetFreeSkills == true )
						{
							if ( from.Skills[SkillName.AnimalLore].Base <= ms - 1 )
							{
								from.Skills[SkillName.AnimalLore].Cap = sc;
								from.Skills[SkillName.AnimalLore].Base = ms;
							}
							if ( from.Skills[SkillName.AnimalTaming].Base <= ss - 1 )
							{
								from.Skills[SkillName.AnimalTaming].Cap = sc;
								from.Skills[SkillName.AnimalTaming].Base = ss;
							}
							if ( from.Skills[SkillName.Musicianship].Base <= ms - 1 )
							{
								from.Skills[SkillName.Musicianship].Cap = sc;
								from.Skills[SkillName.Musicianship].Base = ms;
							}
						}
						else if ( ControlCenter.SetFreeSkills == false )
						{
							if ( Banker.ConsumeTotal(typeof(Gold), bp) )
							{
								if ( from.Skills[SkillName.AnimalLore].Base <= ms - 1 )
								{
									from.Skills[SkillName.AnimalLore].Cap = sc;
									from.Skills[SkillName.AnimalLore].Base = ms;
								}
								
								if ( from.Skills[SkillName.AnimalTaming].Base <= ss - 1 )
								{
									from.Skills[SkillName.AnimalTaming].Cap = sc;
									from.Skills[SkillName.AnimalTaming].Base = ss;
								}
								
								if ( from.Skills[SkillName.Musicianship].Base <= ms - 1 )
								{
									from.Skills[SkillName.Musicianship].Cap = sc;
									from.Skills[SkillName.Musicianship].Base = ms;
								}
								from.SendMessage( "{0} gold has been withdrawn from your bank.", bp );
							}
							else
								from.PrivateOverheadMessage( MessageType.Whisper, 0, true,"You do not have enough funds to purchase this training.", from.NetState );
						}
						else if ( ControlCenter.SetSkinHues == true )
						{
							from.Hue = 0x58B;
						}
						else if ( ControlCenter.SetGiveBooks == true )
						{
							//pack.DropItem( new SongBook() );
							pack.DropItem( new DruidSpellbook() );
							from.SendMessage( "The spellbooks have been placed in your backpack." );
						}
						else if ( ControlCenter.SetGiveBooks == false )
						{
							from.SendMessage( "You can find a Song Book at the Scribe for {0} gold.", bp );
						}
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c10 );
						from.SendMessage( "Tamers have druid and bard capabilities." );
						break;
					}
					
				case 11: //Farmer
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Farmer ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c11 );
						CSpellCaps( from );
						break;
					}
					
				case 12: //Crafter
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Crafter ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c12 );
						CSpellCaps( from );
						break;
					}
					
				case 13: //Ninja
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Ninja ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c13 );
						CSpellCaps( from );
						if ( ControlCenter.SetFreeSkills == true )
						{
							if ( from.Skills[SkillName.Ninjitsu].Base <= ms - 1 )
							{
								from.Skills[SkillName.Ninjitsu].Cap = sc;
								from.Skills[SkillName.Ninjitsu].Base = ms;
							}
						}
						else if ( ControlCenter.SetFreeSkills == false )
						{
							if ( Banker.ConsumeTotal(typeof(Gold), bp) )
							{
								if ( from.Skills[SkillName.Ninjitsu].Base <= ms - 1 )
								{
									from.Skills[SkillName.Ninjitsu].Cap = sc;
									from.Skills[SkillName.Ninjitsu].Base = ms;
								}
								from.SendMessage( "{0} gold has been withdrawn from your bank.", bp );
							}
							else
								from.PrivateOverheadMessage( MessageType.Whisper, 0, true,"You do not have enough funds to purchase this training.", from.NetState );
						}
						else if ( ControlCenter.SetSkinHues == true )
						{
							from.Hue = 16000;
						}
						else if ( ControlCenter.SetGiveBooks == true )
						{
							pack.DropItem( new BookOfNinjitsu() );
							from.SendMessage( "The spellbook has been placed in your backpack." );
						}
						else if ( ControlCenter.SetGiveBooks == false )
						{
							from.SendMessage( "You can find spellbooks at the Scribe for {0} gold.", bp );
						}
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c13 );
						from.SendMessage( "Ninjas ." );
						break;
					}
					
				case 14: //Samurai
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Samurai ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c14 );
						CSpellCaps( from );
						if ( ControlCenter.SetFreeSkills == true )
						{
							if ( from.Skills[SkillName.Bushido].Base <= ms - 1 )
							{
								from.Skills[SkillName.Bushido].Cap = sc;
								from.Skills[SkillName.Bushido].Base = ms;
							}
						}
						else if ( ControlCenter.SetFreeSkills == false )
						{
							if ( Banker.ConsumeTotal(typeof(Gold), bp) )
							{
								if ( from.Skills[SkillName.Bushido].Base <= ms - 1 )
								{
									from.Skills[SkillName.Bushido].Cap = sc;
									from.Skills[SkillName.Bushido].Base = ms;
									
								}
								from.SendMessage( "{0} gold has been withdrawn from your bank.", bp );
							}
							else
								from.PrivateOverheadMessage( MessageType.Whisper, 0, true,"You do not have enough funds to purchase this training.", from.NetState );
						}
						else if ( ControlCenter.SetSkinHues == true )
						{
							from.Hue = 32;
						}
						else if ( ControlCenter.SetGiveBooks == true )
						{
							pack.DropItem( new BookOfBushido() );
							from.SendMessage( "The spellbook has been placed in your backpack." );
						}
						else if ( ControlCenter.SetGiveBooks == false )
						{
							from.SendMessage( "You can find spellbooks at the Scribe for {0} gold.", bp );
						}
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c14 );
						from.SendMessage( "Samurais ." );
						break;
					}
					
				case 15: //Arcanist
					{
						CentralMemory.AppendModule( from.Serial, new ClassREMModule( from.Serial, ClassREM.Arcanist ), true );
						from.SendMessage( MsgCenter.Msg[8].Text );
						from.SendMessage( "{0}" + " {1}" + "!", cmsg, c15 );
						CSpellCaps( from );
						break;
					}
			}
		}
	}
}

