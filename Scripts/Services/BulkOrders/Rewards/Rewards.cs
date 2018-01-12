using System;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;

namespace Server.Engines.BulkOrders
{
    public delegate Item ConstructCallback(int type);

    public sealed class RewardType
    {
        private readonly int m_Points;
        private readonly Type[] m_Types;

        public int Points
        {
            get
            {
                return this.m_Points;
            }
        }
        public Type[] Types
        {
            get
            {
                return this.m_Types;
            }
        }

        public RewardType(int points, params Type[] types)
        {
            this.m_Points = points;
            this.m_Types = types;
        }

        public bool Contains(Type type)
        {
            for (int i = 0; i < this.m_Types.Length; ++i)
            {
                if (this.m_Types[i] == type)
                    return true;
            }

            return false;
        }
    }

    public sealed class RewardItem
    {
        private readonly int m_Weight;
        private readonly ConstructCallback m_Constructor;
        private readonly int m_Type;

        public int Weight
        {
            get
            {
                return this.m_Weight;
            }
        }
        public ConstructCallback Constructor
        {
            get
            {
                return this.m_Constructor;
            }
        }
        public int Type
        {
            get
            {
                return this.m_Type;
            }
        }

        public RewardItem(int weight, ConstructCallback constructor)
            : this(weight, constructor, 0)
        {
        }

        public RewardItem(int weight, ConstructCallback constructor, int type)
        {
            this.m_Weight = weight;
            this.m_Constructor = constructor;
            this.m_Type = type;
        }

        public Item Construct()
        {
            try
            {
                return this.m_Constructor(this.m_Type);
            }
            catch
            {
                return null;
            }
        }
    }

    public class BODCollectionItem : CollectionItem
    {
        public ConstructCallback Constructor { get; set; }
        public int RewardType { get; set; }

		public BODCollectionItem(int itemID, string name, int hue, double points, ConstructCallback constructor, int type = 0)
			: this(itemID, 0, name, hue, points, constructor, type)
		{
		}

		public BODCollectionItem(int itemID, int tooltip, int hue, double points, ConstructCallback constructor, int type = 0)
			: this(itemID, tooltip, null, hue, points, constructor, type)
		{
		}

        public BODCollectionItem(int itemID, int tooltip, string name, int hue, double points, ConstructCallback constructor, int type = 0)
            : base(null, itemID, tooltip, name, hue, points, false)
        {
            Constructor = constructor;
            RewardType = type;
        }
    }

    public sealed class RewardGroup
    {
        private readonly int m_Points;
        private readonly RewardItem[] m_Items;

        public int Points
        {
            get
            {
                return this.m_Points;
            }
        }
        public RewardItem[] Items
        {
            get
            {
                return this.m_Items;
            }
        }

        public RewardGroup(int points, params RewardItem[] items)
        {
            this.m_Points = points;
            this.m_Items = items;
        }

        public RewardItem AcquireItem()
        {
            if (this.m_Items.Length == 0)
                return null;
            else if (this.m_Items.Length == 1)
                return this.m_Items[0];

            int totalWeight = 0;

            for (int i = 0; i < this.m_Items.Length; ++i)
                totalWeight += this.m_Items[i].Weight;

            int randomWeight = Utility.Random(totalWeight);

            for (int i = 0; i < this.m_Items.Length; ++i)
            {
                RewardItem item = this.m_Items[i];

                if (randomWeight < item.Weight)
                    return item;

                randomWeight -= item.Weight;
            }

            return null;
        }
    }

    public abstract class RewardCalculator
    {
        private RewardGroup[] m_Groups;

        public RewardGroup[] Groups
        {
            get
            {
                return this.m_Groups;
            }
            set
            {
                this.m_Groups = value;
            }
        }

        public List<CollectionItem> RewardCollection { get; set; }

        public abstract int ComputePoints(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type);

        public abstract int ComputeGold(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type);

        public virtual int ComputeFame(SmallBOD bod)
        {
            int points = this.ComputePoints(bod) / 50;

            return points * points;
        }

        public virtual int ComputeFame(LargeBOD bod)
        {
            int points = this.ComputePoints(bod) / 50;

            return points * points;
        }

        public virtual int ComputePoints(SmallBOD bod)
        {
            return this.ComputePoints(bod.AmountMax, bod.RequireExceptional, bod.Material, 1, bod.Type);
        }

        public virtual int ComputePoints(LargeBOD bod)
        {
            Type type = bod.Entries == null || bod.Entries.Length == 0 ? null : bod.Entries[0].Details.Type;

            return this.ComputePoints(bod.AmountMax, bod.RequireExceptional, bod.Material, bod.Entries.Length, type);
        }

        public virtual int ComputeGold(SmallBOD bod)
        {
            return this.ComputeGold(bod.AmountMax, bod.RequireExceptional, bod.Material, 1, bod.Type);
        }

        public virtual int ComputeGold(LargeBOD bod)
        {
            return this.ComputeGold(bod.AmountMax, bod.RequireExceptional, bod.Material, bod.Entries.Length, bod.Entries[0].Details.Type);
        }

        public virtual RewardGroup LookupRewards(int points)
        {
            for (int i = this.m_Groups.Length - 1; i >= 1; --i)
            {
                RewardGroup group = this.m_Groups[i];

                if (points >= group.Points)
                    return group;
            }

            return this.m_Groups[0];
        }

        public virtual int LookupTypePoints(RewardType[] types, Type type)
        {
            for (int i = 0; i < types.Length; ++i)
            {
                if (type == null || types[i].Contains(type))
                    return types[i].Points;
            }

            return 0;
        }

        protected static Item RewardTitle(int type)
        {
            return new BODRewardTitleDeed(type);
        }

        protected static Item NaturalDye(int type)
        {
            switch (type)
            {
                default:
                case 0: return new SpecialNaturalDye(DyeType.WindAzul);
                case 1: return new SpecialNaturalDye(DyeType.DullRuby);
                case 2: return new SpecialNaturalDye(DyeType.PoppieWhite);
                case 3: return new SpecialNaturalDye(DyeType.WindAzul, true);
                case 4: return new SpecialNaturalDye(DyeType.UmbranViolet, true);
                case 5: return new SpecialNaturalDye(DyeType.ZentoOrchid, true);
                case 6: return new SpecialNaturalDye(DyeType.DullRuby, true);
                case 7: return new SpecialNaturalDye(DyeType.PoppieWhite, true);
                case 8: return new SpecialNaturalDye(DyeType.UmbranViolet);
                case 9: return new SpecialNaturalDye(DyeType.ZentoOrchid);
            }
        }

        protected static Item RockHammer(int type)
        {
            return new RockHammer();
        }

        protected static Item HarvestMap(int type)
        {
            return new HarvestMap((CraftResource)type);
        }

        protected static Item Recipe(int type)
        {
            switch (type)
            {
                case 0: return new RecipeScroll(170);
                case 1: return new RecipeScroll(457);
                case 2: return new RecipeScroll(458);
                case 3: return new RecipeScroll(800);
                case 4: return new RecipeScroll(599);
            }

            return null;
        }

        protected static Item SmeltersTalisman(int type)
        {
            return new SmeltersTalisman((CraftResource)type);
        }

        protected static Item WoodsmansTalisman(int type)
        {
            return new WoodsmansTalisman((CraftResource)type);
        }

        public RewardCalculator()
        {
        }
    }

