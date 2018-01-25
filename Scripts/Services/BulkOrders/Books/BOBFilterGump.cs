using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Engines.BulkOrders
{
    public class BOBFilterGump : Gump
    {
        private static int[,] m_MaterialFilters = new int[,]
			{
				{ 1044067,  1 }, // Blacksmithy
				{ 1062226,  3 }, // Iron
				{ 1018332,  4 }, // Dull Copper
				{ 1018333,  5 }, // Shadow Iron
				{ 1018334,  6 }, // Copper
				{ 1018335,  7 }, // Bronze

				{       0,  0 }, // --Blank--
				{ 1018336,  8 }, // Golden
				{ 1018337,  9 }, // Agapite
				{ 1018338, 10 }, // Verite
				{ 1018339, 11 }, // Valorite
				{       0,  0 }, // --Blank--

				{ 1044094,  2 }, // Tailoring
				{ 1044286, 12 }, // Cloth
				{ 1062235, 13 }, // Leather
				{ 1062236, 14 }, // Spined
				{ 1062237, 15 }, // Horned
				{ 1062238, 16 }  // Barbed
			};

        private static readonly int[,] m_MaterialFiltersNew = new int[,]
        {
            { 1044067, 1 }, // Blacksmithy
            { 1062226, 9 }, // Iron
            { 1018332, 10 }, // Dull Copper
            { 1018333, 11 }, // Shadow Iron
            { 1018334, 12 }, // Copper
            { 1018335, 13 }, // Bronze

            { 0, 0 }, // --Blank--
            { 1018336, 14 }, // Golden
            { 1018337, 15 }, // Agapite
            { 1018338, 16 }, // Verite
            { 1018339, 17 }, // Valorite
            { -1, 18 }, // Blaze

            { 0, 0 }, // --Blank--
            { -1, 19 }, // Ice
            { -1, 20 }, // Toxic
            { -1, 21 }, // Electrum
            { -1, 22 }, // Platinum
            { 0, 0 }, // --Blank--

            { 1044094, 2 }, // Tailoring
            { 1044286, 23 }, // Cloth
            { 1062235, 24 }, // Leather
            { 1062236, 25 }, // Spined
            { 1062237, 26 }, // Horned
            { 1062238, 27 }, // Barbed

            { 0, 0 }, // --Blank--
            { -1, 28 }, // Polar
            { -1, 29 }, // Synthetic
            { -1, 30 }, // Blaze
            { -1, 31 }, // Daemonic
            { -1, 32 }, // Shadow

            { 0, 0 }, // --Blank--
            { -1, 33 }, // Frost
            { -1, 34 }, // Ethereal
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--


            { 1044097, 3 }, // Tinkering
            { 1062226, 35 }, // Iron
            { 1018332, 36 }, // Dull Copper
            { 1018333, 37 }, // Shadow Iron
            { 1018334, 38 }, // Copper
            { 1018335, 39 }, // Bronze

            { 0, 0 }, // --Blank--
            { 1018336, 40 }, // Golden
            { 1018337, 41 }, // Agapite
            { 1018338, 42 }, // Verite
            { 1018339, 43 }, // Valorite
            { -1, 44 }, // Blaze

            { 0, 0 }, // --Blank--
            { -1, 45 }, // Ice
            { -1, 46 }, // Toxic
            { -1, 47 }, // Electrum
            { -1, 48 }, // Platinum
            { 0, 0 }, // --Blank--

            { 1044071, 4 }, // Carpentry
            { 1079435, 49 }, // Wood
            { 1071428, 50 }, // Oak
            { 1071429, 51 }, // Ash
            { 1071430, 52 }, // Yew
            { 0, 0 }, // --Blank--

            { 0, 0 }, // --Blank--
            { 1071431, 53 }, // Bloodwood
            { 1071432, 54 }, // Heartwood
            { 1071433, 55 }, // Frostwood
            { -1, 56 }, // Ebony
            { -1, 57 }, // Bamboo

            { 0, 0 }, // --Blank--
            { -1, 58 }, // PurpleHeart
            { -1, 59 }, // Redwood
            { -1, 60 }, // Petrified
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--

            { 1044068, 5 }, // Fletching
            { 1079435, 61 }, // Wood
            { 1071428, 62 }, // Oak
            { 1071429, 63 }, // Ash
            { 1071430, 64 }, // Yew
            { 0, 0 }, // --Blank--

            { 0, 0 }, // --Blank--
            { 1071431, 65 }, // Bloodwood
            { 1071432, 66 }, // Heartwood
            { 1071433, 67 }, // Frostwood
            { -1, 68 }, // Ebony
            { -1, 69 }, // Bamboo

            { 0, 0 }, // --Blank--
            { -1, 70 }, // PurpleHeart
            { -1, 71 }, // Redwood
            { -1, 72 }, // Petrified
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--

            { 1044060, 6 }, // Alchemy
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--

            { 1044083, 7 }, // Inscription
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--

            { 1044073, 8 }, // Cooking
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--
            { 0, 0 }, // --Blank--
        };

        private static readonly int[,] m_TypeFilters = new int[,]
        {
            { 1062229, 0 }, // All
            { 1062224, 1 }, // Small
            { 1062225, 2 }// Large
        };

        private static readonly int[,] m_QualityFilters = new int[,]
        {
            { 1062229, 0 }, // All
            { 1011542, 1 }, // Normal
            { 1060636, 2 }// Exceptional
        };

        private static readonly int[,] m_AmountFilters = new int[,]
        {
            { 1062229, 0 }, // All
            { 1049706, 1 }, // 10
            { 1016007, 2 }, // 15
            { 1062239, 3 }// 20
        };

        private static readonly int[][,] m_Filters = new int[][,]
        {
            m_TypeFilters,
            m_QualityFilters,
            m_MaterialFilters,
            m_AmountFilters
        };

        private static readonly int[][,] m_FiltersNew = new int[][,]
        {
            m_TypeFilters,
            m_QualityFilters,
            m_MaterialFiltersNew,
            m_AmountFilters
        };

        private static readonly int[] m_XOffsets_Type = new int[] { 0, 75, 170 };
        private static readonly int[] m_XOffsets_Quality = new int[] { 0, 75, 170 };
        private static readonly int[] m_XOffsets_Amount = new int[] { 0, 75, 180, 275 };
        private static readonly int[] m_XOffsets_Material = new int[] { 0, 108, 212, 307, 392, 487 };
        private static readonly int[] m_XWidths_Small = new int[] { 50, 50, 70, 50 };
        private static readonly int[] m_XWidths_Large = new int[] { 80, 60, 60, 60, 60, 60 };

        private const int LabelColor = 0x7FFF;
        private readonly PlayerMobile m_From;
        private readonly BulkOrderBook m_Book;

        public BOBFilterGump(PlayerMobile from, BulkOrderBook book)
            : base(12, 24)
        {
            from.CloseGump(typeof(BOBGump));
            from.CloseGump(typeof(BOBFilterGump));

            m_From = from;
            m_Book = book;

            BOBFilter f = (from.UseOwnFilter ? from.BOBFilter : book.Filter);

            AddPage(0);

            AddBackground(10, 10, 600, 895, 5054); //695

            AddImageTiled(18, 20, 583, 876, 2624); //676
            AddAlphaRegion(18, 20, 583, 876); //676

            AddImage(5, 5, 10460);
            AddImage(585, 5, 10460);
            AddImage(5, 890, 10460); //690
            AddImage(585, 890, 10460); //690

            AddHtmlLocalized(270, 32, 200, 32, 1062223, LabelColor, false, false); // Filter Preference

            AddHtmlLocalized(26, 64, 120, 32, 1062228, LabelColor, false, false); // Bulk Order Type
            AddFilterList(25, 96, m_XOffsets_Type, 40, m_TypeFilters, m_XWidths_Small, f.Type, 0);

            AddHtmlLocalized(320, 64, 50, 32, 1062215, LabelColor, false, false); // Quality
            AddFilterList(320, 96, m_XOffsets_Quality, 40, m_QualityFilters, m_XWidths_Small, f.Quality, 1);

            AddHtmlLocalized(26, 130, 120, 32, 1062232, LabelColor, false, false); // Material Type
            AddFilterList(25, 162, m_XOffsets_Material, 35, BulkOrderSystem.NewSystemEnabled ? m_MaterialFiltersNew : m_MaterialFilters, m_XWidths_Large, f.Material, 2);

            AddHtmlLocalized(26, 808, 120, 32, 1062217, LabelColor, false, false); // Amount //608
            AddFilterList(25, 840, m_XOffsets_Amount, 40, m_AmountFilters, m_XWidths_Small, f.Quantity, 3); //640

            AddHtmlLocalized(75, 870, 120, 32, 1062477, (from.UseOwnFilter ? LabelColor : 16927), false, false); // Set Book Filter //670
            AddButton(40, 870, 4005, 4007, 1, GumpButtonType.Reply, 0); //670

			AddHtmlLocalized(235, 870, 120, 32, 1062478, (from.UseOwnFilter ? 16927 : LabelColor), false, false); // Set Your Filter //670
			AddButton(200, 870, 4005, 4007, 2, GumpButtonType.Reply, 0); //670

			AddHtmlLocalized(405, 870, 120, 32, 1062231, LabelColor, false, false); // Clear Filter //670
			AddButton(370, 870, 4005, 4007, 3, GumpButtonType.Reply, 0); //670

			AddHtmlLocalized(540, 870, 50, 32, 1011046, LabelColor, false, false); // APPLY //670
			AddButton(505, 870, 4017, 4018, 0, GumpButtonType.Reply, 0); //670
		}

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            BOBFilter f = (m_From.UseOwnFilter ? m_From.BOBFilter : m_Book.Filter);

            int index = info.ButtonID;

            switch ( index )
            {
                case 0: // Apply
                    {
                        m_From.SendGump(new BOBGump(m_From, m_Book));

                        break;
                    }
                case 1: // Set Book Filter
                    {
                        m_From.UseOwnFilter = false;
                        m_From.SendGump(new BOBFilterGump(m_From, m_Book));

                        break;
                    }
                case 2: // Set Your Filter
                    {
                        m_From.UseOwnFilter = true;
                        m_From.SendGump(new BOBFilterGump(m_From, m_Book));

                        break;
                    }
                case 3: // Clear Filter
                    {
                        f.Clear();
                        m_From.SendGump(new BOBFilterGump(m_From, m_Book));

                        break;
                    }
                default:
                    {
                        index -= 4;

                        int type = index % 4;
                        index /= 4;

                        int[][,] filter = BulkOrderSystem.NewSystemEnabled ? m_FiltersNew : m_Filters;

                        if (type >= 0 && type < filter.Length)
                        {
                            int[,] filters = filter[type];

                            if (index >= 0 && index < filters.GetLength(0))
                            {
                                if (filters[index, 0] == 0)
                                    break;

                                switch ( type )
                                {
                                    case 0:
                                        f.Type = filters[index, 1];
                                        break;
                                    case 1:
                                        f.Quality = filters[index, 1];
                                        break;
                                    case 2:
                                        f.Material = filters[index, 1];
                                        break;
                                    case 3:
                                        f.Quantity = filters[index, 1];
                                        break;
                                }

                                m_From.SendGump(new BOBFilterGump(m_From, m_Book));
                            }
                        }

                        break;
                    }
            }
        }

        private void AddFilterList(int x, int y, int[] xOffsets, int yOffset, int[,] filters, int[] xWidths, int filterValue, int filterIndex)
        {
            for (int i = 0; i < filters.GetLength(0); ++i)
            {
                int number = filters[i, 0];

                if (number == 0)
                    continue;

                bool isSelected = (filters[i, 1] == filterValue);

                if (!isSelected && (i % xOffsets.Length) == 0)
                    isSelected = (filterValue == 0);

                if (number == -1)
                {
                    int materialValue = filters[i, 1];
                    string text = GetMaterialName(materialValue);
                    AddHtml(x + 35 + xOffsets[i % xOffsets.Length], y + ((i / xOffsets.Length) * yOffset), xWidths[i % xOffsets.Length], 32,
                            "<BASEFONT COLOR=" + (isSelected ? 16927 : LabelColor) + ">" + text + "</FONT>", false, false);
                }
                else
                {
                    AddHtmlLocalized(x + 35 + xOffsets[i % xOffsets.Length], y + ((i / xOffsets.Length) * yOffset), xWidths[i % xOffsets.Length], 32, number, isSelected ? 16927 : LabelColor, false, false);
                }
                AddButton(x + xOffsets[i % xOffsets.Length], y + ((i / xOffsets.Length) * yOffset), 4005, 4007, 4 + filterIndex + (i * 4), GumpButtonType.Reply, 0);
            }
        }

        private static String GetMaterialName(int materialValue) {
			switch (materialValue)
			{
				default:
				case 0:
					return "";
				case 18:
					return "Blaze";
				case 19:
					return "Ice";
				case 20:
					return "Toxic";
				case 21:
					return "Electrum";
				case 22:
					return "Platinum";
				case 28:
					return "Polar";
				case 29:
					return "Synthetic";
				case 30:
					return "Blaze";
				case 31:
					return "Daemonic";
				case 32:
					return "Shadow";
				case 33:
					return "Frost";
				case 34:
					return "Ethereal";
				case 44:
					return "Blaze";
				case 45:
					return "Ice";
				case 46:
					return "Toxic";
				case 47:
					return "Electrum";
				case 48:
					return "Platinum";
				case 56:
					return "Ebony";
				case 57:
					return "Bamboo";
				case 58:
					return "PurpleHeart";
				case 59:
					return "Redwood";
				case 60:
					return "Petrified";
				case 68:
					return "Ebony";
				case 69:
					return "Bamboo";
				case 70:
					return "PurpleHeart";
				case 71:
					return "Redwood";
				case 72:
					return "Petrified";
			}
        }
    }
}