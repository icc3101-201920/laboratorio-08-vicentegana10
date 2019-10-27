using Laboratorio_7_OOP_201902.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratorio_7_OOP_201902.Cards
{
    public class SpecialCard : Card
    {
        //Atributos
        private string buffType;

        //Propiedades
        public string BuffType
        {
            get
            {
                return this.buffType;
            }
            set
            {
                this.buffType = value;
            }
        }
        //Constructor
        public SpecialCard(string name, EnumType type, string effect)
        {
            Name = name;
            Type = type;
            Effect = effect;
            BuffType = null;
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

            Console.WriteLine(buffType);
            caracs.Add(($"Bufftype of card:{buffType}"));

            return caracs;

        }

    }
}
