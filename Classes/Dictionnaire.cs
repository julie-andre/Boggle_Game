using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;


namespace Probleme
{
    class Dictionnaire
    {
        // Attribut
        private string [] motsDico;

        // Constructeur 
        public Dictionnaire (string FileName)
        {
            motsDico = new string[14];
            ReadFile(FileName);
        }

        // Propriété
        public string [] MotsDico
        {
            get { return motsDico; }
        }

        // Méthodes
        public void ReadFile (string filename)
        {
            StreamReader str = null;

            string line;
            int i = -1;     //On commence à -1 car la boucle qui suit va incrémenter i de 1 car la première ligne du fichier contient un chiffre
            try
            {
                str = new StreamReader(filename);
                while ((line = str.ReadLine()) != null)
                {
                    if (line.Length <= 2)   // chaque fois qu'on rencontre un chiffre dans le fichier indiquant la taille des mots qui le suivent
                    {
                        i++;
                    }
                    else
                    {
                        motsDico[i] += line;
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (str != null)
                {
                    str.Close();
                }
            }

        }
        
        public string toString()
        {
            string rep = "";
            for (int i =0; i < motsDico.Length; i++)
            {
                int nbreLettres = i + 2;
                rep += "Les mots de " + nbreLettres + " lettres de ce dictionnaire sont : \n";
                rep += motsDico[i] + "\n";
            }
            return rep;
        }

        public string [] SplitageLigne (int  indice)
        {
            string[] rep = Regex.Split(motsDico[indice], " ");
            return rep;
        }


        public bool RechDichoRecursif(int debut, int fin, string mot, string [] tableau = null, int milieu=0)
        {
            if (tableau == null)
            {
                tableau = SplitageLigne(mot.Length - 2);
            }
            milieu = (debut + fin) / 2;
            // Ligne nécessaire car autrement le dernier mot du tableau n'est pas testé
            if (mot.CompareTo(tableau[fin]) == 0) return true;
            if (debut == fin) return false;
            if (mot.CompareTo(tableau[milieu]) == 0) return true;
            // Si le mot vient avant le mot du tableau dans l'ordre alphabétique
            if (mot.CompareTo(tableau[milieu]) < 0)
            {
                return RechDichoRecursif(debut, milieu, mot, tableau);
            }
            else return RechDichoRecursif(milieu+1, fin, mot, tableau);
        }
        
    }
}
