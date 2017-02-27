using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an anchisaur corpse")]
    public class Anchisaur : BaseCreature
    {
        public override bool AttacksFocus { get { return true; } }

        [Constructable]
        public Anchisaur()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, .2, .4)
        {
            Name = "an anchisaur";
            Body = 1292;
            BaseSoundID = 422;

            SetStr(441, 511);
            SetDex(166, 185);
            SetInt(362, 431);

            SetDamage(16, 19);

            SetHits(2663, 3718);

            SetResistance(ResistanceType.Physical, 3, 4);
            SetResistance(ResistanceType.Fire, 3, 4);
            SetResistance(ResistanceType.Cold, 1);
            SetResistance(ResistanceType.Poison, 2, 3);
            SetResistance(ResistanceType.Energy, 2, 3);

            SetDamageType(ResistanceType.Physical, 100);

            SetSkill(SkillName.MagicResist, 30.1, 43.5);
            SetSkill(SkillName.Tactics, 30.1, 49.0);
            SetSkill(SkillName.Wrestling, 40, 50);

            Fame = 8000;
            Karma = -8000;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 1);
        }

        public override int DragonBlood { get { return 6; } }
        public override int Meat { get { return 6; } }
        public override int Hides { get { return 11; } }

        public Anchisaur(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}