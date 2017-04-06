
using System;
using Server;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Targeting;
using Server.Items;
using Server.Gumps;

namespace Server.Items
{
	public class WeaponTypeDeed : Item
	{

		[Constructable]
		public WeaponTypeDeed() : base( 0x14F0 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			Hue = 1152;
			Name = "Weapon Type Deed";
		}

        public WeaponTypeDeed(Serial serial): base(serial)
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

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else
			{
                from.CloseGump(typeof(WeaponIDDeedGump));
                from.SendGump(new WeaponIDDeedGump());
			}
		}
	}
}

namespace Server.Gumps
{
    public class WeaponIDDeedGump : Gump
    {
        public WeaponIDDeedGump()
            : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(225, 21, 455, 576, 9270);
            this.AddBlackAlpha(225, 25, 455, 568);
            this.AddCheck(232, 66, 10810, 10830, false, 50);//Kryss
            this.AddCheck(454, 202, 10810, 10830, false, 71);// Scimitar
            this.AddCheck(454, 229, 10810, 10830, false, 72);// ThinLongSword
            this.AddCheck(454, 255, 10810, 10830, false, 73);//Katana
            this.AddCheck(454, 282, 10810, 10830, false, 74);//Mace
            this.AddCheck(454, 309, 10810, 10830, false, 75);//Spear
            this.AddCheck(454, 67, 10810, 10830, false, 66);//Bow
            this.AddCheck(454, 417, 10810, 10830, false, 79);//Cutlass
            this.AddCheck(232, 470, 10810, 10830, false, 65);// BoneHavester
            this.AddCheck(454, 175, 10810, 10830, false, 70);//No-Dachi
            this.AddCheck(454, 363, 10810, 10830, false, 77);//Tessen
            this.AddCheck(454, 336, 10810, 10830, false, 76);//Wakizashi
            this.AddCheck(454, 390, 10810, 10830, false, 78);//Kama
            this.AddCheck(454, 148, 10810, 10830, false, 69);//Sai
            this.AddCheck(454, 121, 10810, 10830, false, 68);//Daisho
            this.AddCheck(454, 94, 10810, 10830, false, 67);//RadiantScimitar
            this.AddCheck(232, 444, 10810, 10830, false, 64);//QrnateAxe
            this.AddCheck(232, 417, 10810, 10830, false, 63);//ElvenMachete
            this.AddCheck(232, 390, 10810, 10830, false, 62);//RuneBlade
            this.AddCheck(232, 363, 10810, 10830, false, 61);//LeafBlade
            this.AddCheck(232, 336, 10810, 10830, false, 60);//AssassinSpike
            this.AddCheck(232, 309, 10810, 10830, false, 59);//Bokuto
            this.AddCheck(232, 283, 10810, 10830, false, 58);//Tetsubo
            this.AddCheck(232, 256, 10810, 10830, false, 57);//Tekagi
            this.AddCheck(232, 229, 10810, 10830, false, 56);//Nunchaku
            this.AddCheck(232, 202, 10810, 10830, false, 55);//Wand
            this.AddCheck(232, 175, 10810, 10830, false, 54);//Cleaver
            this.AddCheck(232, 148, 10810, 10830, false, 53);//Dagger
            this.AddCheck(232, 121, 10810, 10830, false, 52);//ShortSpear
            this.AddCheck(232, 94, 10810, 10830, false, 51);//WarFork
            this.AddCheck(454, 495, 10810, 10830, false, 82);//WarMace
            this.AddCheck(454, 522, 10810, 10830, false, 83);//Maul
            this.AddCheck(454, 549, 10810, 10830, false, 84);//WarHammer
            this.AddCheck(454, 469, 10810, 10830, false, 81);//ButcherKnife
            this.AddCheck(454, 443, 10810, 10830, false, 80);//Axe
			this.AddButton(271, 539, 10800, 10820, 1, GumpButtonType.Reply, 0);
			this.AddLabel(304, 516, 1160, @"START");
            this.AddLabel(266, 70, 27, @"Kryss");
            this.AddLabel(266, 99, 27, @"WarFork");
            this.AddLabel(266, 154, 27, @"Dagger");
            this.AddLabel(266, 126, 27, @"ShortSpear");
            this.AddLabel(266, 181, 27, @"Cleaver");
            this.AddLabel(266, 208, 27, @"Wand");
            this.AddLabel(266, 234, 27, @"Nunchaku");
            this.AddLabel(266, 263, 27, @"Tekagi");
            this.AddLabel(266, 289, 27, @"Tetsubo");
            this.AddLabel(266, 315, 27, @"Bokuto");
            this.AddLabel(266, 342, 27, @"AssassinSpike");
            this.AddLabel(266, 368, 27, @"LeafBlade");
            this.AddLabel(266, 394, 27, @"RuneBlade");
            this.AddLabel(266, 421, 27, @"ElvenMachete");
            this.AddLabel(266, 448, 27, @"QrnateAxe");
            this.AddLabel(266, 475, 27, @"BoneHavester");
            this.AddLabel(491, 72, 27, @"Bow");
            this.AddLabel(491, 100, 27, @"RadiantScimitar");
            this.AddLabel(491, 127, 27, @"Daisho");
            this.AddLabel(492, 181, 27, @"No-Dachi");
            this.AddLabel(492, 154, 27, @"Sai");
            this.AddLabel(492, 207, 27, @"Scimitar");
            this.AddLabel(492, 233, 27, @"ThinLongSword");
            this.AddLabel(492, 258, 27, @"Katana");
            this.AddLabel(492, 285, 27, @"Mace");
            this.AddLabel(492, 315, 27, @"Spear");
            this.AddLabel(492, 342, 27, @"Wakizashi");
            this.AddLabel(492, 365, 27, @"Tessen");
            this.AddLabel(492, 394, 27, @"Kama");
            this.AddLabel(492, 474, 27, @"ButcherKnife");
            this.AddLabel(492, 447, 27, @"Axe");
            this.AddLabel(492, 501, 27, @"WarMace");
            this.AddLabel(492, 527, 27, @"Maul");
            this.AddLabel(492, 553, 27, @"WarHammer");
            this.AddLabel(492, 420, 27, @"Cutlass");
			this.AddLabel(260, 38, 37, ServerList.ServerName.ToString());
            this.AddItem(387, 71, 5121);//Kryss*
            this.AddItem(388, 96, 5125);//WarFork*
            this.AddItem(386, 126, 5123);//ShortSpear*
            this.AddItem(388, 155, 3922);//Dagger*
            this.AddItem(382, 174, 3779);//Cleaver*
            this.AddItem(386, 193, 3570);//Wand*
            this.AddItem(388, 229, 10233);//Nunchaku*
            this.AddItem(389, 259, 10230);//Tekagi*
            this.AddItem(389, 291, 10225);//Tetsubo*
            this.AddItem(391, 314, 10227);//Bokuto*
            this.AddItem(388, 341, 11553);//AssassinSpike*
            this.AddItem(393, 365, 11554);//LeafBlade*
            this.AddItem(390, 387, 11558);//RuneBlade*
            this.AddItem(389, 411, 11561);//ElvenMachete*
            this.AddItem(393, 443, 11560);//QrnateAxe*
            this.AddItem(386, 468, 9915);//BoneHavester*
            this.AddItem(579, 67, 5042);//Bow*
            this.AddItem(570, 85, 11559);//RadiantScimitar*
            this.AddItem(564, 110, 10228);//Daisho*
            this.AddItem(551, 151, 10234);//Sai*
            this.AddItem(566, 165, 10221);//No-Dachi*
            this.AddItem(570, 204, 5046);//Scimitar*
            this.AddItem(572, 230, 5048);//ThinLongSword*
            this.AddItem(572, 262, 5119);//Katana*
            this.AddItem(571, 289, 3932);//Mace*
            this.AddItem(567, 292, 3938);//Spear*
            this.AddItem(574, 335, 10223);//Wakizashi*
            this.AddItem(577, 364, 10222);//Tessen*
            this.AddItem(572, 393, 10232);//Kama*
            this.AddItem(576, 421, 5185);//Cutlass*
            this.AddItem(576, 441, 3913);//Axe*
            this.AddItem(568, 470, 5110);//ButcherKnife*
            this.AddItem(563, 498, 5127);//WarMace*
            this.AddItem(567, 520, 5179);//Maul*
            this.AddItem(575, 540, 5177);//WarHammer*
			this.AddImage(162, 1, 10400);
			this.AddImage(162, 200, 10401);
			this.AddImage(162, 421, 10402);
		}
		
