using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Items
{	
	public class TravelBook : Item 
	{
		[Constructable]
		public TravelBook() : base( 0x2D50 )
		{
			Movable = true;
			Weight = 0;
                        Hue = 96;
			Name = "Travel Book";
			LootType = LootType.Blessed;			
		}		

		public override void OnDoubleClick( Mobile from )
		{
			
		from.SendGump( new TravelBookGump( (PlayerMobile)from, this ) );
		}



		public TravelBook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
		}
	}
}

namespace Server.Gumps
{
	
	public class TravelBookGump : Gump
	{
      	   
		public TravelBookGump( PlayerMobile from, Item item ): base( 0, 0 )
		{;
			
                        this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddImage(18, 20, 500);
			this.AddLabel(102, 57, 0, @"Trammel");
			this.AddButton(87, 61, 1209, 1209, (int)Buttons.b1, GumpButtonType.Page, 1);
			this.AddButton(86, 88, 1209, 1209, (int)Buttons.b2, GumpButtonType.Page, 2);
			this.AddButton(86, 115, 1209, 1209, (int)Buttons.b3, GumpButtonType.Page, 3);
			this.AddButton(86, 140, 1209, 1209, (int)Buttons.b4, GumpButtonType.Page, 4);
			this.AddLabel(104, 83, 0, @"Ilshenar");
			this.AddLabel(106, 110, 0, @"Malas");
			this.AddLabel(106, 136, 0, @"Tokuno");
			this.AddLabel(71, 219, 0, @"Travel Book");
			this.AddLabel(107, 159, 0, @"Dungeons");
			this.AddButton(86, 162, 1209, 1209, (int)Buttons.b5, GumpButtonType.Page, 5);
			this.AddPage(1);
			this.AddButton(271, 33, 1209, 1209, (int)Buttons.t1, GumpButtonType.Reply, 0);
			this.AddButton(271, 53, 1209, 1209, (int)Buttons.t2, GumpButtonType.Reply, 0);
			this.AddButton(271, 72, 1209, 1209, (int)Buttons.t3, GumpButtonType.Reply, 0);
			this.AddButton(271, 92, 1209, 1209, (int)Buttons.t4, GumpButtonType.Reply, 0);
			this.AddButton(270, 130, 1209, 1209, (int)Buttons.t6, GumpButtonType.Reply, 0);
			this.AddButton(270, 150, 1209, 1209, (int)Buttons.t7, GumpButtonType.Reply, 0);
			this.AddLabel(293, 30, 0, @"Britain");
			this.AddLabel(293, 50, 0, @"Bucaneer's Den");
			this.AddLabel(293, 90, 0, @"Delucia");
			this.AddLabel(293, 70, 0, @"Cove");
			this.AddLabel(293, 129, 0, @"Jhelom");
			this.AddLabel(293, 147, 0, @"Minoc");
			this.AddButton(270, 188, 1209, 1209, (int)Buttons.t9, GumpButtonType.Reply, 0);
			this.AddButton(270, 169, 1209, 1209, (int)Buttons.t8, GumpButtonType.Reply, 0);
			this.AddLabel(293, 165, 0, @"Moonglow");
			this.AddLabel(294, 184, 0, @"Nujel'm");
			this.AddButton(271, 112, 1209, 1209, (int)Buttons.t5, GumpButtonType.Reply, 0);
			this.AddLabel(295, 108, 0, @"Haven");
			this.AddButton(270, 216, 1209, 1209, (int)Buttons.tnp, GumpButtonType.Page, 7);
			this.AddLabel(292, 213, 0, @"Next Page");
			this.AddPage(2);
			this.AddButton(251, 49, 1209, 1209, (int)Buttons.i1, GumpButtonType.Reply, 0);
			this.AddButton(251, 69, 1209, 1209, (int)Buttons.i2, GumpButtonType.Reply, 0);
			this.AddButton(251, 88, 1209, 1209, (int)Buttons.i3, GumpButtonType.Reply, 0);
			this.AddButton(251, 108, 1209, 1209, (int)Buttons.i4, GumpButtonType.Reply, 0);
			this.AddButton(250, 146, 1209, 1209, (int)Buttons.i6, GumpButtonType.Reply, 0);
			this.AddButton(250, 166, 1209, 1209, (int)Buttons.i7, GumpButtonType.Reply, 0);
			this.AddLabel(273, 46, 0, @"Alecandretta's Bowl");
			this.AddLabel(273, 66, 0, @"Ancient Citadel");
			this.AddLabel(273, 106, 0, @"Lakeshire");
			this.AddLabel(273, 86, 0, @"Gargoyle City");
			this.AddLabel(273, 145, 0, @"Montor");
			this.AddLabel(273, 163, 0, @"Req Volon");
			this.AddButton(250, 204, 1209, 1209, (int)Buttons.i9, GumpButtonType.Reply, 0);
			this.AddButton(250, 185, 1209, 1209, (int)Buttons.i8, GumpButtonType.Reply, 0);
			this.AddLabel(273, 181, 0, @"Savage Camp");
			this.AddLabel(274, 200, 0, @"Terort Skitas");
			this.AddButton(251, 128, 1209, 1209, (int)Buttons.i5, GumpButtonType.Reply, 0);
			this.AddLabel(275, 124, 0, @"Mistas");
			this.AddPage(3);
			this.AddButton(287, 92, 1209, 1209, (int)Buttons.m1, GumpButtonType.Reply, 0);
			this.AddButton(287, 112, 1209, 1209, (int)Buttons.m2, GumpButtonType.Reply, 0);
			this.AddLabel(309, 89, 0, @"Luna");
			this.AddLabel(309, 109, 0, @"Umbra");
			this.AddPage(4);
			this.AddButton(258, 94, 1209, 1209, (int)Buttons.tt1, GumpButtonType.Reply, 0);
			this.AddButton(258, 111, 1209, 1209, (int)Buttons.tt2, GumpButtonType.Reply, 0);
			this.AddLabel(280, 91, 0, @"Zento");
			this.AddLabel(280, 107, 0, @"Bushido Dojo");
			this.AddButton(258, 127, 1209, 1209, (int)Buttons.tt3, GumpButtonType.Reply, 0);
			this.AddLabel(281, 122, 0, @"Fan Dancer's Dojo");
			this.AddPage(5);
			this.AddButton(265, 31, 1209, 1209, (int)Buttons.d1, GumpButtonType.Reply, 0);
			this.AddButton(265, 51, 1209, 1209, (int)Buttons.d2, GumpButtonType.Reply, 0);
			this.AddButton(265, 70, 1209, 1209, (int)Buttons.d3, GumpButtonType.Reply, 0);
			this.AddButton(265, 90, 1209, 1209, (int)Buttons.d4, GumpButtonType.Reply, 0);
			this.AddButton(265, 128, 1209, 1209, (int)Buttons.d6, GumpButtonType.Reply, 0);
			this.AddButton(264, 148, 1209, 1209, (int)Buttons.d7, GumpButtonType.Reply, 0);
			this.AddLabel(287, 28, 0, @"Anka");
			this.AddLabel(287, 48, 0, @"Blood");
			this.AddLabel(287, 88, 0, @"Covetous");
			this.AddLabel(287, 68, 0, @"Britain Passage");
			this.AddLabel(287, 124, 0, @"Despise");
			this.AddLabel(287, 145, 0, @"Destard");
			this.AddButton(265, 110, 1209, 1209, (int)Buttons.d5, GumpButtonType.Reply, 0);
			this.AddLabel(288, 106, 0, @"Deceit");
			this.AddButton(265, 164, 1209, 1209, (int)Buttons.d8, GumpButtonType.Reply, 0);
			this.AddButton(265, 184, 1209, 1209, (int)Buttons.d9, GumpButtonType.Reply, 0);
			this.AddButton(265, 203, 1209, 1209, (int)Buttons.d10, GumpButtonType.Reply, 0);
			this.AddLabel(287, 161, 0, @"Doom");
			this.AddLabel(287, 181, 0, @"Exodus");
			this.AddLabel(287, 201, 0, @"Fire");
			this.AddButton(264, 223, 1209, 1209, (int)Buttons.dnp, GumpButtonType.Page, 6);
			this.AddLabel(286, 220, 0, @"Next Page");
			this.AddPage(6);
			this.AddButton(259, 100, 1209, 1209, (int)Buttons.d15, GumpButtonType.Reply, 0);
			this.AddButton(259, 120, 1209, 1209, (int)Buttons.d16, GumpButtonType.Reply, 0);
			this.AddButton(259, 139, 1209, 1209, (int)Buttons.d17, GumpButtonType.Reply, 0);
			this.AddButton(259, 159, 1209, 1209, (int)Buttons.d18, GumpButtonType.Reply, 0);
			this.AddButton(259, 197, 1209, 1209, (int)Buttons.d20, GumpButtonType.Reply, 0);
			this.AddButton(258, 217, 1209, 1209, (int)Buttons.d21, GumpButtonType.Reply, 0);
			this.AddLabel(281, 97, 0, @"Shame");
			this.AddLabel(280, 117, 0, @"Sorcerrs");
			this.AddLabel(281, 157, 0, @"Terathen Keep");
			this.AddLabel(281, 137, 0, @"Spectre");
			this.AddLabel(281, 193, 0, @"Wisp");
			this.AddLabel(281, 214, 0, @"Wroung");
			this.AddButton(259, 179, 1209, 1209, (int)Buttons.d19, GumpButtonType.Reply, 0);
			this.AddLabel(282, 175, 0, @"Trinsic Passage");
			this.AddButton(258, 235, 1209, 1209, (int)Buttons.dlp, GumpButtonType.Page, 5);
			this.AddLabel(281, 234, 0, @"Last Page");
			this.AddButton(260, 24, 1209, 1209, (int)Buttons.d11, GumpButtonType.Reply, 0);
			this.AddButton(259, 62, 1209, 1209, (int)Buttons.d13, GumpButtonType.Reply, 0);
			this.AddButton(259, 82, 1209, 1209, (int)Buttons.d14, GumpButtonType.Reply, 0);
			this.AddLabel(282, 22, 0, @"Graveyards");
			this.AddLabel(282, 61, 0, @"Ice");
			this.AddLabel(282, 79, 0, @"Rock");
			this.AddButton(260, 44, 1209, 1209, (int)Buttons.d12, GumpButtonType.Reply, 0);
			this.AddLabel(284, 40, 0, @"Hythloth");
			this.AddPage(7);
			this.AddButton(278, 55, 1209, 1209, (int)Buttons.t10, GumpButtonType.Reply, 0);
			this.AddButton(278, 73, 1209, 1209, (int)Buttons.t11, GumpButtonType.Reply, 0);
			this.AddButton(278, 92, 1209, 1209, (int)Buttons.t12, GumpButtonType.Reply, 0);
			this.AddButton(278, 110, 1209, 1209, (int)Buttons.t13, GumpButtonType.Reply, 0);
			this.AddButton(278, 127, 1209, 1209, (int)Buttons.t14, GumpButtonType.Reply, 0);
			this.AddLabel(300, 70, 0, @"Papua");
			this.AddLabel(301, 51, 0, @"Occlo");
			this.AddLabel(300, 89, 0, @"Serpent's Hold");
			this.AddLabel(300, 107, 0, @"Skara Brea");
			this.AddLabel(300, 123, 0, @"Trinsic");
			this.AddButton(278, 144, 1209, 1209, (int)Buttons.t15, GumpButtonType.Reply, 0);
			this.AddButton(278, 162, 1209, 1209, (int)Buttons.t16, GumpButtonType.Reply, 0);
			this.AddLabel(300, 140, 0, @"Vesper");
			this.AddLabel(300, 158, 0, @"Wind");
			this.AddButton(278, 181, 1209, 1209, (int)Buttons.t17, GumpButtonType.Reply, 0);
			this.AddLabel(300, 177, 0, @"Yew");
			this.AddButton(278, 210, 1209, 1209, (int)Buttons.tlp, GumpButtonType.Page, 1);
			this.AddLabel(303, 207, 0, @"Last Page");

		}
		
