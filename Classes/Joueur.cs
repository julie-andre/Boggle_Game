using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probleme
{
    public class Joueur
    {
        // Attributs
        private string nom;
        private int score;
        private string [] motsTrouves;

        // Constructeur
        public Joueur (string nom)
        {
            if (nom != null && nom!="")   //!string.IsNullOrEmpty(nom)
            {
                this.nom = nom;
            }
            score = 0;
            motsTrouves = new string[0];
        }

        // Propriétés
        public string Nom
        {
            get { return nom; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }          // car on veut pouvoir modifier le score au fil du jeu
        }

        public string [] MotsTrouves
        {
            get { return motsTrouves; }
        }

        // Méthodes
        public bool Contain (string mot)
        {
            bool rep = false;
            for (int i =0; i< motsTrouves.Length && !(rep); i++)
            {
                if (motsTrouves[i] == mot)
                {
                    rep = true;
                }
            }
            return rep;
        }

        public void Add_Mot (string mot)        
        {
            if (mot!= null && mot.Length >= 3)     
            {
                if (Contain(mot) == false)
                {
                    if (motsTrouves != null && motsTrouves.Length != 0)
                    {
                        string[] temp = new string[motsTrouves.Length];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            temp[i] = motsTrouves[i];
                        }
                        motsTrouves = new string[temp.Length + 1];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            motsTrouves[i] = temp[i];
                        }
                        motsTrouves[temp.Length] = mot;
                    }
                    else
                    {
                        motsTrouves = new string[1];
                        motsTrouves[0] = mot;
                    }
                    
                }
            }
        }

        public string toString()
        {
            string rep = "";
            rep += "Le score de " + nom + " est de " + score + " grâce aux mots cités suivants\n";
            for (int i = 0; i < motsTrouves.Length; i++)
            {
                rep += motsTrouves[i] + " ";
            }
            return rep;
        }

    }
}
