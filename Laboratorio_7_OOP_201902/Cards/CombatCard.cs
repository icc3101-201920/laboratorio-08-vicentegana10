using Laboratorio_7_OOP_201902.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Laboratorio_7_OOP_201902.Interfaces;

namespace Laboratorio_7_OOP_201902.Cards
{
    public class CombatCard : Card , ICharacteristics
    {
        //Atributos
        private int attackPoints;
        private bool hero;

        //Constructor
        public CombatCard(string name, EnumType type, string effect, int attackPoints, bool hero)
        {
            Name = name;
            Type = type;
            Effect = effect;
            AttackPoints = attackPoints;
            Hero = hero;
        }

        //Propiedades
        public int AttackPoints
        {
            get
            {
                return this.attackPoints;
            }
            set
            {
                this.attackPoints = value;
            }
        }
        public bool Hero
        { get
            {
                return this.hero;
            }
            set
            {
                this.hero = value;
            }
        }

        public List<string> GetCharacteristics()
        {
            List<string> caracs = new List<string>();

            Console.WriteLine(name);
            caracs.Add(($"name of card:{name}"));

            Console.WriteLine(type);
            caracs.Add(($"type of card:{type}"));

            Console.WriteLine(effect);
            caracs.Add(($"effect of card:{effect}"));

            Console.WriteLine(attackPoints);
            caracs.Add(($"attackPoints of card:{attackPoints}"));

            Console.WriteLine(hero);
            caracs.Add(($"is hero?:{hero}"));

            return caracs;

        }

    }
}
