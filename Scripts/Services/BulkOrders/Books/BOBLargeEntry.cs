using System;

namespace Server.Engines.BulkOrders
{
    public class BOBLargeEntry
    {
        private readonly bool m_RequireExceptional;
        private readonly BODType m_DeedType;
        private readonly BulkMaterialType m_Material;
        private readonly int m_AmountMax;
        private readonly BOBLargeSubEntry[] m_Entries;
        private int m_Price;
        public BOBLargeEntry(LargeBOD bod)
        {
            this.m_RequireExceptional = bod.RequireExceptional;

            if (bod is LargeTailorBOD)
                this.m_DeedType = BODType.Tailor;
            else if (bod is LargeSmithBOD)
                this.m_DeedType = BODType.Smith;

			//daat--<< Custom BODs Edit>>-------------------------------[Start 2 of 2]
			else if (bod is LargeCarpenterBOD)
				m_DeedType = BODType.Carpenter;
			else if (bod is LargeFletcherBOD)
				m_DeedType = BODType.Fletcher;
			//daat--<< Custom BODs Edit>>-------------------------------[End 2 of 2]

			this.m_Material = bod.Material;
            this.m_AmountMax = bod.AmountMax;

            this.m_Entries = new BOBLargeSubEntry[bod.Entries.Length];

            for (int i = 0; i < this.m_Entries.Length; ++i)
                this.m_Entries[i] = new BOBLargeSubEntry(bod.Entries[i]);
        }

        public BOBLargeEntry(GenericReader reader)
        {
            int version = reader.ReadEncodedInt();

            switch ( version )
            {
                case 0:
                    {
                        this.m_RequireExceptional = reader.ReadBool();

                        this.m_DeedType = (BODType)reader.ReadEncodedInt();

                        this.m_Material = (BulkMaterialType)reader.ReadEncodedInt();
                        this.m_AmountMax = reader.ReadEncodedInt();
                        this.m_Price = reader.ReadEncodedInt();

                        this.m_Entries = new BOBLargeSubEntry[reader.ReadEncodedInt()];

                        for (int i = 0; i < this.m_Entries.Length; ++i)
                            this.m_Entries[i] = new BOBLargeSubEntry(reader);

                        break;
                    }
            }
        }

        public bool RequireExceptional
        {
            get
            {
                return this.m_RequireExceptional;
            }
        }
        public BODType DeedType
        {
            get
            {
                return this.m_DeedType;
            }
        }
        public BulkMaterialType Material
        {
            get
            {
                return this.m_Material;
            }
        }
        public int AmountMax
        {
            get
            {
                return this.m_AmountMax;
            }
        }
        public int Price
        {
            get
            {
                return this.m_Price;
            }
            set
            {
                this.m_Price = value;
            }
        }
        public BOBLargeSubEntry[] Entries
        {
            get
            {
                return this.m_Entries;
            }
        }
        public Item Reconstruct()
        {
            LargeBOD bod = null;

            if (this.m_DeedType == BODType.Smith)
                bod = new LargeSmithBOD(this.m_AmountMax, this.m_RequireExceptional, this.m_Material, this.ReconstructEntries());
            else if (this.m_DeedType == BODType.Tailor)
                bod = new LargeTailorBOD(this.m_AmountMax, this.m_RequireExceptional, this.m_Material, this.ReconstructEntries());

			//daat--<< Custom BODs Edit>>-------------------------------[Start 1 of 2]
			else if (m_DeedType == BODType.Carpenter)
				bod = new LargeFletcherBOD(m_AmountMax, m_RequireExceptional, m_Material, ReconstructEntries());
			else if (m_DeedType == BODType.Fletcher)
				bod = new LargeCarpenterBOD(m_AmountMax, m_RequireExceptional, m_Material, ReconstructEntries());
			//daat--<< Custom BODs Edit>>-------------------------------[End 1 of 2]

			for (int i = 0; bod != null && i < bod.Entries.Length; ++i)
                bod.Entries[i].Owner = bod;

            return bod;
        }

        public void Serialize(GenericWriter writer)
        {
            writer.WriteEncodedInt(0); // version

            writer.Write((bool)this.m_RequireExceptional);

            writer.WriteEncodedInt((int)this.m_DeedType);
            writer.WriteEncodedInt((int)this.m_Material);
            writer.WriteEncodedInt((int)this.m_AmountMax);
            writer.WriteEncodedInt((int)this.m_Price);

            writer.WriteEncodedInt((int)this.m_Entries.Length);

            for (int i = 0; i < this.m_Entries.Length; ++i)
                this.m_Entries[i].Serialize(writer);
        }

        private LargeBulkEntry[] ReconstructEntries()
        {
            LargeBulkEntry[] entries = new LargeBulkEntry[this.m_Entries.Length];

            for (int i = 0; i < this.m_Entries.Length; ++i)
            {
                entries[i] = new LargeBulkEntry(null, new SmallBulkEntry(this.m_Entries[i].ItemType, this.m_Entries[i].Number, this.m_Entries[i].Graphic));
                entries[i].Amount = this.m_Entries[i].AmountCur;
            }

            return entries;
        }
    }
}