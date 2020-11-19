using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probleme
{
    class De
    {
        // Attributs
        private char[] ensembleLettres;
        private char faceVisible;

        // Constructeur
        public De (char [] ensembleLettres, Random r)
        {
            if (ensembleLettres!=null && ensembleLettres.Length == 6)
            {
                this.ensembleLettres = ensembleLettres;
            }
            Lance(r);
        }

        // Propriétés
        public char[] EnsembleLettres
        {
            get { return ensembleLettres; }
        }

        public char FaceVisible
        {
            get { return faceVisible; }
        }

        // Méthodes
        public void Lance(Random r)
        {
            int i = r.Next(6);           // génère un entier aléatoire entre 0 et 5
            faceVisible = ensembleLettres[i];
        }

        public string toString()
        {
            string rep = "";
            rep += "Les 6 faces du dé sont : ";
            for (int i=0; i< ensembleLettres.Length; i++)
            {
                rep += ensembleLettres[i] + " ";
            }
            rep += "\n" + "Face visible : " + faceVisible;
            return rep;
        } 

    }
}