		public enum Buttons
		{
			b1,
			b2,
			b3,
			b4,
			b5,
			t1,
			t2,
			t3,
			t4,
			t6,
			t7,
			t9,
			t8,
			t5,
			tnp,
			i1,
			i2,
			i3,
			i4,
			i6,
			i7,
			i9,
			i8,
			i5,
			m1,
			m2,
			tt1,
			tt2,
			tt3,
			d1,
			d2,
			d3,
			d4,
			d6,
			d7,
			d5,
			d8,
			d9,
			d10,
			dnp,
			d15,
			d16,
			d17,
			d18,
			d20,
			d21,
			d19,
			dlp,
			d11,
			d13,
			d14,
			d12,
			t10,
			t11,
			t12,
			t13,
			t14,
			t15,
			t16,
			t17,
			tlp,
		}
            public override void OnResponse( NetState sender, RelayInfo info )
		{
                            Mobile m = sender.Mobile; 
			if ( info.ButtonID == (int)Buttons.t1)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(1422,1697,0);
			}
			if ( info.ButtonID == (int)Buttons.t2)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(2723,2189,0);
			}
			if ( info.ButtonID == (int)Buttons.t3)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(2234,1198,0);
			}
			if ( info.ButtonID == (int)Buttons.t4)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(2267,1211,0);
			}
			if ( info.ButtonID == (int)Buttons.t5)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(3686,2519,0);
			}
			if ( info.ButtonID == (int)Buttons.t6)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(1324,3779,0);
			}
			if ( info.ButtonID == (int)Buttons.t7)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(2517,553,0);
			}
			if ( info.ButtonID == (int)Buttons.t8)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(4471,1178,0);
			}
			if ( info.ButtonID == (int)Buttons.t9)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(3770,1315,0);
			}
			if ( info.ButtonID == (int)Buttons.t10)
			{
				m.Map = Map.Felucca;
                                m.Location = new Point3D(3686,2519,0);
			}
			if ( info.ButtonID == (int)Buttons.t11)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(5675,3144,12);
			}
			if ( info.ButtonID == (int)Buttons.t12)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(2875,3471,15);
			}
			if ( info.ButtonID == (int)Buttons.t13)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(591,2155,0);
			}
			if ( info.ButtonID == (int)Buttons.t14)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(1820,2822,0);
			}
			if ( info.ButtonID == (int)Buttons.t15)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(2899,676,0);
			}
			if ( info.ButtonID == (int)Buttons.t16)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(5172,242,15);
			}
			if ( info.ButtonID == (int)Buttons.t17)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(546,991,0);
			}

                       if ( info.ButtonID == (int)Buttons.i1)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(1396,432,-17);
			}
			if ( info.ButtonID == (int)Buttons.i2)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(1517,568,-13);
			}
			if ( info.ButtonID == (int)Buttons.i3)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(852,602,-40);
			}
		   if ( info.ButtonID == (int)Buttons.i4)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(1203,1124,-25);
			}
			  if ( info.ButtonID == (int)Buttons.i5)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(820,1073,-30);
			}
		 if ( info.ButtonID == (int)Buttons.i6)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(1643,310,48);
			}
			if ( info.ButtonID == (int)Buttons.i7)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(1362,1073,-13);
			}
		if ( info.ButtonID == (int)Buttons.i8)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(1251,743,-80);
			}
			    if ( info.ButtonID == (int)Buttons.i9)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(567,437,21);
			}
			
					if ( info.ButtonID == (int)Buttons.m1)
			{
				m.Map = Map.Malas;
                                m.Location = new Point3D(998,519,-50);
			}
			    if ( info.ButtonID == (int)Buttons.m2)
			{
				m.Map = Map.Malas;
                                m.Location = new Point3D(2057,1343,-85);
			}

			    if ( info.ButtonID == (int)Buttons.tt1)
			{
				m.Map = Map.Tokuno;
                                m.Location = new Point3D(707,1237,25);
			}
			    if ( info.ButtonID == (int)Buttons.tt2)
			{
				m.Map = Map.Tokuno;
                                m.Location = new Point3D(319,442,32);
			}
			    if ( info.ButtonID == (int)Buttons.tt3)
			{
				m.Map = Map.Tokuno;
                                m.Location = new Point3D(977,227,23);
			}
                        

		        if ( info.ButtonID == (int)Buttons.d1)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(576,1150,-100);
			}
			    if ( info.ButtonID == (int)Buttons.d2)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(1747,1228,-1);
			}
			
			if ( info.ButtonID == (int)Buttons.d3)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(6032,1500,25);
			}
			    if ( info.ButtonID == (int)Buttons.d4)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(2499,922,0);
			}

			    if ( info.ButtonID == (int)Buttons.d5)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(4111,434,5);
			}
			    if ( info.ButtonID == (int)Buttons.d6)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(5584,631,30);
			}
			    if ( info.ButtonID == (int)Buttons.d7)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(1176,2639,0);
			}
	               	    if ( info.ButtonID == (int)Buttons.d8)
			{
				m.Map = Map.Malas;
                                m.Location = new Point3D(2367,1268,-85);
			}
			    if ( info.ButtonID == (int)Buttons.d9)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(835,777,-80);
			}
			
			if ( info.ButtonID == (int)Buttons.d10)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(5761,2907,15);
			}
			    if ( info.ButtonID == (int)Buttons.d11)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(5222,3661,3);
			}

			    if ( info.ButtonID == (int)Buttons.d12)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(4722,3825,0);
			}
			    if ( info.ButtonID == (int)Buttons.d13)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(5209,2322,33);
			}
			    if ( info.ButtonID == (int)Buttons.d14)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(1788,573,71);
			}
                            
		           if ( info.ButtonID == (int)Buttons.d15)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(514,1561,0);
			}
			    if ( info.ButtonID == (int)Buttons.d16)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(548,462,-53);
			}
			
			if ( info.ButtonID == (int)Buttons.d17)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(1363,1033,-8);
			}
			    if ( info.ButtonID == (int)Buttons.d18)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(5499,3224,0);
			}

			    if ( info.ButtonID == (int)Buttons.d19)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(5154,4063,37);
			}
			    if ( info.ButtonID == (int)Buttons.d20)
			{
				m.Map = Map.Ilshenar;
                                m.Location = new Point3D(651,1302,-58);
			}
			    if ( info.ButtonID == (int)Buttons.d21)
			{
				m.Map = Map.Trammel;
                                m.Location = new Point3D(2044,238,10);
			}



}

	}
}