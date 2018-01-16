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
using Server;

namespace Server.Spells.Druid
{
   public abstract class DruidSpell : Spell
   {
        public abstract double CastDelay{ get; }
        public abstract double RequiredSkill{ get; }
        public abstract int RequiredMana{ get; }
        public abstract SpellCircle Circle { get; }

        public override SkillName CastSkill{ get{ return SkillName.AnimalLore; } }
        public override SkillName DamageSkill{ get{ return SkillName.AnimalTaming; } }

        public override bool ClearHandsOnCast{ get{ return false; } }

        public DruidSpell( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info )
        {
        }

        public override void GetCastSkills( out double min, out double max )
        {
            min = RequiredSkill;
            max = RequiredSkill + 30.0;
        }

        public override int GetMana()
        {
            return RequiredMana;
        }

    	public override TimeSpan CastDelayBase
    	{
            get
            {
                return TimeSpan.FromSeconds(CastDelay);
            }
    	}

        public override TimeSpan GetCastDelay()
        {
         return TimeSpan.FromSeconds( CastDelay );
        }

		public virtual bool CheckResisted(Mobile target)
		{
			double n = this.GetResistPercent(target);

			n /= 100.0;

			if (n <= 0.0)
				return false;

			if (n >= 1.0)
				return true;

			int maxSkill = (1 + (int)this.Circle) * 10;
			maxSkill += (1 + ((int)this.Circle / 6)) * 25;

			if (target.Skills[SkillName.MagicResist].Value < maxSkill)
				target.CheckSkill(SkillName.MagicResist, 0.0, target.Skills[SkillName.MagicResist].Cap);

			return (n >= Utility.RandomDouble());
		}

		public virtual double GetResistPercentForCircle(Mobile target, SpellCircle circle)
		{
			double value = GetResistSkill(target);
			double firstPercent = value / 5.0;
			double secondPercent = value - (((this.Caster.Skills[this.CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);

			return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; // Seems should be about half of what stratics says.
		}

		public virtual double GetResistPercent(Mobile target)
		{
			return this.GetResistPercentForCircle(target, this.Circle);
		}
    }
}
