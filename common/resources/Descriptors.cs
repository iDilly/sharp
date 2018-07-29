using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace common.resources
{
    /// <summary>
    /// This is the XmlProjectile class, it represents a parsed XML data projectile.
    /// </summary>
    public class XmlProjectile
    {
        /// <param name="e">Root element of your projectile XML data.</param>
        public XmlProjectile(XElement e)
        {
            Id = e.GetAttribute<int>("id");

            Speed = e.GetValue<double>("Speed");
            LifetimeMS = e.GetValue<double>("LifetimeMS");

            MultiHit = e.GetValue<bool>("MultiHit");
            ArmorPiercing = e.GetValue<bool>("ArmorPiercing");

            Damage = e.GetValue("Damage", -1);
            if (Damage == -1)
            {
                var min = e.GetValue<int>("MinDamage");
                var max = e.GetValue<int>("MaxDamage");
                Damage = (min + max) / 2;
            }
        }

        /// <summary>
        /// This is the Id int variable, it defines the unique Id of the projectile (per object).
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// This is the Damage int variable, it defines the amount of damage dealt by the projectile.
        /// </summary>
        public int Damage { get; private set; }

        /// <summary>
        /// This is the Speed int variable, it defines the speed of the projectile.
        /// </summary>
        public double Speed { get; private set; }

        /// <summary>
        /// This is the LifetimeMS double variable, it defines the lifetime of the projectile in miliseconds.
        /// </summary>
        public double LifetimeMS { get; private set; }

        /// <summary>
        /// This is the MultiHit bool variable, it defines whether the projectile can MultiHit objects.
        /// </summary>
        public bool MultiHit { get; private set; }

        /// <summary>
        /// This is the ArmorPiercing bool variable, it defines whether the projectile can Armor pierce objects.
        /// </summary>
        public bool ArmorPiercing { get; private set; }
    }

    /// <summary>
    /// This is the XmlPlayer class, it represents a parsed XML data player.
    /// </summary>
    public class XmlPlayer
    {
        /// <param name="e">Root element of your object XML data.</param>
        /// <param name="type">Type (ushort) of the object: ObjectType.</param>
        /// <param name="id">Id (string) of the object: ObjectId.</param>
        public XmlPlayer(XElement e, ushort type, string id)
        {
            ObjectType = type;
            ObjectId = id;

            Class = e.GetValue<string>("Class");

            LevelIncreases = new List<Tuple<int, int>>();
            foreach (var i in e.Elements("LevelIncrease"))
                LevelIncreases.Add(Tuple.Create(
                    i.GetAttribute<int>("min"),
                    i.GetAttribute<int>("max")));

            Unlocks = new List<Tuple<int, ushort>>();
            foreach (var i in e.Elements("UnlockLevel"))
                Unlocks.Add(Tuple.Create(
                    i.GetAttribute<int>("level"),
                    i.GetAttribute<ushort>("type")));
            
            Equipment = e.GetValue<string>("Equipment").ToIntArray(", ");
            SlotTypes = e.GetValue<string>("SlotTypes").ToIntArray(", ");

            MaxHP = e.Element("MaxHitPoints").GetAttribute<int>("max");
            HP = e.GetValue<int>("MaxHitPoints");
            MaxMP = e.Element("MaxMagicPoints").GetAttribute<int>("max");
            MP = e.GetValue<int>("MaxMagicPoints");
            MaxATK = e.Element("Attack").GetAttribute<int>("max");
            ATK = e.GetValue<int>("Attack");
            MaxDEF = e.Element("Defense").GetAttribute<int>("max");
            DEF = e.GetValue<int>("Defense");
            MaxSPD = e.Element("Speed").GetAttribute<int>("max");
            SPD = e.GetValue<int>("Speed");
            MaxDEX = e.Element("Dexterity").GetAttribute<int>("max");
            DEX = e.GetValue<int>("Dexterity");
            MaxVIT = e.Element("HpRegen").GetAttribute<int>("max");
            VIT = e.GetValue<int>("HpRegen");
            MaxWIS = e.Element("MpRegen").GetAttribute<int>("max");
            WIS = e.GetValue<int>("MpRegen");
        }

        /// <summary>
        /// This is the ObjectType ushort variable, it defines the object type of the object.
        /// </summary>
        public ushort ObjectType { get; private set; }

        /// <summary>
        /// This is the ObjectId string variable, it defines the name/unique Id of the object.
        /// </summary>
        public string ObjectId { get; private set; }

        /// <summary>
        /// This is the Class string variable, it defines the class type of the object.
        /// </summary>
        public string Class { get; private set; }

        /// <summary>
        /// This is the Equipment int array variable, it defines the starting equipment of the player class.
        /// </summary>
        public int[] Equipment { get; private set; }

        /// <summary>
        /// This is the SlotTypes int array variable, it defines the slot types of the player class.
        /// </summary>
        public int[] SlotTypes { get; private set; }

        /// <summary>
        /// This is the MaxHP int variable, it defines the maximum amount of HP of the player class.
        /// </summary>
        public int MaxHP { get; private set; }

        /// <summary>
        /// This is the HP int variable, it defines the starting amount of HP of the player class.
        /// </summary>
        public int HP { get; private set; }

        /// <summary>
        /// This is the MaxMP int variable, it defines the maximum amount of MP of the player class.
        /// </summary>
        public int MaxMP { get; private set; }

        /// <summary>
        /// This is the MP int variable, it defines the starting amount of MP of the player class.
        /// </summary>
        public int MP { get; private set; }

        /// <summary>
        /// This is the MaxATK int variable, it defines the maximum amount of ATK of the player class.
        /// </summary>
        public int MaxATK { get; private set; }

        /// <summary>
        /// This is the ATK int variable, it defines the starting amount of ATK of the player class.
        /// </summary>
        public int ATK { get; private set; }
        
        /// <summary>
        /// This is the MaxDEF int variable, it defines the maximum amount of DEF of the player class.
        /// </summary>
        public int MaxDEF { get; private set; }

        /// <summary>
        /// This is the DEF int variable, it defines the starting amount of DEF of the player class.
        /// </summary>
        public int DEF { get; private set; }

        /// <summary>
        /// This is the MaxSPD int variable, it defines the maximum amount of SPD of the player class.
        /// </summary>
        public int MaxSPD { get; private set; }

        /// <summary>
        /// This is the SPD int variable, it defines the starting amount of SPD of the player class.
        /// </summary>
        public int SPD { get; private set; }

        /// <summary>
        /// This is the MaxDEX int variable, it defines the maximum amount of DEX of the player class.
        /// </summary>
        public int MaxDEX { get; private set; }

        /// <summary>
        /// This is the DEX int variable, it defines the starting amount of DEX of the player class.
        /// </summary>
        public int DEX { get; private set; }

        /// <summary>
        /// This is the MaxVIT int variable, it defines the maximum amount of VIT of the player class.
        /// </summary>
        public int MaxVIT { get; private set; }

        /// <summary>
        /// This is the VIT int variable, it defines the starting amount of VIT of the player class.
        /// </summary>
        public int VIT { get; private set; }

        /// <summary>
        /// This is the MaxWIS int variable, it defines the maximum amount of WIS of the player class.
        /// </summary>
        public int MaxWIS { get; private set; }

        /// <summary>
        /// This is the WIS int variable, it defines the starting amount of WIS of the player class.
        /// </summary>
        public int WIS { get; private set; }

        /// <summary>
        /// This is the tuple list variable, it stores all data about unlocks for the player class.
        /// </summary>
        public List<Tuple<int, ushort>> Unlocks { get; private set; }

        /// <summary>
        /// This is the tuple list variable, it stores all data about level increases for the player class.
        /// </summary>
        public List<Tuple<int, int>> LevelIncreases { get; private set; }
    }

    /// <summary>
    /// This is the XmlObject class, it represents a parsed XML data object.
    /// </summary>
    public class XmlObject
    {
        /// <param name="e">Root element of your object XML data.</param>
        /// <param name="type">Type (ushort) of the object: ObjectType.</param>
        /// <param name="id">Id (string) of the object: ObjectId.</param>
        public XmlObject(XElement e, ushort type, string id)
        {
            ObjectType = type;
            ObjectId = id;

            Experience = e.GetValue<double>("Exp");

            Projectiles = new List<XmlProjectile>();
            foreach (var i in e.Elements("Projectile"))
                Projectiles.Add(new XmlProjectile(i));

            Level = e.GetValue<int>("Level");
            Defense = e.GetValue<int>("Defense");
            MaxHP = e.GetValue<int>("MaxHitPoints");
            Size = e.GetValue("Size", 100);

            Class = e.GetValue<string>("Class");
            Group = e.GetValue<string>("Group");
            DisplayId = e.GetValue("DisplayId", ObjectId);

            Enemy = e.GetValue<bool>("Enemy");
            God = e.GetValue<bool>("God");
            Quest = e.GetValue<bool>("Quest");
            Encounter = e.GetValue<bool>("Encounter");
            Hero = e.GetValue<bool>("Hero");
            EventChestBoss = e.GetValue<bool>("EventChestBoss");
            KeepDamageRecord = e.GetValue<bool>("KeepDamageRecord");
            StasisImmune = e.GetValue<bool>("StasisImmune");
            ParalyzeImmune = e.GetValue<bool>("ParalyzeImmune");
            StunImmune = e.GetValue<bool>("StunImmune");
            DazedImmune = e.GetValue<bool>("DazedImmune");
            ArmorBreakImmune = e.GetValue<bool>("ArmorBreakImmune");
        }

        /// <summary>
        /// This is the Enemy bool variable, it tells you whether the object is an enemy type.
        /// </summary>
        public bool Enemy { get; private set; }

        /// <summary>
        /// This is the God bool variable, it tells you whether the object is a god (such as Medusa).
        /// </summary>
        public bool God { get; private set; }

        /// <summary>
        /// This is the Quest bool variable, it tells you whether the object is a god (such as Medusa).
        /// </summary>
        public bool Quest { get; private set; }

        /// <summary>
        /// This is the Encounter bool variable, it tells you whether the object is an encounter (usually a Realm Event).
        /// </summary>
        public bool Encounter { get; private set; }

        /// <summary>
        /// This is the Hero bool variable, it tells you whether the object is a hero (usually a Realm Event).
        /// </summary>
        public bool Hero { get; private set; }

        /// <summary>
        /// This is the EventChestBoss bool variable, it tells you whether the object is a event chest boss.
        /// </summary>
        public bool EventChestBoss { get; private set; }

        /// <summary>
        /// This is the KeepDamageRecord bool variable, it tells you whether the object keeps its damage record throughout its life and death.
        /// </summary>
        public bool KeepDamageRecord { get; private set; }

        /// <summary>
        /// This is the StasisImmune bool variable, it tells you whether the object is immune to the Stasis condition effect upon creation.
        /// </summary>
        public bool StasisImmune { get; private set; }

        /// <summary>
        /// This is the ParalyzeImmune bool variable, it tells you whether the object is immune to the Paralyze condition effect upon creation.
        /// </summary>
        public bool ParalyzeImmune { get; private set; }

        /// <summary>
        /// This is the StunImmune bool variable, it tells you whether the object is immune to the Stun condition effect upon creation.
        /// </summary>
        public bool StunImmune { get; private set; }

        /// <summary>
        /// This is the DazedImmune bool variable, it tells you whether the object is immune to the Dazed condition effect upon creation.
        /// </summary>
        public bool DazedImmune { get; private set; }

        /// <summary>
        /// This is the ArmorBreakImmune bool variable, it tells you whether the object is immune to the ArmorBreak condition effect upon creation.
        /// </summary>
        public bool ArmorBreakImmune { get; private set; }

        /// <summary>
        /// This is the Class string variable, it defines the class type of the object.
        /// </summary>
        public string Class { get; private set; }

        /// <summary>
        /// This is the Group string variable, it is the family/group that the object is assigned to.
        /// </summary>
        public string Group { get; private set; }

        /// <summary>
        /// This is the DisplayId string variable, it defines the mask Id of the object.
        /// </summary>
        public string DisplayId { get; private set; }

        /// <summary>
        /// This is the ObjectType ushort variable, it defines the object type of the object.
        /// </summary>
        public ushort ObjectType { get; private set; }

        /// <summary>
        /// This is the ObjectId string variable, it defines the name/unique Id of the object.
        /// </summary>
        public string ObjectId { get; private set; }

        /// <summary>
        /// This is the Defense int variable, it defines the amount of defense that the object has upon creation.
        /// </summary>
        public int Defense { get; private set; }

        /// <summary>
        /// This is the Size int variable, it defines the size of the object upon creation.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// This is the MaxHP int variable, it defines the maximum amount of hit points of the object upon creation.
        /// </summary>
        public int MaxHP { get; private set; }

        /// <summary>
        /// This is the Level int variable, it defines the level of the object.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// This is the Experience double variable, it defines the amount of experience given upon object death.
        /// </summary>
        public double Experience { get; private set; }

        /// <summary>
        /// This is the XmlProjectile list variable, it defines all the Xml projectiles of the object.
        /// </summary>
        public List<XmlProjectile> Projectiles { get; private set; }
    }
}
