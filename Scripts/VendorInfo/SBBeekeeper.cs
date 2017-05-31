using System;
using System.Collections.Generic;
using Server.Items;
using Server.Engines.Apiculture;

namespace Server.Mobiles
{
    public class SBBeekeeper : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBBeekeeper()
        {
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(JarHoney), 3, 20, 0x9EC, 0));
				Add(new GenericBuyInfo(typeof(Beeswax), 2, 20, 0x1422, 0));
				//Apiculture start
				Add(new GenericBuyInfo(typeof(apiBeeHiveDeed), 2000, 10, 2330, 0));
				Add(new GenericBuyInfo(typeof(HiveTool), 100, 20, 2549, 0));
				Add(new GenericBuyInfo(typeof(apiSmallWaxPot), 250, 20, 2532, 0));
				Add(new GenericBuyInfo(typeof(apiLargeWaxPot), 400, 20, 2541, 0));
				//Apiculture end
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(JarHoney), 1);
				Add(typeof(Beeswax), 1);
				//Apiculture start
				Add(typeof(apiBeeHiveDeed), 1000);
				Add(typeof(HiveTool), 50);
				Add(typeof(apiSmallWaxPot), 125);
				Add(typeof(apiLargeWaxPot), 200);
				//Apiculture end
            }
        }
    }
}