		public void AddBlackAlpha( int x, int y, int width, int height )
		{
			AddImageTiled( x, y, width, height, 9354 );
			//AddAlphaRegion( x, y, width, height );
		}
		
		public enum Buttons
		{
		}
		
		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;
			int m_ItemID;
			
			switch( info.ButtonID )
			{
				case 1:
				{
                    if (info.IsSwitched(50))//Kryss
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 5121;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(51))//WarFork
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 5125;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(52))//ShortSpear
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 5123;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(53))//Dagger
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 3922;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(54))//Cleaver
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 3779;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(55))//Wand
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 3570;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(56))//Nunchaku
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 10233;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(57))//Tekagi
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 10230;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(58))//Tetsubo
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 10225;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(59))//Bokuto
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 10227;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(60))//AssassinSpike
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 11553;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(61))//LeafBlade
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 11554;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(62))//RuneBlade
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 11558;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(63))//ElvenMachete
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 11561;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(64))//QrnateAxe
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 11560;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(65))//BoneHavester
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 9915;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(66))//Bow
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 5042;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(67))//RadiantScimitar
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 11559;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(68))//Daisho
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 10228;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(69))//Sai
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 10234;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(70))//No-Dachi
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 10221;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(71))//Scimitar
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 5046;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(72))//ThinLongSword
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 5048;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(73))//Katana
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 5119;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(74))//Mace
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 3932;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(75))//Spear
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 3938;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(76))//Wakizashi
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 10223;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(77))//Tessen
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 10222;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(78))//Kama
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 10232;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(79))//Cutlass
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 5185;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(80))//Axe
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 3913;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(81))//ButcherKnife
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 5110;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(82))//WarMace
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 5127;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(83))//Maul
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 5179;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

                    else if (info.IsSwitched(84))//WarHammer
					{
						if( info.Switches.Length == 1 )
						{
                            m_ItemID = 5177;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}

						
					
					else
					{
						m.SendMessage( 38,"You cant do this");
					}
					break;
				}
				
				case 0:
				{
					m.SendMessage( 38,"Cancelled" ); 
					break;
				}
			}
		}
		
		public class ItemIDTarget : Target
		{
			int m_ItemID;
			
			public ItemIDTarget( int itemid ) : base( -1, true, TargetFlags.None )
			{
				m_ItemID = itemid;
			}	
		
			protected override void OnTarget( Mobile from, object targeted ) // Override the protected OnTarget() for our feature
			{
                Item a = from.Backpack.FindItemByType(typeof(WeaponTypeDeed));
			
				if( targeted is BaseWeapon )
				{
					if( a != null )
					{
						Item item = (Item)targeted;
					
							if( item.RootParent == from ) // Make sure its in their pack or they are wearing it
							{
								item.ItemID = m_ItemID;
								a.Delete();
								from.SendMessage( "You have changed the weapon type" ); 
							}
						
							else
							{
								from.SendMessage( 38,"It should be in your backpack");
							}
					}
					
					else
					{
						from.SendMessage( 38," You dont have a weapon type deed in your backpack ");
                        from.CloseGump(typeof(WeaponIDDeedGump));
					}
				}
			
				else
				{
					from.SendMessage( 38,"You can change only weapons !");
				}
			}
		}
	}	
}