    #region Smith Rewards
    public sealed class SmithRewardCalculator : RewardCalculator
    {
        public SmithRewardCalculator()
        {
            if (BulkOrderSystem.NewSystemEnabled)
            {
                RewardCollection = new List<CollectionItem>();

                /*RewardCollection.Add(new BODCollectionItem(0x13E3, 1157219, 0, 10, SmithHammer));
                RewardCollection.Add(new BODCollectionItem(0xF39, 1157084, 0x973, 10, SturdyShovel));
                RewardCollection.Add(new BODCollectionItem(0xE86, 1157085, 0x973, 25, SturdyPickaxe));
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157181, 0, 25, RewardTitle, 0));
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157182, 0, 100, RewardTitle, 1));
                RewardCollection.Add(new BODCollectionItem(0x13C6, 1157086, 0, 100, MiningGloves, 1));
                RewardCollection.Add(new BODCollectionItem(0x13D5, 1157087, 0, 200, MiningGloves, 3));
                RewardCollection.Add(new BODCollectionItem(0xFB4, 1157090, 0, 200, ProspectorsTool));
                RewardCollection.Add(new BODCollectionItem(0xE86, 1157089, 0, 200, GargoylesPickaxe));
                RewardCollection.Add(new BODCollectionItem(0x2F5B, 1152674, CraftResources.GetHue(CraftResource.Gold), 350, SmeltersTalisman, (int)CraftResource.Gold));
                RewardCollection.Add(new BODCollectionItem(0x9E2A, 1157264, 0, 400, CraftsmanTalisman, 10));
                RewardCollection.Add(new BODCollectionItem(0x13EB, 1157088, 0, 450, MiningGloves, 5));
                RewardCollection.Add(new BODCollectionItem(4102, 1157091, 0, 450, PowderOfTemperament));
                RewardCollection.Add(new BODCollectionItem(0x2F5B, 1152675, CraftResources.GetHue(CraftResource.Agapite), 475, SmeltersTalisman, (int)CraftResource.Agapite));
                RewardCollection.Add(new BODCollectionItem(0x9E7E, 1157216, 0, 500, RockHammer));
                RewardCollection.Add(new BODCollectionItem(0x13E3, 1157092, CraftResources.GetHue(CraftResource.DullCopper), 500, RunicHammer, 1));
                RewardCollection.Add(new BODCollectionItem(0x2F5B, 1152676, CraftResources.GetHue(CraftResource.Verite), 525, SmeltersTalisman, (int)CraftResource.Verite));
                RewardCollection.Add(new BODCollectionItem(0x13E3, 1157093, CraftResources.GetHue(CraftResource.ShadowIron), 550, RunicHammer, 2));
                RewardCollection.Add(new BODCollectionItem(0x9E2A, 1157218, 0, 550, CraftsmanTalisman, 25));
                RewardCollection.Add(new BODCollectionItem(0x2F5B, 1152677, CraftResources.GetHue(CraftResource.Valorite), 575, SmeltersTalisman, (int)CraftResource.Valorite));
                RewardCollection.Add(new BODCollectionItem(0x14EC, 1152665, CraftResources.GetHue(CraftResource.Gold), 600, HarvestMap, (int)CraftResource.Gold));
                RewardCollection.Add(new BODCollectionItem(0xFAF, 1157100, 0, 625, ColoredAnvil));
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157105, 0x481, 625, PowerScroll, 5));
                RewardCollection.Add(new BODCollectionItem(0x13E3, 1157094, CraftResources.GetHue(CraftResource.Copper), 650, RunicHammer, 3));
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157106, 0x481, 675, PowerScroll, 10));
                RewardCollection.Add(new BODCollectionItem(0x13E3, 1157095, CraftResources.GetHue(CraftResource.Bronze), 700, RunicHammer, 4));
                RewardCollection.Add(new BODCollectionItem(0x13E3, 1157101, 0x482, 750, AncientHammer, 10));
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157107, 0x481, 800, PowerScroll, 15));
                RewardCollection.Add(new BODCollectionItem(0x13E3, 1157102, 0x482, 850, AncientHammer, 15));
                RewardCollection.Add(new BODCollectionItem(0x14EC, 1152666, CraftResources.GetHue(CraftResource.Agapite), 850, HarvestMap, (int)CraftResource.Agapite));
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157108, 0x481, 900, PowerScroll, 20));
                RewardCollection.Add(new BODCollectionItem(0x9E2A, 1157265, 0, 900, CraftsmanTalisman, 50));
                RewardCollection.Add(new BODCollectionItem(0x13E3, 1157096, CraftResources.GetHue(CraftResource.Gold), 950, RunicHammer, 5));
                RewardCollection.Add(new BODCollectionItem(0x14EC, 1152667, CraftResources.GetHue(CraftResource.Verite), 950, HarvestMap, (int)CraftResource.Verite));
                RewardCollection.Add(new BODCollectionItem(0x13E3, 1157103, 0x482, 1000, AncientHammer, 30));
                RewardCollection.Add(new BODCollectionItem(0x13E3, 1157097, CraftResources.GetHue(CraftResource.Agapite), 1050, RunicHammer, 6));
                RewardCollection.Add(new BODCollectionItem(0x14EC, 1152668, CraftResources.GetHue(CraftResource.Valorite), 1050, HarvestMap, (int)CraftResource.Valorite));
                RewardCollection.Add(new BODCollectionItem(0x13E3, 1157104, 0x482, 1100, AncientHammer, 60));
                RewardCollection.Add(new BODCollectionItem(0x13E3, 1157098, CraftResources.GetHue(CraftResource.Verite), 1150, RunicHammer, 7));
                RewardCollection.Add(new BODCollectionItem(0x13E3, 1157099, CraftResources.GetHue(CraftResource.Valorite), 1200, RunicHammer, 8));*/
				
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157219, "Smith Hammer", 0, 10, SmithHammer));
				RewardCollection.Add(new BODCollectionItem(0xF39, 1157084, "Sturdy Shovel", 0x973, 10, SturdyShovel));
				RewardCollection.Add(new BODCollectionItem(0xE86, 1157085, "Sturdy Pickaxe", 0x973, 25, SturdyPickaxe));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157181, "Weaponsmith Reward Title", 0, 25, RewardTitle, 0));
				RewardCollection.Add(new BODCollectionItem(0x13E4, "Sturdy Smith Hammer", 0, 25, SturdySmithHammer)); //daat
				RewardCollection.Add(new BODCollectionItem(5062, "+1 Armor Of Mining Gloves", CraftResources.GetHue(CraftResource.DullCopper), 50, ArmorOfMining, 1)); //daat
				RewardCollection.Add(new BODCollectionItem(7609, "+1 Armor Of Mining Cap", CraftResources.GetHue(CraftResource.DullCopper), 50, ArmorOfMining, 2)); //daat
				RewardCollection.Add(new BODCollectionItem(5068, "+1 Armor Of Mining Tunic", CraftResources.GetHue(CraftResource.DullCopper), 50, ArmorOfMining, 3)); //daat
				RewardCollection.Add(new BODCollectionItem(5063, "+1 Armor Of Mining Gorget", CraftResources.GetHue(CraftResource.DullCopper), 50, ArmorOfMining, 4)); //daat
				RewardCollection.Add(new BODCollectionItem(5069, "+1 Armor Of Mining Arms", CraftResources.GetHue(CraftResource.DullCopper), 50, ArmorOfMining, 5)); //daat
				RewardCollection.Add(new BODCollectionItem(5067, "+1 Armor Of Mining Leggings", CraftResources.GetHue(CraftResource.DullCopper), 50, ArmorOfMining, 6)); //daat
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157182, "Armorer Reward Title", 0, 100, RewardTitle, 1));
				RewardCollection.Add(new BODCollectionItem(0x13C6, 1157086, "Leather Gloves Of Mining", 0, 100, MiningGloves, 1));
				RewardCollection.Add(new BODCollectionItem(0x13D5, 1157087, "Studded Gloves Of Mining", 0, 200, MiningGloves, 3));
				RewardCollection.Add(new BODCollectionItem(0xFB4, 1157090, "Prospectors Tool", 0, 200, ProspectorsTool));
				RewardCollection.Add(new BODCollectionItem(0xE86, 1157089, "Gargoyles Pickaxe", 0, 200, GargoylesPickaxe));
				RewardCollection.Add(new BODCollectionItem(5062, "+1 Armor Of Smithing Gloves", CraftResources.GetHue(CraftResource.Valorite), 200, ArmorOfSmithing, 1)); //daat
				RewardCollection.Add(new BODCollectionItem(7609, "+1 Armor Of Smithing Cap", CraftResources.GetHue(CraftResource.Valorite), 200, ArmorOfSmithing, 2)); //daat
				RewardCollection.Add(new BODCollectionItem(5068, "+1 Armor Of Smithing Tunic", CraftResources.GetHue(CraftResource.Valorite), 200, ArmorOfSmithing, 3)); //daat
				RewardCollection.Add(new BODCollectionItem(5063, "+1 Armor Of Smithing Gorget", CraftResources.GetHue(CraftResource.Valorite), 200, ArmorOfSmithing, 4)); //daat
				RewardCollection.Add(new BODCollectionItem(5069, "+1 Armor Of Smithing Arms", CraftResources.GetHue(CraftResource.Valorite), 200, ArmorOfSmithing, 5)); //daat
				RewardCollection.Add(new BODCollectionItem(5067, "+1 Armor Of Smithing Leggings", CraftResources.GetHue(CraftResource.Valorite), 200, ArmorOfSmithing, 6)); //daat
				RewardCollection.Add(new BODCollectionItem(0x2F5B, 1152674, "Gold Smelters Talisman", CraftResources.GetHue(CraftResource.Gold), 350, SmeltersTalisman, (int)CraftResource.Gold));
				RewardCollection.Add(new BODCollectionItem(0x9E2A, 1157264, "Craftsman Talisman 10 Charges", 0, 400, CraftsmanTalisman, 10));
				RewardCollection.Add(new BODCollectionItem(0x13EB, 1157088, "Ringmail Gloves Of Mining", 0, 450, MiningGloves, 5));
				RewardCollection.Add(new BODCollectionItem(4102, 1157091, "Powder Of Temperament", 0, 450, PowderOfTemperament));
				RewardCollection.Add(new BODCollectionItem(5053, "Decorative Chainmail Tunic", 0, 450, Deco)); //daat
				RewardCollection.Add(new BODCollectionItem(5052, "Decorative Chainmail Leggings", 0, 450, Deco, 1)); //daat
				RewardCollection.Add(new BODCollectionItem(5402, "Decorative Armor", 0, 450, Deco, 2)); //daat
				RewardCollection.Add(new BODCollectionItem(5509, "Decorative Shield And Sword", 0, 450, Deco, 3)); //daat
				RewardCollection.Add(new BODCollectionItem(7110, "Decorative Scale Shield", 0, 450, Deco, 4)); //daat
				RewardCollection.Add(new BODCollectionItem(10324, "Sword Display", 0, 450, Deco, 5)); //daat
				RewardCollection.Add(new BODCollectionItem(0x2F5B, 1152675, "Agapite Smelters Talisman", CraftResources.GetHue(CraftResource.Agapite), 475, SmeltersTalisman, (int)CraftResource.Agapite));
				RewardCollection.Add(new BODCollectionItem(0x9E7E, 1157216, "Rock Hammer", 0, 500, RockHammer));
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157092, "Dull Runic Hammer", CraftResources.GetHue(CraftResource.DullCopper), 500, RunicHammer, 1));
				RewardCollection.Add(new BODCollectionItem(0x2F5B, 1152676, "Verite Smelters Talisman", CraftResources.GetHue(CraftResource.Verite), 475, SmeltersTalisman, (int)CraftResource.Verite));
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157093, "Shadow Runic Hammer", CraftResources.GetHue(CraftResource.ShadowIron), 550, RunicHammer, 2));
				RewardCollection.Add(new BODCollectionItem(0x9E2A, 1157218, "Craftsman Talisman 25 Charges", 0, 550, CraftsmanTalisman, 25));
				RewardCollection.Add(new BODCollectionItem(0x14EC, 1152665, "Gold Harvest Map", CraftResources.GetHue(CraftResource.Gold), 600, HarvestMap, (int)CraftResource.Gold));
				RewardCollection.Add(new BODCollectionItem(0xFB1, "Colored Forge", CraftResources.GetHue(CraftResource.DullCopper), 600, ColoredForgeDeed)); //daat
				RewardCollection.Add(new BODCollectionItem(0xFAF, 1157100, "Colored Anvil", 0, 625, ColoredAnvil));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157105, "A Wondrous Scroll Of Blacksmith", 0x481, 625, PowerScroll, 5));
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157094, "Copper Runic Hammer", CraftResources.GetHue(CraftResource.Copper), 650, RunicHammer, 3));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157105, "An Exalted Scroll Of Blacksmith", 0x481, 675, PowerScroll, 10));
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157095, "Bronze Runic Hammer", CraftResources.GetHue(CraftResource.Bronze), 700, RunicHammer, 4));
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157101, "+10 Ancient Hammer", 0x482, 750, AncientHammer, 10));
				RewardCollection.Add(new BODCollectionItem(5062, "+3 Armor Of Smithing Gloves", CraftResources.GetHue(CraftResource.Ice), 750, ArmorOfSmithing, 7)); //daat
				RewardCollection.Add(new BODCollectionItem(7609, "+3 Armor Of Smithing Cap", CraftResources.GetHue(CraftResource.Ice), 750, ArmorOfSmithing, 8)); //daat
				RewardCollection.Add(new BODCollectionItem(5068, "+3 Armor Of Smithing Tunic", CraftResources.GetHue(CraftResource.Ice), 750, ArmorOfSmithing, 9)); //daat
				RewardCollection.Add(new BODCollectionItem(5063, "+3 Armor Of Smithing Gorget", CraftResources.GetHue(CraftResource.Ice), 750, ArmorOfSmithing, 10)); //daat
				RewardCollection.Add(new BODCollectionItem(5069, "+3 Armor Of Smithing Arms", CraftResources.GetHue(CraftResource.Ice), 750, ArmorOfSmithing, 11)); //daat
				RewardCollection.Add(new BODCollectionItem(5067, "+3 Armor Of Smithing Leggings", CraftResources.GetHue(CraftResource.Ice), 750, ArmorOfSmithing, 12)); //daat
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157106, "A Mythical Scroll Of Blacksmith", 0x481, 800, PowerScroll, 15));
				RewardCollection.Add(new BODCollectionItem(5062, "+3 Armor Of Mining Gloves", CraftResources.GetHue(CraftResource.Blaze), 800, ArmorOfMining, 7)); //daat
				RewardCollection.Add(new BODCollectionItem(7609, "+3 Armor Of Mining Cap", CraftResources.GetHue(CraftResource.Blaze), 800, ArmorOfMining, 8)); //daat
				RewardCollection.Add(new BODCollectionItem(5068, "+3 Armor Of Mining Tunic", CraftResources.GetHue(CraftResource.Blaze), 800, ArmorOfMining, 9)); //daat
				RewardCollection.Add(new BODCollectionItem(5063, "+3 Armor Of Mining Gorget", CraftResources.GetHue(CraftResource.Blaze), 800, ArmorOfMining, 10)); //daat
				RewardCollection.Add(new BODCollectionItem(5069, "+3 Armor Of Mining Arms", CraftResources.GetHue(CraftResource.Blaze), 800, ArmorOfMining, 11)); //daat
				RewardCollection.Add(new BODCollectionItem(5067, "+3 Armor Of Mining Leggings", CraftResources.GetHue(CraftResource.Blaze), 800, ArmorOfMining, 12)); //daat
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157102, "+15 Ancient Hammer", 0x482, 850, AncientHammer, 15));
				RewardCollection.Add(new BODCollectionItem(0x14EC, 1152666, "Agapite Harvest Map", CraftResources.GetHue(CraftResource.Agapite), 850, HarvestMap, (int)CraftResource.Agapite));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157106, "A Legendary Scroll Of Blacksmith", 0x481, 900, PowerScroll, 20));
				RewardCollection.Add(new BODCollectionItem(0x9E2A, 1157265, "Craftsman Talisman 50 Charges", 0, 900, CraftsmanTalisman, 50));
				RewardCollection.Add(new BODCollectionItem(0xFAB, "Leather Armor Dye Tub", 0, 900, ChargedDyeTub)); //daat
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157096, "Gold Runic Hammer", CraftResources.GetHue(CraftResource.Gold), 950, RunicHammer, 5));
				RewardCollection.Add(new BODCollectionItem(0x14EC, 1152667, "Verite Harvest Map", CraftResources.GetHue(CraftResource.Verite), 950, HarvestMap, (int)CraftResource.Verite));
				RewardCollection.Add(new BODCollectionItem(0xFAB, "Metal Armor Dye Tub", 0, 950, ChargedDyeTub, 1)); //daat
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157103, "+30 Ancient Hammer", 0x482, 1000, AncientHammer, 30));
				RewardCollection.Add(new BODCollectionItem(0xFAB, "Metal Weapon Dye Tub", 0, 1000, ChargedDyeTub, 2)); //daat
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157097, "Agapite Runic Hammer", CraftResources.GetHue(CraftResource.Agapite), 1050, RunicHammer, 6));
				RewardCollection.Add(new BODCollectionItem(0x14EC, 1152668, "Valorite Harvest Map", CraftResources.GetHue(CraftResource.Valorite), 1050, HarvestMap, (int)CraftResource.Valorite));
				RewardCollection.Add(new BODCollectionItem(0xE86, "Smithers Protector", 0, 1050, SmithersProtector)); //daat
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157104, "+60 Ancient Hammer", 0x482, 1100, AncientHammer, 60));
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157098, "Verite Runic Hammer", CraftResources.GetHue(CraftResource.Verite), 1150, RunicHammer, 7));
				RewardCollection.Add(new BODCollectionItem(5062, "+5 Armor Of Mining Gloves", CraftResources.GetHue(CraftResource.Electrum), 1150, ArmorOfMining, 13)); //daat
				RewardCollection.Add(new BODCollectionItem(7609, "+5 Armor Of Mining Cap", CraftResources.GetHue(CraftResource.Electrum), 1150, ArmorOfMining, 14)); //daat
				RewardCollection.Add(new BODCollectionItem(5068, "+5 Armor Of Mining Tunic", CraftResources.GetHue(CraftResource.Electrum), 1150, ArmorOfMining, 15)); //daat
				RewardCollection.Add(new BODCollectionItem(5063, "+5 Armor Of Mining Gorget", CraftResources.GetHue(CraftResource.Electrum), 1150, ArmorOfMining, 16)); //daat
				RewardCollection.Add(new BODCollectionItem(5069, "+5 Armor Of Mining Arms", CraftResources.GetHue(CraftResource.Electrum), 1150, ArmorOfMining, 17)); //daat
				RewardCollection.Add(new BODCollectionItem(5067, "+5 Armor Of Mining Leggings", CraftResources.GetHue(CraftResource.Electrum), 1150, ArmorOfMining, 18)); //daat
				RewardCollection.Add(new BODCollectionItem(0x13E3, 1157099, "Valorite Runic Hammer", CraftResources.GetHue(CraftResource.Valorite), 1200, RunicHammer, 8));
				RewardCollection.Add(new BODCollectionItem(5062, "+5 Armor Of Smithing Gloves", CraftResources.GetHue(CraftResource.Toxic), 1200, ArmorOfMining, 13)); //daat
				RewardCollection.Add(new BODCollectionItem(7609, "+5 Armor Of Smithing Cap", CraftResources.GetHue(CraftResource.Toxic), 1200, ArmorOfMining, 14)); //daat
				RewardCollection.Add(new BODCollectionItem(5068, "+5 Armor Of Smithing Tunic", CraftResources.GetHue(CraftResource.Toxic), 1200, ArmorOfMining, 15)); //daat
				RewardCollection.Add(new BODCollectionItem(5063, "+5 Armor Of Smithing Gorget", CraftResources.GetHue(CraftResource.Toxic), 1200, ArmorOfMining, 16)); //daat
				RewardCollection.Add(new BODCollectionItem(5069, "+5 Armor Of Smithing Arms", CraftResources.GetHue(CraftResource.Toxic), 1200, ArmorOfMining, 17)); //daat
				RewardCollection.Add(new BODCollectionItem(5067, "+5 Armor Of Smithing Leggings", CraftResources.GetHue(CraftResource.Toxic), 1200, ArmorOfMining, 18)); //daat
				RewardCollection.Add(new BODCollectionItem(0x13E3, "Blaze Runic Hammer", CraftResources.GetHue(CraftResource.Blaze), 1250, RunicHammer, 9)); //daat
				RewardCollection.Add(new BODCollectionItem(0xE76, "Resources Bag", 0, 1300, ResourcesBag)); //daat
				RewardCollection.Add(new BODCollectionItem(0x13E3, "Ice Runic Hammer", CraftResources.GetHue(CraftResource.Ice), 1300, RunicHammer, 10)); //daat
				RewardCollection.Add(new BODCollectionItem(0x13E3, "Toxic Runic Hammer", CraftResources.GetHue(CraftResource.Toxic), 1350, RunicHammer, 11)); //daat
				RewardCollection.Add(new BODCollectionItem(4731, "Sharpening Blade", 0, 1400, SharpeningBlade)); //daat
				RewardCollection.Add(new BODCollectionItem(0x13E3, "Electrum Runic Hammer", CraftResources.GetHue(CraftResource.Electrum), 1400, RunicHammer, 12)); //daat
				RewardCollection.Add(new BODCollectionItem(0x13E3, "Platinum Runic Hammer", CraftResources.GetHue(CraftResource.Platinum), 1450, RunicHammer, 13)); //daat
            }
            else
            {
                this.Groups = new RewardGroup[]
                {
                    /*new RewardGroup(0, new RewardItem(1, SturdyShovel)),
                    new RewardGroup(25, new RewardItem(1, SturdyPickaxe)),
                    new RewardGroup(50, new RewardItem(45, SturdyShovel), new RewardItem(45, SturdyPickaxe), new RewardItem(10, MiningGloves, 1)),
                    new RewardGroup(200, new RewardItem(45, GargoylesPickaxe), new RewardItem(45, ProspectorsTool), new RewardItem(10, MiningGloves, 3)),
                    new RewardGroup(400, new RewardItem(2, GargoylesPickaxe), new RewardItem(2, ProspectorsTool), new RewardItem(1, PowderOfTemperament)),
                    new RewardGroup(450, new RewardItem(9, PowderOfTemperament), new RewardItem(1, MiningGloves, 5)),
                    new RewardGroup(500, new RewardItem(1, RunicHammer, 1)),
                    new RewardGroup(550, new RewardItem(3, RunicHammer, 1), new RewardItem(2, RunicHammer, 2)),
                    new RewardGroup(600, new RewardItem(1, RunicHammer, 2)),
                    new RewardGroup(625, new RewardItem(3, RunicHammer, 2), new RewardItem(6, PowerScroll, 5), new RewardItem(1, ColoredAnvil)),
                    new RewardGroup(650, new RewardItem(1, RunicHammer, 3)),
                    new RewardGroup(675, new RewardItem(1, ColoredAnvil), new RewardItem(6, PowerScroll, 10), new RewardItem(3, RunicHammer, 3)),
                    new RewardGroup(700, new RewardItem(1, RunicHammer, 4)),
                    new RewardGroup(750, new RewardItem(1, AncientHammer, 10)),
                    new RewardGroup(800, new RewardItem(1, PowerScroll, 15)),
                    new RewardGroup(850, new RewardItem(1, AncientHammer, 15)),
                    new RewardGroup(900, new RewardItem(1, PowerScroll, 20)),
                    new RewardGroup(950, new RewardItem(1, RunicHammer, 5)),
                    new RewardGroup(1000, new RewardItem(1, AncientHammer, 30)),
                    new RewardGroup(1050, new RewardItem(1, RunicHammer, 6)),
                    new RewardGroup(1100, new RewardItem(1, AncientHammer, 60)),
                    new RewardGroup(1150, new RewardItem(1, RunicHammer, 7)),
                    new RewardGroup(1200, new RewardItem(1, RunicHammer, 8))*/

					//daat99 OWLTR start - bod reward
					new RewardGroup(    0, new RewardItem( 1, SturdyShovel ), new RewardItem( 1, SturdySmithHammer ) ),
					new RewardGroup(   25, new RewardItem( 1, SturdyPickaxe ), new RewardItem( 1, SturdySmithHammer ) ),
					new RewardGroup(   50, new RewardItem( 90, SturdyShovel ), new RewardItem( 10, ArmorOfMining, Utility.RandomMinMax(1,6) ) ),
					new RewardGroup(  200, new RewardItem( 90, SturdyPickaxe ), new RewardItem( 10, ArmorOfSmithing, Utility.RandomMinMax(1,6) ) ),
					new RewardGroup(  400, new RewardItem( 90, ProspectorsTool ), new RewardItem( 10, ArmorOfMining, Utility.RandomMinMax(1,6) ) ),
					new RewardGroup(  450, new RewardItem( 2, PowderOfTemperament ), new RewardItem( 1, GargoylesPickaxe ), new RewardItem( 1, Deco, Utility.Random(6) ) ),
					new RewardGroup(  500, new RewardItem( 1, RunicHammer, 1 ), new RewardItem( 1, GargoylesPickaxe ), new RewardItem( 1, Deco, Utility.Random(6) ) ),
					new RewardGroup(550, new RewardItem(3, RunicHammer, 1), new RewardItem(2, RunicHammer, 2)),
					new RewardGroup(  600, new RewardItem( 1, RunicHammer, 2 ), new RewardItem( 1, ColoredForgeDeed ) ),
					new RewardGroup(  625, new RewardItem( 3, RunicHammer, 2 ), new RewardItem( 1, ColoredAnvil ) ),
					new RewardGroup(  650, new RewardItem( 1, RunicHammer, 3 ), new RewardItem( 1, Deco, Utility.Random(6) ) ),
					new RewardGroup(  675, new RewardItem( 1, ColoredAnvil ), new RewardItem( 3, RunicHammer, 3 ) ),
					new RewardGroup(  700, new RewardItem( 1, RunicHammer, 4 ), new RewardItem( 1, Deco, Utility.Random(6) ) ),
					new RewardGroup(  750, new RewardItem( 1, AncientHammer, 10 ), new RewardItem( 1, ArmorOfSmithing, Utility.RandomMinMax(7,12) ) ),
					new RewardGroup(  800, new RewardItem( 1, GargoylesPickaxe ), new RewardItem( 1, ArmorOfMining, Utility.RandomMinMax(7,12) ) ),
					new RewardGroup(  850, new RewardItem( 1, AncientHammer, 15 ), new RewardItem( 1, ArmorOfSmithing, Utility.RandomMinMax(7,12) ) ),
					new RewardGroup(  900, new RewardItem( 1, RunicHammer, 5 ), new RewardItem( 1, ChargedDyeTub, Utility.RandomMinMax(1,2) ) ),
					new RewardGroup(  950, new RewardItem( 1, RunicHammer, 5 ), new RewardItem( 1, ChargedDyeTub, Utility.RandomMinMax(1,2) ) ),
					new RewardGroup( 1000, new RewardItem( 1, AncientHammer, 30 ), new RewardItem( 1, ChargedDyeTub, Utility.RandomMinMax(1,2) ) ),
					new RewardGroup( 1050, new RewardItem( 1, RunicHammer, 6 ), new RewardItem( 1, SmithersProtector ) ),
					new RewardGroup( 1100, new RewardItem( 1, AncientHammer, 60 ), new RewardItem( 1, SmithersProtector ) ),
					new RewardGroup( 1150, new RewardItem( 1, RunicHammer, 7 ), new RewardItem( 1, ArmorOfMining, Utility.RandomMinMax(13,18) ) ),
					new RewardGroup( 1200, new RewardItem( 1, RunicHammer, 8 ), new RewardItem( 1, ArmorOfSmithing, Utility.RandomMinMax(13,18) ) ),
					new RewardGroup( 1250, new RewardItem( 1, RunicHammer, 9 ), new RewardItem( 1, ArmorOfSmithing, Utility.RandomMinMax(13,18) ) ),
					new RewardGroup( 1300, new RewardItem( 1, RunicHammer, 10 ), new RewardItem( 1, ResourcesBag ) ),
					new RewardGroup( 1350, new RewardItem( 1, RunicHammer, 11 ), new RewardItem( 1, ResourcesBag ) ),
					new RewardGroup( 1400, new RewardItem( 1, RunicHammer, 12 ), new RewardItem( 1, SharpeningBlade ) ),
					new RewardGroup( 1450, new RewardItem( 1, RunicHammer, 13 ), new RewardItem( 1, SharpeningBlade ) )
					//daat99 OWLTR end - bod reward
                };
            }
        }

        #region Constructors
        private static readonly ConstructCallback SmithHammer = new ConstructCallback(CreateSmithHammer);
        private static readonly ConstructCallback SturdyShovel = new ConstructCallback(CreateSturdyShovel);
        private static readonly ConstructCallback SturdyPickaxe = new ConstructCallback(CreateSturdyPickaxe);
        private static readonly ConstructCallback MiningGloves = new ConstructCallback(CreateMiningGloves);
        private static readonly ConstructCallback GargoylesPickaxe = new ConstructCallback(CreateGargoylesPickaxe);
        private static readonly ConstructCallback ProspectorsTool = new ConstructCallback(CreateProspectorsTool);
        private static readonly ConstructCallback PowderOfTemperament = new ConstructCallback(CreatePowderOfTemperament);
        private static readonly ConstructCallback RunicHammer = new ConstructCallback(CreateRunicHammer);
        private static readonly ConstructCallback PowerScroll = new ConstructCallback(CreatePowerScroll);
        private static readonly ConstructCallback ColoredAnvil = new ConstructCallback(CreateColoredAnvil);
        private static readonly ConstructCallback AncientHammer = new ConstructCallback(CreateAncientHammer);
		//daat99 OWLTR start - bod rewards
		private static readonly ConstructCallback Deco = new ConstructCallback(CreateDeco);
		private static readonly ConstructCallback SturdySmithHammer = new ConstructCallback(CreateSturdySmithHammer);
		private static readonly ConstructCallback SmithersProtector = new ConstructCallback(CreateSmithersProtector);
		private static readonly ConstructCallback SharpeningBlade = new ConstructCallback(CreateSharpeningBlade);
		private static readonly ConstructCallback ColoredForgeDeed = new ConstructCallback(CreateColoredForgeDeed);
		private static readonly ConstructCallback ChargedDyeTub = new ConstructCallback(CreateChargedDyeTub);
		private static readonly ConstructCallback ResourcesBag = new ConstructCallback(CreateResourcesBag);
		private static readonly ConstructCallback ArmorOfMining = new ConstructCallback(CreateArmorOfMining);
		private static readonly ConstructCallback ArmorOfSmithing = new ConstructCallback(CreateArmorOfSmithing);

		private static Item CreateDeco(int type)
		{
			switch (type)
			{
				case 0: default: return new Deco(5053, "Chainmail Tunic");
				case 1: return new Deco(5052, "chainmail Leggings");
				case 2: return new Deco(5402, "Decorative Armor");
				case 3: return new Deco(5509, "Decorative Shield and Sword");
				case 4: return new Deco(7110, "Decorative Scale Shield");
				case 5: return new Deco(10324, "Sword Display");
			}
		}

		private static Item CreateSturdySmithHammer(int type)
		{
			return new SturdySmithHammer();
		}

		private static Item CreateSmithersProtector(int type)
		{
			return new SmithersProtector();
		}

		private static Item CreateSharpeningBlade(int type)
		{
			return new SharpeningBlade();
		}

		private static Item CreateColoredForgeDeed(int type)
		{
			return new ColoredForgeDeed(CraftResources.GetHue((CraftResource)Utility.RandomMinMax((int)CraftResource.DullCopper, (int)CraftResource.Platinum)));
		}

		private static Item CreateChargedDyeTub(int type)
		{
			return new ChargedDyeTub(10, type);
		}

		private static Item CreateResourcesBag(int type)
		{
			return new ResourcesBag();
		}

		private static Item CreateArmorOfMining(int type)
		{
			switch (type)
			{
				case 1: default: return new ArmorOfMining(1, 5062, Utility.Random(2)); //gloves
				case 2: return new ArmorOfMining(1, 7609, Utility.Random(2)); //cap
				case 3: return new ArmorOfMining(1, 5068, Utility.Random(2)); //tunic
				case 4: return new ArmorOfMining(1, 5063, Utility.Random(2)); //gorget
				case 5: return new ArmorOfMining(1, 5069, Utility.Random(2)); //arms
				case 6: return new ArmorOfMining(1, 5067, Utility.Random(2)); //leggings
				case 7: return new ArmorOfMining(3, 5062, Utility.Random(2)); //gloves
				case 8: return new ArmorOfMining(3, 7609, Utility.Random(2)); //cap
				case 9: return new ArmorOfMining(3, 5068, Utility.Random(2)); //tunic
				case 10: return new ArmorOfMining(3, 5063, Utility.Random(2)); //gorget
				case 11: return new ArmorOfMining(3, 5069, Utility.Random(2)); //arms
				case 12: return new ArmorOfMining(3, 5067, Utility.Random(2)); //leggings
				case 13: return new ArmorOfMining(5, 5062, Utility.Random(2)); //gloves
				case 14: return new ArmorOfMining(5, 7609, Utility.Random(2)); //cap
				case 15: return new ArmorOfMining(5, 5068, Utility.Random(2)); //tunic
				case 16: return new ArmorOfMining(5, 5063, Utility.Random(2)); //gorget
				case 17: return new ArmorOfMining(5, 5069, Utility.Random(2)); //arms
				case 18: return new ArmorOfMining(5, 5067, Utility.Random(2)); //leggings
			}
		}
		private static Item CreateArmorOfSmithing(int type)
		{
			switch (type)
			{
				case 1:
				default: return new ArmorOfSmithing(1, 5062, Utility.Random(2)); //gloves
				case 2: return new ArmorOfSmithing(1, 7609, Utility.Random(2)); //cap
				case 3: return new ArmorOfSmithing(1, 5068, Utility.Random(2)); //tunic
				case 4: return new ArmorOfSmithing(1, 5063, Utility.Random(2)); //gorget
				case 5: return new ArmorOfSmithing(1, 5069, Utility.Random(2)); //arms
				case 6: return new ArmorOfSmithing(1, 5067, Utility.Random(2)); //leggings
				case 7: return new ArmorOfSmithing(3, 5062, Utility.Random(2)); //gloves
				case 8: return new ArmorOfSmithing(3, 7609, Utility.Random(2)); //cap
				case 9: return new ArmorOfSmithing(3, 5068, Utility.Random(2)); //tunic
				case 10: return new ArmorOfSmithing(3, 5063, Utility.Random(2)); //gorget
				case 11: return new ArmorOfSmithing(3, 5069, Utility.Random(2)); //arms
				case 12: return new ArmorOfSmithing(3, 5067, Utility.Random(2)); //leggings
				case 13: return new ArmorOfSmithing(5, 5062, Utility.Random(2)); //gloves
				case 14: return new ArmorOfSmithing(5, 7609, Utility.Random(2)); //cap
				case 15: return new ArmorOfSmithing(5, 5068, Utility.Random(2)); //tunic
				case 16: return new ArmorOfSmithing(5, 5063, Utility.Random(2)); //gorget
				case 17: return new ArmorOfSmithing(5, 5069, Utility.Random(2)); //arms
				case 18: return new ArmorOfSmithing(5, 5067, Utility.Random(2)); //leggings
			}
		}
		//daat99 OWLTR end - bod rewards

		private static Item CreateSmithHammer(int type)
        {
            var hammer = new SmithHammer();
            hammer.UsesRemaining = 250;

            return hammer;
        }

        private static Item CreateSturdyShovel(int type)
        {
            return new SturdyShovel();
        }

        private static Item CreateSturdyPickaxe(int type)
        {
            return new SturdyPickaxe();
        }

        private static Item CreateMiningGloves(int type)
        {
            if (type == 1)
                return new LeatherGlovesOfMining(1);
            else if (type == 3)
                return new StuddedGlovesOfMining(3);
            else if (type == 5)
                return new RingmailGlovesOfMining(5);

            throw new InvalidOperationException();
        }

        private static Item CreateGargoylesPickaxe(int type)
        {
            return new GargoylesPickaxe();
        }

        private static Item CreateProspectorsTool(int type)
        {
            return new ProspectorsTool();
        }

        private static Item CreatePowderOfTemperament(int type)
        {
            return new PowderOfTemperament();
        }

        private static Item CreateRunicHammer(int type)
        {
            if (type >= 1 && type <= 13)
                return new RunicHammer(CraftResource.Iron + type, Core.AOS ? (55 - (type * 5)) : 50);

            throw new InvalidOperationException();
        }

        private static Item CreatePowerScroll(int type)
        {
            if (type == 5 || type == 10 || type == 15 || type == 20)
                return new PowerScroll(SkillName.Blacksmith, 100 + type);

            throw new InvalidOperationException();
        }

        private static Item CreateColoredAnvil(int type)
        {
            return new ColoredAnvil();
        }

        private static Item CreateAncientHammer(int type)
        {
            if (type == 10 || type == 15 || type == 30 || type == 60)
                return new AncientSmithyHammer(type);

            throw new InvalidOperationException();
        }

        private static Item CraftsmanTalisman(int type)
        {
            return new MasterCraftsmanTalisman(type, 0x9E2A, TalismanSkill.Blacksmithy);
        }
        #endregion

        public static readonly SmithRewardCalculator Instance = new SmithRewardCalculator();

        private readonly RewardType[] m_Types = new RewardType[]
        {
            // Armors
            new RewardType(200, typeof(RingmailGloves), typeof(RingmailChest), typeof(RingmailArms), typeof(RingmailLegs)),
            new RewardType(300, typeof(ChainCoif), typeof(ChainLegs), typeof(ChainChest)),
            new RewardType(400, typeof(PlateArms), typeof(PlateLegs), typeof(PlateHelm), typeof(PlateGorget), typeof(PlateGloves), typeof(PlateChest)),

            // Weapons
            new RewardType(200, typeof(Bardiche), typeof(Halberd)),
            new RewardType(300, typeof(Dagger), typeof(ShortSpear), typeof(Spear), typeof(WarFork), typeof(Kryss)), //OSI put the dagger in there.  Odd, ain't it.
            new RewardType(350, typeof(Axe), typeof(BattleAxe), typeof(DoubleAxe), typeof(ExecutionersAxe), typeof(LargeBattleAxe), typeof(TwoHandedAxe)),
            new RewardType(350, typeof(Broadsword), typeof(Cutlass), typeof(Katana), typeof(Longsword), typeof(Scimitar), /*typeof( ThinLongsword ),*/ typeof(VikingSword)),
            new RewardType(350, typeof(WarAxe), typeof(HammerPick), typeof(Mace), typeof(Maul), typeof(WarHammer), typeof(WarMace))
        };

        public override int ComputePoints(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int points = 0;

            if (quantity == 10)
                points += 10;
            else if (quantity == 15)
                points += 25;
            else if (quantity == 20)
                points += 50;

            if (exceptional)
                points += 200;

            if (itemCount > 1)
                points += this.LookupTypePoints(this.m_Types, type);

			//daat99 OWLTR start - custom resources
			if (material >= BulkMaterialType.DullCopper && material <= BulkMaterialType.Platinum)
            //if (material >= BulkMaterialType.DullCopper && material <= BulkMaterialType.Valorite)
			//daat99 OWLTR end - custom resources
                points += 200 + (50 * (material - BulkMaterialType.DullCopper));

            return points;
        }

        private static readonly int[][][] m_GoldTable = new int[][][]
        {
            /*new int[][] // 1-part (regular)
            {
                new int[] { 150, 250, 250, 400, 400, 750, 750, 1200, 1200 },
                new int[] { 225, 375, 375, 600, 600, 1125, 1125, 1800, 1800 },
                new int[] { 300, 500, 750, 800, 1050, 1500, 2250, 2400, 4000 }
            },
            new int[][] // 1-part (exceptional)
            {
                new int[] { 250, 400, 400, 750, 750, 1500, 1500, 3000, 3000 },
                new int[] { 375, 600, 600, 1125, 1125, 2250, 2250, 4500, 4500 },
                new int[] { 500, 800, 1200, 1500, 2500, 3000, 6000, 6000, 12000 }
            },
            new int[][] // Ringmail (regular)
            {
                new int[] { 3000, 5000, 5000, 7500, 7500, 10000, 10000, 15000, 15000 },
                new int[] { 4500, 7500, 7500, 11250, 11500, 15000, 15000, 22500, 22500 },
                new int[] { 6000, 10000, 15000, 15000, 20000, 20000, 30000, 30000, 50000 }
            },
            new int[][] // Ringmail (exceptional)
            {
                new int[] { 5000, 10000, 10000, 15000, 15000, 25000, 25000, 50000, 50000 },
                new int[] { 7500, 15000, 15000, 22500, 22500, 37500, 37500, 75000, 75000 },
                new int[] { 10000, 20000, 30000, 30000, 50000, 50000, 100000, 100000, 200000 }
            },
            new int[][] // Chainmail (regular)
            {
                new int[] { 4000, 7500, 7500, 10000, 10000, 15000, 15000, 25000, 25000 },
                new int[] { 6000, 11250, 11250, 15000, 15000, 22500, 22500, 37500, 37500 },
                new int[] { 8000, 15000, 20000, 20000, 30000, 30000, 50000, 50000, 100000 }
            },
            new int[][] // Chainmail (exceptional)
            {
                new int[] { 7500, 15000, 15000, 25000, 25000, 50000, 50000, 100000, 100000 },
                new int[] { 11250, 22500, 22500, 37500, 37500, 75000, 75000, 150000, 150000 },
                new int[] { 15000, 30000, 50000, 50000, 100000, 100000, 200000, 200000, 200000 }
            },
            new int[][] // Platemail (regular)
            {
                new int[] { 5000, 10000, 10000, 15000, 15000, 25000, 25000, 50000, 50000 },
                new int[] { 7500, 15000, 15000, 22500, 22500, 37500, 37500, 75000, 75000 },
                new int[] { 10000, 20000, 30000, 30000, 50000, 50000, 100000, 100000, 200000 }
            },
            new int[][] // Platemail (exceptional)
            {
                new int[] { 10000, 25000, 25000, 50000, 50000, 100000, 100000, 100000, 100000 },
                new int[] { 15000, 37500, 37500, 75000, 75000, 150000, 150000, 150000, 150000 },
                new int[] { 20000, 50000, 100000, 100000, 200000, 200000, 200000, 200000, 200000 }
            },
            new int[][] // 2-part weapons (regular)
            {
                new int[] { 3000, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 4500, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 6000, 0, 0, 0, 0, 0, 0, 0, 0 }
            },
            new int[][] // 2-part weapons (exceptional)
            {
                new int[] { 5000, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 7500, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 10000, 0, 0, 0, 0, 0, 0, 0, 0 }
            },
            new int[][] // 5-part weapons (regular)
            {
                new int[] { 4000, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 6000, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 8000, 0, 0, 0, 0, 0, 0, 0, 0 }
            },
            new int[][] // 5-part weapons (exceptional)
            {
                new int[] { 7500, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 11250, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 15000, 0, 0, 0, 0, 0, 0, 0, 0 }
            },
            new int[][] // 6-part weapons (regular)
            {
                new int[] { 4000, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 6000, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 10000, 0, 0, 0, 0, 0, 0, 0, 0 }
            },
            new int[][] // 6-part weapons (exceptional)
            {
                new int[] { 7500, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 11250, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 15000, 0, 0, 0, 0, 0, 0, 0, 0 }
            }*/

			//daat99 OWLTR start - custom gold reward
            new int[][] // 1-part (regular)
            {
				new int[]{ 100, 200, 300,  400,  500,  600,  700,  800,  900, 1000, 1100, 1200, 1300, 1400 },
				new int[]{ 200, 400, 600,  800, 1000, 1200, 1400, 1600, 1800, 2000, 2200, 2400, 2600, 2800 },
				new int[]{ 300, 600, 900, 1200, 1500, 1800, 2100, 2400, 2700, 3000, 3300, 3600, 3900, 4200 }
			},
			new int[][] // 1-part (exceptional)
            {
				new int[]{ 250, 500,  750, 1000, 1250, 1500, 1750, 2000,  2250, 2500, 2750, 3000, 3250, 3500 },
				new int[]{ 350, 700, 1050, 1400, 1750, 2100, 2450, 2800,  3150, 3500, 3850, 4200, 4550, 4900 },
				new int[]{ 450, 900, 1350, 1800, 2250, 2700, 3150, 3600,  4050, 4500, 4950, 5400, 5850, 6300 }
			},
			new int[][] // Ringmail (regular)
            {
				new int[]{ 2000, 4000,  6000,  8000, 10000, 12000, 14000, 16000, 18000, 20000, 22000, 24000, 26000, 28000 },
				new int[]{ 3000, 6000,  9000, 12000, 15000, 18000, 21000, 24000, 27000, 30000, 33000, 36000, 39000, 42000 },
				new int[]{ 4000, 8000, 12000, 16000, 20000, 24000, 28000, 32000, 36000, 40000, 44000, 48000, 52000, 56000 }
			},
			new int[][] // Ringmail (exceptional)
            {
				new int[]{ 4000,  8000, 12000, 16000, 20000, 24000, 28000, 32000, 36000, 40000, 44000, 48000,  52000,  56000 },
				new int[]{ 6000, 12000, 18000, 24000, 30000, 36000, 42000, 48000, 54000, 60000, 66000, 72000,  78000,  84000 },
				new int[]{ 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 72000, 80000, 88000, 96000, 104000, 112000 }
			},
			new int[][] // Chainmail (regular)
            {
				new int[]{ 4000,  8000, 12000, 16000, 20000, 24000, 28000, 32000, 36000, 40000, 44000, 48000,  52000,  56000 },
				new int[]{ 6000, 12000, 18000, 24000, 30000, 36000, 42000, 48000, 54000, 60000, 66000, 72000,  78000,  84000 },
				new int[]{ 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 72000, 80000, 88000, 96000, 104000, 112000 }
			},
			new int[][] // Chainmail (exceptional)
            {
				new int[]{  7000, 14000, 21000, 28000, 35000, 42000,  49000,  56000,  63000,  70000,  77000,  84000,  91000,  98000 },
				new int[]{ 10000, 20000, 30000, 40000, 50000, 60000,  70000,  80000,  90000, 100000, 110000, 120000, 130000, 140000 },
				new int[]{ 15000, 30000, 45000, 60000, 75000, 90000, 105000, 120000, 135000, 150000, 165000, 180000, 195000, 210000 }
			},
			new int[][] // Platemail (regular)
            {
				new int[]{  5000, 10000, 15000, 20000, 25000, 30000,  35000,  40000,  45000,  50000,  55000,  60000,  65000,  70000 },
				new int[]{  7500, 15000, 22500, 30000, 37500, 45000,  52500,  60000,  67500,  75000,  82500,  90000,  97500, 105000 },
				new int[]{ 10000, 20000, 30000, 40000, 50000, 60000,  70000,  80000,  90000, 100000, 110000, 120000, 130000, 140000 }
			},
			new int[][] // Platemail (exceptional)
            {
				new int[]{ 10000, 20000, 30000, 40000,  50000,  60000,  70000,  80000,  90000, 100000, 110000, 120000, 130000, 140000 },
				new int[]{ 15000, 30000, 45000, 60000,  75000,  90000, 105000, 120000, 135000, 150000, 165000, 180000, 195000, 210000 },
				new int[]{ 20000, 40000, 60000, 80000, 100000, 120000, 140000, 160000, 180000, 200000, 220000, 240000, 260000, 280000 }
			},
			new int[][] // 2-part weapons (regular)
            {
				new int[]{ 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				new int[]{ 4500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				new int[]{ 6000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
			},
			new int[][] // 2-part weapons (exceptional)
            {
				new int[]{ 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				new int[]{ 7500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				new int[]{ 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
			},
			new int[][] // 5-part weapons (regular)
            {
				new int[]{ 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				new int[]{ 6000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				new int[]{ 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
			},
			new int[][] // 5-part weapons (exceptional)
            {
				new int[]{ 7500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				new int[]{ 11250, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				new int[]{ 15000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
			},
			new int[][] // 6-part weapons (regular)
            {
				new int[]{ 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				new int[]{ 6000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				new int[]{ 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
			},
			new int[][] // 6-part weapons (exceptional)
            {
				new int[]{ 7500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				new int[]{ 11250, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
				new int[]{ 15000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
			}
			//daat99 OWLTR start - custom gold reward
        };

        private int ComputeType(Type type, int itemCount)
        {
            // Item count of 1 means it's a small BOD.
            if (itemCount == 1)
                return 0;

            int typeIdx;

            // Loop through the RewardTypes defined earlier and find the correct one.
			//daat99 OWLTR start - don't use magic numbers...
			for ( typeIdx = 0; typeIdx < m_Types.Length; ++typeIdx)
            //for (typeIdx = 0; typeIdx < 7; ++typeIdx)
			//daat99 OWLTR end - don't use magic numbers...
            {
                if (this.m_Types[typeIdx].Contains(type))
                    break;
            }

            // Types 5, 6 and 7 are Large Weapon BODs with the same rewards.
            if (typeIdx > 5)
                typeIdx = 5;

            return (typeIdx + 1) * 2;
        }

        public override int ComputeGold(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int gold = 0;

            if (itemCount == 1 && BulkOrderSystem.NewSystemEnabled && BulkOrderSystem.ComputeGold(type, quantity, out gold))
            {
                return gold;
            }

            int[][][] goldTable = m_GoldTable;

            int typeIndex = this.ComputeType(type, itemCount);
            int quanIndex = (quantity == 20 ? 2 : quantity == 15 ? 1 : 0);
			//daat99 OWLTR start - custom resource
			int mtrlIndex = (material >= BulkMaterialType.DullCopper && material <= BulkMaterialType.Platinum) ? 1 + (int)(material - BulkMaterialType.DullCopper) : 0;
            //int mtrlIndex = (material >= BulkMaterialType.DullCopper && material <= BulkMaterialType.Valorite) ? 1 + (int)(material - BulkMaterialType.DullCopper) : 0;
			//daat99 OWLTR end - custom resource

            if (exceptional)
                typeIndex++;

            gold = goldTable[typeIndex][quanIndex][mtrlIndex];

            int min = (gold * 9) / 10;
            int max = (gold * 10) / 9;

            return Utility.RandomMinMax(min, max);
        }
    }
    #endregion

    #region Tailor Rewards
    public sealed class TailorRewardCalculator : RewardCalculator
    {
        public TailorRewardCalculator()
        {
            if (BulkOrderSystem.NewSystemEnabled)
            {
                RewardCollection = new List<CollectionItem>();

				/*RewardCollection.Add(new BODCollectionItem(0xF9D, 1157219, 0, 10, SewingKit));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157183, 0, 10, RewardTitle, 2));
				RewardCollection.Add(new BODCollectionItem(0x1765, 1157109, 0, 10, Cloth, 0));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157184, 0, 25, RewardTitle, 3));
				RewardCollection.Add(new BODCollectionItem(0x1761, 1157109, 0, 50, Cloth, 1));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157185, 0, 100, RewardTitle, 4));
				RewardCollection.Add(new BODCollectionItem(0x1765, 1157109, 0, 100, Cloth, 2));
				RewardCollection.Add(new BODCollectionItem(0x1765, 1157109, 0, 150, Cloth, 3));
				RewardCollection.Add(new BODCollectionItem(0x170D, 1157110, 0, 150, Sandals, 3));
				RewardCollection.Add(new BODCollectionItem(0x1765, 1157109, 0, 200, Cloth, 4));
				RewardCollection.Add(new BODCollectionItem(0x9E25, 1157264, 0, 200, CraftsmanTalisman, 10)); // todo: Get id
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157111, 0, 300, StretchedHide));
				RewardCollection.Add(new BODCollectionItem(0x1765, 1157109, 0, 300, Cloth, 5)); // TODO: Get other 4 colors
				RewardCollection.Add(new BODCollectionItem(0x9E25, 1157218, 0, 300, CraftsmanTalisman, 25)); // todo: Get id
				RewardCollection.Add(new BODCollectionItem(0xF9D, 1157115, CraftResources.GetHue(CraftResource.SpinedLeather), 350, RunicKit, 1));
				RewardCollection.Add(new BODCollectionItem(0x9E25, 1157265, 0, 350, CraftsmanTalisman, 50)); // todo: Get id
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157118, 0x481, 400, PowerScroll, 5));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157112, 0, 400, Tapestry));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157113, 0, 450, BearRug));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157119, 0x481, 500, PowerScroll, 10));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157114, 0, 550, ClothingBlessDeed));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157120, 0x481, 575, PowerScroll, 15));
				RewardCollection.Add(new BODCollectionItem(0xF9D, 1157116, CraftResources.GetHue(CraftResource.HornedLeather), 600, RunicKit, 2));
				RewardCollection.Add(new BODCollectionItem(0x14F0, 1157121, 0x481, 650, PowerScroll, 20));
				RewardCollection.Add(new BODCollectionItem(0xF9D, 1157117, CraftResources.GetHue(CraftResource.BarbedLeather), 700, RunicKit, 3));*/

                RewardCollection.Add(new BODCollectionItem(0xF9D, 1157219, "Sewing Kit", 0, 10, SewingKit));
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157183, "Weaver Reward Title", 0, 10, RewardTitle, 2));
                RewardCollection.Add(new BODCollectionItem(0x1765, 1157109, "Cloth", 0, 10, Cloth, 0));
				RewardCollection.Add(new BODCollectionItem(0x105F, "Colored Loom", 0, 10, ColoredLoom)); //daat
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157184, "Dressmaker Reward Title", 0, 25, RewardTitle, 3));
                RewardCollection.Add(new BODCollectionItem(0x1761, 1157109, "Cloth", 0, 50, Cloth, 1));
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157185, "Haberdasher Reward Title", 0, 100, RewardTitle, 4));
                RewardCollection.Add(new BODCollectionItem(0x1765, 1157109, "Cloth", 0, 100, Cloth, 2));
                RewardCollection.Add(new BODCollectionItem(0x1765, 1157109, "Cloth", 0, 150, Cloth, 3));
                RewardCollection.Add(new BODCollectionItem(0x170D, 1157110, "Sandals", 0, 150, Sandals, 3));
				RewardCollection.Add(new BODCollectionItem(5062, "+1 Armor Of Tailoring Gloves", CraftResources.GetHue(CraftResource.SpinedLeather), 150, ArmorOfTailoring, 1)); //daat
				RewardCollection.Add(new BODCollectionItem(7609, "+1 Armor Of Tailoring Cap", CraftResources.GetHue(CraftResource.SpinedLeather), 150, ArmorOfTailoring, 2)); //daat
				RewardCollection.Add(new BODCollectionItem(5068, "+1 Armor Of Tailoring Tunic", CraftResources.GetHue(CraftResource.SpinedLeather), 150, ArmorOfTailoring, 3)); //daat
				RewardCollection.Add(new BODCollectionItem(5063, "+1 Armor Of Tailoring Gorget", CraftResources.GetHue(CraftResource.SpinedLeather), 150, ArmorOfTailoring, 4)); //daat
				RewardCollection.Add(new BODCollectionItem(5069, "+1 Armor Of Tailoring Arms", CraftResources.GetHue(CraftResource.SpinedLeather), 150, ArmorOfTailoring, 5)); //daat
				RewardCollection.Add(new BODCollectionItem(5067, "+1 Armor Of Tailoring Leggings", CraftResources.GetHue(CraftResource.SpinedLeather), 150, ArmorOfTailoring, 6)); //daat
                RewardCollection.Add(new BODCollectionItem(0x1765, 1157109, "Cloth", 0, 200, Cloth, 4));
                RewardCollection.Add(new BODCollectionItem(0x9E25, 1157264, "Craftsman Talisman 10 Charges", 0, 200, CraftsmanTalisman, 10)); // todo: Get id
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157111, "Stretched Hide", 0, 300, StretchedHide));
                RewardCollection.Add(new BODCollectionItem(0x1765, 1157109, "Cloth", 0, 300, Cloth, 5)); // TODO: Get other 4 colors
                RewardCollection.Add(new BODCollectionItem(0x9E25, 1157218, "Craftsman Talisman 25 Charges", 0, 300, CraftsmanTalisman, 25)); // todo: Get id
				RewardCollection.Add(new BODCollectionItem(0xF9F, "Colored Scissors", 0, 300, ColoredScissors)); //daat
                RewardCollection.Add(new BODCollectionItem(0xF9D, 1157115, "Spined Runic Sewing Kit", CraftResources.GetHue(CraftResource.SpinedLeather), 350, RunicKit, 1));
                RewardCollection.Add(new BODCollectionItem(0x9E25, 1157265, "Craftsman Talisman 50 Charges", 0, 350, CraftsmanTalisman, 50)); // todo: Get id
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157118, "A Wondrous Scroll Of Tailoring", 0x481, 400, PowerScroll, 5));
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157112, "Tapestry", 0, 400, Tapestry));
				RewardCollection.Add(new BODCollectionItem(0xF9D, "Sturdy Sewing Kit", 0, 400, SturdySewingKit)); //daat
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157113, "Bear Rug", 0, 450, BearRug));
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157119, "An Exalted Scroll Of Tailoring", 0x481, 500, PowerScroll, 10));
				RewardCollection.Add(new BODCollectionItem(0x13F6, "Masters Knife", 0x973, 500, MastersKnife)); //daat
				RewardCollection.Add(new BODCollectionItem(4054, "Tapestry", 0, 500, Deco)); //daat
				RewardCollection.Add(new BODCollectionItem(9036, "Rose of Trinsic", 0, 500, Deco, 1)); //daat
				RewardCollection.Add(new BODCollectionItem(15721, "Deer Corspe", 0, 500, Deco, 2)); //daat
				RewardCollection.Add(new BODCollectionItem(5610, "Banner", 0, 500, Deco, 3)); //daat
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157114, "Clothing Bless Deed", 0, 550, ClothingBlessDeed));
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157120, "A Mythical Scroll Of Tailoring", 0x481, 575, PowerScroll, 15));
                RewardCollection.Add(new BODCollectionItem(0xF9D, 1157116, "Horned Runic Sewing Kit", CraftResources.GetHue(CraftResource.HornedLeather), 600, RunicKit, 2));
                RewardCollection.Add(new BODCollectionItem(0x14F0, 1157121, "A Legendary Scroll Of Tailoring", 0x481, 650, PowerScroll, 20));
				RewardCollection.Add(new BODCollectionItem(5062, "+3 Armor Of Tailoring Gloves", CraftResources.GetHue(CraftResource.BlazeLeather), 650, ArmorOfTailoring, 7)); //daat
				RewardCollection.Add(new BODCollectionItem(7609, "+3 Armor Of Tailoring Cap", CraftResources.GetHue(CraftResource.BlazeLeather), 650, ArmorOfTailoring, 8)); //daat
				RewardCollection.Add(new BODCollectionItem(5068, "+3 Armor Of Tailoring Tunic", CraftResources.GetHue(CraftResource.BlazeLeather), 650, ArmorOfTailoring, 9)); //daat
				RewardCollection.Add(new BODCollectionItem(5063, "+3 Armor Of Tailoring Gorget", CraftResources.GetHue(CraftResource.BlazeLeather), 650, ArmorOfTailoring, 10)); //daat
				RewardCollection.Add(new BODCollectionItem(5069, "+3 Armor Of Tailoring Arms", CraftResources.GetHue(CraftResource.BlazeLeather), 650, ArmorOfTailoring, 11)); //daat
				RewardCollection.Add(new BODCollectionItem(5067, "+3 Armor Of Tailoring Leggings", CraftResources.GetHue(CraftResource.BlazeLeather), 650, ArmorOfTailoring, 12)); //daat
                RewardCollection.Add(new BODCollectionItem(0xF9D, 1157117, "Barbed Runic Sewing Kit", CraftResources.GetHue(CraftResource.BarbedLeather), 700, RunicKit, 3));
				RewardCollection.Add(new BODCollectionItem(0x13F6, "Gargoyles Knife", 0, 750, GargoylesKnife)); //daat
				RewardCollection.Add(new BODCollectionItem(0xF9D, "Polar Runic Sewing Kit", CraftResources.GetHue(CraftResource.PolarLeather), 800, RunicKit, 4)); //daat
				RewardCollection.Add(new BODCollectionItem(0xFAB, "Charged Dye Tub", 0, 800, ChargedDyeTub)); //daat
				RewardCollection.Add(new BODCollectionItem(0xF9D, "Synthetic Runic Sewing Kit", CraftResources.GetHue(CraftResource.SyntheticLeather), 900, RunicKit, 5)); //daat
				RewardCollection.Add(new BODCollectionItem(5062, "+5 Armor Of Tailoring Gloves", CraftResources.GetHue(CraftResource.BlazeLeather), 900, ArmorOfTailoring, 13)); //daat
				RewardCollection.Add(new BODCollectionItem(7609, "+5 Armor Of Tailoring Cap", CraftResources.GetHue(CraftResource.BlazeLeather), 900, ArmorOfTailoring, 14)); //daat
				RewardCollection.Add(new BODCollectionItem(5068, "+5 Armor Of Tailoring Tunic", CraftResources.GetHue(CraftResource.BlazeLeather), 900, ArmorOfTailoring, 15)); //daat
				RewardCollection.Add(new BODCollectionItem(5063, "+5 Armor Of Tailoring Gorget", CraftResources.GetHue(CraftResource.BlazeLeather), 900, ArmorOfTailoring, 16)); //daat
				RewardCollection.Add(new BODCollectionItem(5069, "+5 Armor Of Tailoring Arms", CraftResources.GetHue(CraftResource.BlazeLeather), 900, ArmorOfTailoring, 17)); //daat
				RewardCollection.Add(new BODCollectionItem(5067, "+5 Armor Of Tailoring Leggings", CraftResources.GetHue(CraftResource.BlazeLeather), 900, ArmorOfTailoring, 18)); //daat
				RewardCollection.Add(new BODCollectionItem(0xF9D, "Blaze Runic Sewing Kit", CraftResources.GetHue(CraftResource.BlazeLeather), 950, RunicKit, 6)); //daat
				RewardCollection.Add(new BODCollectionItem(0xEC3, "Tailors Protector", 0, 1000, TailorsProtector)); //daat
				RewardCollection.Add(new BODCollectionItem(0xF9D, "Daemonic Runic Sewing Kit", CraftResources.GetHue(CraftResource.DaemonicLeather), 1000, RunicKit, 7)); //daat
				RewardCollection.Add(new BODCollectionItem(0xF9D, "Shadow Runic Sewing Kit", CraftResources.GetHue(CraftResource.ShadowLeather), 1050, RunicKit, 8)); //daat
				RewardCollection.Add(new BODCollectionItem(0xF9D, "Frost Runic Sewing Kit", CraftResources.GetHue(CraftResource.FrostLeather), 1100, RunicKit, 9)); //daat
				RewardCollection.Add(new BODCollectionItem(0xE76, "Resources Bag", 0, 1100, ResourcesBag)); //daat
				RewardCollection.Add(new BODCollectionItem(0xF9D, "Ethereal Runic Sewing Kit", CraftResources.GetHue(CraftResource.EtherealLeather), 1150, RunicKit, 10)); //daat
            }
            else
            {
                this.Groups = new RewardGroup[]
                {
                    /*new RewardGroup(0, new RewardItem(1, Cloth, 0)),
                    new RewardGroup(50, new RewardItem(1, Cloth, 1)),
                    new RewardGroup(100, new RewardItem(1, Cloth, 2)),
                    new RewardGroup(150, new RewardItem(9, Cloth, 3), new RewardItem(1, Sandals)),
                    new RewardGroup(200, new RewardItem(4, Cloth, 4), new RewardItem(1, Sandals)),
                    new RewardGroup(300, new RewardItem(1, StretchedHide)),
                    new RewardGroup(350, new RewardItem(1, RunicKit, 1)),
                    new RewardGroup(400, new RewardItem(2, PowerScroll, 5), new RewardItem(3, Tapestry)),
                    new RewardGroup(450, new RewardItem(1, BearRug)),
                    new RewardGroup(500, new RewardItem(1, PowerScroll, 10)),
                    new RewardGroup(550, new RewardItem(1, ClothingBlessDeed)),
                    new RewardGroup(575, new RewardItem(1, PowerScroll, 15)),
                    new RewardGroup(600, new RewardItem(1, RunicKit, 2)),
                    new RewardGroup(650, new RewardItem(1, PowerScroll, 20)),
                    new RewardGroup(700, new RewardItem(1, RunicKit, 3))*/
					
					//daat start
					new RewardGroup(    0, new RewardItem( 1, Cloth, 0 ), new RewardItem( 1, ColoredLoom ) ),
					new RewardGroup(   50, new RewardItem( 1, Cloth, 1 ), new RewardItem( 1, ColoredLoom ) ),
					new RewardGroup(  100, new RewardItem( 1, Cloth, 2 ), new RewardItem( 1, Sandals ) ),
					new RewardGroup(  150, new RewardItem( 6, Cloth, 3 ), new RewardItem( 1, ArmorOfTailoring, Utility.RandomMinMax(1,6) ), new RewardItem( 3, Sandals ) ),
					new RewardGroup(  200, new RewardItem( 2, Cloth, 4 ), new RewardItem( 1, ArmorOfTailoring, Utility.RandomMinMax(1,6) ), new RewardItem( 2, Sandals ) ),
					new RewardGroup(  300, new RewardItem( 1, StretchedHide ), new RewardItem( 1, ArmorOfTailoring, Utility.RandomMinMax(1,6) ), new RewardItem( 1, ColoredScissors ) ),
					new RewardGroup(  350, new RewardItem( 1, RunicKit, 1 ), new RewardItem( 1, ColoredScissors ) ),
					new RewardGroup(  400, new RewardItem( 3, Tapestry ), new RewardItem( 1, SturdySewingKit ), new RewardItem( 1, ColoredScissors ) ),
					new RewardGroup(  450, new RewardItem( 1, BearRug ), new RewardItem( 1, SturdySewingKit ) ),
					new RewardGroup(  500, new RewardItem( 1, Deco, Utility.Random(4) ), new RewardItem( 1, MastersKnife ) ),
					new RewardGroup(  550, new RewardItem( 1, ClothingBlessDeed ), new RewardItem( 1, Deco, Utility.Random(4) ), new RewardItem( 1, MastersKnife ) ),
					new RewardGroup(  600, new RewardItem( 1, RunicKit, 2 ), new RewardItem( 1, MastersKnife ) ),
					new RewardGroup(  650, new RewardItem( 1, RunicKit, 2 ), new RewardItem( 1, ArmorOfTailoring, Utility.RandomMinMax(7,12) ) ),
					new RewardGroup(  700, new RewardItem( 1, RunicKit, 3 ), new RewardItem( 1, ArmorOfTailoring, Utility.RandomMinMax(7,12) ) ),
					new RewardGroup(  750, new RewardItem( 1, RunicKit, 3 ), new RewardItem( 1, ArmorOfTailoring, Utility.RandomMinMax(7,12) ), new RewardItem( 1, GargoylesKnife ) ),
					new RewardGroup(  800, new RewardItem( 1, RunicKit, 4 ), new RewardItem( 1, ChargedDyeTub ), new RewardItem( 1, GargoylesKnife ) ),
					new RewardGroup(  850, new RewardItem( 1, RunicKit, 4 ), new RewardItem( 1, ChargedDyeTub ) ),
					new RewardGroup(  900, new RewardItem( 1, RunicKit, 5 ), new RewardItem( 1, ArmorOfTailoring, Utility.RandomMinMax(13,18) ) ),
					new RewardGroup(  950, new RewardItem( 1, RunicKit, 6 ), new RewardItem( 1, ArmorOfTailoring, Utility.RandomMinMax(13,18) ) ),
					new RewardGroup( 1000, new RewardItem( 1, RunicKit, 7 ), new RewardItem( 1, TailorsProtector ), new RewardItem( 1, ArmorOfTailoring, Utility.RandomMinMax(13,18) ) ),
					new RewardGroup( 1050, new RewardItem( 1, RunicKit, 8 ), new RewardItem( 1, TailorsProtector ) ),
					new RewardGroup( 1100, new RewardItem( 1, RunicKit, 9 ), new RewardItem( 1, ResourcesBag ) ),
					new RewardGroup( 1150, new RewardItem( 1, RunicKit, 10 ), new RewardItem( 1, ResourcesBag ) )
					//daat end
                };
            }
        }

        #region Constructors
        private static readonly ConstructCallback SewingKit = new ConstructCallback(CreateSewingKit);
        private static readonly ConstructCallback Cloth = new ConstructCallback(CreateCloth);
        private static readonly ConstructCallback Sandals = new ConstructCallback(CreateSandals);
        private static readonly ConstructCallback StretchedHide = new ConstructCallback(CreateStretchedHide);
        private static readonly ConstructCallback RunicKit = new ConstructCallback(CreateRunicKit);
        private static readonly ConstructCallback Tapestry = new ConstructCallback(CreateTapestry);
        private static readonly ConstructCallback PowerScroll = new ConstructCallback(CreatePowerScroll);
        private static readonly ConstructCallback BearRug = new ConstructCallback(CreateBearRug);
        private static readonly ConstructCallback ClothingBlessDeed = new ConstructCallback(CreateCBD);
        private static readonly ConstructCallback CraftsmanTalisman = new ConstructCallback(CreateCraftsmanTalisman);
		//daat99 OWLTR start - bod reward
		private static readonly ConstructCallback ArmorOfTailoring = new ConstructCallback(CreateArmorOfTailoring);
		private static readonly ConstructCallback TailorsProtector = new ConstructCallback(CreateTailorsProtector);
		private static readonly ConstructCallback SturdySewingKit = new ConstructCallback(CreateSturdySewingKit);
		private static readonly ConstructCallback MastersKnife = new ConstructCallback(CreateMastersKnife);
		private static readonly ConstructCallback GargoylesKnife = new ConstructCallback(CreateGargoylesKnife);
		private static readonly ConstructCallback ColoredScissors = new ConstructCallback(CreateColoredScissors);
		private static readonly ConstructCallback ColoredLoom = new ConstructCallback(CreateColoredLoom);
		private static readonly ConstructCallback ChargedDyeTub = new ConstructCallback(CreateChargedDyeTub);
		private static readonly ConstructCallback ResourcesBag = new ConstructCallback(CreateResourcesBag);
		private static readonly ConstructCallback Deco = new ConstructCallback(CreateDeco);

		private static Item CreateArmorOfTailoring(int type)
		{
			switch (type)
			{
				case 1: default: return new ArmorOfTailoring(1, 5062, 2); //gloves
				case 2: return new ArmorOfTailoring(1, 7609, 2); //cap
				case 3: return new ArmorOfTailoring(1, 5068, 2); //tunic
				case 4: return new ArmorOfTailoring(1, 5063, 2); //gorget
				case 5: return new ArmorOfTailoring(1, 5069, 2); //arms
				case 6: return new ArmorOfTailoring(1, 5067, 2); //leggings
				case 7: return new ArmorOfTailoring(3, 5062, 2); //gloves
				case 8: return new ArmorOfTailoring(3, 7609, 2); //cap
				case 9: return new ArmorOfTailoring(3, 5068, 2); //tunic
				case 10: return new ArmorOfTailoring(3, 5063, 2); //gorget
				case 11: return new ArmorOfTailoring(3, 5069, 2); //arms
				case 12: return new ArmorOfTailoring(3, 5067, 2); //leggings
				case 13: return new ArmorOfTailoring(5, 5062, 2); //gloves
				case 14: return new ArmorOfTailoring(5, 7609, 2); //cap
				case 15: return new ArmorOfTailoring(5, 5068, 2); //tunic
				case 16: return new ArmorOfTailoring(5, 5063, 2); //gorget
				case 17: return new ArmorOfTailoring(5, 5069, 2); //arms
				case 18: return new ArmorOfTailoring(5, 5067, 2); //leggings
			}
		}

		private static Item CreateTailorsProtector(int type)
		{
			return new TailorsProtector();
		}

		private static Item CreateSturdySewingKit(int type)
		{
			return new SturdySewingKit();
		}

		private static Item CreateGargoylesKnife(int type)
		{
			return new GargoylesKnife();
		}

		private static Item CreateMastersKnife(int type)
		{
			return new MastersKnife();
		}

		private static Item CreateColoredScissors(int type)
		{
			return new ColoredScissors(CraftResources.GetHue((CraftResource)Utility.RandomMinMax((int)CraftResource.SpinedLeather, (int)CraftResource.EtherealLeather)), 25);
		}

		private static Item CreateColoredLoom(int type)
		{
			if (Utility.Random(2) == 1)
				return new ColoredLoomSouthDeed(CraftResources.GetHue((CraftResource)Utility.RandomMinMax((int)CraftResource.SpinedLeather, (int)CraftResource.EtherealLeather)));
			else
				return new ColoredLoomEastDeed(CraftResources.GetHue((CraftResource)Utility.RandomMinMax((int)CraftResource.SpinedLeather, (int)CraftResource.EtherealLeather)));
		}

		private static Item CreateChargedDyeTub(int type)
		{
			return new ChargedDyeTub(0);
		}

		private static Item CreateResourcesBag(int type)
		{
			return new ResourcesBag();
		}

		private static Item CreateDeco(int type)
		{
			switch (type)
			{
				case 0: default: return new Deco(4054, "Tapestry");
				case 1: return new Deco(9036, "Rose of Trinsic");
				case 2: return new Deco(15721, "Deer Corspe");
				case 3: return new Deco(5610, "Banner");
			}
		}
		//daat99 OWLTR end - bod reward

        private static Item CreateSewingKit(int type)
        {
            var kit = new SewingKit();
            kit.UsesRemaining = 250;

            return kit;
        }

        private static readonly int[][] m_ClothHues = new int[][]
        {
            new int[] { 0x483, 0x48C, 0x488, 0x48A },
            new int[] { 0x495, 0x48B, 0x486, 0x485 },
            new int[] { 0x48D, 0x490, 0x48E, 0x491 },
            new int[] { 0x48F, 0x494, 0x484, 0x497 },
            new int[] { 0x489, 0x47F, 0x482, 0x47E },
            new int[] { 0xAAC, 0xAB4, 0xAAF, 0xAB5 },
        };

        private static Item CreateCloth(int type)
        {
            if (type >= 0 && type < m_ClothHues.Length)
            {
                UncutCloth cloth = new UncutCloth(100);
                cloth.Hue = m_ClothHues[type][Utility.Random(m_ClothHues[type].Length)];
                return cloth;
            }

            throw new InvalidOperationException();
        }

        private static readonly int[] m_SandalHues = new int[]
        {
            0x489, 0x47F, 0x482,
            0x47E, 0x48F, 0x494,
            0x484, 0x497
        };

        private static Item CreateSandals(int type)
        {
            return new Sandals(m_SandalHues[Utility.Random(m_SandalHues.Length)]);
        }

        private static Item CreateStretchedHide(int type)
        {
            switch ( Utility.Random(4) )
            {
                default:
                case 0:
                    return new SmallStretchedHideEastDeed();
                case 1:
                    return new SmallStretchedHideSouthDeed();
                case 2:
                    return new MediumStretchedHideEastDeed();
                case 3:
                    return new MediumStretchedHideSouthDeed();
            }
        }

        private static Item CreateTapestry(int type)
        {
            switch ( Utility.Random(4) )
            {
                default:
                case 0:
                    return new LightFlowerTapestryEastDeed();
                case 1:
                    return new LightFlowerTapestrySouthDeed();
                case 2:
                    return new DarkFlowerTapestryEastDeed();
                case 3:
                    return new DarkFlowerTapestrySouthDeed();
            }
        }

        private static Item CreateBearRug(int type)
        {
            switch ( Utility.Random(4) )
            {
                default:
                case 0:
                    return new BrownBearRugEastDeed();
                case 1:
                    return new BrownBearRugSouthDeed();
                case 2:
                    return new PolarBearRugEastDeed();
                case 3:
                    return new PolarBearRugSouthDeed();
            }
        }

        private static Item CreateRunicKit(int type)
        {
            /*if (type >= 1 && type <= 3)
                return new RunicSewingKit(CraftResource.RegularLeather + type, 60 - (type * 15));*/
			//daat99 OWLTR start - bod reward
			if ( type >= 1 && type <= 10)
				return new RunicSewingKit(CraftResource.RegularLeather + type, 100 - (type * 5));
			//daat99 OWLTR end - bod reward

			throw new InvalidOperationException();
        }

        private static Item CreatePowerScroll(int type)
        {
            if (type == 5 || type == 10 || type == 15 || type == 20)
                return new PowerScroll(SkillName.Tailoring, 100 + type);

            throw new InvalidOperationException();
        }

        private static Item CreateCBD(int type)
        {
            return new ClothingBlessDeed();
        }

        private static Item CreateCraftsmanTalisman(int type)
        {
            return new MasterCraftsmanTalisman(type, 0x9E25, TalismanSkill.Tailoring);
        }

        #endregion

        public static readonly TailorRewardCalculator Instance = new TailorRewardCalculator();

        public override int ComputePoints(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int points = 0;

            if (quantity == 10)
                points += 10;
            else if (quantity == 15)
                points += 25;
            else if (quantity == 20)
                points += 50;

            if (exceptional)
                points += 100;

            if (itemCount == 4)
                points += 300;
            else if (itemCount == 5)
                points += 400;
            else if (itemCount == 6)
                points += 500;

            /*if (material == BulkMaterialType.Spined)
                points += 50;
            else if (material == BulkMaterialType.Horned)
                points += 100;
            else if (material == BulkMaterialType.Barbed)
                points += 150;*/
			//daat99 OWLTR start - bod rewards
			if ( material >= BulkMaterialType.Spined && material <= BulkMaterialType.Ethereal)
				points += 50 + (50 * (material - BulkMaterialType.Spined));
			//daat99 OWLTR end - bod rewards

            return points;
        }

        private static readonly int[][][] m_AosGoldTable = new int[][][]
        {
            /*new int[][] // 1-part (regular)
            {
                new int[] { 150, 150, 300, 300 },
                new int[] { 225, 225, 450, 450 },
                new int[] { 300, 400, 600, 750 }
            },
            new int[][] // 1-part (exceptional)
            {
                new int[] { 300, 300, 600, 600 },
                new int[] { 450, 450, 900, 900 },
                new int[] { 600, 750, 1200, 1800 }
            },
            new int[][] // 4-part (regular)
            {
                new int[] { 4000, 4000, 5000, 5000 },
                new int[] { 6000, 6000, 7500, 7500 },
                new int[] { 8000, 10000, 10000, 15000 }
            },
            new int[][] // 4-part (exceptional)
            {
                new int[] { 5000, 5000, 7500, 7500 },
                new int[] { 7500, 7500, 11250, 11250 },
                new int[] { 10000, 15000, 15000, 20000 }
            },
            new int[][] // 5-part (regular)
            {
                new int[] { 5000, 5000, 7500, 7500 },
                new int[] { 7500, 7500, 11250, 11250 },
                new int[] { 10000, 15000, 15000, 20000 }
            },
            new int[][] // 5-part (exceptional)
            {
                new int[] { 7500, 7500, 10000, 10000 },
                new int[] { 11250, 11250, 15000, 15000 },
                new int[] { 15000, 20000, 20000, 30000 }
            },
            new int[][] // 6-part (regular)
            {
                new int[] { 7500, 7500, 10000, 10000 },
                new int[] { 11250, 11250, 15000, 15000 },
                new int[] { 15000, 20000, 20000, 30000 }
            },
            new int[][] // 6-part (exceptional)
            {
                new int[] { 10000, 10000, 15000, 15000 },
                new int[] { 15000, 15000, 22500, 22500 },
                new int[] { 20000, 30000, 30000, 50000 }
            }*/

			//daat start
			new int[][] // 1-part (regular)
            {
					new int[]{ 100, 200, 300,  400,  500,  600,  700,  800,  900, 1000, 1100 },
					new int[]{ 200, 400, 600,  800, 1000, 1200, 1400, 1600, 1800, 2000, 2200 },
					new int[]{ 300, 600, 900, 1200, 1500, 1800, 2100, 2400, 2700, 3000, 3300 }
			},
			new int[][] // 1-part (exceptional)
            {
					new int[]{ 250, 500,  750, 1000, 1250, 1500, 1750, 2000,  2250, 2500, 2750 },
					new int[]{ 350, 700, 1050, 1400, 1750, 2100, 2450, 2800,  3150, 3500, 3850 },
					new int[]{ 450, 900, 1350, 1800, 2250, 2700, 3150, 3600,  4050, 4500, 4950 }
			},
			new int[][] // 4-part (regular)
            {
					new int[]{ 2000, 4000,  6000,  8000, 10000, 12000, 14000, 16000, 18000, 20000, 22000 },
					new int[]{ 3000, 6000,  9000, 12000, 15000, 18000, 21000, 24000, 27000, 30000, 33000 },
					new int[]{ 4000, 8000, 12000, 16000, 20000, 24000, 28000, 32000, 36000, 40000, 44000 }
			},
			new int[][] // 4-part (exceptional)
            {
					new int[]{ 4000,  8000, 12000, 16000, 20000, 24000, 28000, 32000, 36000, 40000, 44000 },
					new int[]{ 6000, 12000, 18000, 24000, 30000, 36000, 42000, 48000, 54000, 60000, 66000 },
					new int[]{ 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 72000, 80000, 88000 }
			},
			new int[][] // 5-part (regular)
            {
					new int[]{ 4000,  8000, 12000, 16000, 20000, 24000, 28000, 32000, 36000, 40000, 44000 },
					new int[]{ 6000, 12000, 18000, 24000, 30000, 36000, 42000, 48000, 54000, 60000, 66000 },
					new int[]{ 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 72000, 80000, 88000 }
			},
			new int[][] // 5-part (exceptional)
            {
					new int[]{  7000, 14000, 21000, 28000, 35000, 42000,  49000,  56000,  63000,  70000,  77000 },
					new int[]{ 10000, 20000, 30000, 40000, 50000, 60000,  70000,  80000,  90000, 100000, 110000 },
					new int[]{ 15000, 30000, 45000, 60000, 75000, 90000, 105000, 120000, 135000, 150000, 165000 }
			},
			new int[][] // 6-part (regular)
            {
					new int[]{  5000, 10000, 15000, 20000, 25000, 30000,  35000,  40000,  45000,  50000,  55000 },
					new int[]{  7500, 15000, 22500, 30000, 37500, 45000,  52500,  60000,  67500,  75000,  82500 },
					new int[]{ 10000, 20000, 30000, 40000, 50000, 60000,  70000,  80000,  90000, 100000, 110000 }
			},
			new int[][] // 6-part (exceptional)
            {
					new int[]{ 10000, 20000, 30000, 40000,  50000,  60000,  70000,  80000,  90000, 100000, 110000 },
					new int[]{ 15000, 30000, 45000, 60000,  75000,  90000, 105000, 120000, 135000, 150000, 165000 },
					new int[]{ 20000, 40000, 60000, 80000, 100000, 120000, 140000, 160000, 180000, 200000, 220000 }
			}
			//daat end
        };

		//daat start
		private static int[][][] m_OldGoldTable = m_AosGoldTable;
		//daat end

        /*private static readonly int[][][] m_OldGoldTable = new int[][][]
        {
            new int[][] // 1-part (regular)
            {
                new int[] { 150, 150, 300, 300 },
                new int[] { 225, 225, 450, 450 },
                new int[] { 300, 400, 600, 750 }
            },
            new int[][] // 1-part (exceptional)
            {
                new int[] { 300, 300, 600, 600 },
                new int[] { 450, 450, 900, 900 },
                new int[] { 600, 750, 1200, 1800 }
            },
            new int[][] // 4-part (regular)
            {
                new int[] { 3000, 3000, 4000, 4000 },
                new int[] { 4500, 4500, 6000, 6000 },
                new int[] { 6000, 8000, 8000, 10000 }
            },
            new int[][] // 4-part (exceptional)
            {
                new int[] { 4000, 4000, 5000, 5000 },
                new int[] { 6000, 6000, 7500, 7500 },
                new int[] { 8000, 10000, 10000, 15000 }
            },
            new int[][] // 5-part (regular)
            {
                new int[] { 4000, 4000, 5000, 5000 },
                new int[] { 6000, 6000, 7500, 7500 },
                new int[] { 8000, 10000, 10000, 15000 }
            },
            new int[][] // 5-part (exceptional)
            {
                new int[] { 5000, 5000, 7500, 7500 },
                new int[] { 7500, 7500, 11250, 11250 },
                new int[] { 10000, 15000, 15000, 20000 }
            },
            new int[][] // 6-part (regular)
            {
                new int[] { 5000, 5000, 7500, 7500 },
                new int[] { 7500, 7500, 11250, 11250 },
                new int[] { 10000, 15000, 15000, 20000 }
            },
            new int[][] // 6-part (exceptional)
            {
                new int[] { 7500, 7500, 10000, 10000 },
                new int[] { 11250, 11250, 15000, 15000 },
                new int[] { 15000, 20000, 20000, 30000 }
            }
        };*/

        public override int ComputeGold(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int gold = 0;

            if (itemCount == 1 && BulkOrderSystem.NewSystemEnabled && BulkOrderSystem.ComputeGold(type, quantity, out gold))
            {
                return gold;
            }

            int[][][] goldTable = (Core.AOS ? m_AosGoldTable : m_OldGoldTable);

            int typeIndex = ((itemCount == 6 ? 3 : itemCount == 5 ? 2 : itemCount == 4 ? 1 : 0) * 2) + (exceptional ? 1 : 0);
            int quanIndex = (quantity == 20 ? 2 : quantity == 15 ? 1 : 0);
            //daat99 OWLTR start - bod material
			int mtrlIndex = (material >= BulkMaterialType.Spined && material <= BulkMaterialType.Ethereal) ? 1 + (int)(material - BulkMaterialType.Spined) : 0;
			//int mtrlIndex = (material == BulkMaterialType.Barbed ? 3 : material == BulkMaterialType.Horned ? 2 : material == BulkMaterialType.Spined ? 1 : 0);
			//daat99 OWLTR end - bod material

            gold = goldTable[typeIndex][quanIndex][mtrlIndex];

            int min = (gold * 9) / 10;
            int max = (gold * 10) / 9;

            return Utility.RandomMinMax(min, max);
        }
    }
    #endregion
     
    #region Tinkering Rewards
    public sealed class TinkeringRewardCalculator : RewardCalculator
    {
        public TinkeringRewardCalculator()
        {
            RewardCollection = new List<CollectionItem>();

            RewardCollection.Add(new BODCollectionItem(0x1EBC, 1157219, 0, 10, TinkerTools));
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157186, 0, 25, RewardTitle, 5)); // metallurgist
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157187, 0, 50, RewardTitle, 6)); // dabbler
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157190, 0, 210, RewardTitle, 9)); // fixer
            RewardCollection.Add(new BODCollectionItem(0x2831, 1157288, 0, 225, Recipe, 0));
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157188, 0, 250, RewardTitle, 7)); // clockmaker
            RewardCollection.Add(new BODCollectionItem(0x2831, 1157287, 0, 310, Recipe, 1));
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157189, 0, 225, RewardTitle, 8)); // mechanic
            RewardCollection.Add(new BODCollectionItem(0x2831, 1157289, 0, 350, Recipe, 2));
            RewardCollection.Add(new BODCollectionItem(0x9E2B, 1157264, 0, 400, CraftsmanTalisman, 10));
            RewardCollection.Add(new BODCollectionItem(0x2F5B, 1152674, CraftResources.GetHue(CraftResource.Gold), 450, SmeltersTalisman, (int)CraftResource.Gold));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152665, CraftResources.GetHue(CraftResource.Gold), 500, HarvestMap, (int)CraftResource.Gold));
            RewardCollection.Add(new BODCollectionItem(0x9E2B, 1157218, 0, 550, CraftsmanTalisman, 25)); // todo: Get id
            RewardCollection.Add(new BODCollectionItem(0x2F5B, 1152675, CraftResources.GetHue(CraftResource.Agapite), 600, SmeltersTalisman, (int)CraftResource.Agapite));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152666, CraftResources.GetHue(CraftResource.Agapite), 650, HarvestMap, (int)CraftResource.Agapite));
            RewardCollection.Add(new BODCollectionItem(0x1940, 1157221, 0, 700, CreateItem, 0)); // powder of fort keg
            RewardCollection.Add(new BODCollectionItem(0x9CE9, 1157290, 0, 750, CreateItem, 1)); // automaton actuator
            RewardCollection.Add(new BODCollectionItem(0x2F5B, 1152676, CraftResources.GetHue(CraftResource.Verite), 800, SmeltersTalisman, (int)CraftResource.Verite));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152667, CraftResources.GetHue(CraftResource.Verite), 850, HarvestMap, (int)CraftResource.Verite));
            RewardCollection.Add(new BODCollectionItem(0x9E2B, 1157265, 0, 900, CraftsmanTalisman, 50));
            RewardCollection.Add(new BODCollectionItem(0x9E7E, 1157216, 0, 950, RockHammer));
            RewardCollection.Add(new BODCollectionItem(0x9CAA, 1157286, 1175, 1000, CreateItem, 2));
            RewardCollection.Add(new BODCollectionItem(0x2F5B, 1152677, CraftResources.GetHue(CraftResource.Valorite), 1050, SmeltersTalisman, (int)CraftResource.Valorite));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152668, CraftResources.GetHue(CraftResource.Valorite), 1100, HarvestMap, (int)CraftResource.Valorite));
            RewardCollection.Add(new BODCollectionItem(0x9DB1, 1157220, 1175, 1200, CreateItem, 3));
        }

        #region Constructors
        // Do I need these since they aren't era-specific???

        private static Item TinkerTools(int type)
        {
            BaseTool tool = new TinkerTools();
            tool.UsesRemaining = 250;

            return tool;
        }

        public static Item CreateItem(int type)
        {
            switch (type)
            {
                case 0: return new PowderOfFortKeg();
                case 1: return new AutomatonActuator();
                case 2: return new BlackrockMoonstone();
                case 3: return new BlackrockAutomatonHead();
            }

            return null;
        }

        private static Item CraftsmanTalisman(int type)
        {
            return new MasterCraftsmanTalisman(type, 0x9E2B, TalismanSkill.Tinkering);
        }
        #endregion

        public static readonly TinkeringRewardCalculator Instance = new TinkeringRewardCalculator();

        public override int ComputePoints(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int points = 0;

            if (quantity == 10)
                points += 10;
            else if (quantity == 15)
                points += 25;
            else if (quantity == 20)
                points += 50;

            switch (itemCount)
            {
                case 3: points += 200; break;
                case 4: points += 300; break;
                case 5: points += 350; break;
                case 6: points += 400; break;
            }

			// daat99 OWLTR start -custom resources
			if (material >= BulkMaterialType.DullCopper && material <= BulkMaterialType.Platinum)
            //if (material >= BulkMaterialType.DullCopper && material <= BulkMaterialType.Valorite)
			//daat99 OWLTR end - custom resources
                points += 200 + (50 * (material - BulkMaterialType.DullCopper));

            if (exceptional)
                points += 200;

            return points;
        }

        private static readonly int[][][] m_GoldTable = new int[][][]
        {
            /*new int[][] // 1-part (regular)
            {
                new int[] { 150, 150, 225, 225, 300, 300, 350, 350, 400 },
                new int[] { 225, 225, 325, 325, 450, 450, 450, 500, 500 },
                new int[] { 300, 400, 500, 500, 600, 750, 750, 900, 900 }
            },
            new int[][] // 1-part (exceptional)
            {
                new int[] { 300, 300, 400, 500, 600, 600, 600, 700, 700 },
                new int[] { 450, 450, 650, 750, 900, 900, 900, 1000, 1000 },
                new int[] { 600, 750, 850, 1000, 1200, 1800, 1800, 1800, 2000 }
            },
            new int[][] // 3-part (regular)
            {
                new int[] { 2500, 2500, 2500, 3500, 3500, 3500, 4500, 4500, 4500 },
                new int[] { 4000, 4000, 4000, 5500, 5500, 5500, 7000, 7000, 7000 },
                new int[] { 6000, 7000, 7500, 8000, 8000, 9000, 9000, 10000, 10000 }
            },
            new int[][] // 3-part (exceptional)
            {
                new int[] { 4000, 4000, 5000, 5750, 6500, 6500, 6500, 7500, 8500 },
                new int[] { 6500, 6500, 7500, 8500, 10000, 10000, 10000, 12500, 12500 },
                new int[] { 8000, 10000, 10000, 12500, 12500, 15000, 15000, 20000, 20000 }
            },
            new int[][] // 4-part (regular)
            {
                new int[] { 4000, 4000, 4000, 5000, 5000, 5000, 6000, 6000, 6000 },
                new int[] { 6000, 6000, 6000, 7500, 7500, 7500, 9000, 9000, 9000 },
                new int[] { 8000, 9000, 9500, 10000, 10000, 15000, 17500, 17500, 17500 }
            },
            new int[][] // 4-part (exceptional)
            {
                new int[] { 5000, 5000, 6000, 6750, 7500, 7500, 8500, 8500, 9500 },
                new int[] { 7500, 7500, 8500, 9500, 11250, 11250, 11250, 15000, 15000 },
                new int[] { 10000, 1250, 1250, 15000, 15000, 20000, 20000, 25000, 25000 }
            },
            new int[][] // 4-part (regular)
            {
                new int[] { 4000, 4000, 4000, 5000, 5000, 5000, 7000, 7000, 7000 },
                new int[] { 6000, 6000, 6000, 7500, 7500, 7500, 9000, 9000, 9000 },
                new int[] { 8000, 9000, 9500, 10000, 10000, 15000, 15000, 20000, 20000 }
            },
            new int[][] // 4-part (exceptional)
            {
                new int[] { 5000, 5000, 6000, 6750, 7500, 7500, 9000, 9000, 15000 },
                new int[] { 7500, 7500, 8500, 9500, 11250, 11250, 15000, 15000, 15000 },
                new int[] { 10000, 1250, 1250, 15000, 15000, 20000, 20000, 25000, 25000 }
            },
            new int[][] // 5-part (regular)
            {
                new int[] { 5000, 5000, 60000, 6000, 7500, 7500, 9000, 9000, 10500 },
                new int[] { 7500, 7500, 7500, 11250, 11250, 11250, 15000, 15000, 15000 },
                new int[] { 10000, 10000, 1250, 15000, 15000, 20000, 20000, 25000, 25000 }
            },
            new int[][] // 5-part (exceptional)
            {
                new int[] { 7500, 7500, 8500, 9500, 10000, 10000, 12500, 12500, 15000 },
                new int[] { 11250, 11250, 1250, 13500, 15000, 15000, 20000, 20000, 25000 },
                new int[] { 15000, 1750, 1750, 20000, 20000, 30000, 30000, 40000, 50000 }
            },*/

			//daat99 OWLTR start - custom gold reward
            new int[][] // 1-part (regular)
            {
				new int[]{ 100, 200, 300,  400,  500,  600,  700,  800,  900, 1000, 1100, 1200, 1300, 1400 },
				new int[]{ 200, 400, 600,  800, 1000, 1200, 1400, 1600, 1800, 2000, 2200, 2400, 2600, 2800 },
				new int[]{ 300, 600, 900, 1200, 1500, 1800, 2100, 2400, 2700, 3000, 3300, 3600, 3900, 4200 }
			},
			new int[][] // 1-part (exceptional)
            {
				new int[]{ 250, 500,  750, 1000, 1250, 1500, 1750, 2000,  2250, 2500, 2750, 3000, 3250, 3500 },
				new int[]{ 350, 700, 1050, 1400, 1750, 2100, 2450, 2800,  3150, 3500, 3850, 4200, 4550, 4900 },
				new int[]{ 450, 900, 1350, 1800, 2250, 2700, 3150, 3600,  4050, 4500, 4950, 5400, 5850, 6300 }
			},
			new int[][] // 3-part (regular)
            {
				new int[]{ 2000, 4000,  6000,  8000, 10000, 12000, 14000, 16000, 18000, 20000, 22000, 24000, 26000, 28000 },
				new int[]{ 3000, 6000,  9000, 12000, 15000, 18000, 21000, 24000, 27000, 30000, 33000, 36000, 39000, 42000 },
				new int[]{ 4000, 8000, 12000, 16000, 20000, 24000, 28000, 32000, 36000, 40000, 44000, 48000, 52000, 56000 }
			},
			new int[][] // 3-part (exceptional)
            {
				new int[]{ 4000,  8000, 12000, 16000, 20000, 24000, 28000, 32000, 36000, 40000, 44000, 48000,  52000,  56000 },
				new int[]{ 6000, 12000, 18000, 24000, 30000, 36000, 42000, 48000, 54000, 60000, 66000, 72000,  78000,  84000 },
				new int[]{ 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 72000, 80000, 88000, 96000, 104000, 112000 }
			},
			new int[][] // 4-part (regular)
            {
				new int[]{ 4000,  8000, 12000, 16000, 20000, 24000, 28000, 32000, 36000, 40000, 44000, 48000,  52000,  56000 },
				new int[]{ 6000, 12000, 18000, 24000, 30000, 36000, 42000, 48000, 54000, 60000, 66000, 72000,  78000,  84000 },
				new int[]{ 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 72000, 80000, 88000, 96000, 104000, 112000 }
			},
			new int[][] // 4-part (exceptional)
            {
				new int[]{  7000, 14000, 21000, 28000, 35000, 42000,  49000,  56000,  63000,  70000,  77000,  84000,  91000,  98000 },
				new int[]{ 10000, 20000, 30000, 40000, 50000, 60000,  70000,  80000,  90000, 100000, 110000, 120000, 130000, 140000 },
				new int[]{ 15000, 30000, 45000, 60000, 75000, 90000, 105000, 120000, 135000, 150000, 165000, 180000, 195000, 210000 }
			},
			new int[][] // 4-part (regular)
            {
				new int[]{  5000, 10000, 15000, 20000, 25000, 30000,  35000,  40000,  45000,  50000,  55000,  60000,  65000,  70000 },
				new int[]{  7500, 15000, 22500, 30000, 37500, 45000,  52500,  60000,  67500,  75000,  82500,  90000,  97500, 105000 },
				new int[]{ 10000, 20000, 30000, 40000, 50000, 60000,  70000,  80000,  90000, 100000, 110000, 120000, 130000, 140000 }
			},
			new int[][] // 4-part (exceptional)
            {
				new int[]{ 10000, 20000, 30000, 40000,  50000,  60000,  70000,  80000,  90000, 100000, 110000, 120000, 130000, 140000 },
				new int[]{ 15000, 30000, 45000, 60000,  75000,  90000, 105000, 120000, 135000, 150000, 165000, 180000, 195000, 210000 },
				new int[]{ 20000, 40000, 60000, 80000, 100000, 120000, 140000, 160000, 180000, 200000, 220000, 240000, 260000, 280000 }
			},
			new int[][] // 5-part (regular)
            {
				new int[]{ 10000, 20000, 30000, 40000,  50000,  60000,  70000,  80000,  90000, 100000, 110000, 120000, 130000, 140000 },
				new int[]{ 15000, 30000, 45000, 60000,  75000,  90000, 105000, 120000, 135000, 150000, 165000, 180000, 195000, 210000 },
				new int[]{ 20000, 40000, 60000, 80000, 100000, 120000, 140000, 160000, 180000, 200000, 220000, 240000, 260000, 280000 }
			},
			new int[][] // 5-part (exceptional)
            {
				new int[]{ 10000, 20000, 30000, 40000,  50000,  60000,  70000,  80000,  90000, 100000, 110000, 120000, 130000, 140000 },
				new int[]{ 15000, 30000, 45000, 60000,  75000,  90000, 105000, 120000, 135000, 150000, 165000, 180000, 195000, 210000 },
				new int[]{ 20000, 40000, 60000, 80000, 100000, 120000, 140000, 160000, 180000, 200000, 220000, 240000, 260000, 280000 }
			}
			//daat99 OWLTR start - custom gold reward
        };

        public override int ComputeGold(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int gold = 0;

            if (itemCount == 1 && BulkOrderSystem.NewSystemEnabled && BulkOrderSystem.ComputeGold(type, quantity, out gold))
            {
                return gold;
            }

            int[][][] goldTable = m_GoldTable;

            int typeIndex = ((itemCount == 6 ? 3 : itemCount == 5 ? 2 : itemCount == 4 ? 1 : 0) * 2) + (exceptional ? 1 : 0);
            int quanIndex = (quantity == 20 ? 2 : quantity == 15 ? 1 : 0);
            //daat99 OWLTR start - custom resource
            int mtrlIndex = (material >= BulkMaterialType.DullCopper && material <= BulkMaterialType.Platinum) ? 1 + (int)(material - BulkMaterialType.DullCopper) : 0;
			//daat99 OWLTR end - custom resource
			//int mtrlIndex = (material >= BulkMaterialType.DullCopper && material <= BulkMaterialType.Valorite) ? 1 + (int)(material - BulkMaterialType.DullCopper) : 0;

            gold = goldTable[typeIndex][quanIndex][mtrlIndex];

            int min = (gold * 9) / 10;
            int max = (gold * 10) / 9;

            return Utility.RandomMinMax(min, max);
        }
    }
    #endregion

    #region Carpentry Rewards
    public sealed class CarpentryRewardCalculator : RewardCalculator
    {
        public CarpentryRewardCalculator()
        {
            RewardCollection = new List<CollectionItem>();

            /*RewardCollection.Add(new BODCollectionItem(0x1028, 1157219, 0, 10, DovetailSaw));
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157191, 0, 25, RewardTitle, 10));
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157192, 0, 50, RewardTitle, 11));
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157193, 0, 250, RewardTitle, 12));
            RewardCollection.Add(new BODCollectionItem(0x9E2C, 1157264, 0, 300, CraftsmanTalisman, 10));
            RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152678, CraftResources.GetHue(CraftResource.YewWood), 350, WoodsmansTalisman, (int)CraftResource.YewWood));
            RewardCollection.Add(new BODCollectionItem(0x9E2C, 1157218, 0, 450, CraftsmanTalisman, 25));
            RewardCollection.Add(new BODCollectionItem(0x12B3, 1157293, CraftResources.GetHue(CraftResource.DullCopper), 450, RunicMalletAndChisel, 1));
            RewardCollection.Add(new BODCollectionItem(0x12B3, 1157294, CraftResources.GetHue(CraftResource.ShadowIron), 450, RunicMalletAndChisel, 2));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152669, CraftResources.GetHue(CraftResource.YewWood), 500, HarvestMap, (int)CraftResource.YewWood));
            RewardCollection.Add(new BODCollectionItem(0x1029, 1157223, CraftResources.GetHue(CraftResource.OakWood), 550, RunicDovetailSaw, 0));
            RewardCollection.Add(new BODCollectionItem(0x12B3, 1157295, CraftResources.GetHue(CraftResource.Copper), 600, RunicMalletAndChisel, 3));
            RewardCollection.Add(new BODCollectionItem(0x12B3, 1157296, CraftResources.GetHue(CraftResource.Bronze), 650, RunicMalletAndChisel, 4));
            RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152679, CraftResources.GetHue(CraftResource.Heartwood), 650, WoodsmansTalisman, (int)CraftResource.Heartwood));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152670, CraftResources.GetHue(CraftResource.Heartwood), 700, HarvestMap, (int)CraftResource.Heartwood));
            RewardCollection.Add(new BODCollectionItem(0x1029, 1157224, CraftResources.GetHue(CraftResource.AshWood), 750, RunicDovetailSaw, 1));
            RewardCollection.Add(new BODCollectionItem(0x9E2C, 1157265, 0, 800, CraftsmanTalisman, 50));
            RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152680, CraftResources.GetHue(CraftResource.Bloodwood), 850, WoodsmansTalisman, (int)CraftResource.Bloodwood));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152671, CraftResources.GetHue(CraftResource.Bloodwood), 900, HarvestMap, (int)CraftResource.Bloodwood));
            RewardCollection.Add(new BODCollectionItem(0x12B3, 1157297, CraftResources.GetHue(CraftResource.Gold), 900, RunicMalletAndChisel, 5));
            RewardCollection.Add(new BODCollectionItem(0x1029, 1157225, CraftResources.GetHue(CraftResource.YewWood), 950, RunicDovetailSaw, 2));
            RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152681, CraftResources.GetHue(CraftResource.Frostwood), 1000, WoodsmansTalisman, (int)CraftResource.Frostwood));
            RewardCollection.Add(new BODCollectionItem(0x12B3, 1157298, CraftResources.GetHue(CraftResource.Agapite), 1000, RunicMalletAndChisel, 6));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152672, CraftResources.GetHue(CraftResource.Frostwood), 1050, HarvestMap, (int)CraftResource.Frostwood));
            RewardCollection.Add(new BODCollectionItem(0x12B3, 1157299, CraftResources.GetHue(CraftResource.Verite), 1100, RunicMalletAndChisel, 7));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1157226, CraftResources.GetHue(CraftResource.Heartwood), 1150, RunicDovetailSaw, 3));
            RewardCollection.Add(new BODCollectionItem(0x12B3, 1157300, CraftResources.GetHue(CraftResource.Valorite), 1150, RunicMalletAndChisel, 8));*/

			RewardCollection.Add(new BODCollectionItem(0xF49, "Sturdy Lumberjack Axe", 0, 0, SturdyLumberjackAxe));
			RewardCollection.Add(new BODCollectionItem(0x1028, 1157219, "Dovetail Saw", 0, 10, DovetailSaw));
			RewardCollection.Add(new BODCollectionItem(0x13C6, "Leather Gloves Of Lumberjacking", 0, 10, LeatherGlovesOfLumberjacking, 2));
			RewardCollection.Add(new BODCollectionItem(0x14F0, 1157191, "Woodworker Reward Title", 0, 25, RewardTitle, 10));
			RewardCollection.Add(new BODCollectionItem(0x13C6, "Leather Gloves Of Lumberjacking", 0, 25, LeatherGlovesOfLumberjacking, 5));
			RewardCollection.Add(new BODCollectionItem(0xF49, "Sturdy Axe", 0, 25, SturdyAxe));
			RewardCollection.Add(new BODCollectionItem(0x14F0, 1157192, "Builder Reward Title", 0, 50, RewardTitle, 11));
			RewardCollection.Add(new BODCollectionItem(5062, "+1 Armor Of Carpentry Gloves", 0, 50, ArmorOfCarpentry, 1));
			RewardCollection.Add(new BODCollectionItem(7609, "+1 Armor Of Carpentry Cap", 0, 50, ArmorOfCarpentry, 2));
			RewardCollection.Add(new BODCollectionItem(5068, "+1 Armor Of Carpentry Tunic", 0, 50, ArmorOfCarpentry, 3));
			RewardCollection.Add(new BODCollectionItem(5063, "+1 Armor Of Carpentry Gorget", 0, 50, ArmorOfCarpentry, 4));
			RewardCollection.Add(new BODCollectionItem(5069, "+1 Armor Of Carpentry Arms", 0, 50, ArmorOfCarpentry, 5));
			RewardCollection.Add(new BODCollectionItem(5067, "+1 Armor Of Carpentry Leggings", 0, 50, ArmorOfCarpentry, 6));
			RewardCollection.Add(new BODCollectionItem(5062, "+1 Armor Of Lumberjacking Gloves", 0, 200, ArmorOfLumberjacking, 1));
			RewardCollection.Add(new BODCollectionItem(7609, "+1 Armor Of Lumberjacking Cap", 0, 200, ArmorOfLumberjacking, 2));
			RewardCollection.Add(new BODCollectionItem(5068, "+1 Armor Of Lumberjacking Tunic", 0, 200, ArmorOfLumberjacking, 3));
			RewardCollection.Add(new BODCollectionItem(5063, "+1 Armor Of Lumberjacking Gorget", 0, 200, ArmorOfLumberjacking, 4));
			RewardCollection.Add(new BODCollectionItem(5069, "+1 Armor Of Lumberjacking Arms", 0, 200, ArmorOfLumberjacking, 5));
			RewardCollection.Add(new BODCollectionItem(5067, "+1 Armor Of Lumberjacking Leggings", 0, 200, ArmorOfLumberjacking, 6));
			RewardCollection.Add(new BODCollectionItem(0x14F0, 1157193, "Craftsman Reward Title", 0, 250, RewardTitle, 12));
			RewardCollection.Add(new BODCollectionItem(0x9E2C, 1157264, "Craftsman Talisman 10 Charges", 0, 300, CraftsmanTalisman, 10));
			RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152678, "Yew Woodsmans Talisman", CraftResources.GetHue(CraftResource.YewWood), 350, WoodsmansTalisman, (int)CraftResource.YewWood));
			RewardCollection.Add(new BODCollectionItem(0xF44, "Lumberjacking Prospectors Tool", 0, 400, LumberjackingProspectorsTool));
			RewardCollection.Add(new BODCollectionItem(0x9E2C, 1157218, "Craftsman Talisman 25 Charges", 0, 450, CraftsmanTalisman, 25));
			RewardCollection.Add(new BODCollectionItem(6215, "Stain Of Durability", 0, 450, StainOfDurability));
			RewardCollection.Add(new BODCollectionItem(0xf45, "Gargoyles Axe", 0, 450, GargoylesAxe));
			RewardCollection.Add(new BODCollectionItem(6644, "Vise East", 0, 450, Deco, 0));
			RewardCollection.Add(new BODCollectionItem(6648, "Vise South", 0, 450, Deco, 1));
			RewardCollection.Add(new BODCollectionItem(7800, "Unfinished Chair East", 0, 450, Deco, 2));
			RewardCollection.Add(new BODCollectionItem(7791, "Unfinished Chair South", 0, 450, Deco, 3));
			RewardCollection.Add(new BODCollectionItem(0x12B3, 1157293, "DullCopper Runic Mallet And Chisel", CraftResources.GetHue(CraftResource.DullCopper), 450, RunicMalletAndChisel, 1));
			RewardCollection.Add(new BODCollectionItem(0x12B3, 1157294, "Shadow Runic Mallet And Chisel", CraftResources.GetHue(CraftResource.ShadowIron), 450, RunicMalletAndChisel, 2));
			RewardCollection.Add(new BODCollectionItem(7802, "Unfinished Chest East", 0, 500, Deco, 4));
			RewardCollection.Add(new BODCollectionItem(7793, "Unfinished Chest South", 0, 500, Deco, 5));
			RewardCollection.Add(new BODCollectionItem(0x14EC, 1152669, CraftResources.GetHue(CraftResource.YewWood), 500, HarvestMap, (int)CraftResource.YewWood));
            RewardCollection.Add(new BODCollectionItem(0x1029, "Runic Dovetail Saw", CraftResources.GetHue(CraftResource.RegularWood), 500, RunicDovetailSaw, 0));
			RewardCollection.Add(new BODCollectionItem(0x1029, 1157223, "Oak Runic Dovetail Saw", CraftResources.GetHue(CraftResource.OakWood), 600, RunicDovetailSaw, 1));
			RewardCollection.Add(new BODCollectionItem(0x0FBF, "Engraver", 0, 600, Engraver));
			RewardCollection.Add(new BODCollectionItem(0x12B3, 1157295, "Copper Runic Mallet And Chisel", CraftResources.GetHue(CraftResource.Copper), 600, RunicMalletAndChisel, 3));
			RewardCollection.Add(new BODCollectionItem(7806, "Unfinished Shelves East", 0, 650, Deco, 6));
			RewardCollection.Add(new BODCollectionItem(7798, "Unfinished Shelves South", 0, 650, Deco, 7));
			RewardCollection.Add(new BODCollectionItem(0x1029, 1157224, "Ashwood Runic Dovetail Saw", CraftResources.GetHue(CraftResource.AshWood), 650, RunicDovetailSaw, 2));
			RewardCollection.Add(new BODCollectionItem(0x12B3, 1157296, "Bronze Runic Mallet And Chisel", CraftResources.GetHue(CraftResource.Bronze), 650, RunicMalletAndChisel, 4));
			RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152679, "Heartwood Woodsmans Talisman", CraftResources.GetHue(CraftResource.Heartwood), 650, WoodsmansTalisman, (int)CraftResource.Heartwood));
			RewardCollection.Add(new BODCollectionItem(0x14F0, "A Wondrous Scroll Of Carpentry", 0, 675, CarpenterPowerScroll, 5));
			RewardCollection.Add(new BODCollectionItem(0x1029, 1157225, "Yew Runic Dovetail Saw", CraftResources.GetHue(CraftResource.YewWood), 700, RunicDovetailSaw, 3));
			RewardCollection.Add(new BODCollectionItem(0x14F0, "A Wondrous Scroll Of Lumberjacking", 0, 700, LumberjackingPowerScroll, 5));
			RewardCollection.Add(new BODCollectionItem(0x14EC, 1152670, "Heartwood Harvest Map", CraftResources.GetHue(CraftResource.Heartwood), 700, HarvestMap, (int)CraftResource.Heartwood));
			RewardCollection.Add(new BODCollectionItem(0x13E4, "+10 Ancient Carpenter Hammer", 0, 750, AncientCarpenterHammer, 10));
			RewardCollection.Add(new BODCollectionItem(5062, "+3 Armor Of Lumberjacking Gloves", 0, 750, ArmorOfLumberjacking, 7));
			RewardCollection.Add(new BODCollectionItem(7609, "+3 Armor Of Lumberjacking Cap", 0, 750, ArmorOfLumberjacking, 8));
			RewardCollection.Add(new BODCollectionItem(5068, "+3 Armor Of Lumberjacking Tunic", 0, 750, ArmorOfLumberjacking, 9));
			RewardCollection.Add(new BODCollectionItem(5063, "+3 Armor Of Lumberjacking Gorget", 0, 750, ArmorOfLumberjacking, 10));
			RewardCollection.Add(new BODCollectionItem(5069, "+3 Armor Of Lumberjacking Arms", 0, 750, ArmorOfLumberjacking, 11));
			RewardCollection.Add(new BODCollectionItem(5067, "+3 Armor Of Lumberjacking Leggings", 0, 750, ArmorOfLumberjacking, 12));
			RewardCollection.Add(new BODCollectionItem(0x1029, 1157224, "Ashwood Runic Dovetail Saw", CraftResources.GetHue(CraftResource.AshWood), 750, RunicDovetailSaw, 1));
			RewardCollection.Add(new BODCollectionItem(5062, "+3 Armor Of Carpentry Gloves", 0, 800, ArmorOfCarpentry, 7));
			RewardCollection.Add(new BODCollectionItem(7609, "+3 Armor Of Carpentry Cap", 0, 800, ArmorOfCarpentry, 8));
			RewardCollection.Add(new BODCollectionItem(5068, "+3 Armor Of Carpentry Tunic", 0, 800, ArmorOfCarpentry, 9));
			RewardCollection.Add(new BODCollectionItem(5063, "+3 Armor Of Carpentry Gorget", 0, 800, ArmorOfCarpentry, 10));
			RewardCollection.Add(new BODCollectionItem(5069, "+3 Armor Of Carpentry Arms", 0, 800, ArmorOfCarpentry, 11));
			RewardCollection.Add(new BODCollectionItem(5067, "+3 Armor Of Carpentry Leggings", 0, 800, ArmorOfCarpentry, 12));
			RewardCollection.Add(new BODCollectionItem(0x9E2C, 1157265, "Craftsman Talisman 50 Charges", 0, 800, CraftsmanTalisman, 50));
			RewardCollection.Add(new BODCollectionItem(0x13E4, "+20 Ancient Carpenter Hammer", 0, 850, AncientCarpenterHammer, 20));
			RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152680, "Bloodwood Woodsmans Talisman", CraftResources.GetHue(CraftResource.Bloodwood), 850, WoodsmansTalisman, (int)CraftResource.Bloodwood));
			RewardCollection.Add(new BODCollectionItem(0x1029, 1157226, "Heartwood Runic Dovetail Saw", CraftResources.GetHue(CraftResource.Heartwood), 900, RunicDovetailSaw, 4));
			RewardCollection.Add(new BODCollectionItem(0x14F0, "A Exalted Scroll Of Carpentry", 0, 900, CarpenterPowerScroll, 10));
			RewardCollection.Add(new BODCollectionItem(0x14EC, 1152671, "Bloodwood Harvest Map", CraftResources.GetHue(CraftResource.Bloodwood), 900, HarvestMap, (int)CraftResource.Bloodwood));
			RewardCollection.Add(new BODCollectionItem(0x12B3, 1157297, "Gold Runic Mallet And Chisel", CraftResources.GetHue(CraftResource.Gold), 900, RunicMalletAndChisel, 5));
			RewardCollection.Add(new BODCollectionItem(0x13E4, "+30 Ancient Carpenter Hammer", 0, 1000, AncientCarpenterHammer, 30));
			RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152681, "Frostwood Woodsmans Talisman", CraftResources.GetHue(CraftResource.Frostwood), 1000, WoodsmansTalisman, (int)CraftResource.Frostwood));
			RewardCollection.Add(new BODCollectionItem(0x12B3, 1157298, "Agapite Runic Mallet And Chisel", CraftResources.GetHue(CraftResource.Agapite), 1000, RunicMalletAndChisel, 6));
			RewardCollection.Add(new BODCollectionItem(0x1029, "Bloodwood Runic Dovetail Saw", CraftResources.GetHue(CraftResource.Bloodwood), 1050, RunicDovetailSaw, 5));
			RewardCollection.Add(new BODCollectionItem(0x14F0, "A Exalted Scroll Of Lumberjacking", 0, 1050, LumberjackingPowerScroll, 10));
			RewardCollection.Add(new BODCollectionItem(0x14EC, 1152672, "Frostwood Harvest Map", CraftResources.GetHue(CraftResource.Frostwood), 1050, HarvestMap, (int)CraftResource.Frostwood));
			RewardCollection.Add(new BODCollectionItem(0x12B3, 1157299, "Verite Runic Mallet And Chisel", CraftResources.GetHue(CraftResource.Verite), 1100, RunicMalletAndChisel, 7));
			RewardCollection.Add(new BODCollectionItem(0x1029, "Frostwood Runic Dovetail Saw", CraftResources.GetHue(CraftResource.Frostwood), 1150, RunicDovetailSaw, 6));
			RewardCollection.Add(new BODCollectionItem(0x14F0, "A Mythical Scroll Of Lumberjacking", 0, 1150, LumberjackingPowerScroll, 15));
			RewardCollection.Add(new BODCollectionItem(0x12B3, 1157300, "Valorite Runic Mallet And Chisel", CraftResources.GetHue(CraftResource.Valorite), 1150, RunicMalletAndChisel, 8));
			RewardCollection.Add(new BODCollectionItem(5062, "+5 Armor Of Lumberjacking Gloves", 0, 1150, ArmorOfLumberjacking, 13));
			RewardCollection.Add(new BODCollectionItem(7609, "+5 Armor Of Lumberjacking Cap", 0, 1150, ArmorOfLumberjacking, 14));
			RewardCollection.Add(new BODCollectionItem(5068, "+5 Armor Of Lumberjacking Tunic", 0, 1150, ArmorOfLumberjacking, 15));
			RewardCollection.Add(new BODCollectionItem(5063, "+5 Armor Of Lumberjacking Gorget", 0, 1150, ArmorOfLumberjacking, 16));
			RewardCollection.Add(new BODCollectionItem(5069, "+5 Armor Of Lumberjacking Arms", 0, 1150, ArmorOfLumberjacking, 17));
			RewardCollection.Add(new BODCollectionItem(5067, "+5 Armor Of Lumberjacking Leggings", 0, 1150, ArmorOfLumberjacking, 18));
			RewardCollection.Add(new BODCollectionItem(0x1029, "Ebony Runic Dovetail Saw", CraftResources.GetHue(CraftResource.Ebony), 1200, RunicDovetailSaw, 7));
			RewardCollection.Add(new BODCollectionItem(0x14F0, "A Mythical Scroll Of Carpentry", 0, 1200, CarpenterPowerScroll, 15));
			RewardCollection.Add(new BODCollectionItem(5062, "+5 Armor Of Carpentry Gloves", 0, 1200, ArmorOfCarpentry, 13));
			RewardCollection.Add(new BODCollectionItem(7609, "+5 Armor Of Carpentry Cap", 0, 1200, ArmorOfCarpentry, 14));
			RewardCollection.Add(new BODCollectionItem(5068, "+5 Armor Of Carpentry Tunic", 0, 1200, ArmorOfCarpentry, 15));
			RewardCollection.Add(new BODCollectionItem(5063, "+5 Armor Of Carpentry Gorget", 0, 1200, ArmorOfCarpentry, 16));
			RewardCollection.Add(new BODCollectionItem(5069, "+5 Armor Of Carpentry Arms", 0, 1200, ArmorOfCarpentry, 17));
			RewardCollection.Add(new BODCollectionItem(5067, "+5 Armor Of Carpentry Leggings", 0, 1200, ArmorOfCarpentry, 18));
			RewardCollection.Add(new BODCollectionItem(0x1029, "Bamboo Runic Dovetail Saw", CraftResources.GetHue(CraftResource.Bamboo), 1250, RunicDovetailSaw, 8));
			RewardCollection.Add(new BODCollectionItem(0x14F0, "A Legendary Scroll Of Lumberjacking", 0, 1250, LumberjackingPowerScroll, 20));
			RewardCollection.Add(new BODCollectionItem(0x1029, "Purple Heart Runic Dovetail Saw", CraftResources.GetHue(CraftResource.PurpleHeart), 1300, RunicDovetailSaw, 9));
			RewardCollection.Add(new BODCollectionItem(0xE76, "Resources Bag", 0, 1300, ResourcesBag));
			RewardCollection.Add(new BODCollectionItem(0x14F0, "A Legendary Scroll Of Carpentry", 0, 1300, CarpenterPowerScroll, 20));
			RewardCollection.Add(new BODCollectionItem(0x1029, "Redwood Runic Dovetail Saw", CraftResources.GetHue(CraftResource.Redwood), 1350, RunicDovetailSaw, 10));
			RewardCollection.Add(new BODCollectionItem(0x1029, "Petrified Runic Dovetail Saw", CraftResources.GetHue(CraftResource.Petrified), 1400, RunicDovetailSaw, 11));
			RewardCollection.Add(new BODCollectionItem(0x13E4, "+40 Ancient Carpenter Hammer", 0, 1450, AncientCarpenterHammer, 40));

			/*Groups = new RewardGroup[]
			{
				new RewardGroup(    0, new RewardItem( 1, SturdyLumberjackAxe ), new RewardItem( 10, LeatherGlovesOfLumberjacking, 1 ) ),
				new RewardGroup(   25, new RewardItem( 1, LeatherGlovesOfLumberjacking, 3 ), new RewardItem( 1, SturdyAxe ) ),
				new RewardGroup(   50, new RewardItem( 90, SturdyLumberjackAxe ), new RewardItem( 10, ArmorOfCarpentry, Utility.RandomMinMax(1,6) ) ),
				new RewardGroup(  200, new RewardItem( 90, SturdyAxe ), new RewardItem( 10, ArmorOfLumberjacking, Utility.RandomMinMax(1,6) ) ),
				new RewardGroup(  400, new RewardItem( 90, LumberjackingProspectorsTool ), new RewardItem( 10, ArmorOfCarpentry, Utility.RandomMinMax(1,6) ) ),
				new RewardGroup(  450, new RewardItem( 2, StainOfDurability ), new RewardItem( 1, GargoylesAxe ), new RewardItem( 1, Deco, Utility.Random(6) ) ),
				new RewardGroup(  500, new RewardItem( 1, RunicDovetailSaw, 1 ), new RewardItem( 1, GargoylesAxe ), new RewardItem( 1, Deco, Utility.Random(6) ) ),
				new RewardGroup(  550, new RewardItem( 3, RunicDovetailSaw, 1 ), new RewardItem( 2, RunicDovetailSaw, 2 ) ),
				new RewardGroup(  600, new RewardItem( 1, RunicDovetailSaw, 2 ), new RewardItem( 1, Engraver ) ),
				new RewardGroup(  625, new RewardItem( 3, RunicDovetailSaw, 2 ), new RewardItem( 1, Engraver ) ),
				new RewardGroup(  650, new RewardItem( 1, RunicDovetailSaw, 3 ), new RewardItem( 1, Deco, Utility.Random(7) )),
				new RewardGroup(  675, new RewardItem( 1, Engraver ), new RewardItem( 1, RunicDovetailSaw, 3 ), new RewardItem( 6, CarpenterPowerScroll, 5 ) ),
				new RewardGroup(  700, new RewardItem( 1, RunicDovetailSaw, 4 ), new RewardItem( 1, Deco, Utility.Random(7) ), new RewardItem( 6, LumberjackingPowerScroll, 5 ) ),
				new RewardGroup(  750, new RewardItem( 1, AncientCarpenterHammer, 10 ), new RewardItem( 1, ArmorOfLumberjacking, Utility.RandomMinMax(1,6) ), new RewardItem( 6, CarpenterPowerScroll, 5 ) ),
				new RewardGroup(  800, new RewardItem( 1, RunicDovetailSaw, 4 ), new RewardItem( 1, ArmorOfCarpentry, Utility.RandomMinMax(7,12) ), new RewardItem( 6, LumberjackingPowerScroll, 5 ) ),
				new RewardGroup(  850, new RewardItem( 5, AncientCarpenterHammer, 20 ), new RewardItem( 1, ArmorOfLumberjacking, Utility.RandomMinMax(7,12) ), new RewardItem( 6, CarpenterPowerScroll, 5 ) ),
				new RewardGroup(  900, new RewardItem( 1, RunicDovetailSaw, 5 ), new RewardItem( 6, CarpenterPowerScroll, 10 ) ),
				new RewardGroup(  950, new RewardItem( 1, LeatherGlovesOfLumberjacking, 5 ), new RewardItem( 6, CarpenterPowerScroll, 10 ) ),
				new RewardGroup( 1000, new RewardItem( 5, AncientCarpenterHammer, 30 ), new RewardItem( 6, CarpenterPowerScroll, 10 ) ),
				new RewardGroup( 1050, new RewardItem( 1, RunicDovetailSaw, 6 ), new RewardItem( 1, Engraver ), new RewardItem( 6, LumberjackingPowerScroll, 10 ) ),
				new RewardGroup( 1100, new RewardItem( 3, AncientCarpenterHammer, 30 ), new RewardItem( 1, Engraver ), new RewardItem( 6, CarpenterPowerScroll, 10 ) ),
				new RewardGroup( 1150, new RewardItem( 1, RunicDovetailSaw, 7 )), //new RewardItem( 1, ArmorOfLumberjacking, Utility.RandomMinMax(13,18)), new RewardItem( 6, LumberjackingPowerScroll, 15 ) ),
				new RewardGroup( 1200, new RewardItem( 1, RunicDovetailSaw, 8 )), //new RewardItem( 1, ArmorOfCarpentry, Utility.RandomMinMax(13,18)), new RewardItem( 6, CarpenterPowerScroll, 15 ) ),
				new RewardGroup( 1250, new RewardItem( 1, RunicDovetailSaw, 9 )), //new RewardItem( 1, ArmorOfLumberjacking, Utility.RandomMinMax(13,18)), new RewardItem( 6, LumberjackingPowerScroll, 20 ) ),
				new RewardGroup( 1300, new RewardItem( 1, RunicDovetailSaw, 10 ), new RewardItem( 1, ResourcesBag ), new RewardItem( 6, CarpenterPowerScroll, 20 ) ),
				new RewardGroup( 1350, new RewardItem( 1, RunicDovetailSaw, 11 ), new RewardItem( 1, ResourcesBag ) ),
				new RewardGroup( 1400, new RewardItem( 3, RunicDovetailSaw, 12 )),
				new RewardGroup( 1450, new RewardItem( 3, AncientCarpenterHammer, 40 ))
			};*/
        }

        #region Constructors

		//daat start
        private static readonly ConstructCallback RunicDovetailSaw = new ConstructCallback( CreateDovetailRunicSaw );
        private static readonly ConstructCallback SturdyLumberjackAxe = new ConstructCallback( CreateSturdyLumberjackAxe );
        private static readonly ConstructCallback SturdyAxe = new ConstructCallback( CreateSturdyAxe );
        private static readonly ConstructCallback ArmorOfCarpentry = new ConstructCallback( CreateArmorOfCarpentry );
        private static readonly ConstructCallback ArmorOfLumberjacking = new ConstructCallback(CreateArmorOfLumberjacking);
        private static readonly ConstructCallback StainOfDurability = new ConstructCallback( CreateStainOfDurability );
        private static readonly ConstructCallback GargoylesAxe = new ConstructCallback( CreateGargoylesAxe );
        private static readonly ConstructCallback Engraver = new ConstructCallback( CreateEngraver );
        private static readonly ConstructCallback LumberjackingProspectorsTool = new ConstructCallback( CreateLumberjackingProspectorsTool );
        private static readonly ConstructCallback AncientCarpenterHammer = new ConstructCallback( CreateAncientCarpenterHammer );
        private static readonly ConstructCallback LeatherGlovesOfLumberjacking = new ConstructCallback( CreateLeatherGlovesOfLumberjacking );
        private static readonly ConstructCallback CarpenterPowerScroll = new ConstructCallback( CreateCarpenterPowerScroll );
        private static readonly ConstructCallback ResourcesBag = new ConstructCallback( CreateResourcesBag );
        private static readonly ConstructCallback Deco = new ConstructCallback( CreateDeco );
        private static readonly ConstructCallback LumberjackingPowerScroll = new ConstructCallback( CreateLumberjackingPowerScroll );
        
        private static Item CreateDeco( int type )
        {
            switch (type)
            {
                    case 0: default: return new Deco( 6644, "Vise East" );
                    case 1: return new Deco( 6648, "Vise South" );
                    case 2: return new Deco( 7800, "Unfinished Chair East" );
                    case 3: return new Deco( 7791, "Unfinished Chair South" );
                    case 4: return new Deco( 7802, "Unfinished Chest East" );
                    case 5: return new Deco( 7793, "Unfinished Chest South" );
                    case 6: return new Deco( 7806, "Unfinished Shelves East" );
                    case 7: return new Deco( 7798, "Unfinished Shelves South" );
            }
        }
        
        private static Item CreateDovetailRunicSaw( int type )
        {
            if ( type >= 0 && type <= 11 )
                return new RunicDovetailSaw( CraftResource.RegularWood + type, Core.AOS ? ( 100 - (type*5) ) : 50 );
            
            throw new InvalidOperationException();
        }
        
        private static Item CreateSturdyLumberjackAxe( int type )
        {
            return new SturdyLumberjackAxe();
        }

        private static Item CreateSturdyAxe( int type )
        {
            return new SturdyAxe();
        }
        
        private static Item CreateArmorOfCarpentry( int type )
        {
            switch (type)
            {
                    // public ArmorOfCarpentry( int bonus, int itemID, int skill ) : base( itemID ) example: 3 == bonus skill, 7609 == itemid, 2 == skill (mining)
                    //notes:
                    //total of 10 reward points
                    //cloth have 6 points out of 10 (60%)
                    //armor have 1 point out of 10 (10%)
                    //sandals have 3 points out of 10 (30%)
                    //new RewardGroup(  200, new RewardItem( 2, Cloth, 4 ), new RewardItem( 1, ArmorOfCarpentry, Utility.RandomMinMax(1,6) ), new RewardItem( 2, Sandals ) ),
                    //3 items 5 points
                    //cloth have 2/5 40%
                    //armor have 1/5 20%
                    //sandals 2/5 40%
                    
                    //cloth:
                    case 1: default: return new ArmorOfCarpentry( 1, 5062, Utility.Random(2)); //gloves
                    case 2: return new ArmorOfCarpentry( 1, 7609, Utility.Random(2)); //cap
                    case 3: return new ArmorOfCarpentry( 1, 5068, Utility.Random(2)); //tunic
                    case 4: return new ArmorOfCarpentry( 1, 5063, Utility.Random(2)); //gorget
                    case 5: return new ArmorOfCarpentry( 1, 5069, Utility.Random(2)); //arms
                    case 6: return new ArmorOfCarpentry( 1, 5067, Utility.Random(2)); //leggings
                    case 7: return new ArmorOfCarpentry( 3, 5062, Utility.Random(2)); //gloves
                    case 8: return new ArmorOfCarpentry( 3, 7609, Utility.Random(2)); //cap
                    case 9: return new ArmorOfCarpentry( 3, 5068, Utility.Random(2)); //tunic
                    case 10: return new ArmorOfCarpentry( 3, 5063, Utility.Random(2)); //gorget
                    case 11: return new ArmorOfCarpentry( 3, 5069, Utility.Random(2)); //arms
                    case 12: return new ArmorOfCarpentry( 3, 5067, Utility.Random(2)); //leggings
                    case 13: return new ArmorOfCarpentry( 5, 5062, Utility.Random(2)); //gloves
                    case 14: return new ArmorOfCarpentry( 5, 7609, Utility.Random(2)); //cap
                    case 15: return new ArmorOfCarpentry( 5, 5068, Utility.Random(2)); //tunic
                    case 16: return new ArmorOfCarpentry( 5, 5063, Utility.Random(2)); //gorget
                    case 17: return new ArmorOfCarpentry( 5, 5069, Utility.Random(2)); //arms
                    case 18: return new ArmorOfCarpentry( 5, 5067, Utility.Random(2)); //leggings
            }
        }
        private static Item CreateArmorOfLumberjacking(int type)
        {
            switch (type)
            {
                // public ArmorOfCarpentry( int bonus, int itemID, int skill ) : base( itemID ) example: 3 == bonus skill, 7609 == itemid, 2 == skill (mining)
                //notes:
                //total of 10 reward points
                //cloth have 6 points out of 10 (60%)
                //armor have 1 point out of 10 (10%)
                //sandals have 3 points out of 10 (30%)
                //new RewardGroup(  200, new RewardItem( 2, Cloth, 4 ), new RewardItem( 1, ArmorOfCarpentry, Utility.RandomMinMax(1,6) ), new RewardItem( 2, Sandals ) ),
                //3 items 5 points
                //cloth have 2/5 40%
                //armor have 1/5 20%
                //sandals 2/5 40%

                //cloth:
                case 1:
                default: return new ArmorOfLumberjacking(1, 5062, Utility.Random(2)); //gloves
                case 2: return new ArmorOfLumberjacking(1, 7609, Utility.Random(2)); //cap
                case 3: return new ArmorOfLumberjacking(1, 5068, Utility.Random(2)); //tunic
                case 4: return new ArmorOfLumberjacking(1, 5063, Utility.Random(2)); //gorget
                case 5: return new ArmorOfLumberjacking(1, 5069, Utility.Random(2)); //arms
                case 6: return new ArmorOfLumberjacking(1, 5067, Utility.Random(2)); //leggings
                case 7: return new ArmorOfLumberjacking(3, 5062, Utility.Random(2)); //gloves
                case 8: return new ArmorOfLumberjacking(3, 7609, Utility.Random(2)); //cap
                case 9: return new ArmorOfLumberjacking(3, 5068, Utility.Random(2)); //tunic
                case 10: return new ArmorOfLumberjacking(3, 5063, Utility.Random(2)); //gorget
                case 11: return new ArmorOfLumberjacking(3, 5069, Utility.Random(2)); //arms
                case 12: return new ArmorOfLumberjacking(3, 5067, Utility.Random(2)); //leggings
                case 13: return new ArmorOfLumberjacking(5, 5062, Utility.Random(2)); //gloves
                case 14: return new ArmorOfLumberjacking(5, 7609, Utility.Random(2)); //cap
                case 15: return new ArmorOfLumberjacking(5, 5068, Utility.Random(2)); //tunic
                case 16: return new ArmorOfLumberjacking(5, 5063, Utility.Random(2)); //gorget
                case 17: return new ArmorOfLumberjacking(5, 5069, Utility.Random(2)); //arms
                case 18: return new ArmorOfLumberjacking(5, 5067, Utility.Random(2)); //leggings
            }
        }
        private static Item CreateStainOfDurability( int type )
        {
            return new StainOfDurability();
        }
        
        private static Item CreateGargoylesAxe( int type )
        {
            return new GargoylesAxe();
        }

        private static Item CreateEngraver( int type )
        {
            return new Engraver();
        }
        
        private static Item CreateLumberjackingProspectorsTool( int type )
        {
            return new LumberjackingProspectorsTool();
        }
        
        private static Item CreateCarpenterPowerScroll( int type )
        {
            if ( type == 5 || type == 10 || type == 15 || type == 20 )
                return new PowerScroll( SkillName.Carpentry, 100 + type );
            
            throw new InvalidOperationException();
        }
        
        private static Item CreateAncientCarpenterHammer( int type )
        {
            if ( type == 10 || type == 20 || type == 30 || type == 40 )
                return new AncientCarpenterHammer( type );
            
            throw new InvalidOperationException();
        }
        
        private static Item CreateLeatherGlovesOfLumberjacking( int type )
        {
            if ( type == 2 || type == 5 || type == 7 || type == 10 )
                return new LeatherGlovesOfLumberjacking( type );
            
            throw new InvalidOperationException();
        }
        
        private static Item CreateResourcesBag( int type )
        {
            return new ResourcesBag();
        }
        
        private static Item CreateLumberjackingPowerScroll( int type )
        {
            if ( type == 5 || type == 10 || type == 15 || type == 20 )
                return new PowerScroll( SkillName.Lumberjacking, 100 + type );
            
            throw new InvalidOperationException();
        }
		//daat end

        private static Item DovetailSaw(int type)
        {
            BaseTool tool = new DovetailSaw();
            tool.UsesRemaining = 250;

            return tool;
        }

        private static Item RunicMalletAndChisel(int type)
        {
            if (type >= 1 && type <= 8)
                return new RunicMalletAndChisel(CraftResource.Iron + type, Core.AOS ? (55 - (type * 5)) : 50);

            return null;
        }

        /*private static Item RunicDovetailSaw(int type)
        {
            switch (type)
            {
                default:
				case 0: return new RunicDovetailSaw(CraftResource.OakWood, 50);
                case 1: return new RunicDovetailSaw(CraftResource.AshWood, 35);
                case 2: return new RunicDovetailSaw(CraftResource.YewWood, 25);
                case 3: return new RunicDovetailSaw(CraftResource.Heartwood, 15);
            }
        }*/

        private static Item CraftsmanTalisman(int type)
        {
            return new MasterCraftsmanTalisman(type, 0x9E2C, TalismanSkill.Carpentry);
        }
        #endregion

        public static readonly CarpentryRewardCalculator Instance = new CarpentryRewardCalculator();

        public override int ComputePoints(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int points = 0;

            if (quantity == 10)
                points += 10;
            else if (quantity == 15)
                points += 25;
            else if (quantity == 20)
                points += 50;

            if (exceptional)
                points += 200;

            /*switch (material)
            {
                case BulkMaterialType.None: break;
                case BulkMaterialType.OakWood: points += 300;  break;
                case BulkMaterialType.AshWood: points += 350; break;
                case BulkMaterialType.YewWood: points += 400; break;
                case BulkMaterialType.Heartwood: points += 450; break;
                case BulkMaterialType.Bloodwood: points += 500; break;
                case BulkMaterialType.Frostwood: points += 550; break;
            }*/

			//daat start
			if (material >= BulkMaterialType.OakWood && material <= BulkMaterialType.Petrified)
				points += 200 + (50 * (material - BulkMaterialType.OakWood));
			//daat end

            if (itemCount > 1)
                points += this.LookupTypePoints(m_Types, type);

            return points;
        }

        private RewardType[] m_Types =
        {
            new RewardType(250, typeof(TallCabinet), typeof(ShortCabinet)),
            new RewardType(250, typeof(RedArmoire), typeof(ElegantArmoire), typeof(MapleArmoire), typeof(CherryArmoire)),
            new RewardType(300, typeof(PlainWoodenChest), typeof(OrnateWoodenChest), typeof(GildedWoodenChest), typeof(WoodenFootLocker), typeof(FinishedWoodenChest)),
            new RewardType(350, typeof(WildStaff), typeof(ArcanistsWildStaff), typeof(AncientWildStaff), typeof(ThornedWildStaff), typeof(HardenedWildStaff)),
            new RewardType(250, typeof(LapHarp), typeof(Lute), typeof(Drums), typeof(Harp)),
            new RewardType(200, typeof(GnarledStaff), typeof(QuarterStaff), typeof(ShepherdsCrook), typeof(Tetsubo), typeof(Bokuto)),
            new RewardType(300, typeof(WoodenBox), typeof(EmptyBookcase), typeof(WoodenBench), typeof(WoodenThrone)),
        };

        private static readonly int[][][] m_GoldTable = new int[][][]
        {
            /*new int[][] // 1-part (regular)
            {
                new int[] { 150, 150, 225, 225, 300, 300 },
                new int[] { 225, 225, 325, 325, 450, 450 },
                new int[] { 300, 400, 500, 500, 600, 750 }
            },
            new int[][] // 1-part (exceptional)
            {
                new int[] { 300, 300, 400, 500, 600, 600 },
                new int[] { 450, 450, 650, 750, 900, 900 },
                new int[] { 600, 750, 850, 1000, 1200, 1800 }
            },
            new int[][] // 2-part (regular)
            {
                new int[] { 2000, 2000, 2000, 2500, 2500, 2500 },
                new int[] { 3000, 3000, 3000, 3750, 3750, 3750 },
                new int[] { 4000, 4500, 4500, 5000, 5000, 7500 }
            },
            new int[][] // 2-part (exceptional)
            {
                new int[] { 2500, 2500, 3000, 3350, 3350, 4000 },
                new int[] { 3750, 3750, 4250, 4750, 5200, 5200 },
                new int[] { 5000, 6100, 6100, 7000, 7000, 10000 }
            },
            new int[][] // 4-part (regular)
            {
                new int[] { 4000, 4000, 4000, 5000, 5000, 5000 },
                new int[] { 6000, 6000, 6000, 7500, 7500, 7500 },
                new int[] { 8000, 9000, 9500, 10000, 10000, 15000 }
            },
            new int[][] // 4-part (exceptional)
            {
                new int[] { 5000, 5000, 6000, 6750, 7500, 7500 },
                new int[] { 7500, 7500, 8500, 9500, 11250, 11250 },
                new int[] { 10000, 1250, 1250, 15000, 15000, 20000 }
            },
            new int[][] // 5-part (regular)
            {
                new int[] { 5000, 5000, 60000, 6000, 7500, 7500 },
                new int[] { 7500, 7500, 7500, 11250, 11250, 11250 },
                new int[] { 10000, 10000, 1250, 15000, 15000, 20000 }
            },
            new int[][] // 5-part (exceptional)
            {
                new int[] { 7500, 7500, 8500, 9500, 10000, 10000 },
                new int[] { 11250, 11250, 1250, 1350, 15000, 15000 },
                new int[] { 15000, 1750, 1750, 20000, 20000, 30000 }
            },*/

			//daat start
			new int[][] // 1-part (regular)
            {
				new int[]{ 150, 250, 250, 400,  400,  750,  750, 1200, 1200, 1500, 1500, 1800 },
				new int[]{ 225, 375, 375, 600,  600, 1125, 1125, 1800, 1800, 2100, 2100, 2400 },
				new int[]{ 300, 500, 750, 800, 1050, 1500, 2250, 2400, 3000, 3500, 4000, 4500 }
			},
			new int[][] // 1-part (exceptional)
            {
				new int[]{ 250, 400,  400,  750,  750, 1500, 1500, 3000, 3000, 3500, 3500, 4000 },
				new int[]{ 375, 600,  600, 1125, 1125, 2250, 2250, 4500, 4500, 5000, 5000, 5500 },
				new int[]{ 500, 800, 1200, 1500, 2500, 3000, 4000, 5000, 6000, 7000, 8000, 9000 }
			},
			new int[][] // 2-part (regular)
			{
				new int[]{ 150, 250, 250, 400,  400,  750,  750, 1200, 1200, 1500, 1500, 1800 },
				new int[]{ 225, 375, 375, 600,  600, 1125, 1125, 1800, 1800, 2100, 2100, 2400 },
				new int[]{ 300, 500, 750, 800, 1050, 1500, 2250, 2400, 3000, 3500, 4000, 4500 }
			},
			new int[][] // 2-part (exceptional)
			{
				new int[]{ 250, 400,  400,  750,  750, 1500, 1500, 3000, 3000, 3500, 3500, 4000 },
				new int[]{ 375, 600,  600, 1125, 1125, 2250, 2250, 4500, 4500, 5000, 5000, 5500 },
				new int[]{ 500, 800, 1200, 1500, 2500, 3000, 4000, 5000, 6000, 7000, 8000, 9000 }
			},
			new int[][] // 4-part (regular)
            {
				new int[]{ 3000,  5000,  5000,  7500,  7500,  8000,  8500,  9000, 10000, 11000, 12000, 13000 },
				new int[]{ 4500,  7500,  7500, 11250, 11500, 15000, 15000, 22500, 22500, 30000, 30000, 35000 },
				new int[]{ 6000, 10000, 15000, 15000, 20000, 20000, 30000, 30000, 50000, 60000, 60000, 70000 }
			},
			new int[][] // 4-part (exceptional)
            {
				new int[]{  5000, 10000, 15000, 20000, 25000, 30000,  35000,  40000,  45000,  50000,  60000,  70000 },
				new int[]{  7500, 15000, 15000, 22500, 22500, 37500,  37500,  75000,  75000, 100000, 100000, 120000 },
				new int[]{ 10000, 20000, 30000, 30000, 50000, 50000, 100000, 100000, 200000, 250000, 250000, 300000 }
			},
			new int[][] // 5-part (regular)
            {
				new int[] { 5000, 5000, 60000, 6000, 7500, 7500 },
				new int[] { 7500, 7500, 7500, 11250, 11250, 11250 },
				new int[] { 10000, 10000, 1250, 15000, 15000, 20000 }
			},
			new int[][] // 5-part (exceptional)
            {
				new int[] { 7500, 7500, 8500, 9500, 10000, 10000 },
				new int[] { 11250, 11250, 1250, 1350, 15000, 15000 },
				new int[] { 15000, 1750, 1750, 20000, 20000, 30000 }
			}
			//daat end
        };

        public override int ComputeGold(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int gold = 0;

            if (itemCount == 1 && BulkOrderSystem.NewSystemEnabled && BulkOrderSystem.ComputeGold(type, quantity, out gold))
            {
                return gold;
            }

            int[][][] goldTable = m_GoldTable;

            int typeIndex = ((itemCount == 5 ? 2 : itemCount == 4 ? 1 : 0) * 2) + (exceptional ? 1 : 0);
            int quanIndex = (quantity == 20 ? 2 : quantity == 15 ? 1 : 0);
            //int mtrlIndex = (material == BulkMaterialType.Frostwood ? 5 : material == BulkMaterialType.Bloodwood ? 4 : material == BulkMaterialType.Heartwood ? 3 : material == BulkMaterialType.YewWood ? 2 : material == BulkMaterialType.AshWood ? 1 : 0);
			//daat start
			int mtrlIndex = (material >= BulkMaterialType.OakWood && material <= BulkMaterialType.Petrified) ? 1 + (int)(material - BulkMaterialType.OakWood) : 0;
			//daat end

            gold = goldTable[typeIndex][quanIndex][mtrlIndex];

            int min = (gold * 9) / 10;
            int max = (gold * 10) / 9;

            return Utility.RandomMinMax(min, max);
        }
    }
    #endregion

    #region Inscription Rewards
    public sealed class InscriptionRewardCalculator : RewardCalculator
    {
        public InscriptionRewardCalculator()
        {
            RewardCollection = new List<CollectionItem>();

            RewardCollection.Add(new BODCollectionItem(0x0FBF, 1157219, 0, 10, ScribesPen));
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157194, 0, 25, RewardTitle, 13)); //clerk
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157195, 0, 50, RewardTitle, 14)); // transcriber
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157196, 0, 210, RewardTitle, 15)); // scrivener
            RewardCollection.Add(new BODCollectionItem(0x2831, 1156443, 0, 210, Recipe, 3));
            RewardCollection.Add(new BODCollectionItem(0x182B, 1157205, 2741, 250, NaturalDye, 3));
            RewardCollection.Add(new BODCollectionItem(0x9E28, 1157264, 0, 275, CraftsmanTalisman, 10));
            RewardCollection.Add(new BODCollectionItem(0x182B, 1157205, 2740, 310, NaturalDye, 4));
            RewardCollection.Add(new BODCollectionItem(0x9E28, 1157218, 0, 350, CraftsmanTalisman, 25));
            RewardCollection.Add(new BODCollectionItem(0x182B, 1157205, 2732, 375, NaturalDye, 5));
            RewardCollection.Add(new BODCollectionItem(0x9E28, 1157265, 0, 410, CraftsmanTalisman, 50));
            RewardCollection.Add(new BODCollectionItem(0x182B, 1157205, 2731, 450, NaturalDye, 6));
            RewardCollection.Add(new BODCollectionItem(0x182B, 1157205, 2735, 475, NaturalDye, 7));
            RewardCollection.Add(new BODCollectionItem(0x9E28, 1157291, 0, 500, ImprovementTalisman, 10));
        }

        #region Constructors

        private static Item ScribesPen(int type)
        {
            BaseTool tool = new ScribesPen();
            tool.UsesRemaining = 250;

            return tool;
        }

        private static Item CraftsmanTalisman(int type)
        {
            return new MasterCraftsmanTalisman(type, 0x9E28, TalismanSkill.Inscription);
        }

        private static Item ImprovementTalisman(int type)
        {
            return new GuaranteedSpellbookImprovementTalisman(type);
        }

        #endregion

        public static readonly InscriptionRewardCalculator Instance = new InscriptionRewardCalculator();

        public override int ComputePoints(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int points = 0;

            if (quantity == 10)
                points += 10;
            else if (quantity == 15)
                points += 25;
            else if (quantity == 20)
                points += 50;

            if (itemCount > 1)
                points += this.LookupTypePoints(m_Types, type);

            return points;
        }

        private RewardType[] m_Types =
        {
            new RewardType(200, typeof(ClumsyScroll), typeof(FeeblemindScroll), typeof(WeakenScroll)),
            new RewardType(300, typeof(CurseScroll), typeof(GreaterHealScroll), typeof(RecallScroll)),
            new RewardType(300, typeof(PoisonStrikeScroll), typeof(WitherScroll), typeof(StrangleScroll)),
            new RewardType(250, typeof(MindRotScroll), typeof(SummonFamiliarScroll), typeof(AnimateDeadScroll), typeof(HorrificBeastScroll)),
            new RewardType(200, typeof(HealScroll), typeof(AgilityScroll), typeof(CunningScroll), typeof(CureScroll), typeof(StrengthScroll)),
            new RewardType(250, typeof(BloodOathScroll), typeof(CorpseSkinScroll), typeof(CurseWeaponScroll), typeof(EvilOmenScroll), typeof(PainSpikeScroll)),
            new RewardType(300, typeof(BladeSpiritsScroll), typeof(DispelFieldScroll), typeof(MagicReflectScroll), typeof(ParalyzeScroll), typeof(SummonCreatureScroll)),
            new RewardType(350, typeof(ChainLightningScroll), typeof(FlamestrikeScroll), typeof(ManaVampireScroll), typeof(MeteorSwarmScroll), typeof(PolymorphScroll)),
            new RewardType(400, typeof(SummonAirElementalScroll), typeof(SummonDaemonScroll), typeof(SummonEarthElementalScroll), typeof(SummonFireElementalScroll), typeof(SummonWaterElementalScroll)),
            new RewardType(450, typeof(Spellbook), typeof(NecromancerSpellbook), typeof(Runebook), typeof(RunicAtlas))
        };

        private static readonly int[][] m_GoldTable = new int[][]
        {
            new int[] // singles
            {
                150, 225, 300
            },
            new int[] // no 2 piece
            {
            },
            new int[] // 3-part
            {
                4000, 6000, 8000
            },
            new int[] // 4-part
            {
                5000, 7500, 10000
            },
            new int[] // 5-part
            {
                7500, 11250, 15000
            },
        };

        public override int ComputeGold(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int gold = 0;

            if (itemCount == 1 && BulkOrderSystem.NewSystemEnabled && BulkOrderSystem.ComputeGold(type, quantity, out gold))
            {
                return gold;
            }

            int[][] goldTable = m_GoldTable;

            int quanIndex = (quantity == 20 ? 2 : quantity == 15 ? 1 : 0);

            gold = goldTable[itemCount - 1][quanIndex];

            int min = (gold * 9) / 10;
            int max = (gold * 10) / 9;

            return Utility.RandomMinMax(min, max);
        }
    }
    #endregion

    #region Cooking Rewards
    public sealed class CookingRewardCalculator : RewardCalculator
    {
        public CookingRewardCalculator()
        {
            RewardCollection = new List<CollectionItem>();

            RewardCollection.Add(new BODCollectionItem(0x97F, 1157219, 0, 10, Skillet));
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157197, 0, 25, RewardTitle, 16)); // baker
            RewardCollection.Add(new BODCollectionItem(0x2831, 1031233, 0, 25, Recipe, 4));
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157198, 0, 50, RewardTitle, 17)); // gourmet
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157199, 0, 210, RewardTitle, 18)); // culinarian
            RewardCollection.Add(new BODCollectionItem(0x9E27, 1157264, 0, 250, CraftsmanTalisman, 10));
            RewardCollection.Add(new BODCollectionItem(0x9E27, 1157218, 0, 300, CraftsmanTalisman, 25));
            RewardCollection.Add(new BODCollectionItem(0x9E27, 1157265, 0, 350, CraftsmanTalisman, 50));
            RewardCollection.Add(new BODCollectionItem(0x153D, 1157227, 1990, 410, CreateItem, 0));
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1076605, 0, 475, CreateItem, 1));
            RewardCollection.Add(new BODCollectionItem(0x182B, 1157278, 2740, 525, NaturalDye, 8));
            RewardCollection.Add(new BODCollectionItem(0x182B, 1157278, 2732, 625, NaturalDye, 9));
            RewardCollection.Add(new BODCollectionItem(0x9E36, 1157229, 0, 625, CreateItem, 2));
        }

        #region Constructors

        private static Item Skillet(int type)
        {
            BaseTool tool = new Skillet();
            tool.UsesRemaining = 250;

            return tool;
        }

        private static Item CraftsmanTalisman(int type)
        {
            return new MasterCraftsmanTalisman(type, 0x9E27, TalismanSkill.Cooking);
        }

        private static Item CreateItem(int type)
        {
            switch (type)
            {
                case 0: return new MasterChefsApron();
                case 1: return new PlumTreeAddonDeed();
                case 2: return new FermentationBarrel();
            }

            return null;
        }

        #endregion

        public static readonly CookingRewardCalculator Instance = new CookingRewardCalculator();

        public override int ComputePoints(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int points = 0;

            if (quantity == 10)
                points += 10;
            else if (quantity == 15)
                points += 25;
            else if (quantity == 20)
                points += 50;

            if (exceptional)
                points += 200;

            if (itemCount > 1)
                points += this.LookupTypePoints(m_Types, type);

            return points;
        }

        private RewardType[] m_Types =
        {
            new RewardType(200, typeof(SweetCocoaButter), typeof(SackFlour), typeof(Dough)),
            new RewardType(250, typeof(UnbakedFruitPie), typeof(UnbakedPeachCobbler), typeof(UnbakedApplePie), typeof(UnbakedPumpkinPie)),
            new RewardType(300, typeof(CookedBird), typeof(FishSteak), typeof(FriedEggs), typeof(LambLeg), typeof(Ribs)),
            new RewardType(350, typeof(Cookies), typeof(Cake), typeof(Muffins), typeof(ThreeTieredCake)),
            new RewardType(400, typeof(EnchantedApple), typeof(TribalPaint), typeof(WrathGrapes), typeof(EggBomb)),
            new RewardType(450, typeof(MisoSoup), typeof(WhiteMisoSoup), typeof(RedMisoSoup), typeof(AwaseMisoSoup)),
            new RewardType(500, typeof(WasabiClumps), typeof(SushiRolls), typeof(SushiPlatter), typeof(GreenTea)),
        };

        private static readonly int[][] m_GoldTable = new int[][]
        {
            new int[] // singles
            {
                150, 225, 300
            },
            new int[] // no 2 piece
            {
            },
            new int[] // 3-part
            {
                4000, 6000, 8000
            },
            new int[] // 4-part
            {
                5000, 7500, 10000
            },
            new int[] // 5-part
            {
                7500, 11250, 15000
            },
            new int[] // 6-part (regular)
            {
                7500, 11250, 15000
            },
        };

        public override int ComputeGold(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int gold = 0;

            if (itemCount == 1 && BulkOrderSystem.NewSystemEnabled && BulkOrderSystem.ComputeGold(type, quantity, out gold))
            {
                return gold;
            }

            int[][] goldTable = m_GoldTable;

            int quanIndex = (quantity == 20 ? 2 : quantity == 15 ? 1 : 0);

            gold = goldTable[itemCount - 1][quanIndex];

            int min = (gold * 9) / 10;
            int max = (gold * 10) / 9;

            return Utility.RandomMinMax(min, max);
        }
    }
    #endregion

    #region Fletching Rewards
    public sealed class FletchingRewardCalculator : RewardCalculator
    {
        public FletchingRewardCalculator()
        {
            RewardCollection = new List<CollectionItem>();

			/*RewardCollection.Add(new BODCollectionItem(0x1022, 1157219, 0, 10, FletcherTools));
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157200, 0, 25, RewardTitle, 17));
            RewardCollection.Add(new BODCollectionItem(0x9E29, 1157264, 0, 210, CraftsmanTalisman, 10));
            RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152678, CraftResources.GetHue(CraftResource.YewWood), 225, WoodsmansTalisman, (int)CraftResource.YewWood));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152669, CraftResources.GetHue(CraftResource.YewWood), 310, HarvestMap, (int)CraftResource.YewWood));
            RewardCollection.Add(new BODCollectionItem(0x9E29, 1157218, 0, 325, CraftsmanTalisman, 25));
            RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152679, CraftResources.GetHue(CraftResource.Heartwood), 360, WoodsmansTalisman, (int)CraftResource.Heartwood));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152670, CraftResources.GetHue(CraftResource.Heartwood), 375, HarvestMap, (int)CraftResource.Heartwood));
            RewardCollection.Add(new BODCollectionItem(0x9E29, 1157265, 0, 410, CraftsmanTalisman, 50));
            RewardCollection.Add(new BODCollectionItem(0x1022, 1157223, CraftResources.GetHue(CraftResource.OakWood), 425, CreateRunicFletcherTools, 0));
            RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152680, CraftResources.GetHue(CraftResource.Bloodwood), 510, WoodsmansTalisman, (int)CraftResource.Bloodwood));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152671, CraftResources.GetHue(CraftResource.Bloodwood), 525, HarvestMap, (int)CraftResource.Bloodwood));
            RewardCollection.Add(new BODCollectionItem(0x1022, 1157224, CraftResources.GetHue(CraftResource.AshWood), 650, CreateRunicFletcherTools, 1));
            RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152681, CraftResources.GetHue(CraftResource.Frostwood), 750, WoodsmansTalisman, (int)CraftResource.Frostwood));
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152672, CraftResources.GetHue(CraftResource.Frostwood), 950, HarvestMap, (int)CraftResource.Frostwood));
            RewardCollection.Add(new BODCollectionItem(0x1022, 1157225, CraftResources.GetHue(CraftResource.YewWood), 1000, CreateRunicFletcherTools, 2));
            RewardCollection.Add(new BODCollectionItem(0x1022, 1157226, CraftResources.GetHue(CraftResource.Heartwood), 1100, CreateRunicFletcherTools, 3));*/

            RewardCollection.Add(new BODCollectionItem(0xF49, "Sturdy Axe", 0, 10, SturdyAxe)); // daat
			RewardCollection.Add(new BODCollectionItem(0x1022, 1157219, "Fletcher Tools", 0, 10, FletcherTools));
			RewardCollection.Add(new BODCollectionItem(0x14F0, 1157200, "Fletcher Reward Title", 0, 25, RewardTitle, 19));
			RewardCollection.Add(new BODCollectionItem(0xF49, "Sturdy Lumberjack Axe", 0, 50, SturdyLumberjackAxe)); // daat
			RewardCollection.Add(new BODCollectionItem(0x13C6, "+1 Leather Gloves Of Lumberjacking", 0, 50, LeatherGlovesOfLumberjacking, 1)); // daat
			RewardCollection.Add(new BODCollectionItem(0x13C6, "+3 Leather Gloves Of Lumberjacking", 0, 200, LeatherGlovesOfLumberjacking, 3)); // daat
			RewardCollection.Add(new BODCollectionItem(0xf45, "Gargoyles Axe", 0, 200, GargoylesAxe)); // daat
			RewardCollection.Add(new BODCollectionItem(0x9E29, 1157264, "Craftsman Talisman 10 Charges", 0, 210, CraftsmanTalisman, 10));
			RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152678, "Yew Woodsmans Talisman", CraftResources.GetHue(CraftResource.YewWood), 225, WoodsmansTalisman, (int)CraftResource.YewWood));
			RewardCollection.Add(new BODCollectionItem(0x14EC, 1152669, "Yew Harvest Map", CraftResources.GetHue(CraftResource.YewWood), 310, HarvestMap, (int)CraftResource.YewWood));
			RewardCollection.Add(new BODCollectionItem(0x9E29, 1157218, "Craftsman Talisman 25 Charges", 0, 325, CraftsmanTalisman, 25));
			RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152679, "Heartwood Woodsmans Talisman", CraftResources.GetHue(CraftResource.Heartwood), 360, WoodsmansTalisman, (int)CraftResource.Heartwood));
			RewardCollection.Add(new BODCollectionItem(0x14EC, 1152670, "Heartwood Harvest Map", CraftResources.GetHue(CraftResource.Heartwood), 375, HarvestMap, (int)CraftResource.Heartwood));
			RewardCollection.Add(new BODCollectionItem(6215, "Stain Of Durability", 0, 400, StainOfDurability)); // daat
            RewardCollection.Add(new BODCollectionItem(4107, "Archery Butte North", 0, 400, Deco, 0)); // daat
            RewardCollection.Add(new BODCollectionItem(4106, "Archery Butte West", 0, 400, Deco, 1)); // daat
            RewardCollection.Add(new BODCollectionItem(3905, "Stack of Arrows", 0, 400, Deco, 2)); // daat
            RewardCollection.Add(new BODCollectionItem(7135, "Stack of Logs West", 0, 400, Deco, 3)); // daat
            RewardCollection.Add(new BODCollectionItem(7138, "Stack of Logs North", 0, 400, Deco, 4)); // daat
            RewardCollection.Add(new BODCollectionItem(7129, "Stack of Boards West", 0, 400, Deco, 5)); // daat
			RewardCollection.Add(new BODCollectionItem(7132, "Stack of Boards North", 0, 400, Deco, 6)); // daat
			RewardCollection.Add(new BODCollectionItem(0x9E29, 1157265, "Craftsman Talisman 50 Charges", 0, 410, CraftsmanTalisman, 50));
			RewardCollection.Add(new BODCollectionItem(5062, "Apprentices Gloves Of Lumberjacking", 0, 450, ArmorOfLumberjacking, 1)); // daat
			RewardCollection.Add(new BODCollectionItem(7609, "Apprentices Cap Of Lumberjacking", 0, 450, ArmorOfLumberjacking, 2)); // daat
			RewardCollection.Add(new BODCollectionItem(5068, "Apprentices Tunic Of Lumberjacking", 0, 450, ArmorOfLumberjacking, 3)); // daat
			RewardCollection.Add(new BODCollectionItem(5063, "Apprentices Gorget Of Lumberjacking", 0, 450, ArmorOfLumberjacking, 4)); // daat
			RewardCollection.Add(new BODCollectionItem(5069, "Apprentices Arms Of Lumberjacking", 0, 450, ArmorOfLumberjacking, 5)); // daat
			RewardCollection.Add(new BODCollectionItem(5067, "Apprentices Leggings Of Lumberjacking", 0, 450, ArmorOfLumberjacking, 6)); // daat
			RewardCollection.Add(new BODCollectionItem(0x1022, "Oak Runic Fletcher Tools", 0, 500, RunicFletcherTools, 1)); // daat
			RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152680, "Bloodwood Woodsmans Talisman", CraftResources.GetHue(CraftResource.Bloodwood), 510, WoodsmansTalisman, (int)CraftResource.Bloodwood));
			RewardCollection.Add(new BODCollectionItem(0x14EC, 1152671, "Bloodwood Harvest Map", CraftResources.GetHue(CraftResource.Bloodwood), 525, HarvestMap, (int)CraftResource.Bloodwood));
			RewardCollection.Add(new BODCollectionItem(0x1022, "Ash Runic Fletcher Tools", 0, 550, RunicFletcherTools, 2)); // daat
			RewardCollection.Add(new BODCollectionItem(5062, "Apprentices Gloves Of Bow Fletching", 0, 550, ArmorOfBowFletching, 1)); // daat
			RewardCollection.Add(new BODCollectionItem(7609, "Apprentices Cap Of Bow Fletching", 0, 550, ArmorOfBowFletching, 2)); // daat
			RewardCollection.Add(new BODCollectionItem(5068, "Apprentices Tunic Of Bow Fletching", 0, 550, ArmorOfBowFletching, 3)); // daat
			RewardCollection.Add(new BODCollectionItem(5063, "Apprentices Gorget Of Bow Fletching", 0, 550, ArmorOfBowFletching, 4)); // daat
			RewardCollection.Add(new BODCollectionItem(5069, "Apprentices Arms Of Bow Fletching", 0, 550, ArmorOfBowFletching, 5)); // daat
			RewardCollection.Add(new BODCollectionItem(5067, "Apprentices Leggings Of Bow Fletching", 0, 550, ArmorOfBowFletching, 6)); // daat
			RewardCollection.Add(new BODCollectionItem(0x1022, "Yew Runic Fletcher Tools", 0, 600, RunicFletcherTools, 3)); // daat
			RewardCollection.Add(new BODCollectionItem(0x1022, "Heartwood Runic Fletcher Tools", 0, 650, RunicFletcherTools, 4)); // daat
			RewardCollection.Add(new BODCollectionItem(0x1022, "Bloodwood Runic Fletcher Tools", 0, 700, RunicFletcherTools, 5)); // daat
            RewardCollection.Add(new BODCollectionItem(0x14F0, "A Wondrous Scroll Of Lumberjacking", 0, 700, LumberjackingPowerScroll, 5)); // daat
            RewardCollection.Add(new BODCollectionItem(0x14F0, "A Wondrous Scroll Of Bowcraft And Fletching", 0, 700, FletchingPowerScroll, 5)); // daat
			RewardCollection.Add(new BODCollectionItem(0x1022, "Frostwood Runic Fletcher Tools", 0, 750, RunicFletcherTools, 6)); // daat
            RewardCollection.Add(new BODCollectionItem(0x2F5A, 1152681, "Frostwood Woodsmans Talisman", CraftResources.GetHue(CraftResource.Frostwood), 750, WoodsmansTalisman, (int)CraftResource.Frostwood));
            RewardCollection.Add(new BODCollectionItem(0x14F0, "A Exalted Scroll Of Bowcraft And Fletching", 0, 800, FletchingPowerScroll, 10)); // daat
			RewardCollection.Add(new BODCollectionItem(5062, "Novices Gloves Of Lumberjacking", 0, 800, ArmorOfLumberjacking, 7)); // daat
			RewardCollection.Add(new BODCollectionItem(7609, "Novices Cap Of Lumberjacking", 0, 800, ArmorOfLumberjacking, 8)); // daat
			RewardCollection.Add(new BODCollectionItem(5068, "Novices Tunic Of Lumberjacking", 0, 800, ArmorOfLumberjacking, 9)); // daat
			RewardCollection.Add(new BODCollectionItem(5063, "Novices Gorget Of Lumberjacking", 0, 800, ArmorOfLumberjacking, 10)); // daat
			RewardCollection.Add(new BODCollectionItem(5069, "Novices Arms Of Lumberjacking", 0, 800, ArmorOfLumberjacking, 11)); // daat
			RewardCollection.Add(new BODCollectionItem(5067, "Novices Leggings Of Lumberjacking", 0, 800, ArmorOfLumberjacking, 12)); // daat
			RewardCollection.Add(new BODCollectionItem(5062, "Novices Gloves Of Bow Fletching", 0, 850, ArmorOfBowFletching, 7)); // daat
			RewardCollection.Add(new BODCollectionItem(7609, "Novices Cap Of Bow Fletching", 0, 850, ArmorOfBowFletching, 8)); // daat
			RewardCollection.Add(new BODCollectionItem(5068, "Novices Tunic Of Bow Fletching", 0, 850, ArmorOfBowFletching, 9)); // daat
			RewardCollection.Add(new BODCollectionItem(5063, "Novices Gorget Of Bow Fletching", 0, 850, ArmorOfBowFletching, 10)); // daat
			RewardCollection.Add(new BODCollectionItem(5069, "Novices Arms Of Bow Fletching", 0, 850, ArmorOfBowFletching, 11)); // daat
			RewardCollection.Add(new BODCollectionItem(5067, "Novices Leggings Of Bow Fletching", 0, 850, ArmorOfBowFletching, 12)); // daat
            RewardCollection.Add(new BODCollectionItem(0x14EC, 1152672, "Frostwood Harvest Map", CraftResources.GetHue(CraftResource.Frostwood), 950, HarvestMap, (int)CraftResource.Frostwood));
			RewardCollection.Add(new BODCollectionItem(0x13E4, "+10 Ancient Fletcher's Hammer", 0, 900, AncientFletcherHammer, 10)); // daat
			RewardCollection.Add(new BODCollectionItem(0x1022, "Ebony Runic Fletcher Tools", 0, 900, RunicFletcherTools, 7)); // daat
			RewardCollection.Add(new BODCollectionItem(0x1022, "Bamboo Runic Fletcher Tools", 0, 950, RunicFletcherTools, 8)); // daat
			RewardCollection.Add(new BODCollectionItem(0x13C6, "+5 Leather Gloves Of Lumberjacking", 0, 950, LeatherGlovesOfLumberjacking, 5)); // daat
			RewardCollection.Add(new BODCollectionItem(0x13E4, "+15 Ancient Fletcher's Hammer", 0, 1000, AncientFletcherHammer, 15)); // daat
			RewardCollection.Add(new BODCollectionItem(5062, "Masters Gloves Of Lumberjacking", 0, 1000, ArmorOfLumberjacking, 13)); // daat
			RewardCollection.Add(new BODCollectionItem(7609, "Masters Cap Of Lumberjacking", 0, 1000, ArmorOfLumberjacking, 14)); // daat
			RewardCollection.Add(new BODCollectionItem(5068, "Masters Tunic Of Lumberjacking", 0, 1000, ArmorOfLumberjacking, 15)); // daat
			RewardCollection.Add(new BODCollectionItem(5063, "Masters Gorget Of Lumberjacking", 0, 1000, ArmorOfLumberjacking, 16)); // daat
			RewardCollection.Add(new BODCollectionItem(5069, "Masters Arms Of Lumberjacking", 0, 1000, ArmorOfLumberjacking, 17)); // daat
			RewardCollection.Add(new BODCollectionItem(5067, "Masters Leggings Of Lumberjacking", 0, 1000, ArmorOfLumberjacking, 18)); // daat
			RewardCollection.Add(new BODCollectionItem(5062, "Masters Gloves Of Bow Fletching", 0, 1050, ArmorOfBowFletching, 13)); // daat
			RewardCollection.Add(new BODCollectionItem(7609, "Masters Cap Of Bow Fletching", 0, 1050, ArmorOfBowFletching, 14)); // daat
			RewardCollection.Add(new BODCollectionItem(5068, "Masters Tunic Of Bow Fletching", 0, 1050, ArmorOfBowFletching, 15)); // daat
			RewardCollection.Add(new BODCollectionItem(5063, "Masters Gorget Of Bow Fletching", 0, 1050, ArmorOfBowFletching, 16)); // daat
			RewardCollection.Add(new BODCollectionItem(5069, "Masters Arms Of Bow Fletching", 0, 1050, ArmorOfBowFletching, 17)); // daat
			RewardCollection.Add(new BODCollectionItem(5067, "Masters Leggings Of Bow Fletching", 0, 1050, ArmorOfBowFletching, 18)); // daat
			RewardCollection.Add(new BODCollectionItem(0x1022, "Purple Heart Runic Fletcher Tools", 0, 1050, RunicFletcherTools, 9)); // daat
			RewardCollection.Add(new BODCollectionItem(0x13E4, "+20 Ancient Fletcher's Hammer", 0, 1100, AncientFletcherHammer, 20)); // daat
			RewardCollection.Add(new BODCollectionItem(0x14F0, "A Mythical Scroll Of Bowcraft And Fletching", 0, 1100, FletchingPowerScroll, 15)); // daat
			RewardCollection.Add(new BODCollectionItem(0x13E4, "+30 Ancient Fletcher's Hammer", 0, 1200, AncientFletcherHammer, 30)); // daat
			RewardCollection.Add(new BODCollectionItem(0x1022, "Redwood Runic Fletcher Tools", 0, 1200, RunicFletcherTools, 10)); // daat
			RewardCollection.Add(new BODCollectionItem(0x13E4, "+40 Ancient Fletcher's Hammer", 0, 1250, AncientFletcherHammer, 40)); // daat
			RewardCollection.Add(new BODCollectionItem(0x1022, "Petrified Runic Fletcher Tools", 0, 1250, RunicFletcherTools, 11)); // daat
			RewardCollection.Add(new BODCollectionItem(0x14F0, "A Legendary Scroll Of Bowcraft And Fletching", 0, 1250, FletchingPowerScroll, 20)); // daat

			//daat start
			/*this.Groups = new RewardGroup[]
			{
				new RewardGroup(    0, new RewardItem( 1, SturdyAxe ) ),
				new RewardGroup(   50, new RewardItem( 45, SturdyAxe ),                         new RewardItem( 45, SturdyLumberjackAxe ),  new RewardItem( 10, LeatherGlovesOfLumberjacking, 1 ) ),
				new RewardGroup(  200, new RewardItem( 1, LeatherGlovesOfLumberjacking, 3 ),    new RewardItem( 1, GargoylesAxe ) ),
				new RewardGroup(  400, new RewardItem( 1, StainOfDurability ),                  new RewardItem( 4, Deco ) ),
				new RewardGroup(  450, new RewardItem( 3, StainOfDurability ),                  new RewardItem( 10, ArmorOfBowFletching, Utility.RandomMinMax(1,6) ) ),
				new RewardGroup(  500, new RewardItem( 1, RunicFletcherTools, 1 ),              new RewardItem( 3, Deco ) ),
				new RewardGroup(  550, new RewardItem( 3, RunicFletcherTools, 1 ),              new RewardItem( 2, RunicFletcherTools, 2 ), new RewardItem( 10, ArmorOfLumberjacking, Utility.RandomMinMax(1,6) ) ),
				new RewardGroup(  600, new RewardItem( 2, RunicFletcherTools, 2 ),              new RewardItem( 1, Deco ) ),
				new RewardGroup(  650, new RewardItem( 3, RunicFletcherTools, 3 ),              new RewardItem( 1, RunicFletcherTools, 4 ) ),
				new RewardGroup(  700, new RewardItem( 3, RunicFletcherTools, 4 ),              new RewardItem( 1, LumberjackingPowerScroll, 5 ), new RewardItem( 1, FletchingPowerScroll, 5 ) ),
				new RewardGroup(  750, new RewardItem( 3, RunicFletcherTools, 5 ),              new RewardItem( 1, RunicFletcherTools, 6 ) ),
				new RewardGroup(  800, new RewardItem( 1, FletchingPowerScroll, 10 ),           new RewardItem( 1, ArmorOfBowFletching, Utility.RandomMinMax(7,12)) ),
				new RewardGroup(  850, new RewardItem( 1, FletchingPowerScroll, 10 ) ,          new RewardItem( 1, ArmorOfLumberjacking, Utility.RandomMinMax(7,12)) ),
				new RewardGroup(  900, new RewardItem( 1, AncientFletcherHammer, 10 ) ,         new RewardItem( 2, RunicFletcherTools, 7 ) ),
				new RewardGroup(  950, new RewardItem( 1, LeatherGlovesOfLumberjacking, 5 ) ,   new RewardItem( 1, RunicFletcherTools, 8 ) ),
				new RewardGroup( 1000, new RewardItem( 1, AncientFletcherHammer, 15 ) ,         new RewardItem( 1, ArmorOfBowFletching, Utility.RandomMinMax(13,18)) ),
				new RewardGroup( 1050, new RewardItem( 2, AncientFletcherHammer, 15 ) ,         new RewardItem( 1, RunicFletcherTools, 9 ), new RewardItem( 1, ArmorOfBowFletching, Utility.RandomMinMax(13,18)) ),
				new RewardGroup( 1100, new RewardItem( 60, AncientFletcherHammer, 20 ) ,        new RewardItem( 30, RunicFletcherTools, 9 ), new RewardItem( 10, FletchingPowerScroll, 15 ) ),
                new RewardGroup( 1150, new RewardItem( 1, AncientFletcherHammer, 20 ),          new RewardItem( 2, RunicFletcherTools, 9 ) ),
				new RewardGroup( 1200, new RewardItem( 1, AncientFletcherHammer, 30 ) ,         new RewardItem( 2, RunicFletcherTools, 10 ) ),
				new RewardGroup( 1250, new RewardItem( 30, AncientFletcherHammer, 40 ) ,        new RewardItem( 60, RunicFletcherTools, 11 ), new RewardItem( 10, FletchingPowerScroll, 20 ) )
			};*/
			//daat end
        }

        #region Constructors

		//daat start
        private static readonly ConstructCallback SturdyLumberjackAxe = new ConstructCallback( CreateSturdyLumberjackAxe );
        private static readonly ConstructCallback SturdyAxe = new ConstructCallback( CreateSturdyAxe );
        private static readonly ConstructCallback StainOfDurability = new ConstructCallback( CreateStainOfDurability );
        private static readonly ConstructCallback ArmorOfBowFletching = new ConstructCallback(CreateArmorOfBowFletching);
        private static readonly ConstructCallback ArmorOfLumberjacking = new ConstructCallback(CreateArmorOfLumberjacking);
        private static readonly ConstructCallback RunicFletcherTools = new ConstructCallback( CreateFletchersTools );
        private static readonly ConstructCallback FletchingPowerScroll = new ConstructCallback( CreateFletchingPowerScroll );
        private static readonly ConstructCallback Deco = new ConstructCallback( CreateDeco );
        private static readonly ConstructCallback GargoylesAxe = new ConstructCallback( CreateGargoylesAxe );
        private static readonly ConstructCallback AncientFletcherHammer = new ConstructCallback( CreateAncientFletcherHammer );
        private static readonly ConstructCallback LeatherGlovesOfLumberjacking = new ConstructCallback( CreateLeatherGlovesOfLumberjacking );
        private static readonly ConstructCallback LumberjackingPowerScroll = new ConstructCallback( CreateLumberjackingPowerScroll );
        
        private static Item CreateSturdyLumberjackAxe( int type )
        {
            return new SturdyLumberjackAxe();
        }
        
        private static Item CreateSturdyAxe( int type )
        {
            return new SturdyAxe();
        }
        
        private static Item CreateStainOfDurability( int type )
        {
            return new StainOfDurability();
        }
        private static Item CreateArmorOfBowFletching(int type)
        {
            switch (type)
            {
                // public ArmorOfBowFletching( int bonus, int itemID, int skill ) : base( itemID ) example: 3 == bonus skill, 7609 == itemid, 2 == skill (mining)
                //notes:
                //total of 10 reward points
                //cloth have 6 points out of 10 (60%)
                //armor have 1 point out of 10 (10%)
                //sandals have 3 points out of 10 (30%)
                //new RewardGroup(  200, new RewardItem( 2, Cloth, 4 ), new RewardItem( 1, ArmorOfBowFletching, Utility.RandomMinMax(1,6) ), new RewardItem( 2, Sandals ) ),
                //3 items 5 points
                //cloth have 2/5 40%
                //armor have 1/5 20%
                //sandals 2/5 40%

                //cloth:
                case 1:
                default: return new ArmorOfBowFletching(1, 5062, Utility.Random(2)); //gloves
                case 2: return new ArmorOfBowFletching(1, 7609, Utility.Random(2)); //cap
                case 3: return new ArmorOfBowFletching(1, 5068, Utility.Random(2)); //tunic
                case 4: return new ArmorOfBowFletching(1, 5063, Utility.Random(2)); //gorget
                case 5: return new ArmorOfBowFletching(1, 5069, Utility.Random(2)); //arms
                case 6: return new ArmorOfBowFletching(1, 5067, Utility.Random(2)); //leggings
                case 7: return new ArmorOfBowFletching(3, 5062, Utility.Random(2)); //gloves
                case 8: return new ArmorOfBowFletching(3, 7609, Utility.Random(2)); //cap
                case 9: return new ArmorOfBowFletching(3, 5068, Utility.Random(2)); //tunic
                case 10: return new ArmorOfBowFletching(3, 5063, Utility.Random(2)); //gorget
                case 11: return new ArmorOfBowFletching(3, 5069, Utility.Random(2)); //arms
                case 12: return new ArmorOfBowFletching(3, 5067, Utility.Random(2)); //leggings
                case 13: return new ArmorOfBowFletching(5, 5062, Utility.Random(2)); //gloves
                case 14: return new ArmorOfBowFletching(5, 7609, Utility.Random(2)); //cap
                case 15: return new ArmorOfBowFletching(5, 5068, Utility.Random(2)); //tunic
                case 16: return new ArmorOfBowFletching(5, 5063, Utility.Random(2)); //gorget
                case 17: return new ArmorOfBowFletching(5, 5069, Utility.Random(2)); //arms
                case 18: return new ArmorOfBowFletching(5, 5067, Utility.Random(2)); //leggings
            }
        }
        private static Item CreateArmorOfLumberjacking(int type)
        {
            switch (type)
            {
                // public ArmorOfCarpentry( int bonus, int itemID, int skill ) : base( itemID ) example: 3 == bonus skill, 7609 == itemid, 2 == skill (mining)
                //notes:
                //total of 10 reward points
                //cloth have 6 points out of 10 (60%)
                //armor have 1 point out of 10 (10%)
                //sandals have 3 points out of 10 (30%)
                //new RewardGroup(  200, new RewardItem( 2, Cloth, 4 ), new RewardItem( 1, ArmorOfCarpentry, Utility.RandomMinMax(1,6) ), new RewardItem( 2, Sandals ) ),
                //3 items 5 points
                //cloth have 2/5 40%
                //armor have 1/5 20%
                //sandals 2/5 40%

                //cloth:
                case 1:
                default: return new ArmorOfLumberjacking(1, 5062, Utility.Random(2)); //gloves
                case 2: return new ArmorOfLumberjacking(1, 7609, Utility.Random(2)); //cap
                case 3: return new ArmorOfLumberjacking(1, 5068, Utility.Random(2)); //tunic
                case 4: return new ArmorOfLumberjacking(1, 5063, Utility.Random(2)); //gorget
                case 5: return new ArmorOfLumberjacking(1, 5069, Utility.Random(2)); //arms
                case 6: return new ArmorOfLumberjacking(1, 5067, Utility.Random(2)); //leggings
                case 7: return new ArmorOfLumberjacking(3, 5062, Utility.Random(2)); //gloves
                case 8: return new ArmorOfLumberjacking(3, 7609, Utility.Random(2)); //cap
                case 9: return new ArmorOfLumberjacking(3, 5068, Utility.Random(2)); //tunic
                case 10: return new ArmorOfLumberjacking(3, 5063, Utility.Random(2)); //gorget
                case 11: return new ArmorOfLumberjacking(3, 5069, Utility.Random(2)); //arms
                case 12: return new ArmorOfLumberjacking(3, 5067, Utility.Random(2)); //leggings
                case 13: return new ArmorOfLumberjacking(5, 5062, Utility.Random(2)); //gloves
                case 14: return new ArmorOfLumberjacking(5, 7609, Utility.Random(2)); //cap
                case 15: return new ArmorOfLumberjacking(5, 5068, Utility.Random(2)); //tunic
                case 16: return new ArmorOfLumberjacking(5, 5063, Utility.Random(2)); //gorget
                case 17: return new ArmorOfLumberjacking(5, 5069, Utility.Random(2)); //arms
                case 18: return new ArmorOfLumberjacking(5, 5067, Utility.Random(2)); //leggings
            }
        }
        private static Item CreateFletchersTools( int type )
        {
            if ( type >= 1 && type <= 11 )
                return new RunicFletcherTools( CraftResource.RegularWood + type, Core.AOS ? ( 100 - (type*5) ) : 50 );
            
            throw new InvalidOperationException();
        }
        
        private static Item CreateFletchingPowerScroll( int type )
        {
            if ( type == 5 || type == 10 || type == 15 || type == 20 )
                return new PowerScroll( SkillName.Fletching, 100 + type );
            
            throw new InvalidOperationException();
        }
        
        private static Item CreateLumberjackingPowerScroll( int type )
        {
            if ( type == 5 || type == 10 || type == 15 || type == 20 )
                return new PowerScroll( SkillName.Lumberjacking, 100 + type );
            
            throw new InvalidOperationException();
        }
        
        private static Item CreateDeco( int type )
        {
            //switch ( Utility.Random( 7 ) )
            switch (type)
            {
                default:
                    case 0: return new Deco( 4107, "Archery Butte North" );
                    case 1: return new Deco( 4106, "Archery Butte West" );
                    case 2: return new Deco( 3905, "Stack of Arrows" );
                    case 3: return new Deco( 7135, "Stack of Logs West" );
                    case 4: return new Deco( 7138, "Stack of Logs North" );
                    case 5: return new Deco( 7129, "Stack of Boards West" );
                    case 6: return new Deco( 7132, "Stack of Boards North" );
            }
        }
        
        private static Item CreateGargoylesAxe( int type )
        {
            return new GargoylesAxe();
        }
        
        private static Item CreateAncientFletcherHammer( int type )
        {
            if ( type == 10 || type == 20 || type == 30 || type == 40 )
                return new AncientFletcherHammer( type );
            
            throw new InvalidOperationException();
        }
        
        private static Item CreateLeatherGlovesOfLumberjacking( int type )
        {
            if ( type == 1 || type == 3 || type == 5 )
                return new LeatherGlovesOfLumberjacking( type );
            
            throw new InvalidOperationException();
        }
		//daat end

        private static Item FletcherTools(int type)
        {
            BaseTool tool = new FletcherTools();
            tool.UsesRemaining = 250;

            return tool;
        }

        private static Item CraftsmanTalisman(int type)
        {
            return new MasterCraftsmanTalisman(type, 0x9E29, TalismanSkill.Fletching);
        }

        private static Item CreateRunicFletcherTools(int type)
        {
            switch (type)
            {
                default:
                case 0: return new RunicFletcherTool(CraftResource.OakWood, 50);
                case 1: return new RunicFletcherTool(CraftResource.AshWood, 35);
                case 2: return new RunicFletcherTool(CraftResource.YewWood, 25);
                case 3: return new RunicFletcherTool(CraftResource.Heartwood, 15);
            }
        }

        #endregion

        public static readonly FletchingRewardCalculator Instance = new FletchingRewardCalculator();

        public override int ComputePoints(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int points = 0;

            if (quantity == 10)
                points += 10;
            else if (quantity == 15)
                points += 25;
            else if (quantity == 20)
                points += 50;

            if (exceptional)
                points += 200;

            /*switch (material)
            {
                case BulkMaterialType.None: break;
                case BulkMaterialType.OakWood: points += 300; break;
                case BulkMaterialType.AshWood: points += 350; break;
                case BulkMaterialType.YewWood: points += 400; break;
                case BulkMaterialType.Heartwood: points += 450; break;
                case BulkMaterialType.Bloodwood: points += 500; break;
                case BulkMaterialType.Frostwood: points += 550; break;
            }*/

			//daat start
			if (material >= BulkMaterialType.OakWood && material <= BulkMaterialType.Petrified)
				points += 200 + (50 * (material - BulkMaterialType.OakWood));
			//daat end

            if (itemCount > 1)
                points += this.LookupTypePoints(m_Types, type);

            return points;
        }

        private RewardType[] m_Types =
        {
            new RewardType(200, typeof(Arrow), typeof(Bolt)),
            new RewardType(300, typeof(Bow), typeof(CompositeBow), typeof(Yumi)),
            new RewardType(300, typeof(Crossbow), typeof(HeavyCrossbow), typeof(RepeatingCrossbow)),
            new RewardType(350, typeof(MagicalShortbow), typeof(RangersShortbow), typeof(LightweightShortbow), typeof(MysticalShortbow), typeof(AssassinsShortbow)),
            new RewardType(250, typeof(ElvenCompositeLongbow), typeof(BarbedLongbow), typeof(SlayerLongbow), typeof(FrozenLongbow), typeof(LongbowOfMight)),
        };

        private static readonly int[][][] m_GoldTable = new int[][][]
        {
            /*new int[][] // 1-part (regular)
            {
                new int[] { 150, 150, 225, 225, 300, 300 },
                new int[] { 225, 225, 325, 325, 450, 450 },
                new int[] { 300, 400, 500, 500, 600, 750 }
            },
            new int[][] // 1-part (exceptional)
            {
                new int[] { 300, 300, 400, 500, 600, 600 },
                new int[] { 450, 450, 650, 750, 900, 900 },
                new int[] { 600, 750, 850, 1000, 1200, 1800 }
            },
            new int[][] // 4-part (regular)
            {
                new int[] { 4000, 4000, 4000, 5000, 5000, 5000 },
                new int[] { 6000, 6000, 6000, 7500, 7500, 7500 },
                new int[] { 8000, 9000, 9500, 10000, 10000, 15000 }
            },
            new int[][] // 4-part (exceptional)
            {
                new int[] { 5000, 5000, 6000, 6750, 7500, 7500 },
                new int[] { 7500, 7500, 8500, 9500, 11250, 11250 },
                new int[] { 10000, 1250, 1250, 15000, 15000, 20000 }
            },
            new int[][] // 5-part (regular)
            {
                new int[] { 5000, 5000, 60000, 6000, 7500, 7500 },
                new int[] { 7500, 7500, 7500, 11250, 11250, 11250 },
                new int[] { 10000, 10000, 1250, 15000, 15000, 20000 }
            },
            new int[][] // 5-part (exceptional)
            {
                new int[] { 7500, 7500, 8500, 9500, 10000, 10000 },
                new int[] { 11250, 11250, 1250, 1350, 15000, 15000 },
                new int[] { 15000, 1750, 1750, 20000, 20000, 30000 }
            },*/

			//daat start
			new int[][] // 1-part (regular)
			{
				new int[]{ 150, 250, 250, 400,  400,  750,  750, 1200, 1200, 1500, 1500, 1800 },
				new int[]{ 225, 375, 375, 600,  600, 1125, 1125, 1800, 1800, 2100, 2100, 2400 },
				new int[]{ 300, 500, 750, 800, 1050, 1500, 2250, 2400, 3000, 3500, 4000, 4500 }
			},
			new int[][] // 1-part (exceptional)
			{
				new int[]{ 250, 400,  400,  750,  750, 1500, 1500, 3000, 3000, 3500, 3500, 4000 },
				new int[]{ 375, 600,  600, 1125, 1125, 2250, 2250, 4500, 4500, 5000, 5000, 5500 },
				new int[]{ 500, 800, 1200, 1500, 2500, 3000, 4000, 5000, 6000, 7000, 8000, 9000 }
			},
			new int[][] // 4-part (regular)
            {
				new int[]{ 2000, 4000,  6000,  8000, 10000, 12000, 14000, 16000, 18000, 20000, 22000, 25000 },
				new int[]{ 3000, 6000,  9000, 12000, 15000, 18000, 21000, 24000, 27000, 30000, 33000, 37500 },
				new int[]{ 4000, 8000, 12000, 16000, 20000, 24000, 28000, 32000, 36000, 40000, 44000, 50000 }
			},
			new int[][] // 4-part (exceptional)
            {
				new int[]{ 4000,  8000, 12000, 16000, 20000, 24000, 28000, 32000, 36000, 40000, 44000, 50000 },
				new int[]{ 6000, 12000, 18000, 24000, 30000, 36000, 42000, 48000, 54000, 60000, 66000, 75000 },
				new int[]{ 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 72000, 80000, 88000, 100000 }
			},
			new int[][] // 5-part (regular)
            {
				new int[]{ 3000,  5000,  5000,  7500,  7500,  8000,  8500,  9000, 10000, 11000, 12000, 13000 },
				new int[]{ 4500,  7500,  7500, 11250, 11500, 15000, 15000, 22500, 22500, 30000, 30000, 35000 },
				new int[]{ 6000, 10000, 15000, 15000, 20000, 20000, 30000, 30000, 50000, 60000, 60000, 70000 }
			},
			new int[][] // 5-part (exceptional)
            {
				new int[]{  5000, 10000, 15000, 20000, 25000, 30000,  35000,  40000,  45000,  50000,  60000,  70000 },
				new int[]{  7500, 15000, 15000, 22500, 22500, 37500,  37500,  75000,  75000, 100000, 100000, 120000 },
				new int[]{ 10000, 20000, 30000, 30000, 50000, 50000, 100000, 100000, 200000, 250000, 250000, 300000 }
			}
			//daat end
        };

        public override int ComputeGold(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int gold = 0;

            if (itemCount == 1 && BulkOrderSystem.NewSystemEnabled && BulkOrderSystem.ComputeGold(type, quantity, out gold))
            {
                return gold;
            }

            int[][][] goldTable = m_GoldTable;

            int typeIndex = ((itemCount == 5 ? 2 : itemCount == 4 ? 1 : 0) * 2) + (exceptional ? 1 : 0);
            int quanIndex = (quantity == 20 ? 2 : quantity == 15 ? 1 : 0);
            //int mtrlIndex = (material == BulkMaterialType.Frostwood ? 5 : material == BulkMaterialType.Bloodwood ? 4 : material == BulkMaterialType.Heartwood ? 3 : material == BulkMaterialType.YewWood ? 2 : material == BulkMaterialType.AshWood ? 1 : 0);
			//daat start
			int mtrlIndex = (material >= BulkMaterialType.OakWood && material <= BulkMaterialType.Petrified) ? 1 + (int)(material - BulkMaterialType.OakWood) : 0;
			//daat end

            gold = goldTable[typeIndex][quanIndex][mtrlIndex];

            int min = (gold * 9) / 10;
            int max = (gold * 10) / 9;

            return Utility.RandomMinMax(min, max);
        }
    }
    #endregion

    #region Alchemy Rewards
    public sealed class AlchemyRewardCalculator : RewardCalculator
    {
        public AlchemyRewardCalculator()
        {
            RewardCollection = new List<CollectionItem>();

            RewardCollection.Add(new BODCollectionItem(0xE9B, 1157219, 0, 10, MortarAndPestle));
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157183, 0, 25, RewardTitle, 20)); // brewer
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157202, 0, 50, RewardTitle, 21)); // concoctor
            RewardCollection.Add(new BODCollectionItem(0x14F0, 1157203, 0, 210, RewardTitle, 22)); // distiller
            RewardCollection.Add(new BODCollectionItem(0x182B, 1157278, 2741, 225, NaturalDye, 0));
            RewardCollection.Add(new BODCollectionItem(0x975, 1152660, CraftResources.GetHue(CraftResource.AshWood), 250, Cauldron, 0));
            RewardCollection.Add(new BODCollectionItem(0x975, 1152656, CraftResources.GetHue(CraftResource.Bronze), 260, Cauldron, 1));
            RewardCollection.Add(new BODCollectionItem(0x9E26, 1157264, 0, 275, CraftsmanTalisman, 10)); // todo: Get id
            RewardCollection.Add(new BODCollectionItem(0x975, 1152661, CraftResources.GetHue(CraftResource.YewWood), 300, Cauldron, 2));
            RewardCollection.Add(new BODCollectionItem(0x975, 1152657, CraftResources.GetHue(CraftResource.Gold), 310, Cauldron, 3));
            RewardCollection.Add(new BODCollectionItem(0x9E26, 1157218, 0, 325, CraftsmanTalisman, 25)); // todo: Get id
            RewardCollection.Add(new BODCollectionItem(0x975, 1152658, CraftResources.GetHue(CraftResource.Agapite), 350, Cauldron, 4));
            RewardCollection.Add(new BODCollectionItem(0x975, 1152662, CraftResources.GetHue(CraftResource.Heartwood), 360, Cauldron, 5));
            RewardCollection.Add(new BODCollectionItem(0x9E26, 1157265, 0, 375, CraftsmanTalisman, 50)); // todo: Get id
            RewardCollection.Add(new BODCollectionItem(0x182B, 1157278, 2731, 400, NaturalDye, 1));
            RewardCollection.Add(new BODCollectionItem(0x975, 1152663, CraftResources.GetHue(CraftResource.Bloodwood), 410, Cauldron, 6));
            RewardCollection.Add(new BODCollectionItem(0x182B, 1157278, 2735, 425, NaturalDye, 2));
            RewardCollection.Add(new BODCollectionItem(0x975, 1152659, CraftResources.GetHue(CraftResource.Verite), 450, Cauldron, 7));
        }

        #region Constructors
        private static Item MortarAndPestle(int type)
        {
            BaseTool tool = new MortarPestle();
            tool.UsesRemaining = 250;

            return tool;
        }

        private static Item Cauldron(int type)
        {
            switch (type)
            {
                default:
                case 0: return new CauldronOfTransmutationDeed(CraftResource.AshWood);
                case 1: return new CauldronOfTransmutationDeed(CraftResource.Bronze);
                case 2: return new CauldronOfTransmutationDeed(CraftResource.YewWood);
                case 3: return new CauldronOfTransmutationDeed(CraftResource.Gold);
                case 4: return new CauldronOfTransmutationDeed(CraftResource.Agapite);
                case 5: return new CauldronOfTransmutationDeed(CraftResource.Heartwood);
                case 6: return new CauldronOfTransmutationDeed(CraftResource.Bloodwood);
                case 7: return new CauldronOfTransmutationDeed(CraftResource.Verite);
            }
        }

        private static Item CraftsmanTalisman(int type)
        {
            return new MasterCraftsmanTalisman(type, 0x9E26, TalismanSkill.Alchemy);
        }
        #endregion

        public static readonly AlchemyRewardCalculator Instance = new AlchemyRewardCalculator();

        public override int ComputePoints(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int points = 0;

            if (quantity == 10)
                points += 10;
            else if (quantity == 15)
                points += 25;
            else if (quantity == 20)
                points += 50;

            if (itemCount == 3)
            {
                if(type == typeof(RefreshPotion) || type == typeof(HealPotion) || type == typeof(CurePotion))
                    points += 250;
                else
                    points += 300;
            }
            else if (itemCount == 4)
                points += 200;
            else if (itemCount == 5)
                points += 400;
            else if (itemCount == 6)
                points += 350;

            return points;
        }

        private static readonly int[][] m_GoldTable = new int[][]
        {
            new int[] // singles
            {
                150, 225, 300
            },
            new int[] // no 2 piece
            {
            },
            new int[] // 3-part
            {
                4000, 6000, 8000
            },
            new int[] // 4-part
            {
                5000, 7500, 10000
            },
            new int[] // 5-part
            {
                7500, 11250, 15000
            },
            new int[] // 6-part (regular)
            {
                7500, 11250, 15000
            },
        };

        public override int ComputeGold(int quantity, bool exceptional, BulkMaterialType material, int itemCount, Type type)
        {
            int gold = 0;
            
            if (itemCount == 1 && BulkOrderSystem.NewSystemEnabled && BulkOrderSystem.ComputeGold(type, quantity, out gold))
            {
                return gold;
            }

            int[][] goldTable = m_GoldTable;

            int quanIndex = (quantity == 20 ? 2 : quantity == 15 ? 1 : 0);

            gold = goldTable[itemCount - 1][quanIndex];

            int min = (gold * 9) / 10;
            int max = (gold * 10) / 9;

            return Utility.RandomMinMax(min, max);
        }
    }
    #endregion


}